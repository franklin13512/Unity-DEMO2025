using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LimitControler : MonoBehaviour
{
    public LeftWalking LeftWalk;
    public RightWalking RightWalk;

    public HingeJoint2D LeftHinge;
    public HingeJoint2D RightHinge;

    public float TheMaxLeft;
    public float TheMinLeft;

    public float TheMaxRight;
    public float TheMinRight;

    public bool WhetherChanged1 = true;

    public float WalkStep;

    // Start is called before the first frame update
    void Start()
    {
        JointAngleLimits2D TheLimitLeft = LeftHinge.limits;
        TheMaxLeft = TheLimitLeft.max;
        TheMinLeft = TheLimitLeft.min;

        JointAngleLimits2D TheLimitRight = RightHinge.limits;
        TheMaxRight = TheLimitRight.max;
        TheMinRight = TheLimitRight.min;
    }

    // Update is called once per frame
    void Update()
    {
        if ((Input.GetButton("Horizontal") || Input.GetButton("HorizontalOP")) && !Input.GetKey(KeyCode.LeftShift) )
        {
            JointAngleLimits2D TheLimitLeft = LeftHinge.limits;
            TheLimitLeft.max = TheMaxLeft / WalkStep;
            TheLimitLeft.min = TheMinLeft / WalkStep;

            JointAngleLimits2D TheLimitRight = RightHinge.limits;
            TheLimitRight.max = TheMaxRight / WalkStep;
            TheLimitRight.min = TheMinRight / WalkStep;

            LeftHinge.limits = TheLimitLeft;
            RightHinge.limits = TheLimitRight;

            WhetherChanged1 = false;
        }else if(WhetherChanged1 == false)
        {
            JointAngleLimits2D TheLimitLeft = LeftHinge.limits;
            TheLimitLeft.max = TheMaxLeft;
            TheLimitLeft.min = TheMinLeft;

            JointAngleLimits2D TheLimitRight = RightHinge.limits;
            TheLimitRight.max = TheMaxRight;
            TheLimitRight.min = TheMinRight;

            LeftHinge.limits = TheLimitLeft;
            RightHinge.limits = TheLimitRight;

            WhetherChanged1 = true;
        }else
        {
            return;
        }
    }
}
