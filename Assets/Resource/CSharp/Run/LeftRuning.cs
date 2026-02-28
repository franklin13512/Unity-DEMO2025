
using UnityEngine;

public class LeftRuning : MonoBehaviour
{
    public HingeJoint2D PointLeft;
    public HingeJoint2D PointRight;
    public HingeJoint2D PointSmallRight;
    public HingeJoint2D PointSmallLeft;

    private float HorizontalValue;
    private float HorizontalOPValue;
    public bool Positive;
    public bool PositiveTurn;
    private bool AllUse;

    public RightRunning RightRun;
    public WhetherPositive WP;

    //保存小腿关节的角度
    private float SmallLeftAngleMin;
    private float SmallLeftAngleMax;

    public WatchMouse Watch;

    public float Step;
    public bool IsForward;

    public float StepSpeed;

    // Start is called before the first frame update
    void Start()
    {
        IsForward = Watch.IsTurned;

        PointLeft.useMotor = false;
        PointRight.useMotor = false;
        PointSmallRight.useMotor = false;

        JointMotor2D MotorLeft = PointLeft.motor;
        JointMotor2D MotorRight = PointRight.motor;
        JointMotor2D MotorSmallRight = PointSmallRight.motor; 
        JointMotor2D MotorSmallLeft = PointSmallLeft.motor;

        MotorLeft.motorSpeed = 0f;
        MotorRight.motorSpeed = 0f;
        MotorSmallRight.motorSpeed = 0f;
        MotorSmallLeft.motorSpeed = 0f;

        MotorLeft.maxMotorTorque = 1000f;
        MotorRight.maxMotorTorque = 1000f;
        MotorSmallRight.maxMotorTorque = 1000f;
        MotorSmallLeft.maxMotorTorque = 1000f;

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

        if (Positive && Input.GetKey(KeyCode.LeftShift) && IsForward == false)
        {
            Running();
        }

        if (PositiveTurn && Input.GetKey(KeyCode.LeftShift) && IsForward == true)
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
                    PointSmallRight.useMotor = true;
                    WP.AllUse = true;
                }

                JointMotor2D TheMotorLeft = PointLeft.motor;
                JointMotor2D TheMotorRight = PointRight.motor;
                JointMotor2D TheMotorSmallRight = PointSmallRight.motor;
                JointMotor2D TheMotorSmallLeft = PointSmallLeft.motor;

                TheMotorLeft.motorSpeed = -StepSpeed;
                TheMotorRight.motorSpeed = StepSpeed;
                TheMotorSmallRight.motorSpeed = 400F;
                TheMotorSmallLeft.motorSpeed = -400f;
                
                PointLeft.motor = TheMotorLeft;
                PointRight.motor = TheMotorRight;
                PointSmallRight.motor = TheMotorSmallRight;
                PointSmallLeft.motor = TheMotorSmallLeft;


                if (PointLeft.jointAngle <= PointLeft.limits.min / Step && PointRight.jointAngle >= PointRight.limits.max / Step && PointSmallRight.jointAngle >= PointSmallRight.limits.max && PointSmallLeft.jointAngle <= PointSmallLeft.limits.min)
                {
                    
                    PointLeft.useMotor = false;
                    PointRight.useMotor = false;
                    PointSmallRight.useMotor = false;
                    PointSmallLeft.useMotor = false;

                    WP.IsPositive = false;


                }
            }
            else
            {
                if (HorizontalValue <= 0.1 && HorizontalValue >= -0.1)
                {
                    WP.AllUse = false;
                    PointLeft.useMotor = false;
                    PointRight.useMotor = false;
                    PointSmallRight.useMotor = false;
                    PointSmallLeft.useMotor = false;

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
                PointSmallRight.useMotor = true;
                PointSmallLeft.useMotor = true;
                WP.AllUse = true;
            }


            JointMotor2D TheMotorLeft = PointLeft.motor;
            JointMotor2D TheMotorRight = PointRight.motor;
            JointMotor2D TheMotorSmallRight = PointSmallRight.motor;
            JointMotor2D TheMotorSmallLeft = PointSmallLeft.motor;  

            TheMotorLeft.motorSpeed = StepSpeed;
            TheMotorRight.motorSpeed = -StepSpeed;
            TheMotorSmallRight.motorSpeed = -400f;
            TheMotorSmallLeft.motorSpeed = 400f;

            PointLeft.motor = TheMotorLeft;
            PointRight.motor = TheMotorRight;
            PointSmallRight.motor = TheMotorSmallRight;
            PointSmallLeft.motor = TheMotorSmallLeft;

            if (PointLeft.jointAngle >= PointLeft.limits.max / Step && PointRight.jointAngle <= PointRight.limits.min / Step && PointSmallRight.jointAngle <= PointSmallRight.limits.min && PointSmallLeft.jointAngle >= PointSmallLeft.limits.max)
            {

                PointLeft.useMotor = false;
                PointRight.useMotor = false;
                PointSmallRight.useMotor = false;
                PointSmallLeft.useMotor = false;

                WP.IsPositiveTurn = false;

            }
        }
        else
        {
            if (HorizontalOPValue <= 0.1 && HorizontalOPValue >= -0.1)
            {
                WP.AllUse = false;
                PointLeft.useMotor = false;
                PointRight.useMotor = false;
                PointSmallRight.useMotor = false;
                PointSmallLeft.useMotor = false;

            }
        }
    }


}

