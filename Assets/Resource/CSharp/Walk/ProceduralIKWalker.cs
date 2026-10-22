using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 统一步态控制器（每个生物挂一个即可，不再每脚一个脚本）。
/// 负责把多条腿的 IK 目标点（如 P1P/P2P）摆放到地面上，支持：
/// - 任意数量的腿（多脚生物）
/// - 走路 / 跑步 两种状态（跑步跨步更大、抬腿更高、节奏更快）
/// - 下蹲状态（可蹲走，但下蹲时不能跑步）
/// - 斜坡 / 地形适应（脚精确落在地面高度，身体保持直立，不旋转腿去贴坡）
///
/// 使用说明：
/// 1. 挂在本体的根节点（如 Player02_Female）上。
/// 2. bodyTransform 指向身体（用于计算脚的相对位置）。
/// 3. bodyRb 指向身体刚体（用于读取移动速度）。
/// 4. legs 数组：每条腿填一个 foot（IK 目标点，如 P1P/P2P），restOffset 为
///    该脚相对身体的固定世界偏移（x 左负右正，y 为脚大约所在的高度）。
///    两条腿时建议 leg0.x=-0.3、leg1.x=+0.3；四条腿依次 -0.5/-0.2/+0.2/+0.5。
/// </summary>
public class ProceduralIKWalker : MonoBehaviour
{
    public enum LocomotionState
    {
        Idle,
        Walk,
        Run
    }

    [Serializable]
    public class Leg
    {
        [Tooltip("脚的 IK 目标点（空物体，如 P1P/P2P）")]
        public Transform foot;

        [Tooltip("脚相对身体的固定世界偏移（x：左负右正；y：脚大致高度，仅作射线起点参考）")]
        public Vector2 restOffset = new Vector2(0.3f, -0.4f);

        [Tooltip("步态相位 0~1，用于多腿错峰；-1 表示按索引自动分配")]
        public float phase = -1f;

        // ---- 运行时状态（不序列化）----
        [NonSerialized] public bool stepping;
        [NonSerialized] public float stepT;
        [NonSerialized] public Vector3 stepFrom;
        [NonSerialized] public Vector3 stepTo;
        [NonSerialized] public Vector3 stepMid;
        [NonSerialized] public float stepLift;
        [NonSerialized] public float stepDuration;
    }

    [Header("身体")]
    [Tooltip("身体/根节点，用于计算脚的相对位置")]
    public Transform bodyTransform;
    [Tooltip("身体刚体，用于读取移动速度")]
    public Rigidbody2D bodyRb;
    [Tooltip("是否玩家（决定是否读取键盘输入）")]
    public bool isPlayer = true;

    [Header("腿")]
    public List<Leg> legs = new List<Leg>();

    [Header("地形")]
    public LayerMask groundLayers = 1;   // 默认 Default 层
    public float raycastHeight = 3f;     // 从脚上方多高向下射线找地面
    public float maxStepUp = 0.5f;       // 单步最大上坡高度（超过则不再硬爬）
    public float maxStepDown = 0.7f;     // 单步最大下坡高度
    public float footSettleOffset = 0.02f; // 落地后略微抬高，避免穿地

    [Header("走路参数")]
    public float walkStride = 0.25f;     // 走路单步距离（越大迈得越远）
    public float walkStepDuration = 0.22f;
    public float walkStepLift = 0.12f;

    [Header("跑步参数")]
    public float runStride = 0.45f;      // 跑步单步距离（更大）
    public float runStepDuration = 0.16f;
    public float runStepLift = 0.22f;
    [Tooltip("身体水平速度超过此值时自动判定为跑步（AI 等无按键输入时使用）")]
    public float runSpeedThreshold = 4f;
    [Tooltip("迈步落点的前倾系数（0~1），越大落点越靠前")]
    public float overshootFactor = 0.5f;

    [Header("下蹲参数")]
    public KeyCode crouchKey = KeyCode.LeftControl;
    [Tooltip("true=按住下蹲，松开站起")]
    public bool holdToCrouch = true;
    public float crouchStrideMul = 0.6f;      // 下蹲步幅系数
    public float crouchLiftMul = 0.6f;        // 下蹲抬腿高度系数
    public float crouchStepDurationMul = 1.2f;// 下蹲节奏系数
    [Tooltip("下蹲时向下移动的身体锚点（如脊椎骨），可选；留空则只影响步态")]
    public Transform crouchAnchor;
    public float crouchBodyDrop = 0.12f;      // 下蹲时锚点下移量（本地 Y）

    [Header("步态")]
    [Tooltip("同时最多抬起的腿数，-1 表示自动（腿数/2，至少 1，保证始终有腿着地）")]
    public int maxLiftingLegs = -1;
    public float idleSettleSpeed = 6f;   // 静止时脚回到站立位的速度
    public float idleSpeedThreshold = 0.1f;
    [Tooltip("站立时每只脚离身体中线的距离（越小两脚越并拢，越接近“回到人物下方”）")]
    public float idleFootSpacing = 0.04f;

    // ---- 对外状态 ----
    public LocomotionState State { get; private set; }
    public bool IsRunning { get { return State == LocomotionState.Run; } }
    public bool IsCrouching { get; private set; }
    public float MoveDir { get; private set; } = 1f; // 1 右，-1 左

    private Vector3 crouchAnchorBaseLocal;
    private bool crouchAnchorInited;

    void Start()
    {
        if (bodyTransform == null)
            bodyTransform = transform;

        if (bodyRb == null && bodyTransform != null)
            bodyRb = bodyTransform.GetComponentInParent<Rigidbody2D>();

        // 自动分配步态相位
        for (int i = 0; i < legs.Count; i++)
        {
            if (legs[i] == null) continue;
            if (legs[i].phase < 0f)
                legs[i].phase = (legs.Count <= 1) ? 0f : (float)i / legs.Count;
        }

        if (crouchAnchor != null)
        {
            crouchAnchorBaseLocal = crouchAnchor.localPosition;
            crouchAnchorInited = true;
        }

        // 初始化：把所有脚直接放到地面
        SnapAllFeetToGround();
    }

    void LateUpdate()
    {
        UpdateStateInput();
        UpdateMoveDir();
        UpdateGait();
        UpdateCrouchPose();
    }

    // ---- 状态输入 ----
    void UpdateStateInput()
    {
        if (isPlayer)
        {
            if (holdToCrouch)
                IsCrouching = Input.GetKey(crouchKey);
        }

        float speed = bodyRb != null ? Mathf.Abs(bodyRb.velocity.x) : 0f;

        bool runRequested = false;
        if (isPlayer)
            runRequested = Input.GetKey(KeyCode.LeftShift);

        if (speed <= idleSpeedThreshold)
            State = LocomotionState.Idle;
        else if (IsCrouching)
            State = LocomotionState.Walk; // 下蹲不能跑
        else if (runRequested || speed >= runSpeedThreshold)
            State = LocomotionState.Run;
        else
            State = LocomotionState.Walk;
    }

    void UpdateMoveDir()
    {
        if (bodyRb != null && Mathf.Abs(bodyRb.velocity.x) > 0.05f)
            MoveDir = Mathf.Sign(bodyRb.velocity.x);
    }

    // ---- 参数取当前状态的数值 ----
    float CurrentStride()
    {
        float s = (State == LocomotionState.Run) ? runStride : walkStride;
        if (IsCrouching) s *= crouchStrideMul;
        return s;
    }

    float CurrentStepDuration()
    {
        float d = (State == LocomotionState.Run) ? runStepDuration : walkStepDuration;
        if (IsCrouching) d *= crouchStepDurationMul;
        return d;
    }

    float CurrentStepLift()
    {
        float l = (State == LocomotionState.Run) ? runStepLift : walkStepLift;
        if (IsCrouching) l *= crouchLiftMul;
        return l;
    }

    int EffectiveMaxLiftingLegs()
    {
        if (maxLiftingLegs >= 0) return Mathf.Max(1, maxLiftingLegs);
        return Mathf.Max(1, legs.Count / 2);
    }

    // ---- 核心步态 ----
    void UpdateGait()
    {
        if (legs.Count == 0) return;

        if (State == LocomotionState.Idle)
        {
            // 静止：双脚平滑回到身体下方并贴地（保持站立）
            for (int i = 0; i < legs.Count; i++)
            {
                var leg = legs[i];
                if (leg == null || leg.foot == null) continue;

                float side = Mathf.Sign(leg.restOffset.x);
                if (side == 0f) side = (i % 2 == 0) ? -1f : 1f;

                float baseX = bodyTransform != null ? bodyTransform.position.x : transform.position.x;
                float baseY = bodyTransform != null ? bodyTransform.position.y : transform.position.y;
                Vector3 rest = new Vector3(baseX + side * idleFootSpacing, baseY + leg.restOffset.y, 0f);

                if (TryGetGround(rest, out Vector3 ground))
                    rest = ground + Vector3.up * footSettleOffset;
                leg.foot.position = Vector3.Lerp(leg.foot.position, rest, Time.deltaTime * idleSettleSpeed);
            }
            return;
        }

        int lifting = 0;
        foreach (var leg in legs)
            if (leg != null && leg.stepping) lifting++;

        // 挑出在运动方向上落后最远、且需要迈步的脚
        Leg best = null;
        float bestTrail = 0f;
        foreach (var leg in legs)
        {
            if (leg == null || leg.foot == null || leg.stepping) continue;

            float baseX = bodyTransform != null ? bodyTransform.position.x : transform.position.x;
            float trail = MoveDir * (baseX - leg.foot.position.x); // >0 表示脚落在身体后方
            if (trail > CurrentStride() && trail > bestTrail)
            {
                best = leg;
                bestTrail = trail;
            }
        }

        if (best != null && lifting < EffectiveMaxLiftingLegs())
            StartStep(best);

        // 更新正在迈步的脚
        foreach (var leg in legs)
        {
            if (leg == null || leg.foot == null || !leg.stepping) continue;

            leg.stepT += Time.deltaTime / leg.stepDuration;
            float t = Mathf.Clamp01(leg.stepT);
            float e = EaseInOut(t);
            leg.foot.position = QuadraticBezier(leg.stepFrom, leg.stepMid, leg.stepTo, e);

            if (leg.stepT >= 1f)
            {
                leg.foot.position = leg.stepTo;
                leg.stepping = false;
            }
        }
    }

    void StartStep(Leg leg)
    {
        leg.stepping = true;
        leg.stepT = 0f;
        leg.stepFrom = leg.foot.position;
        leg.stepDuration = CurrentStepDuration();

        float baseX = bodyTransform != null ? bodyTransform.position.x : transform.position.x;
        float baseY = bodyTransform != null ? bodyTransform.position.y : transform.position.y;

        // 落点：身体前方半个单步距离 + 速度前倾（让脚甩到身体前面，形成交替）
        float overshoot = (bodyRb != null ? bodyRb.velocity.x : 0f) * leg.stepDuration * overshootFactor;
        Vector3 target = new Vector3(
            baseX + MoveDir * CurrentStride() * 0.5f + overshoot,
            baseY + leg.restOffset.y,
            0f);

        Vector3 to = target;
        if (TryGetGround(target, out Vector3 ground))
            to = ground + Vector3.up * footSettleOffset;

        // 高度差钳制（避免爬墙 / 掉悬崖）
        float dh = to.y - leg.stepFrom.y;
        if (dh > maxStepUp) to.y = leg.stepFrom.y + maxStepUp;
        else if (dh < -maxStepDown) to.y = leg.stepFrom.y - maxStepDown;

        leg.stepTo = to;

        // 抬腿高度：基础值 + 因高度差额外抬升
        leg.stepLift = CurrentStepLift() + Mathf.Clamp(Mathf.Abs(dh), 0f, 0.2f);
        leg.stepMid = (leg.stepFrom + leg.stepTo) * 0.5f + Vector3.up * leg.stepLift;
    }

    Vector3 GetRestWorldPos(Leg leg)
    {
        Vector3 basePos = bodyTransform != null ? bodyTransform.position : transform.position;
        return basePos + new Vector3(leg.restOffset.x, leg.restOffset.y, 0f);
    }

    bool TryGetGround(Vector3 pos, out Vector3 ground)
    {
        ground = pos;
        RaycastHit2D hit = Physics2D.Raycast(pos + Vector3.up * raycastHeight, Vector2.down, raycastHeight * 2f, groundLayers);
        if (hit.collider != null)
        {
            ground = hit.point;
            return true;
        }
        return false;
    }

    void SnapAllFeetToGround()
    {
        foreach (var leg in legs)
        {
            if (leg == null || leg.foot == null) continue;
            Vector3 rest = GetRestWorldPos(leg);
            if (TryGetGround(rest, out Vector3 ground))
                leg.foot.position = ground + Vector3.up * footSettleOffset;
        }
    }

    // ---- 下蹲姿势 ----
    void UpdateCrouchPose()
    {
        if (crouchAnchor == null || !crouchAnchorInited) return;
        Vector3 target = crouchAnchorBaseLocal;
        if (IsCrouching)
            target += Vector3.down * crouchBodyDrop;
        crouchAnchor.localPosition = Vector3.Lerp(crouchAnchor.localPosition, target, Time.deltaTime * 8f);
    }

    /// <summary>
    /// 供外部（如 AI）设置下蹲；玩家由按键控制。
    /// </summary>
    public void SetCrouch(bool crouch)
    {
        if (isPlayer) return;
        IsCrouching = crouch;
    }

    /// <summary>
    /// 供外部强制指定走/跑（AI 用）；传 Idle 表示由速度自动判定。
    /// </summary>
    public void SetLocomotion(LocomotionState state)
    {
        if (state == LocomotionState.Idle) return; // Idle 语义留给自动判定
        if (isPlayer) return;                       // 玩家仍以按键为准
        State = state;
    }

    float EaseInOut(float t)
    {
        // 平滑抬脚：三次平滑
        return t * t * (3f - 2f * t);
    }

    Vector3 QuadraticBezier(Vector3 a, Vector3 b, Vector3 c, float t)
    {
        Vector3 ab = Vector3.Lerp(a, b, t);
        Vector3 bc = Vector3.Lerp(b, c, t);
        return Vector3.Lerp(ab, bc, t);
    }

    void OnDrawGizmos()
    {
        if (legs == null) return;
        foreach (var leg in legs)
        {
            if (leg == null || leg.foot == null) continue;
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(GetRestWorldPos(leg), 0.05f);
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(leg.foot.position, 0.03f);
        }
    }
}