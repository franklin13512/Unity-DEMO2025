using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Staring : MonoBehaviour
{
    public HingeJoint2D Head;
    public JointMotor2D HeadAngle;
    public JointAngleLimits2D AngleLimits;
    public Vector3 SpinPointPos;

    public BoxCollider2D HeadBox;

    public WatchMouse Watch;
    public float TheAngle;
    public bool IsForward;

    public float MaxAngle;
    public float MinAngle;

    // Start is called before the first frame update
    void Start()
    {
        //MaxAngle = Head.limits.max;
        //MinAngle = Head.limits.min;
        MaxAngle = 50;
        MinAngle = -50;

        HeadAngle = Head.motor;
        AngleLimits = Head.limits;
        IsForward = Watch.IsTurned;
    }

    // Update is called once per frame
    void Update()
    {
        IsForward = Watch.IsTurned;
        SpinPointPos = new Vector3(HeadBox.bounds.center.x, HeadBox.bounds.center.y, HeadBox.bounds.center.z);

        if (Input.GetMouseButton(1) && IsForward == false)
        {
            Vector3 V2 = Watch.TheMouseCorodinate - SpinPointPos;
            TheAngle = Mathf.Atan2(V2.y, V2.x) * Mathf.Rad2Deg;

            if (TheAngle > 0)
            {
                if(TheAngle > MaxAngle)
                {
                    TheAngle = MaxAngle;
                }

                AngleLimits.max = -TheAngle;
                if (!Head.useMotor)
                {
                    Head.useMotor = true;
                }
                HeadAngle.motorSpeed = -120;

                Head.limits = AngleLimits;
                Head.motor = HeadAngle;
            }else if (TheAngle < 0)
            {
                if (TheAngle < MinAngle)
                {
                    TheAngle = MinAngle;
                }

                AngleLimits.min = -TheAngle;
                if (!Head.useMotor)
                {
                    Head.useMotor = true;
                }
                HeadAngle.motorSpeed = 120;

                Head.limits = AngleLimits;
                Head.motor = HeadAngle;
            }

        }else if (Input.GetMouseButton(1) && IsForward == true)
        {
            Vector3 V2 = Watch.TheMouseCorodinate - SpinPointPos;
            TheAngle = Mathf.Atan2(V2.y, -V2.x) * Mathf.Rad2Deg ;

            if (TheAngle > 0)
            {
                if (TheAngle > MaxAngle)
                {
                    TheAngle = -MinAngle;
                }

                AngleLimits.max = TheAngle;
                if (!Head.useMotor)
                {
                    Head.useMotor = true;
                }
                HeadAngle.motorSpeed = 120;

                Head.limits = AngleLimits;
                Head.motor = HeadAngle;
            }
            else if (TheAngle < 0)
            {
                if (TheAngle < MinAngle)
                {
                    TheAngle = -MaxAngle;
                }

                AngleLimits.min = TheAngle;
                if (!Head.useMotor)
                {
                    Head.useMotor = true;
                }
                HeadAngle.motorSpeed = -120;

                Head.limits = AngleLimits;
                Head.motor = HeadAngle;
            }
        }
        else
        {
            //AngleLimits.min = MinAngle;
            //AngleLimits.max = MaxAngle;
            AngleLimits.min = 0;
            AngleLimits.max = 0;
            Head.useMotor = false;
            Head.limits = AngleLimits;
        }

    }
}
