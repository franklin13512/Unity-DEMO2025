using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class HandBigGunUpRight : MonoBehaviour
{
    [Tooltip("鼠标朝向组件（原来从 Player 继承）")]
    public WatchMouse Watch;

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

    //public BackWardControler Backing;

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

            HandBigGunUp();
    }

    public void HandBigGunUp()
    {
        if (Input.GetMouseButton(1) && IsForward == false && TheGetBigGun.Equipeed == true /*&& Backing.StartBackward == false*/)
        {
            Watch.HandIsUp = true;

            StandingComponent.enabled = false;
            StandingComponent2.enabled = false;

            Vector3 V2 = Watch.TheMouseCorodinate - SpinPointPos;
            PreTheAngle = Mathf.Atan2(V2.y, V2.x) * Mathf.Rad2Deg;
            TheAngle = PreTheAngle + 10;

            if (TheAngle > 0)
            {
                if (TheAngle > MaxAngle)
                {
                    TheAngle = MaxAngle;
                }

                AngleLimits.max = -TheAngle;

                SmallAngleLimits.max = LeftMaxAngle;
                SmallAngleLimits.min = LeftMinAngle;
                SmallAngleLimits.min = -105;
                SmallAngleLimits.max = -105;
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
            else if (TheAngle < 0)
            {
                if (TheAngle < MinAngle)
                {
                    TheAngle = MinAngle;
                }

                AngleLimits.min = -TheAngle;

                SmallAngleLimits.max = LeftMaxAngle;
                SmallAngleLimits.min = LeftMinAngle;
                SmallAngleLimits.min = -105;
                SmallAngleLimits.max = -105;
                if (!Hand.useMotor)
                {
                    Hand.useMotor = true;
                    //SmallHand.useMotor = true;
                }
                HandAngle.motorSpeed = 180;
                SmallHand.useMotor = false;
                //SmallHandAngle.motorSpeed= -200;

                Hand.limits = AngleLimits;
                SmallHand.limits = SmallAngleLimits;

                Hand.motor = HandAngle;
                //SmallHand.motor = SmallHandAngle;
            }

        }
        else if (Input.GetMouseButton(1) && IsForward == true && TheGetBigGun.Equipeed == true /*&& Backing.StartBackward == false*/)
        {
            Watch.HandIsUp = true;

            StandingComponent.enabled = false;
            StandingComponent2.enabled = false;

            Vector3 V2 = Watch.TheMouseCorodinate - SpinPointPos;
            PreTheAngle = Mathf.Atan2(V2.y, -V2.x) * Mathf.Rad2Deg;
            TheAngle = PreTheAngle + 10;

            if (TheAngle > 0)
            {
                if (TheAngle > MaxAngle)
                {
                    TheAngle = -MinAngle;
                }

                AngleLimits.min = TheAngle;

                SmallAngleLimits.min = LeftMaxAngle;
                SmallAngleLimits.max = -LeftMinAngle;
                SmallAngleLimits.max = 105;
                SmallAngleLimits.min = 105;
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
            else if (TheAngle < 0)
            {
                if (TheAngle < MinAngle)
                {
                    TheAngle = -MaxAngle;
                }

                AngleLimits.max = TheAngle;

                SmallAngleLimits.min = LeftMaxAngle;
                SmallAngleLimits.max = -LeftMinAngle;
                SmallAngleLimits.max = 105;
                SmallAngleLimits.min = 105;

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
        }else if (TheGetBigGun.Equipeed == true && IsForward == false)
        {
            Watch.HandIsUp = true;

            StandingComponent.enabled = false;
            StandingComponent2.enabled = false;

            //Vector3 V2 = Watch.TheMouseCorodinate - SpinPointPos;
            PreTheAngle = -45;
            TheAngle = PreTheAngle + 10;


            if (TheAngle < 0)
            {
                if (TheAngle < MinAngle)
                {
                    TheAngle = MinAngle;
                }

                AngleLimits.max = TheAngle;
                AngleLimits.min = -TheAngle;

                SmallAngleLimits.max = LeftMaxAngle;
                SmallAngleLimits.min = LeftMinAngle;
                SmallAngleLimits.min = -105;
                SmallAngleLimits.max = -105;
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
        else if (TheGetBigGun.Equipeed == true && IsForward == true)
        {

            Watch.HandIsUp = true;

            StandingComponent.enabled = false;
            StandingComponent2.enabled = false;

            //Vector3 V2 = Watch.TheMouseCorodinate - SpinPointPos;
            PreTheAngle = -45;
            TheAngle = PreTheAngle + 10;

            if (TheAngle < 0)
            {
                if (TheAngle < MinAngle)
                {
                    TheAngle = MinAngle;
                }

                AngleLimits.max = TheAngle;
                AngleLimits.min = -TheAngle;

                SmallAngleLimits.max = LeftMaxAngle;
                SmallAngleLimits.min = 0;
                SmallAngleLimits.max = 105;
                SmallAngleLimits.min = 105;
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
        }
        else if(Watch.HandIsUp == false)
        {

            if (!Input.GetMouseButton(1) && TheGetBigGun.Equipeed == false ) 
            { 
                Watch.HandIsUp = false;
            }

            if (Watch.HandIsUp == false)
            {
                //StandingComponent.enabled = true;
                //StandingComponent2.enabled = true;

                if (IsForward == false)
                {
                    SmallAngleLimits.min = LeftMinAngle;
                    SmallAngleLimits.max = LeftMaxAngle;
                }else if (IsForward == true)
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
