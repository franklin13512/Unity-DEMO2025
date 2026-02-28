using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting.AssemblyQualifiedNameParser;
using UnityEngine;

public class TakeDamage : Player
{
    //public HealthManager TheHealth;

    public Rigidbody2D ThisRb;

    public float Magnification;

    public Rigidbody2D BodyRb;

    public GameObject BloodPartical;

    public DirtManager TheDirtManager;

    public StatusManager TheStatuseManager;

    public bool IsBodyBox;

    ////////////////////////////////////////ÉÕÉËÐ§¹û
    public bool IsBurn;
    public float BurnSeverity = 1;
    public float BurnSeverityMax = 3;
    public float BurnSeverityMin = 1;
    public bool BurnHasCount;

    public float BurnRecoverTime = 150f;
    public bool StartBurnRecover;

    ////////////////////////////////////////Ç¹ÉË
    public bool IsGunShot;
    public float GunShotSeverity = 0;
    public float GunShotSeverityMax = 10;
    public float GunShotSeverityMin = 0;
    public bool GunShotHasCount;

    public float GunShotRecoverTime = 150f;
    public bool StartGunShotRecover;

    ////////////////////////////////////////¸îÁÑÉË
    public bool IsCut;
    public float CutSeverity = 0;
    public float CutSeverityMax = 5;
    public float CutSeverityMin = 0;
    public bool CutHasCount;

    public float CutRecoverTime = 150f;
    public bool StartCutRecover;

    ////////////////////////////////////////ðöÉË
    public bool IsBruise;
    public float BruiseSeverity = 0;
    public float BruiseSeverityMax = 3;
    public float BruiseSeverityMin = 0;
    public bool BruiseHasCount;

    public float BruiseRecoverTime = 150f;
    public bool StartBruiseRecover;

    ////////////////////////////////////////×¥ÉË
    public bool IsScratch;
    public float ScratchSeverity = 0;
    public float ScratchSeverityMax = 5;
    public float ScratchSeverityMin = 0;
    public bool ScratchHasCount;

    public float ScratchRecoverTime = 150f;
    public bool StartScratchRecover;

    ////////////////////////////////////////Ò§ÉË
    public bool IsBiteInjury;
    public float BiteInjurySeverity = 0;
    public float BiteInjurySeverityMax = 6;
    public float BiteInjurySeverityMin = 0;
    public bool BiteInjuryHasCount;

    public float BiteInjuryRecoverTime = 150f;
    public bool StartBiteInjuryRecover;

    void Start()
    {
        HingeJoint2D TheJoint1 = gameObject.GetComponent<HingeJoint2D>();

        ThisRb = gameObject.GetComponent<Rigidbody2D>();

        TheDirtManager = GameObject.Find("ImportantResource/DirtManager").gameObject.GetComponent<DirtManager>();
        TheHealthManager = FindComponentInAncestors<HealthManager>(transform);

        Player ThePlayer = FindComponentInAncestors<Player>(transform);

        TheStatuseManager = FindComponentInAncestors<StatusManager>(transform);

        if (IsBodyBox)
        {
            if (TheHealthManager)
            {
                TheHealthManager.BodyBox = gameObject.GetComponent<BoxCollider2D>();
                ThePlayer.BodyBox = gameObject.GetComponent<BoxCollider2D>();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        TheBurnFunc();
        TheGunShotFunc();
        TheCut();
        TheBruise();
        Scratch();
        BiteInjury();

        if (TheHealthManager.Health <= 0)
        {
            TheHealthManager.Health = 0;

            HingeJoint2D TheJoint = gameObject.GetComponent<HingeJoint2D>();
            if (TheJoint != null)
            {
                TheJoint.useMotor = false;

                TheJoint.breakAction = JointBreakAction2D.Disable;
                TheJoint.breakForce = 800f;
                TheJoint.breakTorque = 500f;
            }

            Standing TheStanding = gameObject.GetComponent<Standing>();
            Moving TheMoving = gameObject.GetComponent<Moving>();
            if (TheStanding != null)
            {
                TheStanding.enabled = false;
            }

            if (TheMoving != null)
            {
                TheMoving.enabled = false;
            }

            return;
        }

    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Bullet")
        {

            HingeJoint2D TheJoint2 = gameObject.GetComponent<HingeJoint2D>();
            if (TheJoint2 != null)
            {
                if (TheJoint2.enabled == false)
                {
                    GameObject Blooddd = Instantiate(ThePrefabList.BloodPartical, TheJoint2.anchor, transform.rotation);
                }
            }

            BulletTouch TheBullet = collision.gameObject.GetComponent<BulletTouch>();
            BulletStay TheBulletStay = collision.gameObject.GetComponent<BulletStay>();
            Rigidbody2D TheBulletRb = collision.gameObject.GetComponent<Rigidbody2D>();

            ////Éú³ÉÑªÒºÁ£×ÓÌØÐ§
            //Vector2 NewPos = collision.transform.position;
            //if (TheBulletRb.velocity.x > 0)
            //{
            //    NewPos = new Vector2(collision.transform.position.x + 0.4f, collision.transform.position.y);
            //}
            //else if (TheBulletRb.velocity.x <= 0)
            //{
            //    NewPos = new Vector2(collision.transform.position.x - 0.4f, collision.transform.position.y);
            //}

            //GameObject TheBlood = Instantiate(BloodPartical, NewPos, collision.transform.rotation);
            //TheBlood.transform.SetParent(gameObject.transform);

            //BloodPart BRb2 = TheBlood.gameObject.GetComponent<BloodPart>();
            //BRb2.Magnification = new Vector2(0, 2);

            //Destroy(TheBlood, 0.2f);
            ///////////////////////////////////////////////////////////////////////

            //Ëæ»úÉú³ÉÑªÒºÍ¼Æ¬
            float RandomBlood = Random.Range(0, TheDirtManager.Bloods.Length + 50);
            if (0 < RandomBlood && RandomBlood <= TheDirtManager.Bloods.Length - 1)
            {
                int RandomBlood2 = Random.Range(0, TheDirtManager.Bloods.Length - 1);
                Instantiate(TheDirtManager.Bloods[RandomBlood2], collision.transform.position, collision.transform.rotation);
            }


            if (TheBullet != null && TheBullet.Touched == false)
            {

                TakingDamage(TheBullet.Damage);
                GunShotSeverity += 0.5f;
                TheBullet.Touched = true;

                //if (ThisRb != null && TheHealth.Health > 0)
                //{,
                //    BodyRb.bodyType = RigidbodyType2D.Static;
                //    ThisRb.bodyType = RigidbodyType2D.Static;

                //    StartCoroutine(UnFreeze());
                //}

                float TheChance = Random.Range(0, 20);
                if (TheChance <= 1)
                {
                    TheStatuseManager.StartBleeding = true;
                    TheStatuseManager.BleedingSeverity++;
                    TheStatuseManager.BleedingTime += 15;
                    Vector2 NewPos2 = new Vector2(gameObject.transform.position.x + 0.25f, gameObject.transform.position.y);
                    GameObject TheBlood2 = Instantiate(ThePrefabList.BloodPartical, NewPos2, Quaternion.identity);

                    TheBlood2.transform.SetParent(gameObject.transform);

                }

            }
            else if (TheBulletStay != null && TheBulletStay.Touched == false)
            {
                TakingDamage(TheBulletStay.Damage);
                TheBulletStay.Touched = true;

                //if (ThisRb != null && TheHealth.Health > 0)
                //{
                //    BodyRb.bodyType = RigidbodyType2D.Static;
                //    ThisRb.bodyType = RigidbodyType2D.Static;

                //    StartCoroutine(UnFreeze());
                //}

                float TheChance = Random.Range(0, 3);
                if (TheChance <= 1)
                {
                    TheStatuseManager.StartBleeding = true;
                    TheStatuseManager.BleedingSeverity++;
                    TheStatuseManager.BleedingTime += 35;
                    Vector2 NewPos2 = new Vector2(gameObject.transform.position.x + 0.25f, gameObject.transform.position.y);
                    GameObject TheBlood2 = Instantiate(ThePrefabList.BloodPartical, NewPos2, Quaternion.identity);


                }
            }


        }


    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        FireStay TheBullet = collision.gameObject.GetComponent<FireStay>();

        if (TheBullet != null && TheBullet.Touched == false)
        {

            TakingDamage(TheBullet.Damage);
            TheBullet.Touched = true;
        }
    }

    public void TakingDamage(float Damage)
    {
        //Debug.Log(gameObject.name);
        TheHealthManager.Health -= Damage * Magnification * BurnSeverity;

        if (Random.value < 0.4)
        {
            int Num = Random.Range(0, TheDirtManager.Bloods.Length);
            Instantiate(TheDirtManager.Bloods[Num], gameObject.transform.position, Quaternion.identity);
        }

    }

    //IEnumerator UnFreeze()
    //{
    //    yield return new WaitForSeconds(0.0001f);
    //    BodyRb.bodyType = RigidbodyType2D.Dynamic;
    //    ThisRb.bodyType = RigidbodyType2D.Dynamic;
    //}

    ////////////////////////////////////////////////////////////////×´Ì¬À¸ÇøÓò

    public void TheBurnFunc()
    {
        if (BurnSeverity > BurnSeverityMin)
        {
            IsBurn = true;
            if (BurnHasCount == false)
            {
                TheStatuseManager.NumberOfBurn++;
                BurnHasCount = true;
            }

            if (StartBurnRecover == false)
            {
                StopCoroutine("WaitForBurnRecover");
                StartCoroutine("WaitForBurnRecover");
                StartBurnRecover = true;
            }


        }

        if (BurnSeverity >= BurnSeverityMax)
        {
            BurnSeverity = BurnSeverityMax;
        }
        else if (BurnSeverity <= BurnSeverityMin)
        {
            BurnSeverity = BurnSeverityMin;
            IsBurn = false;
            StartBurnRecover = false;
            if (BurnHasCount == true)
            {
                TheStatuseManager.NumberOfBurn--;
                if (TheStatuseManager.NumberOfBurn < 0)
                {
                    TheStatuseManager.NumberOfBurn = 0;
                }
                BurnHasCount = false;
            }
        }
    }

    IEnumerator WaitForBurnRecover()
    {
        yield return new WaitForSeconds(BurnRecoverTime);
        BurnSeverity -= BurnSeverityMax * 0.2f;
        StartBurnRecover = false;
    }

    public void TheGunShotFunc()
    {
        if (GunShotSeverity > GunShotSeverityMin)
        {
            IsGunShot = true;
            if (GunShotHasCount == false)
            {
                TheStatuseManager.NumberOfGunShot++;
                GunShotHasCount = true;
            }

            if (StartGunShotRecover == false)
            {
                StopCoroutine("WaitForGunShotRecover");
                StartCoroutine("WaitForGunShotRecover");
                StartGunShotRecover = true;
            }


        }

        if (GunShotSeverity >= GunShotSeverityMax)
        {
            GunShotSeverity = GunShotSeverityMax;
        }
        else if (GunShotSeverity <= GunShotSeverityMin)
        {
            GunShotSeverity = GunShotSeverityMin;
            IsGunShot = false;
            StartGunShotRecover = false;
            if (GunShotHasCount == true)
            {
                TheStatuseManager.NumberOfGunShot--;
                if (TheStatuseManager.NumberOfGunShot < 0)
                {
                    TheStatuseManager.NumberOfGunShot = 0;
                }
                GunShotHasCount = false;
            }
        }
    }

    IEnumerator WaitForGunShotRecover()
    {
        yield return new WaitForSeconds(GunShotRecoverTime);
        GunShotSeverity -= 1.5f;
        StartGunShotRecover = false;
    }

    public void TheCut()
    {
        if (CutSeverity > CutSeverityMin)
        {
            IsCut = true;
            if (CutHasCount == false)
            {
                TheStatuseManager.NumberOfCut++;
                CutHasCount = true;
            }

            if (StartCutRecover == false)
            {
                StopCoroutine("WaitForCutRecover");
                StartCoroutine("WaitForCutRecover");
                StartCutRecover = true;
            }


        }

        if (CutSeverity >= CutSeverityMax)
        {
            CutSeverity = CutSeverityMax;
        }
        else if (CutSeverity <= CutSeverityMin)
        {
            CutSeverity = CutSeverityMin;
            IsCut = false;
            StartCutRecover = false;
            if (CutHasCount == true)
            {
                TheStatuseManager.NumberOfCut--;
                if (TheStatuseManager.NumberOfCut < 0)
                {
                    TheStatuseManager.NumberOfCut = 0;
                }
                CutHasCount = false;
            }
        }
    }

    IEnumerator WaitForCutRecover()
    {
        yield return new WaitForSeconds(CutRecoverTime);
        CutSeverity -= 2f;
        StartCutRecover = false;
    }

    public void TheBruise()
    {
        if (BruiseSeverity > BruiseSeverityMin)
        {
            IsBruise = true;
            if (BruiseHasCount == false)
            {
                TheStatuseManager.NumberOfBruise++;
                BruiseHasCount = true;
            }

            if (StartBruiseRecover == false)
            {
                StopCoroutine("WaitForBruiseRecover");
                StartCoroutine("WaitForBruiseRecover");
                StartBruiseRecover = true;
            }


        }

        if (BruiseSeverity >= BruiseSeverityMax)
        {
            BruiseSeverity = BruiseSeverityMax;
        }
        else if (BruiseSeverity <= BruiseSeverityMin)
        {
            BruiseSeverity = BruiseSeverityMin;
            IsBruise = false;
            StartBruiseRecover = false;
            if (BruiseHasCount == true)
            {
                TheStatuseManager.NumberOfBruise--;
                if (TheStatuseManager.NumberOfBruise < 0)
                {
                    TheStatuseManager.NumberOfBruise = 0;
                }
                BruiseHasCount = false;
            }
        }
    }

    IEnumerator WaitForBruiseRecover()
    {
        yield return new WaitForSeconds(BruiseRecoverTime);
        BruiseSeverity -= 1f;
        StartBruiseRecover = false;
    }

    public void Scratch()
    {
        if (ScratchSeverity > ScratchSeverityMin)
        {
            IsScratch = true;
            if (ScratchHasCount == false)
            {
                TheStatuseManager.NumberOfScratch++;
                ScratchHasCount = true;
            }

            if (StartScratchRecover == false)
            {
                StopCoroutine("WaitForScratchRecover");
                StartCoroutine("WaitForScratchRecover");
                StartScratchRecover = true;
            }


        }

        if (ScratchSeverity >= ScratchSeverityMax)
        {
            ScratchSeverity = ScratchSeverityMax;
        }
        else if (ScratchSeverity <= ScratchSeverityMin)
        {
            ScratchSeverity = ScratchSeverityMin;
            IsScratch = false;
            StartScratchRecover = false;
            if (ScratchHasCount == true)
            {
                TheStatuseManager.NumberOfScratch--;
                if (TheStatuseManager.NumberOfScratch < 0)
                {
                    TheStatuseManager.NumberOfScratch = 0;
                }
                ScratchHasCount = false;
            }
        }
    }

    IEnumerator WaitForScratchRecover()
    {
        yield return new WaitForSeconds(ScratchRecoverTime);
        ScratchSeverity -= 1f;
        StartScratchRecover = false;
    }

    public void BiteInjury()
    {
        if (BiteInjurySeverity > BiteInjurySeverityMin)
        {
            IsBiteInjury = true;
            if (BiteInjuryHasCount == false)
            {
                TheStatuseManager.NumberOfBiteInjury++;
                BiteInjuryHasCount = true;
            }

            if (StartBiteInjuryRecover == false)
            {
                StopCoroutine("WaitForBiteInjuryRecover");
                StartCoroutine("WaitForBiteInjuryRecover");
                StartBiteInjuryRecover = true;
            }


        }

        if (BiteInjurySeverity >= BiteInjurySeverityMax)
        {
            BiteInjurySeverity = BiteInjurySeverityMax;
        }
        else if (BiteInjurySeverity <= BiteInjurySeverityMin)
        {
            BiteInjurySeverity = BiteInjurySeverityMin;
            IsBiteInjury = false;
            StartBiteInjuryRecover = false;
            if (BiteInjuryHasCount == true)
            {
                TheStatuseManager.NumberOfBiteInjury--;
                if (TheStatuseManager.NumberOfBiteInjury < 0)
                {
                    TheStatuseManager.NumberOfBiteInjury = 0;
                }
                BiteInjuryHasCount = false;
            }
        }
    }

    IEnumerator WaitForBiteInjuryRecover()
    {
        yield return new WaitForSeconds(BiteInjuryRecoverTime);
        BiteInjurySeverity -= 1f;
        StartBiteInjuryRecover = false;
    }
}
