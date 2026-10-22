using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D.Animation;
using UnityEngine.U2D.IK;

/// <summary>
/// 角色状态效果管理器：昏倒 / 流血 / 内伤 / 结痂 / 兴奋-虚弱 / 毒气，以及跌倒（滞空→布娃娃→落地回正）。
/// public 字段与方法名保持不变。
/// </summary>
public class StatusManager : MonoBehaviour
{
    public enum FallPhase { None, Airborne, Recovering }

    // ================= 引用 / 昏倒 Collapse =================
    [Header("引用 / 昏倒 Collapse")]
    public PCWalk ThePCWalk;
    public HandBigGunUpLeft TheHandBigGunLeft;
    public HandBigGunUpRight TheHandBigGunRight;
    public HandGunUp TheHandGunUp;
    public HandGunUp TheHandGunUp2;
    public WatchMouse Watch;
    public Staring TheStaring;
    public MeleeAttackAnimation TheMeleeAttackAnimation;
    public List<Standing> TheStanding;
    public Moving[] TheMoving;

    public AiPath TheAiPath;

    public float CollapseTime = 4f;
    public bool IsCollapse;

    // ================= 跌倒 Fall =================
    [Header("跌倒 Fall")]
    public bool IsFalling;
    public FallPhase TheFallPhase = FallPhase.None;

    [Tooltip("身体离地超过此距离判定为滞空")]
    public float AirborneHeight = 1.6f;
    [Tooltip("落到地面、离地小于此距离开始回正")]
    public float LandHeight = 1.2f;
    [Tooltip("回正速度")]
    public float StandUpSpeed = 4f;
    [Tooltip("地面所在层")]
    public LayerMask GroundMask = 1;
    [Tooltip("滞空/落地判定的去抖时间（秒），避免边缘处每帧切换")]
    public float FallDebounce = 0.15f;
    [Tooltip("落地判定允许的垂直速度")]
    public float LandingVelocity = 1f;

    [Tooltip("身体刚体（身体）")]
    public Rigidbody2D TheBody;
    [Tooltip("IK 行走控制器")]
    public ProceduralIKWalker TheWalker;
    [Tooltip("腿部 IK 管理器")]
    public IKManager2D TheIK;
    [Tooltip("四条腿的物理刚体（左大腿/左小腿/右大腿/右小腿）")]
    public Rigidbody2D[] TheLegRbs;
    [Tooltip("四条腿的 HingeJoint2D")]
    public HingeJoint2D[] TheLegHinges;
    [Tooltip("四条腿的 SpriteSkin（视觉层）")]
    public SpriteSkin[] TheLegSkins;

    private bool[] standingOriginStates;
    private bool[] legOriginSimulated;
    private bool[] legOriginHingeEnabled;
    private bool[] legOriginSkinEnabled;

    private float airborneTimer;
    private float groundedTimer;

    // ================= 流血 Bleed =================
    [Header("流血 Bleed")]
    public HealthManager TheHealthManager;
    public GameObject TheBloodProject;

    public bool StartBleeding;
    public float BleedingTime;
    public float BleedingSeverity = 0;
    public float bleedingSeverityMax = 10;
    public float bleedingSeverityMin = 0;
    public bool StartTimer;
    public float PreviousTime;

    // ================= 内伤 Internal_Injuries =================
    [Header("内伤 Internal Injuries")]
    public bool IsInternalInjures;
    public float InternalInjures_Severity = 0;
    public float InternalInjures_SeverityMax = 10;
    public float InternalInjures_SeverityMin = 0;
    public float InternalInjures_TimeToRecover;
    public bool InternalInjures_TimerStart;

    [Header("灼伤 Burn")]
    public int NumberOfBurn;

    [Header("结痂 Scabbed")]
    public bool IsScabbed;

    [Header("枪伤 GunShot")]
    public int NumberOfGunShot;

    [Header("割伤 Cut")]
    public int NumberOfCut;

    [Header("淤伤 Bruise")]
    public int NumberOfBruise;

    [Header("兴奋 / 虚弱 Excitement / Listless")]
    public bool IsExcitement;
    public bool IsListless;
    public float ExcitementSeverity = 1f;
    public float ExcitementSeverityMin = 1;
    public float ListLessSeverityMin = 0.1f;
    public float ExcitementRecoverTime = 120f;
    public bool ExcitementStartTimer;

    [Header("毒气 GasPoisoning")]
    public bool IsGasPoisoning;
    public float GasPoisoningSeverity = 0;
    public float GasPoisoningSeverityMax = 10;
    public float GasPoisoningSeverityMin = 0;

    [Header("抓伤 / 咬伤 Scratch / Bite")]
    public int NumberOfScratch;
    public int NumberOfBiteInjury;

    // ================= 生命周期 =================
    void Start()
    {
        ThePCWalk = gameObject.GetComponent<PCWalk>();
        TheHealthManager = gameObject.GetComponent<HealthManager>();

        if (TheStanding != null)
        {
            standingOriginStates = new bool[TheStanding.Count];
            for (int i = 0; i < TheStanding.Count; i++)
                standingOriginStates[i] = (TheStanding[i] != null) && TheStanding[i].enabled;
        }

        if (TheLegRbs != null)
        {
            int n = TheLegRbs.Length;
            legOriginSimulated = new bool[n];
            for (int i = 0; i < n; i++)
                legOriginSimulated[i] = (TheLegRbs[i] != null) && TheLegRbs[i].simulated;
        }
        if (TheLegHinges != null)
        {
            legOriginHingeEnabled = new bool[TheLegHinges.Length];
            for (int i = 0; i < TheLegHinges.Length; i++)
                legOriginHingeEnabled[i] = (TheLegHinges[i] != null) && TheLegHinges[i].enabled;
        }
        if (TheLegSkins != null)
        {
            legOriginSkinEnabled = new bool[TheLegSkins.Length];
            for (int i = 0; i < TheLegSkins.Length; i++)
                legOriginSkinEnabled[i] = (TheLegSkins[i] != null) && TheLegSkins[i].enabled;
        }

        IgnoreLegSelfCollisions();
    }

    void Update() { UpdateFallState(); }

    void FixedUpdate()
    {
        UpdateBleedingState();
        UpdateInternalInjuresState();
        UpdateScabbedState();
        UpdateExcitementState();
        UpdateGasPoisoningState();
    }

    // ================= 跌倒 =================
    void UpdateFallState()
    {
        if (TheBody == null) return;

        float groundDist = 999f;
        RaycastHit2D hit = Physics2D.Raycast(TheBody.position, Vector2.down, 20f, GroundMask);
        if (hit.collider != null) groundDist = hit.distance;

        // 去抖：条件要持续 FallDebounce 秒才切换，避免边缘处每帧来回跳
        bool airborne = groundDist > AirborneHeight;
        bool grounded = groundDist <= LandHeight && Mathf.Abs(TheBody.velocity.y) < LandingVelocity;

        if (airborne)
        {
            airborneTimer += Time.deltaTime;
            groundedTimer = 0f;
        }
        else if (grounded)
        {
            groundedTimer += Time.deltaTime;
            airborneTimer = 0f;
        }
        else
        {
            airborneTimer = 0f;
            groundedTimer = 0f;
        }

        switch (TheFallPhase)
        {
            case FallPhase.None:
                if (airborneTimer >= FallDebounce) EnterFall();
                break;
            case FallPhase.Airborne:
                if (groundedTimer >= FallDebounce) StartRecover();
                break;
            case FallPhase.Recovering:
                UpdateRecover();
                break;
        }
    }

    void EnterFall()
    {
        IsFalling = true;
        TheFallPhase = FallPhase.Airborne;
        airborneTimer = 0f;
        groundedTimer = 0f;

        SetMovingEnabled(false);
        if (TheWalker != null) TheWalker.enabled = false;

        if (TheBody != null) TheBody.freezeRotation = false;
        SetStandingEnabled(false);

        // 骨骼层停掉
        if (TheIK != null) TheIK.enabled = false;

        // 视觉层接管：关掉 SpriteSkin（不再跟骨骼），启用铰链、切 Dynamic、参与碰撞
        SetLegSkins(false);
        SetLegHinges(true);
        SetLegDynamic(true);
    }

    void StartRecover()
    {
        if (TheFallPhase != FallPhase.Airborne) return;
        TheFallPhase = FallPhase.Recovering;
        airborneTimer = 0f;
        groundedTimer = 0f;

        // 视觉层交回骨骼层：恢复 SpriteSkin、关铰链、切回 Static
        SetLegDynamic(false);
        SetLegHinges(false);
        SetLegSkins(true);

        // 恢复 IK 和行走
        if (TheIK != null) TheIK.enabled = true;
        if (TheWalker != null) TheWalker.enabled = true;

        // 身体临时 Kinematic：回正不吃物理
        if (TheBody != null)
        {
            TheBody.velocity = Vector2.zero;
            TheBody.angularVelocity = 0f;
            TheBody.bodyType = RigidbodyType2D.Kinematic;
        }
    }

    void UpdateRecover()
    {
        if (TheBody == null) { FinishRecover(); return; }

        TheBody.rotation = Mathf.LerpAngle(TheBody.rotation, 0f, Time.deltaTime * StandUpSpeed);

        if (Mathf.Abs(Mathf.DeltaAngle(TheBody.rotation, 0f)) < 3f)
            FinishRecover();
    }

    void FinishRecover()
    {
        if (TheBody != null)
        {
            TheBody.rotation = 0f;
            TheBody.bodyType = RigidbodyType2D.Dynamic;
            TheBody.freezeRotation = true;
            TheBody.velocity = Vector2.zero;
            TheBody.angularVelocity = 0f;
        }

        SetStandingEnabled(true);
        SetMovingEnabled(true);
        if (TheWalker != null) TheWalker.enabled = true;
        if (TheIK != null) TheIK.enabled = true;

        IsFalling = false;
        TheFallPhase = FallPhase.None;
        airborneTimer = 0f;
        groundedTimer = 0f;
    }

    void SetLegSkins(bool on)
    {
        if (TheLegSkins == null) return;
        for (int i = 0; i < TheLegSkins.Length; i++)
        {
            if (TheLegSkins[i] == null) continue;
            TheLegSkins[i].enabled = on;
        }
    }

    void SetLegHinges(bool on)
    {
        if (TheLegHinges == null) return;
        for (int i = 0; i < TheLegHinges.Length; i++)
        {
            if (TheLegHinges[i] == null) continue;
            TheLegHinges[i].enabled = on;
        }
    }

    void SetLegDynamic(bool dynamic)
    {
        if (TheLegRbs == null) return;
        for (int i = 0; i < TheLegRbs.Length; i++)
        {
            if (TheLegRbs[i] == null) continue;
            if (dynamic)
            {
                TheLegRbs[i].simulated = true;
                TheLegRbs[i].bodyType = RigidbodyType2D.Dynamic;
            }
            else
            {
                TheLegRbs[i].velocity = Vector2.zero;
                TheLegRbs[i].angularVelocity = 0f;
                TheLegRbs[i].bodyType = RigidbodyType2D.Static;
                if (legOriginSimulated != null && i < legOriginSimulated.Length)
                    TheLegRbs[i].simulated = legOriginSimulated[i];
            }
        }
    }

    void IgnoreLegSelfCollisions()
    {
        if (TheLegRbs == null) return;

        Collider2D bodyCol = TheBody != null ? TheBody.GetComponent<Collider2D>() : null;
        for (int i = 0; i < TheLegRbs.Length; i++)
        {
            if (TheLegRbs[i] == null) continue;
            Collider2D ci = TheLegRbs[i].GetComponent<Collider2D>();
            if (ci == null) continue;

            if (bodyCol != null) Physics2D.IgnoreCollision(bodyCol, ci, true);

            for (int j = i + 1; j < TheLegRbs.Length; j++)
            {
                if (TheLegRbs[j] == null) continue;
                Collider2D cj = TheLegRbs[j].GetComponent<Collider2D>();
                if (cj != null) Physics2D.IgnoreCollision(ci, cj, true);
            }
        }
    }

    void SetStandingEnabled(bool enabled)
    {
        if (TheStanding == null) return;
        for (int i = 0; i < TheStanding.Count; i++)
        {
            if (TheStanding[i] == null) continue;
            bool target = enabled;
            if (enabled && standingOriginStates != null && i < standingOriginStates.Length)
                target = standingOriginStates[i];
            TheStanding[i].enabled = target;
        }
    }

    void SetMovingEnabled(bool enabled)
    {
        if (TheMoving == null) return;
        for (int i = 0; i < TheMoving.Length; i++)
            if (TheMoving[i] != null) TheMoving[i].enabled = enabled;
    }

    // ================= 昏倒 =================
    public void Collapse() { StartCoroutine(WaitForAwake()); }

    IEnumerator WaitForAwake()
    {
        IsCollapse = true;
        SetCollapsePartsEnabled(false);
        yield return new WaitForSeconds(CollapseTime);
        IsCollapse = false;
        SetCollapsePartsEnabled(true);
    }

    void SetCollapsePartsEnabled(bool enabled)
    {
        if (ThePCWalk) ThePCWalk.enabled = enabled;
        if (TheAiPath) TheAiPath.enabled = enabled;

        if (TheHandBigGunLeft != null) TheHandBigGunLeft.enabled = enabled;
        if (TheHandBigGunRight != null) TheHandBigGunRight.enabled = enabled;
        if (TheHandGunUp != null) TheHandGunUp.enabled = enabled;
        if (TheHandGunUp2 != null) TheHandGunUp2.enabled = enabled;
        if (Watch != null) Watch.enabled = enabled;
        if (TheStaring != null) TheStaring.enabled = enabled;
        if (TheMeleeAttackAnimation != null) TheMeleeAttackAnimation.enabled = enabled;

        if (TheStanding != null)
        {
            for (int i = 0; i < TheStanding.Count; i++)
            {
                if (TheStanding[i] == null) continue;
                bool target = enabled;
                if (enabled && standingOriginStates != null && i < standingOriginStates.Length)
                    target = standingOriginStates[i];
                TheStanding[i].enabled = target;
            }
        }
        if (TheMoving != null)
        {
            for (int i = 0; i < TheMoving.Length; i++)
                if (TheMoving[i] != null) TheMoving[i].enabled = enabled;
        }
    }

    // ================= 流血 =================
    void UpdateBleedingState()
    {
        if (StartBleeding == true || PreviousTime != BleedingTime)
        {
            if (StartTimer == false || PreviousTime != BleedingTime)
            {
                StopCoroutine("BleedingTimer");
                StartCoroutine("BleedingTimer");
                PreviousTime = BleedingTime;
                StartTimer = true;
            }
            Bleeding();
        }
    }

    public void Bleeding()
    {
        if (BleedingSeverity > bleedingSeverityMax)
        {
            BleedingSeverity = bleedingSeverityMax;
        }
        else if (BleedingSeverity < bleedingSeverityMin)
        {
            BleedingSeverity = bleedingSeverityMin;
            StartBleeding = false;
            StartTimer = false;
            BleedingTime = 0;
            PreviousTime = BleedingTime;
        }
        TheHealthManager.Health = TheHealthManager.Health - 0.01f * BleedingSeverity;
    }

    IEnumerator BleedingTimer()
    {
        yield return new WaitForSeconds(BleedingTime);
        StartBleeding = false;
        StartTimer = false;
        BleedingTime = 0;
        PreviousTime = BleedingTime;
    }

    // ================= 内伤 =================
    void UpdateInternalInjuresState()
    {
        if (IsInternalInjures)
        {
            if (InternalInjures_TimerStart == false)
            {
                StopCoroutine("InternalInjures_Timer");
                StartCoroutine("InternalInjures_Timer");
                InternalInjures_TimerStart = true;
            }
            InternalInjuresFunc();
        }
    }

    public void InternalInjuresFunc()
    {
        if (InternalInjures_Severity >= InternalInjures_SeverityMax)
        {
            InternalInjures_Severity = InternalInjures_SeverityMax;
        }
        else if (InternalInjures_Severity <= InternalInjures_SeverityMin)
        {
            InternalInjures_Severity = InternalInjures_SeverityMin;
            IsInternalInjures = false;
            InternalInjures_TimerStart = false;
            InternalInjures_TimeToRecover = 0;
        }
    }

    IEnumerator InternalInjures_Timer()
    {
        yield return new WaitForSeconds(InternalInjures_TimeToRecover);
        InternalInjures_Severity--;
        InternalInjures_Severity--;
        if (InternalInjures_Severity > InternalInjures_SeverityMin)
        {
            IsInternalInjures = true;
            InternalInjures_TimerStart = false;
        }
        else
        {
            InternalInjures_Severity = InternalInjures_SeverityMin;
            IsInternalInjures = false;
            InternalInjures_TimerStart = false;
            InternalInjures_TimeToRecover = 0;
        }
    }

    // ================= 结痂 =================
    void UpdateScabbedState()
    {
        if (BleedingSeverity > bleedingSeverityMin && BleedingTime == 0)
            IsScabbed = true;
        else if ((BleedingSeverity <= bleedingSeverityMin && BleedingTime == 0) || (BleedingSeverity > bleedingSeverityMin && BleedingTime > 0))
            IsScabbed = false;
    }

    // ================= 兴奋 / 虚弱 =================
    void UpdateExcitementState()
    {
        if (ExcitementSeverity > 1)
        {
            IsExcitement = true;
            IsListless = false;
        }
        else if (ExcitementSeverity < 1)
        {
            IsExcitement = false;
            IsListless = true;
            if (ExcitementSeverity < ListLessSeverityMin)
                ExcitementSeverity = ListLessSeverityMin;
        }
        else if (ExcitementSeverity == ExcitementSeverityMin)
        {
            IsExcitement = false;
            IsListless = false;
        }

        if (IsExcitement || IsListless)
        {
            if (ExcitementStartTimer == false)
            {
                StopCoroutine("ExcitementTimer");
                StartCoroutine("ExcitementTimer");
                ExcitementStartTimer = true;
            }
            Excitement();
        }
    }

    public void Excitement()
    {
        if (IsExcitement == true)
            ExcitementSeverity = 1.8f;
    }

    IEnumerator ExcitementTimer()
    {
        yield return new WaitForSeconds(ExcitementRecoverTime);
        ExcitementSeverity = ExcitementSeverityMin;
        IsExcitement = false;
        IsListless = false;
    }

    // ================= 毒气 =================
    void UpdateGasPoisoningState()
    {
        if (GasPoisoningSeverity > GasPoisoningSeverityMin)
        {
            IsGasPoisoning = true;
        }
        else if (GasPoisoningSeverity <= GasPoisoningSeverityMin)
        {
            IsGasPoisoning = false;
            GasPoisoningSeverity = GasPoisoningSeverityMin;
        }

        if (IsGasPoisoning == true)
            GasPoisoningFunc();
    }

    public void GasPoisoningFunc()
    {
        if (GasPoisoningSeverity > GasPoisoningSeverityMax)
        {
            GasPoisoningSeverity = GasPoisoningSeverityMax;
        }
        else if (GasPoisoningSeverity <= GasPoisoningSeverityMin)
        {
            GasPoisoningSeverity = GasPoisoningSeverityMin;
            IsGasPoisoning = false;
        }

        TheHealthManager.Health = TheHealthManager.Health - 0.005f * GasPoisoningSeverity;
    }
}
