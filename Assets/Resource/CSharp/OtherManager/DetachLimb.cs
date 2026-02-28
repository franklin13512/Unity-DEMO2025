using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetachLimb : MonoBehaviour
{
    public HingeJoint2D Limb;
    public WatchMouse Watch;
    public bool IsForward;
    private float MaxAngle;
    private float MinAngle;

    public bool Changed1 = true;
    public bool Changed2 = false;

    // Start is called before the first frame update
    void Start()
    {
        IsForward = Watch.IsTurned;

            Limb = gameObject.GetComponent<HingeJoint2D>();

        if (Limb != null)
        {
            MaxAngle = Limb.limits.max;
            MinAngle = Limb.limits.min;
        }
    }

    // Update is called once per frame
    void Update()
    {

        if (Limb != null)
        {
            JointAngleLimits2D Limits = Limb.limits;

            //遗留问题，当出生时朝左时会无法生效
            IsForward = Watch.IsTurned;
            if (IsForward == false && Changed1 == true)
            {
                Limits.max = MaxAngle;
                Limits.min = MinAngle;

                Changed1 = false;
                Changed2 = true;

            }
            else if (IsForward == true && Changed2 == true)
            {
                Limits.max = -MinAngle;
                Limits.min = -MaxAngle;

                Changed1 = true;
                Changed2 = false;
            }

            Limb.limits = Limits;
        }
    }

    public void Detaching()
    {
        gameObject.transform.parent = null;
    }

    public void GetParents(GameObject NewPlayer)
    {
        gameObject.transform.SetParent(NewPlayer.transform);
    }
}
