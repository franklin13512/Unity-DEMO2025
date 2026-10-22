using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class PCWalk : MonoBehaviour
{
    public Player ThePlayer;
    private WatchMouse Watch;

    private Vector2 vect;

    public float SmoothTime;//腿部运动时间
    public float SmoothTime_Run;

    public float SmoohSpeed;
    public float ValueSpeed;//插值速度

    public float RunTimeInterval;
    public float WalkTimeInterval;
    public float WalkTimeInterval_0;
    public float WalkTimeInterval_1;

    public float RunTimeInterval_0;
    public float RunTimeInterval_1;

    public float ArcHight;//抬腿高度
    public float ArcHight_Run;//抬腿高度_跑步
    public float StairHight;//上楼梯时抬腿高度
    public float RunMag;//跑步时步子幅度

    public bool WhetherTurnRecorded;

    public float WalkPreCheckValue; //让腿早点反应过来

    [SerializeField] Transform raycastY = null;
    [SerializeField] Transform raycastY_Feet = null;
    [SerializeField] Transform[] legs = null;
    [SerializeField] LayerMask IKLayer = new LayerMask();

    public float IkCheckMaxHight;//当下一步的检测点高于这个数，则踏空
    public float Distance;//当某个脚远离了这个距离时，向前迈一步
    public float Distance_Run;

    public float DistanceMut;//用于减少跑步的检测距离，从而不会导致一条腿拖到太后面去

    public float ForceToStand;//站起来的力，小于等于0时人物可以站立；

    public float PreCheckValue;

    private bool isLeg0Moving = false;
    private bool isLeg1Moving = false;

    public Rigidbody2D BodyRb;

    private Vector2 PreHit;
    private Vector2 PreHit1;
    private Vector2 PreHit_Re;
    private Vector2 PreHit1_Re;

    bool UpStair = false;

    private AiPath TheAiPath;
    private TurnAround TheTurn;

    public bool NotPlayer;
    public bool CanRun;//仅限于ai
    public bool IsRunning;//仅限于ai



    // Start is called before the first frame update
    void Start()
    {
        ThePlayer = GetComponent<Player>();
        if (ThePlayer)
        {
            if (ThePlayer.BodyBox)
            {
                BodyRb = ThePlayer.BodyBox.gameObject.GetComponent<Rigidbody2D>();
            }
            Watch = ThePlayer.Watch;
        }

        HealthManager TheHealth = gameObject.GetComponent<HealthManager>();
        if (TheHealth)
        {
            TheHealth.ThePCWalk = this;
        }


        if (NotPlayer)
        {
            if (BodyRb) {
                TheAiPath = BodyRb.gameObject.GetComponent<AiPath>();
                CanRun = TheAiPath.CanRun;
            }

            TheTurn = gameObject.GetComponent<TurnAround>();
        }
        //legs[0].transform.position = new Vector2(raycastY.position.x - (Distance / 2), raycastY_Feet.position.y);
        //legs[1].transform.position = new Vector2(raycastY.position.x + (Distance / 2), raycastY_Feet.position.y);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (TheAiPath)
            IsRunning = TheAiPath.IsRunning;

        if (Watch && Watch.IsTurned != WhetherTurnRecorded)
        {
            foreach (Transform leg in legs)
            {
                leg.position = raycastY_Feet.position + new Vector3(0,-0.1f,0);
            }
        }

        if (math.abs(Input.GetAxis("Horizontal")) <= 0.01 && !NotPlayer)
        {
            foreach (Transform leg in legs)
            {
                //leg.position = raycastY_Feet.position;
                leg.position = Vector3.MoveTowards(leg.position, raycastY_Feet.position, Time.deltaTime * 4f);
            }
        }


        if (WalkTimeInterval_0 > 0)
            WalkTimeInterval_0 -= Time.deltaTime;

        if (WalkTimeInterval_1 > 0)
            WalkTimeInterval_1 -= Time.deltaTime;

        float TheHorizontalValue = Input.GetAxis("Horizontal");

        RaycastHit2D hit = Physics2D.Raycast(new Vector2(raycastY.position.x + (Distance / 2), raycastY.position.y ),
        Vector2.down,
        IkCheckMaxHight,
        IKLayer);

        float Dis = 0;
        if (hit)
            Dis = Vector2.Distance(hit.point, new Vector2(raycastY.position.x + (Distance / 2), raycastY.position.y));

        RaycastHit2D hit1 = Physics2D.Raycast(new Vector2(raycastY.position.x - (Distance / 2), raycastY.position.y),
        Vector2.down,
        IkCheckMaxHight,
        IKLayer);

        float Dis1 = 0;
        if (hit1)
            Dis1 = Vector2.Distance(hit1.point, new Vector2(raycastY.position.x - (Distance / 2), raycastY.position.y));

        RaycastHit2D hit_Re = Physics2D.Raycast(new Vector2(raycastY.position.x + (Distance_Run * RunMag), raycastY.position.y),
        Vector2.down,
        IkCheckMaxHight,
        IKLayer);


        float Dis_Re = 0;
        if (hit_Re)
            Dis_Re = Vector2.Distance(hit_Re.point, new Vector2(raycastY.position.x + (Distance_Run * RunMag), raycastY.position.y));

        RaycastHit2D hit1_Re = Physics2D.Raycast(new Vector2(raycastY.position.x - (Distance_Run * RunMag), raycastY.position.y),
        Vector2.down,
        IkCheckMaxHight,
        IKLayer);

        float Dis1_Re = 0;
        if (hit1_Re)
            Dis1_Re = Vector2.Distance(hit1_Re.point,new Vector2(raycastY.position.x - (Distance_Run * RunMag), raycastY.position.y));

        if ((!NotPlayer && ( (TheHorizontalValue > 0 && Input.GetKey(KeyCode.LeftShift) && Watch.IsTurned == false) || (TheHorizontalValue < 0 && Input.GetKey(KeyCode.LeftShift) && Watch.IsTurned == true))) || (NotPlayer && IsRunning && CanRun))
        {


            float Distance0 = Mathf.Abs(legs[0].transform.position.x - hit_Re.point.x);
            float Distance1 = Mathf.Abs(legs[1].transform.position.x - hit_Re.point.x);

            float Distance00 = Mathf.Abs(legs[0].transform.position.x - hit1_Re.point.x);
            float Distance11 = Mathf.Abs(legs[1].transform.position.x - hit1_Re.point.x);

            if ((!NotPlayer && ((Mathf.Abs(hit_Re.point.y - raycastY_Feet.position.y) > 0.1f && TheHorizontalValue > 0) || (Dis_Re > 0.03 && (Distance0 >= Distance_Run * DistanceMut || Distance1 >= Distance_Run * DistanceMut) && Input.GetKey(KeyCode.LeftShift) && Watch.IsTurned == false))) || (NotPlayer && TheTurn && !TheTurn.IsTurned))
            {


                if (hit_Re.transform == null)
                {
                    return;
                }

                if (Distance0 > Distance1 && WalkTimeInterval_0 <= 0 && isLeg0Moving == false)
                {
                    StartCoroutine(UpLegCounter(legs[0], hit_Re, ArcHight_Run, SmoothTime_Run));
                    WalkTimeInterval_1 = RunTimeInterval;

                    //StartCoroutine(WalkTime(WalkTimeInterval_1));
                }
                else if (Distance0 < Distance1 && WalkTimeInterval_1 <= 0 && isLeg1Moving == false)
                {
                    StartCoroutine(UpLegCounter(legs[1], hit_Re, ArcHight_Run, SmoothTime_Run));
                    WalkTimeInterval_0 = RunTimeInterval;

                    //StartCoroutine(WalkTime(WalkTimeInterval_0));
                }
            }
            else if ((!NotPlayer && ((Mathf.Abs(hit1_Re.point.y - raycastY_Feet.position.y) > 0.1f && TheHorizontalValue < 0) || (Dis1_Re > 0.03 && (Distance00 >= Distance_Run * DistanceMut || Distance11 >= Distance_Run * DistanceMut) && Input.GetKey(KeyCode.LeftShift) && Watch.IsTurned == true))) || (NotPlayer && TheTurn && TheTurn.IsTurned))
            {
                if (hit1_Re.transform == null)
                {
                    return;
                }

                if (Distance00 > Distance11 && WalkTimeInterval_0 <= 0 && isLeg0Moving == false)
                {
                    StartCoroutine(UpLegCounter(legs[0], hit1_Re, ArcHight_Run, SmoothTime_Run));
                    WalkTimeInterval_1 = RunTimeInterval;

                    //StartCoroutine(WalkTime(WalkTimeInterval_1));
                }
                else if (Distance00 < Distance11 && WalkTimeInterval_1 <= 0 && isLeg1Moving == false)
                {
                    StartCoroutine(UpLegCounter(legs[1], hit1_Re, ArcHight_Run, SmoothTime_Run));
                    WalkTimeInterval_0 = RunTimeInterval;

                    //StartCoroutine(WalkTime(WalkTimeInterval_0));

                }
            }
        }
        else
        {
            float Distance0 = Mathf.Abs(legs[0].transform.position.x - hit.point.x);
            float Distance1 = Mathf.Abs(legs[1].transform.position.x - hit.point.x);

            float Distance00 = Mathf.Abs(legs[0].transform.position.x - hit1.point.x);
            float Distance11 = Mathf.Abs(legs[1].transform.position.x - hit1.point.x);

        if (BodyRb && BodyRb.velocity.magnitude >= 0.1f)
        {

            

            if ((Mathf.Abs(hit.point.y - raycastY_Feet.position.y) > 0.1f && TheHorizontalValue > 0) || (Dis > 0.03 && Distance0 >= Distance / WalkPreCheckValue || Distance1 >= Distance / WalkPreCheckValue))
            {
                if (hit.transform == null)
                {
                    return;
                }

                //if (PreHit == null || Mathf.Abs(hit.point.y - PreHit.y)  <= 0.15f)
                //{
                if (Distance0 > Distance1 && WalkTimeInterval_0 <= 0 && isLeg0Moving == false)
                {
                    hit = CheckStep(hit, 1);

                    StartCoroutine(UpLegCounter(legs[0], hit, ArcHight, SmoothTime));
                    WalkTimeInterval_1 = WalkTimeInterval;

                    //StartCoroutine(WalkTime(WalkTimeInterval_1));
                }
                else if (Distance0 < Distance1 && WalkTimeInterval_1 <= 0 && isLeg1Moving == false)
                {
                    hit = CheckStep(hit, -1);

                    StartCoroutine(UpLegCounter(legs[1], hit, ArcHight, SmoothTime));
                    WalkTimeInterval_0 = WalkTimeInterval;

                    //StartCoroutine(WalkTime(WalkTimeInterval_0));

                }
                //}



            } else if ((Mathf.Abs(hit1.point.y - raycastY_Feet.position.y) > 0.1f && TheHorizontalValue < 0) || (Dis1 > 0.03 && Distance00 >= Distance / WalkPreCheckValue || Distance11 >= Distance / WalkPreCheckValue))
            {
                if (hit1.transform == null)
                {
                    return;
                }



                if (Distance00 > Distance11 && WalkTimeInterval_0 <= 0 && isLeg0Moving == false)
                {
                    hit1 = CheckStep(hit1, 1);

                    StartCoroutine(UpLegCounter(legs[0], hit1, ArcHight, SmoothTime));
                    WalkTimeInterval_1 = WalkTimeInterval;

                    //StartCoroutine(WalkTime(WalkTimeInterval_1));
                }
                else if (Distance00 < Distance11 && WalkTimeInterval_1 <= 0 && isLeg1Moving == false)
                {
                    hit1 = CheckStep(hit1, -1);

                    StartCoroutine(UpLegCounter(legs[1], hit1, ArcHight, SmoothTime));
                    WalkTimeInterval_0 = WalkTimeInterval;

                    //StartCoroutine(WalkTime(WalkTimeInterval_0));

                }
            }
        }

                
        }

       

        if(hit)
            PreHit = hit.point;
        if(hit1)
            PreHit1 = hit1.point;
        if(hit_Re)
            PreHit_Re = hit_Re.point;
        if (hit1_Re)
            PreHit1_Re = hit1_Re.point;

        if(Watch)
            WhetherTurnRecorded = Watch.IsTurned;
    }

    public RaycastHit2D CheckStep(RaycastHit2D TheHit, int PosOrMinus)
    {
        if ((Mathf.Abs(TheHit.point.y - raycastY_Feet.position.y) > 0.1f))
        {
            TheHit = Physics2D.Raycast(new Vector2(TheHit.point.x + PreCheckValue * PosOrMinus, raycastY.position.y),
            Vector2.down,
            IkCheckMaxHight,
            IKLayer);

            UpStair = true;
            return TheHit;
        }
        else
        {
            UpStair = false;
            return TheHit;
        }
    }

    IEnumerator UpLegCounter(Transform Tsf, RaycastHit2D Hit, float Hight, float TheTime)
    {

        if (Tsf == legs[0]) isLeg0Moving = true;
        else if (Tsf == legs[1]) isLeg1Moving = true;

        float moveTimer = 0f;
        float moveDuration = TheTime; // 移动总时间（秒）


        while (moveTimer < moveDuration)
        {
            moveTimer += Time.deltaTime * ValueSpeed;
            //Debug.Log(moveTimer);

            float t = Mathf.Clamp01(moveTimer / moveDuration );

            // 中间点：抬腿的最高点
            if(UpStair == true)
            {
                Hight = StairHight;
            }

            Vector2 mid = (new Vector2(Tsf.transform.position.x, Tsf.transform.position.y) + Hit.point) / 2 + Vector2.up * Hight;

            Vector2 NewVec = new Vector2(Hit.point.x, Hit.point.y + ForceToStand);

            // 贝塞尔插值公式
            Vector2 legPos = Vector2.Lerp(
                Vector2.Lerp(new Vector2(Tsf.transform.position.x, Tsf.transform.position.y), mid, t),
                Vector2.Lerp(mid, NewVec, t),
                t);

            //Vector2 legPos = Vector2.Lerp(new Vector2(Tsf.transform.position.x, Tsf.transform.position.y), NewVec, t);

            Tsf.position = legPos;
            yield return null;
        }
        Tsf.position = Hit.point;

        if (Tsf == legs[0]) isLeg0Moving = false;
        else if (Tsf == legs[1]) isLeg1Moving = false;
    }

}
