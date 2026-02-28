using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class HandBigGunUpLeft : Player
{
    public HingeJoint2D Hand;
    public JointMotor2D HandAngle;
    public JointAngleLimits2D AngleLimits;

    public HingeJoint2D SmallHand;
    //public JointMotor2D SmallHandAngle;
    public JointAngleLimits2D SmallAngleLimits;

    public Vector3 SpinPointPos;

    public BoxCollider2D HeadBox;

    public Standing StandingComponent;
    public Standing StandingComponent2;

    public float TheAngle;
    public float PreTheAngle;

    public bool IsForward;

    public float MaxAngle;
    public float MinAngle;

    public float LeftMaxAngle;
    public float LeftMinAngle;

    public GetSmallGun TheGetSmallGun;
    public GetBigGun TheGetBigGun;

    public float UpValue;

    // Start is called before the first frame update
    void Start()
    {
        MaxAngle = Hand.limits.max;
        MinAngle = Hand.limits.min;

        LeftMaxAngle = SmallHand.limits.max;
        LeftMinAngle = SmallHand.limits.min;

        HandAngle = Hand.motor;
        //SmallHandAngle = SmallHand.motor;

        AngleLimits = Hand.limits;
        SmallAngleLimits = SmallHand.limits;

        IsForward = Watch.IsTurned;
    }

    // Update is called once per frame
    void Update()
    {
        IsForward = Watch.IsTurned;
        SpinPointPos = new Vector3(HeadBox.bounds.center.x + UpValue , HeadBox.bounds.center.y, HeadBox.bounds.center.z);

        HandBigGunUp2();
    }

    public void HandBigGunUp2()
    {
        if (!Input.GetMouseButton(1) && TheGetBigGun.Equipeed == true && IsForward == true)
        {

            Watch.HandIsUp2 = true;

            StandingComponent.enabled = false;
            StandingComponent2.enabled = false;

            PreTheAngle = -45;
            TheAngle = PreTheAngle + 50;

            if (TheAngle > 0)
            {
                if (TheAngle > MaxAngle)
                {
                    TheAngle = MaxAngle;
                }

                AngleLimits.max = MaxAngle;
                AngleLimits.min = TheAngle;

                SmallAngleLimits.max = -LeftMinAngle;
                SmallAngleLimits.min = -LeftMinAngle;

                if (!Hand.useMotor)
                {
                    Hand.useMotor = true;
                    //SmallHand.useMotor = true;
                }

                HandAngle.motorSpeed = -180;
                SmallHand.useMotor = false;
                //SmallHandAngle.motorSpeed = 200;

                Hand.limits = AngleLimits;
                SmallHand.limits = SmallAngleLimits;

                Hand.motor = HandAngle;
                //SmallHand.motor = SmallHandAngle;
            }
        }else if (Input.GetMouseButton(1) && IsForward == false && TheGetBigGun.Equipeed == true)
        {
            Watch.HandIsUp2 = true;


            StandingComponent.enabled = false;
            StandingComponent2.enabled = false;

            Vector3 V2 = Watch.TheMouseCorodinate - SpinPointPos;
            PreTheAngle = Mathf.Atan2(V2.y, V2.x) * Mathf.Rad2Deg;
            TheAngle = PreTheAngle + 50;

            if (PreTheAngle > 0)
            {
                if (TheAngle > MaxAngle)
                {
                    TheAngle = MaxAngle;
                }

                AngleLimits.min = -TheAngle;

                SmallAngleLimits.max = LeftMinAngle;
                SmallAngleLimits.min = LeftMinAngle;
                if (!Hand.useMotor)
                {
                    Hand.useMotor = true;
                    //SmallHand.useMotor = true;
                }
                HandAngle.motorSpeed = -180;
                SmallHand.useMotor = false;
                //SmallHandAngle.motorSpeed = -200;

                Hand.limits = AngleLimits;
                SmallHand.limits = SmallAngleLimits;

                Hand.motor = HandAngle;
                //SmallHand.motor = SmallHandAngle;
            }
            else if (PreTheAngle < 0)
            {
                if (TheAngle < MinAngle)
                {
                    TheAngle = MinAngle;
                }

                AngleLimits.max = -TheAngle;

                SmallAngleLimits.max = LeftMinAngle;
                SmallAngleLimits.min = LeftMinAngle;
                if (!Hand.useMotor)
                {
                    Hand.useMotor = true;
                }
                HandAngle.motorSpeed = 180;
                SmallHand.useMotor = false;
                //SmallHandAngle.motorSpeed = -200;

                Hand.limits = AngleLimits;
                SmallHand.limits = SmallAngleLimits;

                Hand.motor = HandAngle;
                //SmallHand.motor = SmallHandAngle;
            }

        }
        else if (Input.GetMouseButton(1) && IsForward == true && TheGetBigGun.Equipeed == true)
        {
            Watch.HandIsUp2 = true;

            StandingComponent.enabled = false;
            StandingComponent2.enabled = false;

            Vector3 V2 = Watch.TheMouseCorodinate - SpinPointPos;
            PreTheAngle = Mathf.Atan2(V2.y, -V2.x) * Mathf.Rad2Deg;
            TheAngle = PreTheAngle + 50;

            if (PreTheAngle > 0)
            {
                if (TheAngle > MaxAngle)
                {
                    TheAngle = -MinAngle;
                }

                AngleLimits.max = TheAngle;

                SmallAngleLimits.min = 55;
                SmallAngleLimits.max = 55;
                if (!Hand.useMotor)
                {
                    Hand.useMotor = true;
                    //SmallHand.useMotor = true;
                }
                HandAngle.motorSpeed = 180;
                SmallHand.useMotor = false;
                //SmallHandAngle.motorSpeed = 200;

                Hand.limits = AngleLimits;
                SmallHand.limits = SmallAngleLimits;

                Hand.motor = HandAngle;
                //SmallHand.motor = SmallHandAngle;
            }
            else if (PreTheAngle < 0)
            {
                if (TheAngle < MinAngle)
                {
                    TheAngle = -MaxAngle;
                }

                AngleLimits.min = TheAngle;

                SmallAngleLimits.min = 55;
                SmallAngleLimits.max = 55;

                if (!Hand.useMotor)
                {
                    Hand.useMotor = true;
                    //SmallHand.useMotor = true;
                }
                HandAngle.motorSpeed = -180;
                SmallHand.useMotor = false;
                //SmallHandAngle.motorSpeed = 200;

                Hand.limits = AngleLimits;
                SmallHand.limits = SmallAngleLimits;

                Hand.motor = HandAngle;
                //SmallHand.motor = SmallHandAngle;
            }
        }else if(TheGetBigGun.Equipeed == true && IsForward == false)
        {
            Watch.HandIsUp2 = true;

            StandingComponent.enabled = false;
            StandingComponent2.enabled = false;

            PreTheAngle = -45;
            TheAngle = PreTheAngle + 50;

            if (TheAngle > 0)
            {
                if (TheAngle > MaxAngle)
                {
                    TheAngle = MaxAngle;
                }

                AngleLimits.max = -TheAngle;
                AngleLimits.min = MinAngle;

                SmallAngleLimits.max = LeftMinAngle;
                SmallAngleLimits.min = LeftMinAngle;
                if (!Hand.useMotor)
                {
                    Hand.useMotor = true;
                    //SmallHand.useMotor = true;
                }
                HandAngle.motorSpeed = 180;
                SmallHand.useMotor = false;
                //SmallHandAngle.motorSpeed = -200;

                Hand.limits = AngleLimits;
                SmallHand.limits = SmallAngleLimits;

                Hand.motor = HandAngle;
                //SmallHand.motor = SmallHandAngle;
            }
            
        }
        else
        {
            if (!Input.GetMouseButton(1) && TheGetBigGun.Equipeed == false)
            {
                Watch.HandIsUp2 = false;
            }

            if (Watch.HandIsUp2 == false)
            {
                //StandingComponent.enabled = true;
                //StandingComponent2.enabled = true;

                if (IsForward == false)
                {
                    SmallAngleLimits.min = LeftMinAngle;
                    SmallAngleLimits.max = LeftMaxAngle;
                }
                else if (IsForward == true)
                {
                    SmallAngleLimits.min = LeftMaxAngle;
                    SmallAngleLimits.max = -LeftMinAngle;
                }

                AngleLimits.min = MinAngle;

                AngleLimits.max = MaxAngle;

                Hand.useMotor = false;
                SmallHand.useMotor = false;

                Hand.limits = AngleLimits;
                SmallHand.limits = SmallAngleLimits;
            }
        }
    }
}
