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

    public float StandingForce; //举枪时人会前倾，因此设定一个稍微向后的力

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

            if (MoveValue > 0 && Input.GetKey(KeyCode.LeftShift))
            {
                Rb.velocity = new Vector2(MoveValue * RunSpeed, Rb.velocity.y);
            }else if (MoveValue < 0 && Input.GetKey(KeyCode.LeftShift))
            {
                Rb.velocity = new Vector2(MoveValue * WalkBackSpeed, Rb.velocity.y);
            }

            if (MoveValue > 0 && !Input.GetKey(KeyCode.LeftShift))
            {
                Rb.velocity = new Vector2(MoveValue * WalkSpeed, Rb.velocity.y);
            }
            else if (MoveValue < 0 && !Input.GetKey(KeyCode.LeftShift))
            {
                Rb.velocity = new Vector2(MoveValue * WalkBackSpeed, Rb.velocity.y);
            }

        }
        else if (IsForward == true)
        {
            MoveValueOP = Input.GetAxis("HorizontalOP");

            if (MoveValueOP > 0 && Input.GetKey(KeyCode.LeftShift))
            {
                Rb.velocity = new Vector2(MoveValueOP * -RunSpeed, Rb.velocity.y);
            }
            else if (MoveValueOP < 0 && Input.GetKey(KeyCode.LeftShift))
            {
                Rb.velocity = new Vector2(MoveValueOP * -WalkBackSpeed, Rb.velocity.y);
            }

            if (MoveValueOP > 0 && !Input.GetKey(KeyCode.LeftShift))
            {
                Rb.velocity = new Vector2(MoveValueOP * -WalkSpeed, Rb.velocity.y);
            }
            else if (MoveValueOP < 0 && !Input.GetKey(KeyCode.LeftShift))
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
