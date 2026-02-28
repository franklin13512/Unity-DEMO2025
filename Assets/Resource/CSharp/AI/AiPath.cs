using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using UnityEngine.Rendering;
using System.Linq;

public class AiPath : MonoBehaviour
{
    public Transform Target;

    public float Speed;
    public float NextWayPointDistance;

    Path ThePath;
    int CurrentWayPoint = 0;

    bool ReadEndOfPath = false;

    Seeker TheSeeker;
    public Rigidbody2D TheRb;

    public bool Accelerate;//»À–Œaiπÿ±’
    public float TheRadius;
    public string[] Tags;

    public float ChasingTime;
    public float MaxChasingTime;

    private bool FoundTarget;

    public bool IsWandering;

    private GameObject WanderingPoint;
    public float WanderdingColdTime;
    public float MaxWanderdingColdTime;

    public bool CanFly;

    public bool CanRun;
    public bool IsRunning;

    // Start is called before the first frame update
    void Start()
    {
        WanderingPoint = new GameObject();
        //WanderingPoint = Instantiate(WanderingPoint);

        HealthManager TheHealth = Player.FindComponentInAncestors<HealthManager>(transform);
        if (TheHealth)
        {
            TheHealth.BodyBox = gameObject.GetComponent<BoxCollider2D>();
            TheHealth.TheAiPath = this;
        }

        StatusManager TheStatus = Player.FindComponentInAncestors<StatusManager>(transform);
        if (TheStatus)
            TheStatus.TheAiPath = this;

        TheSeeker = gameObject.GetComponent<Seeker>();

        //TheRb = gameObject.GetComponent<Rigidbody2D>();


        InvokeRepeating("UpdatePath", 0f, 0.5f);

    }

    void UpdatePath()
    {
        if (Target)
        {
            if (TheSeeker.IsDone())
            {
                TheSeeker.StartPath(TheRb.transform.position, Target.transform.position, OnPathComplete);
            }
        }

    }

    void OnPathComplete(Path P)
    {
        if (Target)
        {
            if (!P.error)
            {
                ThePath = P;
                CurrentWayPoint = 0;
            }
            else
            {
                return;
            }
        }

    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(gameObject.transform.position, TheRadius);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Collider2D[] TheCollier = Physics2D.OverlapCircleAll(gameObject.transform.position, TheRadius);
        
        if (TheCollier != null)
        {
            foreach (Collider2D col in TheCollier)
            {
                if (Tags.Contains<string>(col.gameObject.tag))
                {
                    Target = col.transform;
                    ChasingTime = MaxChasingTime;
                    FoundTarget = true;
                    IsWandering = false;

                    IsRunning = true;
                    break;

                }
                else
                {
                    FoundTarget = false;
                    IsWandering = true;
                }


            }

            if(!FoundTarget)
            {
                ChasingTime -= Time.deltaTime;
                if (ChasingTime <= 0)
                {
                    //Target = null;
                    FoundTarget = false;
                    IsWandering = true;
                    IsRunning = false;
                }
            }

        }

        if (IsWandering && ChasingTime <= 0)
        {
            if (WanderdingColdTime <= 0)
            {
                Vector2 ThePoint = new Vector2();
                if (CanFly)
                {
                    Vector2 RandomOffset = Random.insideUnitCircle * TheRadius;

                    ThePoint = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y) + RandomOffset;
                }
                else
                {
                    float t = Random.Range(-1f, 1f);

                    //Vector2 RandomOffset = Random.insideUnitCircle * TheRadius;

                    ThePoint = new Vector2(gameObject.transform.position.x * TheRadius * t, gameObject.transform.position.y);
                }


                WanderingPoint.transform.position = ThePoint;
                Target = WanderingPoint.transform;

                WanderdingColdTime = MaxWanderdingColdTime;
            }
            else
            {
                WanderdingColdTime -= Time.deltaTime;
            }

            if(Vector2.Distance(WanderingPoint.transform.position, gameObject.transform.position) < 0.1f)
            {
                WanderdingColdTime = 0f;
            }

        }


        if (Target)
        {

            if (ThePath == null)
            {
                return;
            }

            if (CurrentWayPoint >= ThePath.vectorPath.Count)
            {
                ReadEndOfPath = true;
                return;
            }
            else
            {
                ReadEndOfPath = false;
            }

            Vector2 Direction = ((Vector2)ThePath.vectorPath[CurrentWayPoint] - TheRb.position).normalized;

            Vector2 Forece;

            if (Accelerate)
            {

                if (CanRun && IsRunning)
                {
                    Forece = Direction * Speed * 2 * Time.deltaTime;
                }
                else
                {
                    Forece = Direction * Speed * Time.deltaTime;
                }

                TheRb.AddForce(Forece);

            }
            else
            {
                //Forece = Direction * Speed;
                if (CanRun && IsRunning)
                {
                    TheRb.velocity = Direction * Speed * 2;
                }
                else
                {
                    TheRb.velocity = Direction * Speed;
                }



            }




            float Distance = Vector2.Distance(TheRb.position, ThePath.vectorPath[CurrentWayPoint]);

            if (Distance < NextWayPointDistance)
            {
                CurrentWayPoint++;
            }
        }

    }
}
