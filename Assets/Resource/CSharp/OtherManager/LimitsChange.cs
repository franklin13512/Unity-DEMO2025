using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LimitsChange : MonoBehaviour
{
    public WatchMouse Watch;
    public bool IsForward;

    public bool LimitChanged01;
    public bool LimitChanged02;

    public HingeJoint2D[] LowerLegsHands;
    private JointAngleLimits2D[] TheLimitsOfLowerLegs;
    private float[] RecordedAngaleMax;
    private float[] RecordedAngaleMin;

    // Start is called before the first frame update
    void Start()
    {
        IsForward = Watch.IsTurned;
        LimitChanged01 = true;
        LimitChanged02 = false;

        TheLimitsOfLowerLegs = new JointAngleLimits2D[LowerLegsHands.Length];
        RecordedAngaleMax = new float[LowerLegsHands.Length];
        RecordedAngaleMin = new float[LowerLegsHands.Length];

        for (int i = 0; i < LowerLegsHands.Length; i++)
        {
            if (LowerLegsHands[i] != null)
            {
                TheLimitsOfLowerLegs[i] = LowerLegsHands[i].limits;
                RecordedAngaleMax[i] = LowerLegsHands[i].limits.max;
                RecordedAngaleMin[i] = LowerLegsHands[i].limits.min;
                
            }
        }

    }

    // Update is called once per frame
    void Update()
    {
        IsForward = Watch.IsTurned;

        if (IsForward == false && LimitChanged01 == true)
        {
            LimitChanged01 = false;
            LimitChanged02 = true;

            for (int i = 0; i < LowerLegsHands.Length; i++)
            {
                if (LowerLegsHands[i] != null)
                {
                    TheLimitsOfLowerLegs[i].max = RecordedAngaleMax[i];
                    TheLimitsOfLowerLegs[i].min = RecordedAngaleMin[i];
                    LowerLegsHands[i].limits = TheLimitsOfLowerLegs[i];
                }
            }
        }else if (IsForward == true && LimitChanged02 == true)
        {
            LimitChanged01 = true;
            LimitChanged02 = false;

            for (int i = 0; i < LowerLegsHands.Length; i++)
            {
                if (LowerLegsHands[i] != null)
                {
                    TheLimitsOfLowerLegs[i].max = -RecordedAngaleMin[i];
                    TheLimitsOfLowerLegs[i].min = -RecordedAngaleMax[i];
                    LowerLegsHands[i].limits = TheLimitsOfLowerLegs[i];
                }
            }
        }
    }
}
