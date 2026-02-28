using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RightWalking : MonoBehaviour
{
    public HingeJoint2D PointLeft;
    public HingeJoint2D PointRight;
    public HingeJoint2D PointSmallLeft;
    public HingeJoint2D PointSmallRight;

    private float HorizontalValue;
    private float HorizontalOPValue;

    public bool Positive;
    public bool Negative;
    public bool PositiveTurn;
    public bool NegativeTurn;

    private bool AllUse;
    private bool AllUse_Walk;

    public LeftWalking LeftWalk;
    public WhetherPo WP;

    public WatchMouse Watch;
    public bool IsForward;

    public float StepSpeed;

    public float Step;

    public float MinusValue;

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

        MotorSmallLeft.maxMotorTorque = 1000f;
        MotorSmallRight.maxMotorTorque = 1000f;
        MotorLeft.maxMotorTorque = 1000f;
        MotorRight.maxMotorTorque = 1000f;

        Positive = WP.IsPositive;
        Negative = WP.IsNegative;
        PositiveTurn = WP.IsPositiveTurn;
        NegativeTurn = WP.IsNegativeTurn;

        AllUse = WP.AllUse;
        AllUse_Walk = WP.AllUse_Walk;
    }

    // Update is called once per frame
    void Update()
    {
        Positive = WP.IsPositive;
        Negative = WP.IsNegative;
        PositiveTurn = WP.IsPositiveTurn;
        NegativeTurn = WP.IsNegativeTurn;

        AllUse = WP.AllUse;
        AllUse_Walk = WP.AllUse_Walk;
        IsForward = Watch.IsTurned;


            HorizontalValue = Input.GetAxis("Horizontal");

            HorizontalOPValue = Input.GetAxis("HorizontalOP");

        if ((!Positive   && !Input.GetKey(KeyCode.LeftShift)) && IsForward == false && HorizontalValue > 0)
        {
            WalkingForWard();
        }
        
        if ((!Negative  && !Input.GetKey(KeyCode.LeftShift)) && IsForward == false && HorizontalValue < 0)
        {
            WalkBack();
        }

        if ((!PositiveTurn && !Input.GetKey(KeyCode.LeftShift)) && IsForward == true && HorizontalOPValue > 0)
        {
            ForWardTurn();
        }

        if ((!NegativeTurn && !Input.GetKey(KeyCode.LeftShift)) && IsForward == true && HorizontalOPValue < 0)
        {
            WalkBackTurn();
        }

    }

    public void WalkingForWard()
    {
        if (HorizontalValue > 0 && IsForward == false)
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

            if (PointLeft.jointAngle >= PointLeft.limits.max / Step && PointRight.jointAngle <= PointRight.limits.min / Step )
            {

                PointLeft.useMotor = false;
                PointRight.useMotor = false;
                PointSmallLeft.useMotor = false;
                PointSmallRight.useMotor = false;

                WP.IsPositive = true;
            }
        }
        else
        {

            if (HorizontalValue <= 0.1 && HorizontalValue >= -0.1)
            {
                WP.AllUse = false;
                WP.AllUse_Walk = false;
                PointLeft.useMotor = false;
                PointRight.useMotor = false;
                PointSmallLeft.useMotor = false;
                PointSmallRight.useMotor = false;
            }
        }
    }

    public void ForWardTurn()
    {
        if (HorizontalOPValue > 0 && IsForward == true)
        {

            if (AllUse == false)
            {
                PointLeft.useMotor = true;
                PointRight.useMotor = true;
                PointSmallLeft.useMotor = true;
                if (Watch.HandIsUp == false && !Input.GetMouseButton(1) && Watch.HandIsUp2 == false)
                {
                }
                WP.AllUse = true;
            }

            JointMotor2D TheMotorLeft = PointLeft.motor;
            JointMotor2D TheMotorRight = PointRight.motor;
            JointMotor2D TheMotorSmallLeft = PointSmallLeft.motor;
            JointMotor2D TheMotorSmallRight = PointSmallRight.motor;
            TheMotorLeft.motorSpeed = -StepSpeed;
            TheMotorRight.motorSpeed = StepSpeed;
            TheMotorSmallLeft.motorSpeed = -400F;
            TheMotorSmallRight.motorSpeed = 400F;

            PointLeft.motor = TheMotorLeft;
            PointRight.motor = TheMotorRight;
            PointSmallLeft.motor = TheMotorSmallLeft;
            PointSmallRight.motor = TheMotorSmallRight;

            if (PointLeft.jointAngle <= PointLeft.limits.min / Step && PointRight.jointAngle >= PointRight.limits.max / Step )
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
                WP.AllUse_Walk = false;
                PointLeft.useMotor = false;
                PointRight.useMotor = false;
                PointSmallLeft.useMotor = false;
                PointSmallRight.useMotor = false;
            }
        }
    }


    public void WalkBack()
    {
       if (HorizontalValue < 0 && IsForward == false)
        {
            if (AllUse_Walk == false)
            {
                PointLeft.useMotor = true;
                PointRight.useMotor = true;
                WP.AllUse_Walk = true;
            }

            JointMotor2D TheMotorLeft = PointLeft.motor;
            JointMotor2D TheMotorRight = PointRight.motor;
            TheMotorLeft.motorSpeed = -StepSpeed / 2;
            TheMotorRight.motorSpeed = StepSpeed / 2;

            PointLeft.motor = TheMotorLeft;
            PointRight.motor = TheMotorRight;

            if (PointLeft.jointAngle <= PointLeft.limits.min / Step  && PointRight.jointAngle >= PointRight.limits.max / Step)
            {

                PointLeft.useMotor = false;
                PointRight.useMotor = false;

                WP.IsNegative = true;

            }
        }
        else
        {

            if (HorizontalValue <= 0.1 && HorizontalValue >= -0.1)
            {
                WP.AllUse = false;
                WP.AllUse_Walk = false;
                PointLeft.useMotor = false;
                PointRight.useMotor = false;
                PointSmallLeft.useMotor = false;
                PointSmallRight.useMotor = false;
            }
        }
    }


    public void WalkBackTurn()
    {
        if (HorizontalOPValue < 0 && IsForward == true)
        {
            if (AllUse_Walk == false)
            {
                PointLeft.useMotor = true;
                PointRight.useMotor = true;
                WP.AllUse_Walk = true;
                if (Watch.HandIsUp == false && !Input.GetMouseButton(1) && Watch.HandIsUp2 == false)
                {
                }
            }

            JointMotor2D TheMotorLeft = PointLeft.motor;
            JointMotor2D TheMotorRight = PointRight.motor;
            TheMotorLeft.motorSpeed = StepSpeed / 2;
            TheMotorRight.motorSpeed = -StepSpeed / 2;

            PointLeft.motor = TheMotorLeft;
            PointRight.motor = TheMotorRight;

            if (PointLeft.jointAngle >= PointLeft.limits.max / Step && PointRight.jointAngle <= PointRight.limits.min / Step)
            {

                PointLeft.useMotor = false;
                PointRight.useMotor = false;

                WP.IsNegativeTurn = true;

            }
        }
        else
        {
            if (Watch.HandIsUp == false && !Input.GetMouseButton(1) && Watch.HandIsUp2 == false)
            {
            }

            if (HorizontalOPValue <= 0.1 && HorizontalOPValue >= -0.1)
            {
                WP.AllUse = false;
                WP.AllUse_Walk = false;
                PointLeft.useMotor = false;
                PointRight.useMotor = false;
                PointSmallLeft.useMotor = false;
                PointSmallRight.useMotor = false;
            }
        }
    }
}
