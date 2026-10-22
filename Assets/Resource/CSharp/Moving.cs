using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Moving : MonoBehaviour
{
    public Rigidbody2D Rb;
    private float MoveValue;
    private float MoveValueOP;

    //public RunAim TheRunAim;

    public float RunSpeed;
    public float WalkSpeed;
    public float WalkBackSpeed;
    public float CrouchSpeed; // 下蹲行走速度（<=0 时自动取 WalkSpeed*0.6）
    [Tooltip("下蹲功能总开关（与 ProceduralIKWalker.enableCrouch 保持一致）")]
    public bool enableCrouch = true;

    public float StandingForce; // 端枪时人会前倾，这里设定一个微小的力

    public WatchMouse Watch;
    public bool IsForward;

    public bool EnableFreeze;

    [Header("爬坡")]
    [Tooltip("地面所在层（默认 Default 层，value=1）")]
    public LayerMask groundMask = 1;
    [Tooltip("求坡度时左右采样点距身体中线的距离")]
    public float slopeSampleHalfWidth = 0.15f;
    [Tooltip("向下射线的探测长度")]
    public float slopeRayLength = 2f;
    [Tooltip("脚下射线命中距离小于此值才认为站在地面上（否则视为腾空，交给重力）")]
    public float maxGroundDistance = 0.9f;
    [Tooltip("坡度钳制（1 = 约 45°），防止阶梯/悬崖边缘产生离谱斜率")]
    public float maxSlopeTan = 1f;
    [Tooltip("坡度平滑速度，越大跟随越快、越小越平滑")]
    public float slopeSmooth = 15f;
    [Tooltip("爬坡增强乘区：上坡时速度 = 原速度 × (1 + climbBoost × 坡度角 / maxClimbAngle)")]
    public float climbBoost = 1f;
    [Tooltip("爬坡增强的最大角度（度）：超过此角度加成失效，且不再沿坡移动（爬不上去）")]
    public float maxClimbAngle = 40f;

    private float currentSlope = 0f;

    // Start is called before the first frame update
    void Start()
    {
        IsForward = Watch.IsTurned;

        //TheRunAim = gameObject.GetComponentInParent<RunAim>();

        Rb = gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        IsForward = Watch.IsTurned;

        bool crouching = enableCrouch && Input.GetKey(KeyCode.LeftControl);
        float crouchSpeed = CrouchSpeed > 0f ? CrouchSpeed : WalkSpeed * 0.6f;

        // 取水平输入（朝向不同用不同的轴）
        float axis = (IsForward == false) ? Input.GetAxis("Horizontal") : Input.GetAxis("HorizontalOP");

        if (IsForward == false) MoveValue = axis;
        else MoveValueOP = axis;

        // 没有输入就不改变速度（保持原行为，让摩擦把身体停下）
        if (Mathf.Abs(axis) <= 0.01f)
        {
            currentSlope = Mathf.Lerp(currentSlope, 0f, Time.deltaTime * slopeSmooth);
            return;
        }

        // 根据状态选择速度
        float speed;
        if (crouching) speed = crouchSpeed;
        else if (axis > 0f) speed = Input.GetKey(KeyCode.LeftShift) ? RunSpeed : WalkSpeed;
        else speed = WalkBackSpeed;

        // 朝向决定水平方向（转身时速度反向）
        float vx = axis * speed * ((IsForward == false) ? 1f : -1f);

        // 采样脚下坡度，并让身体沿坡面移动
        bool onGround;
        float slope = SampleSlope(out onGround);

        if (onGround)
        {
            float vy = vx * slope;
            float angle = Mathf.Atan(Mathf.Abs(slope)) * Mathf.Rad2Deg;

            if (angle > maxClimbAngle)
            {
                // 太陡：不再沿坡移动，交给重力（爬不上去）
                Rb.velocity = new Vector2(vx, Rb.velocity.y);
            }
            else
            {
                // 上坡才增强：坡度越大加成越大；下坡不加成
                float boost = (vy > 0f && maxClimbAngle > 0.01f)
                    ? 1f + climbBoost * (angle / maxClimbAngle)
                    : 1f;
                Rb.velocity = new Vector2(vx, vy) * boost;
            }
        }
        else
        {
            // 腾空：保留原有竖直速度，交给重力
            Rb.velocity = new Vector2(vx, Rb.velocity.y);
        }
    }

    /// <summary>
    /// 用身体下方“左/中/右”三个点求坡度（dy/dx）。
    /// 左右两点都命中时用两者求；只命中一侧时用中心点配合该侧求。
    /// </summary>
    float SampleSlope(out bool onGround)
    {
        onGround = false;

        Vector2 origin = Rb != null ? Rb.position : (Vector2)transform.position;
        float half = slopeSampleHalfWidth;

        RaycastHit2D hitC = Physics2D.Raycast(origin, Vector2.down, slopeRayLength, groundMask);
        if (hitC.collider == null || hitC.distance > maxGroundDistance)
        {
            // 脚下没有地面，或离地面太远 → 视为腾空
            currentSlope = Mathf.Lerp(currentSlope, 0f, Time.deltaTime * slopeSmooth);
            return 0f;
        }

        onGround = true;

        RaycastHit2D hitL = Physics2D.Raycast(origin + new Vector2(-half, 0f), Vector2.down, slopeRayLength, groundMask);
        RaycastHit2D hitR = Physics2D.Raycast(origin + new Vector2(half, 0f), Vector2.down, slopeRayLength, groundMask);

        float slope = 0f;
        if (hitL.collider != null && hitR.collider != null)
        {
            float dx = hitR.point.x - hitL.point.x;
            if (Mathf.Abs(dx) > 0.0001f)
                slope = (hitR.point.y - hitL.point.y) / dx;
        }
        else if (hitR.collider != null)
        {
            float dx = hitR.point.x - hitC.point.x;
            if (Mathf.Abs(dx) > 0.0001f)
                slope = (hitR.point.y - hitC.point.y) / dx;
        }
        else if (hitL.collider != null)
        {
            float dx = hitC.point.x - hitL.point.x;
            if (Mathf.Abs(dx) > 0.0001f)
                slope = (hitC.point.y - hitL.point.y) / dx;
        }

        slope = Mathf.Clamp(slope, -maxSlopeTan, maxSlopeTan);
        currentSlope = Mathf.Lerp(currentSlope, slope, Time.deltaTime * slopeSmooth);
        return currentSlope;
    }

    //private void LateUpdate()
    //{
    //    if (EnableFreeze)
    //    {
    //        if (TheRunAim.OntheGround == true && TheRunAim.TheHealth.Health > 0 && !TheRunAim.TheStatus.IsCollapse && (!Input.GetMouseButton(1) || !Input.GetMouseButton(0)))
    //        {
    //            Rb.velocity = new Vector2(0, 0);
    //            Rb.angularVelocity = 0;
    //            if (Rb.bodyType == RigidbodyType2D.Dynamic)
    //                Rb.bodyType = RigidbodyType2D.Kinematic;
    //        }
    //        else if ((Input.GetMouseButton(1) && Input.GetMouseButton(0)) || (MoveValueOP != 0 || MoveValue != 0) || TheRunAim.OntheGround == false || TheRunAim.TheStatus.IsCollapse || TheRunAim.TheHealth.Health <= 0)
    //        {
    //            if (Rb.bodyType == RigidbodyType2D.Kinematic)
    //                Rb.bodyType = RigidbodyType2D.Dynamic;
    //        }
    //    }

    //}
}
