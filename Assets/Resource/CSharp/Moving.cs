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

    public float StandingForce; // 端枪时人会前倾，这里设定一个微小的力

    public WatchMouse Watch;
    public bool IsForward;

    public bool EnableFreeze;
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

        bool crouching = Input.GetKey(KeyCode.LeftControl);
        float crouchSpeed = CrouchSpeed > 0f ? CrouchSpeed : WalkSpeed * 0.6f;

        //if (MoveValue == 0 && MoveValueOP == 0 && TheRunAim.OntheGround == true && Input.GetMouseButton(1) && !Input.GetMouseButton(0))
        //{
        //    Rb.velocity = new Vector2(0,0);
        //    Rb.angularVelocity = 0;
        //    Rb.bodyType = RigidbodyType2D.Kinematic;
        //}

        //Debug.Log(Rb.velocity.magnitude);

        if (IsForward == false)
        {
            MoveValue = Input.GetAxis("Horizontal");

            if (crouching)
            {
                if (MoveValue != 0)
                    Rb.velocity = new Vector2(MoveValue * crouchSpeed, Rb.velocity.y);
            }
            else if (MoveValue > 0 && Input.GetKey(KeyCode.LeftShift))
            {
                Rb.velocity = new Vector2(MoveValue * RunSpeed, Rb.velocity.y);
            }
            else if (MoveValue < 0 && Input.GetKey(KeyCode.LeftShift))
            {
                Rb.velocity = new Vector2(MoveValue * WalkBackSpeed, Rb.velocity.y);
            }
            else if (MoveValue > 0)
            {
                Rb.velocity = new Vector2(MoveValue * WalkSpeed, Rb.velocity.y);
            }
            else if (MoveValue < 0)
            {
                Rb.velocity = new Vector2(MoveValue * WalkBackSpeed, Rb.velocity.y);
            }

        }
        else if (IsForward == true)
        {
            MoveValueOP = Input.GetAxis("HorizontalOP");

            if (crouching)
            {
                if (MoveValueOP != 0)
                    Rb.velocity = new Vector2(MoveValueOP * -crouchSpeed, Rb.velocity.y);
            }
            else if (MoveValueOP > 0 && Input.GetKey(KeyCode.LeftShift))
            {
                Rb.velocity = new Vector2(MoveValueOP * -RunSpeed, Rb.velocity.y);
            }
            else if (MoveValueOP < 0 && Input.GetKey(KeyCode.LeftShift))
            {
                Rb.velocity = new Vector2(MoveValueOP * -WalkBackSpeed, Rb.velocity.y);
            }
            else if (MoveValueOP > 0)
            {
                Rb.velocity = new Vector2(MoveValueOP * -WalkSpeed, Rb.velocity.y);
            }
            else if (MoveValueOP < 0)
            {
                Rb.velocity = new Vector2(MoveValueOP * -WalkBackSpeed, Rb.velocity.y);
            }

        }

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
