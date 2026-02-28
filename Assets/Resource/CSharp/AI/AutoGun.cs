using Pathfinding;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class AutoGun : Structure
{
    public float Damage;
    public float ChanceToBleed;

    public float ShootSpeed;
    public GameObject ShootPos;
    public AudioSource TheShootSound;
    public AudioClip ShootClip;
    public AudioClip DesClip;
    public float BackwardForce;
    public Vector2 vect;

    public LayerMask ShootLayer;

    public LineRenderer TheLaserLine;
    [Space]
    public float ColdTime;//冷却时间
    public float MaxColdTime;
    public float TheRadius;//范围
    public GameObject TheAngleChecker;//检测中心
    public string[] TargetTag;//目标标签
    [Space]
    public float Heat;//当前热度
    public float MaxHeat;//最大过热值，超过后过热等待冷却
    public float HeatSpeed;//过热速度
    [Space]
    public HingeJoint2D TheHingeJoint;
    private JointAngleLimits2D TheLimits;
    [Space]
    public GameObject PermitLight;
    public GameObject StopLight;
    [Space]
    public GameObject FireLight;
    public GameObject HitLight;
    [Space]
    public GameObject TheSmallSpark;//爆炸后断断续续的小火花
    public GameObject TheLargeSpark;//爆炸瞬间的火花

    public bool FirePermmison;
    public GameObject BulletEffect;
    public ItemInventorySlot TheAddition;

    [Space]
    public bool EnableBullets;
    [Header("BulletAttributes")]
    public GameObject TheBullets;
    public float BulletsSpeed;


    [HideInInspector]
    public Rigidbody2D TheRb;

    private AiPath TheAiPth;



    // Start is called before the first frame update
    void Start()
    {
        //TheRb = ShootPos.transform.parent.gameObject.GetComponent<Rigidbody2D>();

        TheAddition = gameObject.GetComponent<ItemInventorySlot>();
        TheAiPth = gameObject.GetComponent<AiPath>();

        FirePermmison = true;

        if (TheHingeJoint)
            TheLimits = TheHingeJoint.limits;
    }

    // Update is called once per frame
    void Update()
    {
        if(TheAiPth)
            TargetTag = TheAiPth.Tags;

        if(StructureDurability <= 0 && WasDead == false)
        {
            Destruction();
        }

        if(ColdTime > 0)
        {
            ColdTime -= Time.deltaTime;
            if (ColdTime <= 0)
            {
                ColdTime = 0;
                Heat = 0;
            }
        }

        if (PermitLight && StopLight)
        {
            if (ColdTime > 0 && PermitLight.gameObject.activeSelf == true)
            {
                PermitLight.gameObject.SetActive(false);
                StopLight.gameObject.SetActive(true);
            }
            else if (ColdTime <= 0 && StopLight.gameObject.activeSelf == true)
            {
                PermitLight.gameObject.SetActive(true);
                StopLight.gameObject.SetActive(false);
            }
        }

        if (StructureDurability > 0)
        {
            Collider2D[] TheCollier = Physics2D.OverlapCircleAll(TheAngleChecker.gameObject.transform.position, TheRadius);

            if (TheCollier != null)
            {
                foreach (Collider2D col in TheCollier)
                {
                    if (TargetTag.Contains<string>(col.gameObject.tag))
                    {
                        RotateTurret(col.gameObject.transform.position);
                    }
                }
            }
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

        if (FirePermmison == true && ColdTime <= 0 && StructureDurability > 0)
            StartCoroutine(WaitForNextShoot());
    }

    public void Shooting()
    {

        Heat += HeatSpeed;

        FirePermmison = false;

        GameObject SoundObj = new GameObject();

        AudioSource TheAS = SoundObj.gameObject.AddComponent<AudioSource>();
        TheAS.loop = false;
        TheAS.clip = TheShootSound.clip;
        TheAS.Play();
        if (TheAS.isPlaying == false)
        {
            Destroy(SoundObj.gameObject);
        }

        SoundObj.gameObject.AddComponent<DestoryDirt2>();
        DestoryDirt2 TheDestroy = SoundObj.gameObject.GetComponent<DestoryDirt2>();
        TheDestroy.DestroyTime = 8;

        StartCoroutine(TheBulletShoot());
        //if(EnableBullets == false)
        //{

        //    RaycastHit2D TheHit = Physics2D.Raycast(ShootPos.gameObject.transform.position, ShootPos.transform.up);

        //    if (TheHit)
        //    {
        //        TheLaserLine.SetPosition(0, ShootPos.transform.position);
        //        TheLaserLine.SetPosition(1, TheHit.point);

        //        GameObject TheFireLight = Instantiate(HitLight, TheHit.point, transform.rotation);

        //        Destroy(TheFireLight, 0.02f);


        //        TheAddition.ShootEffect(TheHit.collider.gameObject, ShootPos, Damage, Damage * 0.1f, ChanceToBleed, TheHit);
        //    }
        //    else
        //    {
        //        TheLaserLine.SetPosition(0, ShootPos.gameObject.transform.position);
        //        TheLaserLine.SetPosition(1, ShootPos.gameObject.transform.position + ShootPos.transform.localPosition * 100);
        //    }
        //}
        //else
        //{

        //    GameObject CBullet = Instantiate(TheBullets, ShootPos.transform.position, transform.rotation);

        //    Rigidbody2D BulletRb = CBullet.gameObject.GetComponent<Rigidbody2D>();

        //    BulletRb.velocity = ShootPos.transform.up * BulletsSpeed;
        //}



        if (Heat >= MaxHeat)
        {
            Heat = MaxHeat;
            ColdTime = MaxColdTime;

        }

    }

    public override void Destruction()
    {
        GameObject TheObj = Instantiate(TheSmallSpark, TheAngleChecker.gameObject.transform.position, TheAngleChecker.gameObject.transform.rotation);
        TheObj.transform.SetParent(gameObject.transform);
        TheObj.transform.localScale = Vector3.one;
        TheObj.gameObject.GetComponent<ParticleSystem>().Play();
        Instantiate(TheLargeSpark, TheAngleChecker.gameObject.transform.position, TheAngleChecker.gameObject.transform.rotation).gameObject.GetComponent<ParticleSystem>().Play();

        TheShootSound.clip = DesClip;
        TheShootSound.Play();
        
        if(PermitLight != null)
            PermitLight.gameObject.SetActive(false);
        if (StopLight != null)
            StopLight.gameObject.SetActive(false);

        TheHingeJoint.useLimits = false;

        WasDead = true;

        Rigidbody2D TheRb = gameObject.GetComponent<Rigidbody2D>();
        if(TheRb != null)
        {
            TheRb.gravityScale = 2.0f;
        }

        Seeker TheSeeker = gameObject.GetComponent<Seeker>();
        AiPath TheAiPath = gameObject.GetComponent<AiPath>();
        if(TheSeeker && TheAiPath)
        {
            TheSeeker.enabled = false;
            TheAiPath.enabled = false;
        }

        gameObject.tag = "Untagged";


    } 

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(TheAngleChecker.gameObject.transform.position, TheRadius);
    }

    IEnumerator TurnOnLight()
    {
        yield return new WaitForSeconds(1 / 10);

        if (FireLight)
            FireLight.gameObject.SetActive(false);
    }

    IEnumerator WaitForNextShoot()
    {
        if (FireLight)
            FireLight.gameObject.SetActive(true);

        Shooting();
        StartCoroutine(TurnOnLight());
        yield return new WaitForSeconds(ShootSpeed);

        FirePermmison = true;

    }

    IEnumerator TheBulletShoot()
    {
        if (EnableBullets == false)
        {

            RaycastHit2D TheHit = Physics2D.Raycast(ShootPos.gameObject.transform.position, ShootPos.transform.up,100f , ShootLayer);

            if (TheHit)
            {
                TheLaserLine.SetPosition(0, ShootPos.transform.position);
                TheLaserLine.SetPosition(1, TheHit.point);

                GameObject TheFireLight = Instantiate(HitLight, TheHit.point, transform.rotation);

                Destroy(TheFireLight, 0.02f);


                TheAddition.ShootEffect(TheHit.collider.gameObject, ShootPos, Damage, Damage * 0.1f, ChanceToBleed, TheHit);
            }
            else
            {
                TheLaserLine.SetPosition(0, ShootPos.gameObject.transform.position);
                TheLaserLine.SetPosition(1, ShootPos.gameObject.transform.position + ShootPos.transform.localPosition * 100);
            }
        }
        else
        {

            GameObject CBullet = Instantiate(TheBullets, ShootPos.transform.position, transform.rotation);

            Rigidbody2D BulletRb = CBullet.gameObject.GetComponent<Rigidbody2D>();

            BulletRb.velocity = ShootPos.transform.up * BulletsSpeed;
        }

        yield return null;
    }

    //void OnDisable()
    //{
    //    UnityEngine.Debug.Log("OnDisable called on " + gameObject.name);
    //    UnityEngine.Debug.Log(Environment.StackTrace); // 打印堆栈信息
    //}
}
