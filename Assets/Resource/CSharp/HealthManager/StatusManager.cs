using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Experimental.Rendering.RenderGraphModule;

public class StatusManager : MonoBehaviour
{
    [Space]
    [Space]
    //////////////////////////////////////////////////////////////////////////ÔÎµ¹Collapse
    //public RunAim TheRunAime;
    public PCWalk ThePCWalk;
    public HandBigGunUpLeft TheHandBigGunLeft;
    public HandBigGunUpRight TheHandBigGunRight;
    public HandGunUp TheHandGunUp;
    public HandGunUp TheHandGunUp2;
    public WatchMouse Watch;
    public Staring TheStaring;
    public MeleeAttackAnimation TheMeleeAttackAnimation;
    public List<Standing> TheStanding;
    public Moving[] TheMoving;

    public AiPath TheAiPath;

    public float CollapseTime = 4f;
    public bool IsCollapse;

    [Space]
    [Space]
    //////////////////////////////////////////////////////////////////////////Á÷ÑªBleed
    public HealthManager TheHealthManager;
    public GameObject TheBloodProject;

    public bool StartBleeding;
    public float BleedingTime;
    public float BleedingSeverity = 0;
    public float bleedingSeverityMax = 10;
    public float bleedingSeverityMin = 0;
    public bool StartTimer;
    public float PreviousTime;

    [Space]
    [Space]
    //////////////////////////////////////////////////////////////////////////ÄÚÔàËðÉËInternal_Injuries
    public bool IsInternalInjures;
    public float InternalInjures_Severity = 0;
    public float InternalInjures_SeverityMax = 10;
    public float InternalInjures_SeverityMin = 0;
    public float InternalInjures_TimeToRecover;
    public bool InternalInjures_TimerStart;

    [Space]
    [Space]
    //////////////////////////////////////////////////////////////////////////ÉÕÉËBurn
    public int NumberOfBurn;

    [Space]
    [Space]
    //////////////////////////////////////////////////////////////////////////½áðèÉË¿ÚScabbed
    public bool IsScabbed;

    [Space]
    [Space]
    //////////////////////////////////////////////////////////////////////////Ç¹ÉËGunShot
    public int NumberOfGunShot;

    [Space]
    [Space]
    //////////////////////////////////////////////////////////////////////////¸îÁÑÉËCut
    public int NumberOfCut;

    [Space]
    [Space]
    //////////////////////////////////////////////////////////////////////////ðöÉËBruise
    public int NumberOfBruise;

    [Space]
    [Space]
    //////////////////////////////////////////////////////////////////////////¿º·ÜExcitement && Î®ÃÒ²»ÕñListless
    public bool IsExcitement;
    public bool IsListless;
    public float ExcitementSeverity = 1f;
    public float ExcitementSeverityMin = 1;
    public float ListLessSeverityMin = 0.1f;
    public float ExcitementRecoverTime = 120f;
    public bool ExcitementStartTimer;

    [Space]
    [Space]
    //////////////////////////////////////////////////////////////////////////ÆøÌåÖÐ¶¾GasPosisoning
    public bool IsGasPoisoning;
    public float GasPoisoningSeverity = 0;
    public float GasPoisoningSeverityMax = 10;
    public float GasPoisoningSeverityMin = 0;

    [Space]
    [Space]
    //////////////////////////////////////////////////////////////////////////×¥ÉËScratch
    public int NumberOfScratch;

    [Space]
    [Space]
    //////////////////////////////////////////////////////////////////////////Ò§ÉËBiteInjury
    public int NumberOfBiteInjury;

    public void Start()
    {
        ThePCWalk = gameObject.GetComponent<PCWalk>();
        TheHealthManager = gameObject.GetComponent<HealthManager>();
    }

    public void FixedUpdate()
    {
        if (StartBleeding == true || PreviousTime != BleedingTime)
        {
            if (StartTimer == false || PreviousTime != BleedingTime)
            {
                StopCoroutine("BleedingTimer");
                StartCoroutine("BleedingTimer");
                PreviousTime = BleedingTime;
                StartTimer = true;
            }
            Bleeding();
        }

        //////////////////////////////////////////////////////////////////////////
        if (IsInternalInjures)
        {
            if (InternalInjures_TimerStart == false)
            {
                StopCoroutine("InternalInjures_Timer");
                StartCoroutine("InternalInjures_Timer");
                InternalInjures_TimerStart = true;
            }
            InternalInjuresFunc();
        }

        //////////////////////////////////////////////////////////////////////////
        if (BleedingSeverity > bleedingSeverityMin && BleedingTime == 0)
        {
            IsScabbed = true;
        }else if ((BleedingSeverity <= bleedingSeverityMin && BleedingTime == 0) || (BleedingSeverity > bleedingSeverityMin && BleedingTime > 0))
        {
            IsScabbed = false;
        }

        //////////////////////////////////////////////////////////////////////////
        if (ExcitementSeverity > 1)
        {
            IsExcitement = true;
            IsListless = false;
        }
        else if (ExcitementSeverity < 1)
        {
            IsExcitement = false;
            IsListless = true;
            if (ExcitementSeverity < ListLessSeverityMin)
            {
                ExcitementSeverity = ListLessSeverityMin;
            }
        }else if (ExcitementSeverity == ExcitementSeverityMin)
        {
            IsExcitement = false;
            IsListless = false;
        }

        if (IsExcitement || IsListless)
        {

            if (ExcitementStartTimer == false)
            {
                StopCoroutine("ExcitementTimer");
                StartCoroutine("ExcitementTimer");
                ExcitementStartTimer = true;
            }
            Excitement();
        }

        //////////////////////////////////////////////////////////////////////////
        if (GasPoisoningSeverity > GasPoisoningSeverityMin)
        {
            IsGasPoisoning = true;
        }else if (GasPoisoningSeverity <= GasPoisoningSeverityMin)
        {
            IsGasPoisoning = false;
            GasPoisoningSeverity = GasPoisoningSeverityMin;
        }

        if (IsGasPoisoning == true)
        {
            GasPoisoningFunc();
        }

    }

    //ÔÎµ¹×´Ì¬
    public void Collapse()
    {
        StartCoroutine(WaitForAwake());
    }

    IEnumerator WaitForAwake()
    {
        IsCollapse = true;
        //if (TheRunAime != null)
        //TheRunAime.enabled = false;
        if(ThePCWalk)
            ThePCWalk.enabled = false;
        if (TheAiPath)
            TheAiPath.enabled = false;

        if (TheHandBigGunLeft != null)
        TheHandBigGunLeft.enabled = false;
        if (TheHandBigGunRight != null)
        TheHandBigGunRight.enabled = false;
        if (TheHandGunUp != null/* && gameObject.tag == "Player"*/)
        TheHandGunUp.enabled = false;
        if (Watch != null/* && gameObject.tag == "Player"*/)
        Watch.enabled = false;
        if (TheStaring != null/* && gameObject.tag == "Player"*/)
        TheStaring.enabled = false;
        if (TheHandGunUp2 != null/* && gameObject.tag == "Player"*/)
        TheHandGunUp2.enabled = false;
        if(TheMeleeAttackAnimation != null)
        TheMeleeAttackAnimation.enabled = false;
        for (int i = 0; i < TheStanding.Count; i++)
        {
            if (TheStanding[i] != null)
                TheStanding[i].enabled = false;
        }
        for (int i = 0; i < TheMoving.Length; i++)
        {
            if (TheMoving[i] != null)
            TheMoving[i].enabled = false;
        }

        yield return new WaitForSeconds(CollapseTime);

        IsCollapse = false;
        //if (TheRunAime != null)
        //TheRunAime.enabled = true;
        if (ThePCWalk)
            ThePCWalk.enabled = true;
        if (TheAiPath)
            TheAiPath.enabled = true;

        if (TheHandBigGunLeft != null)
        TheHandBigGunLeft.enabled = true;
        if (TheHandBigGunRight != null)
        TheHandBigGunRight.enabled = true;
        if (TheHandGunUp != null)
        TheHandGunUp.enabled = true;
        if (TheHandGunUp != null)
        Watch.enabled = true;
        if (Watch != null)
        TheStaring.enabled = true;
        if (TheHandGunUp2 != null)
        TheHandGunUp2.enabled = true;
        if (TheMeleeAttackAnimation != null)
        TheMeleeAttackAnimation.enabled = true;
        for (int i = 0; i < TheStanding.Count; i++)
        {
            if (TheStanding[i] != null)
            TheStanding[i].enabled = true;
        }
        for (int i = 0; i < TheMoving.Length; i++)
        {
            if (TheMoving[i] != null)
            TheMoving[i].enabled = true;
        }
    }

    //Á÷Ñª×´Ì¬
    public void Bleeding()
    {
        if (BleedingSeverity > bleedingSeverityMax)
        {
            BleedingSeverity = bleedingSeverityMax;
        }else if (BleedingSeverity < bleedingSeverityMin)
        {
            BleedingSeverity = bleedingSeverityMin;
            StartBleeding = false;
            StartTimer = false;
            BleedingTime = 0;
            PreviousTime = BleedingTime;
        }
        TheHealthManager.Health = TheHealthManager.Health - 0.01f * BleedingSeverity;
    }

    IEnumerator BleedingTimer()
    {
        yield return new WaitForSeconds(BleedingTime);
        StartBleeding = false;
        StartTimer = false;
        BleedingTime = 0;
        PreviousTime = BleedingTime;
    }

    //ÄÚÔàËðÉË×´Ì¬
    public void InternalInjuresFunc()
    {
        if (InternalInjures_Severity >= InternalInjures_SeverityMax)
        {
            InternalInjures_Severity = InternalInjures_SeverityMax;
        }else if (InternalInjures_Severity <= InternalInjures_SeverityMin)
        {
            InternalInjures_Severity = InternalInjures_SeverityMin;
            IsInternalInjures = false;
            InternalInjures_TimerStart = false;
            InternalInjures_TimeToRecover = 0;

        }
    }

    IEnumerator InternalInjures_Timer()
    {
        yield return new WaitForSeconds(InternalInjures_TimeToRecover);
        InternalInjures_Severity--;
        InternalInjures_Severity--;
        if (InternalInjures_Severity > InternalInjures_SeverityMin)
        {
            IsInternalInjures = true;
            InternalInjures_TimerStart = false;
        }
        else
        {
            InternalInjures_Severity = InternalInjures_SeverityMin;
            IsInternalInjures = false;
            InternalInjures_TimerStart = false;
            InternalInjures_TimeToRecover = 0;
        }
    }

    //¿º·Ü×´Ì¬
    public void Excitement()
    {
        if (IsExcitement == true)
        {
            ExcitementSeverity = 1.8f;
        }
    }

    IEnumerator ExcitementTimer()
    {
        yield return new WaitForSeconds(ExcitementRecoverTime);
        ExcitementSeverity = ExcitementSeverityMin;
        IsExcitement = false;
        IsListless = false;
    }

    //ÆøÌåÖÐ¶¾
    public void GasPoisoningFunc()
    {
        if (GasPoisoningSeverity > GasPoisoningSeverityMax)
        {
            GasPoisoningSeverity = GasPoisoningSeverityMax;
        }else if (GasPoisoningSeverity <= GasPoisoningSeverityMin)
        {
            GasPoisoningSeverity = GasPoisoningSeverityMin;
            IsGasPoisoning = false;
        }

        TheHealthManager.Health = TheHealthManager.Health - 0.005f * GasPoisoningSeverity;
    }
}
