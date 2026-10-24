using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttackAnimation : MonoBehaviour
{
    [Tooltip("鼠标朝向组件（原来从 Player 继承）")]
    public WatchMouse Watch;

    [Tooltip("预制体列表（原来从 Player 继承）")]
    public PrefabList ThePrefabList;


    public HingeJoint2D RightHinge;
    public HingeJoint2D RightSmallHinge;

    public float UpSpeed;
    public float DownSpeed;
    
    //�������ֱ���ת�Ƕ�
    public float TheRightUpLimit;
    public float TheRightDownLimit;

    public bool AttackFinished;

    public GetMeleeWeapon TheGetMelee;

    public MeleeAttacking TheMeleeAttacking;

    private Player ThePlayer;

    public bool AttackPrepared;

    public float TheTime;//������һ�ι���ʱ��
    public float AttackTime;//�ڴ�ʱ���ڹ�������Ч

    void Start()
    {


        ThePlayer = TheGetMelee.gameObject.GetComponent<Player>();
        if (ThePlayer != null && ThePlayer.ThingInHand)
        {

            TheMeleeAttacking = ThePlayer.ThingInHand.gameObject.GetComponent<MeleeAttacking>();
            MeleeWeapon TheMeleeWeapon = ThePlayer.ThingInHand.gameObject.GetComponent<MeleeWeapon>();
            if (TheMeleeWeapon != null)
            {

                TheTime = TheMeleeWeapon.ThisItem.TimeForNextAttack;
                AttackTime = 0.7f;
            }
            else
                TheTime = 0;
            AttackTime = 0f;
        }
        AttackPrepared = true;
    }

    void Update()
    {

        if (AttackTime > 0f && AttackTime < 0.7f && ThePlayer.ThingInHand.gameObject)
        {
            BoxCollider2D TheBox = ThePlayer.ThingInHand.gameObject.GetComponent<BoxCollider2D>();

            TheBox.enabled = false;
            TheBox.enabled = true;
        }

        if (Input.GetMouseButton(1) && !Input.GetMouseButton(0) && Watch.IsTurned == false && TheGetMelee.Equipeed == true && AttackPrepared == true)
        {
            TheTime = 2f;
            AttackTime = 0.7f;
            Watch.HandIsUp = true;
            UpAnim();
            AttackPrepared = true;

        }else if (Input.GetMouseButton(1) && Input.GetMouseButton(0) && Watch.IsTurned == false && TheGetMelee.Equipeed == true && AttackPrepared == true)
        {
            Watch.HandIsUp = true;

            DownAnim();
        }else if (Input.GetMouseButton(1) && !Input.GetMouseButton(0) && Watch.IsTurned == true && TheGetMelee.Equipeed == true && AttackPrepared == true)
        {
            TheTime = 2f;
            AttackTime = 0.7f;
            Watch.HandIsUp = true;
            UpAnimTurn();
            AttackPrepared = true;
        }
        else if (Input.GetMouseButton(1) && Input.GetMouseButton(0) && Watch.IsTurned == true && TheGetMelee.Equipeed == true && AttackPrepared == true)
        {
            Watch.HandIsUp = true;

            DownAnimTurn();
        }
        else
        {
            Watch.HandIsUp = false;
        }


        if (AttackPrepared == false)
        {
            TheTime -= Time.deltaTime;
            AttackTime -= Time.deltaTime;
        }
       
        if (TheTime <= 0)
        {
            AttackPrepared = true;
        }

    }

    public void UpAnim()
    {
        if (RightHinge.useMotor == false || RightSmallHinge.useMotor == false)
        {
            RightHinge.useMotor = true;
            RightSmallHinge.useMotor = true;
        }

        JointMotor2D RightMotor = RightHinge.motor;
        JointMotor2D RightSmallMotor = RightSmallHinge.motor;

        JointAngleLimits2D RightLimit = RightHinge.limits;

        RightMotor.motorSpeed = -UpSpeed;
        RightSmallMotor.motorSpeed = -UpSpeed;

        RightLimit.max = -TheRightUpLimit;

        RightHinge.motor = RightMotor;
        RightSmallHinge.motor = RightSmallMotor;

        RightHinge.limits = RightLimit;
    }

    public void DownAnim()
    {
        if (RightHinge.useMotor == false || RightSmallHinge.useMotor == false)
        {
            RightHinge.useMotor = true;
            RightSmallHinge.useMotor = true;
        }

        JointMotor2D RightMotor = RightHinge.motor;
        JointMotor2D RightSmallMotor = RightSmallHinge.motor;

        JointAngleLimits2D RightLimit = RightHinge.limits;

        RightMotor.motorSpeed = DownSpeed;
        RightSmallMotor.motorSpeed = -UpSpeed;

        RightLimit.max = -TheRightDownLimit;

        RightHinge.motor = RightMotor;
        RightSmallHinge.motor = RightSmallMotor;

        RightHinge.limits = RightLimit;

        AttackPrepared = false;
    }


    public void UpAnimTurn()
    {
        if (RightHinge.useMotor == false || RightSmallHinge.useMotor == false)
        {
            RightHinge.useMotor = true;
            RightSmallHinge.useMotor = true;
        }

        JointMotor2D RightMotor = RightHinge.motor;
        JointMotor2D RightSmallMotor = RightSmallHinge.motor;

        JointAngleLimits2D RightLimit = RightHinge.limits;

        RightMotor.motorSpeed = UpSpeed;
        RightSmallMotor.motorSpeed = UpSpeed;

        RightLimit.min = TheRightUpLimit;

        RightHinge.motor = RightMotor;
        RightSmallHinge.motor = RightSmallMotor;

        RightHinge.limits = RightLimit;
    }

    public void DownAnimTurn()
    {
        if (RightHinge.useMotor == false || RightSmallHinge.useMotor == false)
        {
            RightHinge.useMotor = true;
            RightSmallHinge.useMotor = true;
        }

        JointMotor2D RightMotor = RightHinge.motor;
        JointMotor2D RightSmallMotor = RightSmallHinge.motor;

        JointAngleLimits2D RightLimit = RightHinge.limits;

        RightMotor.motorSpeed = -DownSpeed;
        RightSmallMotor.motorSpeed = UpSpeed;

        RightLimit.min = TheRightDownLimit;

        RightHinge.motor = RightMotor;
        RightSmallHinge.motor = RightSmallMotor;

        RightHinge.limits = RightLimit;

        AttackPrepared = false;
    }
}
