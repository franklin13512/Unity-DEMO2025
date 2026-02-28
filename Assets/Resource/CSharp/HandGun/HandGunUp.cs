using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandGunUp : Player
{
    public HingeJoint2D Hand;
    public JointMotor2D HandAngle;
    public JointAngleLimits2D AngleLimits;
    public Vector3 SpinPointPos;

    public HingeJoint2D SmallHand;
    public JointAngleLimits2D SmallAngleLimits;

    public BoxCollider2D HeadBox;

    public Standing StandingComponent;
    public Standing StandingComponent2;

    public float TheAngle;
    public float PreTheAngle;

    public bool IsForward;

    public float MaxAngle;
    public float MinAngle;

    public float SMaxAngle;
    public float SMinAngle;

    public GetSmallGun TheGetSmallGun;
    public GetBigGun TheGetBigGun;

    // Start is called before the first frame update
    void Start()
    {
        MaxAngle = Hand.limits.max;
        MinAngle = Hand.limits.min;

        SMaxAngle = -55;
        SMinAngle = 0;

        HandAngle = Hand.motor;
        AngleLimits = Hand.limits;
        SmallAngleLimits = SmallHand.limits;
        IsForward = Watch.IsTurned;
    }

    // Update is called once per frame
    void Update()
    {
        IsForward = Watch.IsTurned;
        SpinPointPos = new Vector3(HeadBox.bounds.center.x, HeadBox.bounds.center.y, HeadBox.bounds.center.z);

            HandSmallGunUp();
    }

    public void HandSmallGunUp()
    {
        if (Input.GetMouseButton(1) && IsForward == false && TheGetSmallGun.Equipeed == true)
        {
            Watch.HandIsUp = true;

            StandingComponent.enabled = false;
            StandingComponent2.enabled = false;

            Vector3 V2 = Watch.TheMouseCorodinate - SpinPointPos;
            PreTheAngle = Mathf.Atan2(V2.y, V2.x) * Mathf.Rad2Deg;
            TheAngle = PreTheAngle + 90;

            if (TheAngle > 0)
            {
                if (TheAngle > MaxAngle)
                {
                    TheAngle = MaxAngle;
                }

                AngleLimits.min = -TheAngle;
                if (!Hand.useMotor)
                {
                    Hand.useMotor = true;
                }
                HandAngle.motorSpeed = -180;

                Hand.limits = AngleLimits;
                Hand.motor = HandAngle;
            }
            else if (TheAngle < 0)
            {
                if (TheAngle < MinAngle)
                {
                    TheAngle = MinAngle;
                }

                AngleLimits.max = -TheAngle;
                if (!Hand.useMotor)
                {
                    Hand.useMotor = true;
                }
                HandAngle.motorSpeed = 180;

                Hand.limits = AngleLimits;
                Hand.motor = HandAngle;
            }

            SmallAngleLimits.min = SMinAngle;
            SmallAngleLimits.max = SMinAngle;

            SmallHand.limits = SmallAngleLimits;

        }
        else if (Input.GetMouseButton(1) && IsForward == true && TheGetSmallGun.Equipeed == true)
        {
            Watch.HandIsUp = true;

            StandingComponent.enabled = false;
            StandingComponent2.enabled = false;

            Vector3 V2 = Watch.TheMouseCorodinate - SpinPointPos;
            PreTheAngle = Mathf.Atan2(V2.y, -V2.x) * Mathf.Rad2Deg;
            TheAngle = PreTheAngle + 90;

            if (TheAngle > 0)
            {
                if (TheAngle > MaxAngle)
                {
                    TheAngle = -MinAngle;
                }

                AngleLimits.max = TheAngle;
                if (!Hand.useMotor)
                {
                    Hand.useMotor = true;
                }
                HandAngle.motorSpeed = 180;

                Hand.limits = AngleLimits;
                Hand.motor = HandAngle;
            }
            else if (TheAngle < 0)
            {
                if (TheAngle < MinAngle)
                {
                    TheAngle = -MaxAngle;
                }

                AngleLimits.min = TheAngle;
                if (!Hand.useMotor)
                {
                    Hand.useMotor = true;
                }
                HandAngle.motorSpeed = -180;

                Hand.limits = AngleLimits;
                Hand.motor = HandAngle;
            }

            SmallAngleLimits.min = SMinAngle ;
            SmallAngleLimits.max = SMinAngle;

            SmallHand.limits = SmallAngleLimits;
        }
        else if(Watch.HandIsUp == false)
        {
            if (!Input.GetMouseButton(1))
            {
                Watch.HandIsUp = false;
            }

            if (Watch.HandIsUp == false)
            {
                StandingComponent.enabled = true;
                StandingComponent2.enabled = true;

                AngleLimits.min = MinAngle;
                AngleLimits.max = MaxAngle;

                if(Watch.IsTurned == false)
                {
                    SmallAngleLimits.min = SMaxAngle;
                    SmallAngleLimits.max = SMinAngle;
                }else if(Watch.IsTurned == true)
                {
                    SmallAngleLimits.min = SMinAngle;
                    SmallAngleLimits.max = -SMaxAngle;
                }


                Hand.useMotor = false;
                Hand.limits = AngleLimits;
                SmallHand.limits = SmallAngleLimits;
            }
        }
    }
}
