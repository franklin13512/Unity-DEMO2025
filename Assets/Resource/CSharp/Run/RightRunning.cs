using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RightRunning : MonoBehaviour
{
    public HingeJoint2D PointLeft;
    public HingeJoint2D PointRight;
    public HingeJoint2D PointSmallLeft;
    public HingeJoint2D PointSmallRight;

    private float HorizontalValue;
    private float HorizontalOPValue;
    public bool Positive;
    public bool PositiveTurn;
    private bool AllUse;

    public LeftRuning LeftRun;
    public WhetherPositive WP;

    //迈步的跨度，越小跨度越大
    public float Step;
    public WatchMouse Watch;
    public bool IsForward;

    //迈步的速度
    public float StepSpeed;

    //小腿motor速度

    // Start is called before the first frame update
    void Start()
    {
        IsForward = Watch.IsTurned;

        PointLeft.useMotor = false;
        PointRight.useMotor = false;
        PointSmallLeft.useMotor = false;
        PointSmallRight.useMotor = false;

        JointMotor2D MotorLeft = PointLeft.motor;
        JointMotor2D MotorRight = PointRight.motor;
        JointMotor2D MotorSmallLeft = PointSmallLeft.motor;
        JointMotor2D MotorSmallRight = PointSmallRight.motor;

        MotorLeft.motorSpeed = 0f;
        MotorRight.motorSpeed = 0f;
        MotorSmallLeft.motorSpeed = 0f;
        MotorSmallRight.motorSpeed = 0f;    

        MotorLeft.maxMotorTorque = 1000f;
        MotorRight.maxMotorTorque = 1000f;
        MotorSmallLeft.maxMotorTorque = 1000f;
        MotorSmallRight.maxMotorTorque = 1000f;

        Positive = WP.IsPositive;
        PositiveTurn = WP.IsPositiveTurn;
        AllUse = WP.AllUse;
    }

    // Update is called once per frame
    void Update()
    {
        IsForward = Watch.IsTurned;
        Positive = WP.IsPositive;
        PositiveTurn = WP.IsPositiveTurn;
        AllUse = WP.AllUse;

        HorizontalValue = Input.GetAxis("Horizontal");

        HorizontalOPValue = Input.GetAxis("HorizontalOP");

        if (!Positive && Input.GetKey(KeyCode.LeftShift) && IsForward == false)
        {
            Running();
        }

        if (!PositiveTurn && Input.GetKey(KeyCode.LeftShift) && IsForward == true)
        {
            RunningTurn();
        }

        if (HorizontalValue <= 0.1 && HorizontalValue >= -0.1 && HorizontalValue <= 0.1 && HorizontalValue >= -0.1)
        {
            WP.AllUse = false;
            PointLeft.useMotor = false;
            PointRight.useMotor = false;
            PointSmallLeft.useMotor = false;
            PointSmallRight.useMotor = false;
        }

    }

    public void Running()
    {
            if (HorizontalValue > 0 && Input.GetKey(KeyCode.LeftShift))
            {
                if (AllUse == false)
                {
                    PointLeft.useMotor = true;
                    PointRight.useMotor = true;
                    PointSmallLeft.useMotor = true;
                    PointSmallRight.useMotor = true;
                    WP.AllUse = true;
                }

                JointMotor2D TheMotorLeft = PointLeft.motor;
                JointMotor2D TheMotorRight = PointRight.motor;
                JointMotor2D TheMotorSmallLeft = PointSmallLeft.motor;
                JointMotor2D TheMotorSmallRight = PointSmallRight.motor;

                TheMotorLeft.motorSpeed = StepSpeed;
                TheMotorRight.motorSpeed = -StepSpeed;
                TheMotorSmallLeft.motorSpeed = 400f;
                TheMotorSmallRight.motorSpeed = -400f;

                PointLeft.motor = TheMotorLeft;
                PointRight.motor = TheMotorRight;
                PointSmallLeft.motor = TheMotorSmallLeft;
                PointSmallRight.motor = TheMotorSmallRight;


                if (PointLeft.jointAngle >= PointLeft.limits.max / Step && PointRight.jointAngle <= PointRight.limits.min / Step && PointSmallLeft.jointAngle >= PointSmallLeft.limits.max && PointSmallRight.jointAngle <= PointSmallRight.limits.min)
                {

                    PointLeft.useMotor = false;
                    PointRight.useMotor = false;
                    PointSmallLeft.useMotor = false;

                    WP.IsPositive = true;

                }
            }
            else
            {
                if (HorizontalValue <= 0.1 && HorizontalValue >= -0.1)
                {
                    WP.AllUse = false;
                    PointLeft.useMotor = false;
                    PointRight.useMotor = false;
                    PointSmallLeft.useMotor = false;
                    PointSmallRight.useMotor = false;

            }
            }
    }

    public void RunningTurn()
    {
        if (HorizontalOPValue > 0 && Input.GetKey(KeyCode.LeftShift))
        {
            if (AllUse == false)
            {
                PointLeft.useMotor = true;
                PointRight.useMotor = true;
                PointSmallLeft.useMotor = true;
                PointSmallRight.useMotor = true;
                WP.AllUse = true;
            }

            JointMotor2D TheMotorLeft = PointLeft.motor;
            JointMotor2D TheMotorRight = PointRight.motor;
            JointMotor2D TheMotorSmallLeft = PointSmallLeft.motor;
            JointMotor2D TheMotorSmallRight = PointSmallRight.motor;

            TheMotorLeft.motorSpeed = -StepSpeed;
            TheMotorRight.motorSpeed = StepSpeed;
            TheMotorSmallLeft.motorSpeed = -400f;
            TheMotorSmallRight.motorSpeed = 400f;

            PointLeft.motor = TheMotorLeft;
            PointRight.motor = TheMotorRight;
            PointSmallLeft.motor = TheMotorSmallLeft;
            PointSmallRight.motor = TheMotorSmallRight;

            if (PointLeft.jointAngle <= PointLeft.limits.min / Step && PointRight.jointAngle >= PointRight.limits.max / Step && PointSmallLeft.jointAngle <= PointSmallLeft.limits.min && PointSmallRight.jointAngle >= PointSmallRight.limits.max)
            {

                PointLeft.useMotor = false;
                PointRight.useMotor = false;
                PointSmallLeft.useMotor = false;
                PointSmallRight.useMotor = false;

                WP.IsPositiveTurn = true;

            }
        }
        else
        {
            if (HorizontalOPValue <= 0.1 && HorizontalOPValue >= -0.1)
            {
                WP.AllUse = false;
                PointLeft.useMotor = false;
                PointRight.useMotor = false;
                PointSmallLeft.useMotor = false;
                PointSmallRight.useMotor = false;

            }
        }
    }

}
