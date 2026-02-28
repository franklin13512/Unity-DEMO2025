using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//脚本用于控制手部方向，像僵尸一样前伸,挂载于手臂上
public class ZombieHand : MonoBehaviour
{
    public PCWalk ThePcWalk;

    private GameObject TheAngleChecker;

    public HingeJoint2D TheHingeJoint;
    private JointAngleLimits2D TheLimits;

    public Transform TheTarget;

    private HealthManager TheHealth;

    // Start is called before the first frame update
    void Start()
    {
        ThePcWalk = Player.FindComponentInAncestors<PCWalk>(transform);
        TheAngleChecker = ThePcWalk.BodyRb.gameObject;

        TheHingeJoint = gameObject.GetComponent<HingeJoint2D>();
        TheHealth = Player.FindComponentInAncestors<HealthManager>(transform);

    }

    // Update is called once per frame
    void Update()
    {
        if (TheHealth && TheHealth.TheAiPath) 
            TheTarget = TheHealth.TheAiPath.Target;
        if (TheHealth && TheHealth.Health <= 0 && TheHingeJoint.useLimits == true)
        {
            TheHingeJoint.useLimits = false;
        }

        if (TheHingeJoint && TheAngleChecker&& ThePcWalk && TheTarget)
        {
            RotateTurret(TheTarget.transform.position);
        }
        else
        {
            TheHingeJoint.useLimits = false;
        }
    }

    public void RotateTurret(Vector3 TheTargetPos)
    {

        Vector3 Direction = TheTargetPos - TheAngleChecker.transform.position;

        float TheAngle = Mathf.Atan2(Direction.x, -Direction.y) * Mathf.Rad2Deg;

        //Debug.Log(TheAngle);

        TheLimits.min = -TheAngle;
        TheLimits.max = -TheAngle;

        TheHingeJoint.limits = TheLimits;
        TheHingeJoint.useLimits = true;
    }
}
