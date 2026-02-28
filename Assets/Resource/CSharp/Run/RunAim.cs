
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.U2D.Animation;
using UnityEngine.U2D.IK;

public class RunAim : MonoBehaviour
{
    public Animator TheAime;
    public AnimationClip Run;
    public AnimationClip Walk;

    public SpriteSkin[] TheSkins;
    public GameObject TheBone;

    public IKManager2D[] TheIKPoint;

    public BoxCollider2D[] TheBoxs;

    public WatchMouse Watch;

    public HealthManager TheHealth;
    public StatusManager TheStatus;

    public Rigidbody2D BodyRig;

    public float Hight;

    public bool OntheGround;

    [SerializeField] Transform HightChecker;

    private Vector2 vect;

    // Start is called before the first frame update
    void Start()
    {
        CloseAllHinge();

        TheAime = gameObject.GetComponent<Animator>();
        TheStatus = gameObject.GetComponent<StatusManager>();
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit2D TheHit = Physics2D.Raycast(HightChecker.position, Vector2.down,1f,1);
        Hight = HightChecker.position.y - TheHit.point.y;

        if (TheAime.enabled == true)
        {
            OntheGround = false;
        }

        //if(OntheGround == true && TheHealth.Health > 0 && !TheStatus.IsCollapse)
        //{
        //    BodyRig.gravityScale = -1.4f;
        //}else
        //{
        //    BodyRig.gravityScale = 3.5f;
        //}

        float TheHorizontalValue = Input.GetAxis("Horizontal");

        if (TheHealth.Health > 0)
        {
            if (Watch.IsTurned == false && TheHorizontalValue > 0 && Input.GetKey(KeyCode.LeftShift))
            {
                if (TheAime.enabled == false)
                    TheAime.enabled = true;

                CloseAllHinge();

                TheAime.SetBool("ToRun", true);
                TheAime.SetBool("ToWalk", false);

                TheAime.SetBool("ToRunBack", false);
                TheAime.SetBool("ToWalkBack", false);
                TheAime.SetBool("Nothing", false);

            }
            else if (Watch.IsTurned == false && TheHorizontalValue > 0 && !Input.GetKey(KeyCode.LeftShift))
            {
                if (TheAime.enabled == false)
                    TheAime.enabled = true;

                CloseAllHinge();

                TheAime.SetBool("ToRun", false);
                TheAime.SetBool("ToWalk", true);

                TheAime.SetBool("ToRunBack", false);
                TheAime.SetBool("ToWalkBack", false);
                TheAime.SetBool("Nothing", false);

            }
            else if (Watch.IsTurned == true && TheHorizontalValue > 0 && Input.GetKey(KeyCode.LeftShift))
            {
                if (TheAime.enabled == false)
                    TheAime.enabled = true;

                CloseAllHinge();

                TheAime.SetBool("ToRun", false);
                TheAime.SetBool("ToWalk", false);

                TheAime.SetBool("ToRunBack", false);
                TheAime.SetBool("ToWalkBack", true);
                TheAime.SetBool("Nothing", false);

            }
            else if (Watch.IsTurned == true && TheHorizontalValue > 0 && !Input.GetKey(KeyCode.LeftShift))
            {
                if (TheAime.enabled == false)
                    TheAime.enabled = true;

                CloseAllHinge();

                TheAime.SetBool("ToRun", false);
                TheAime.SetBool("ToWalk", false);

                TheAime.SetBool("ToRunBack", false);
                TheAime.SetBool("ToWalkBack", true);
                TheAime.SetBool("Nothing", false);
                // /////////////////////////////////////////////////////////////////////
            }
            else if (Watch.IsTurned == false && TheHorizontalValue < 0 && Input.GetKey(KeyCode.LeftShift))
            {
                if (TheAime.enabled == false)
                    TheAime.enabled = true;

                CloseAllHinge();

                TheAime.SetBool("ToRun", false);
                TheAime.SetBool("ToWalk", false);

                TheAime.SetBool("ToRunBack", false);
                TheAime.SetBool("ToWalkBack", true);
                TheAime.SetBool("Nothing", false);

            }
            else if (Watch.IsTurned == false && TheHorizontalValue < 0 && !Input.GetKey(KeyCode.LeftShift))
            {
                if (TheAime.enabled == false)
                    TheAime.enabled = true;

                CloseAllHinge();

                TheAime.SetBool("ToRun", false);
                TheAime.SetBool("ToWalk", false);

                TheAime.SetBool("ToRunBack", false);
                TheAime.SetBool("ToWalkBack", true);
                TheAime.SetBool("Nothing", false);

            }
            else if (Watch.IsTurned == true && TheHorizontalValue < 0 && Input.GetKey(KeyCode.LeftShift))
            {
                if (TheAime.enabled == false)
                    TheAime.enabled = true;

                CloseAllHinge();

                TheAime.SetBool("ToRun", true);
                TheAime.SetBool("ToWalk", false);

                TheAime.SetBool("ToRunBack", false);
                TheAime.SetBool("ToWalkBack", false);
                TheAime.SetBool("Nothing", false);

            }
            else if (Watch.IsTurned == true && TheHorizontalValue < 0 && !Input.GetKey(KeyCode.LeftShift))
            {
                if (TheAime.enabled == false)
                    TheAime.enabled = true;

                CloseAllHinge();

                TheAime.SetBool("ToRun", false);
                TheAime.SetBool("ToWalk", true);

                TheAime.SetBool("ToRunBack", false);
                TheAime.SetBool("ToWalkBack", false);
                TheAime.SetBool("Nothing", false);
            }
            else
            {

                //TheAime.SetBool("ToRun", false);
                //TheAime.SetBool("ToWalk", false);
                //TheAime.SetBool("Nothing", true);
                //TheAime.SetBool("ToWalkBack", false);
                //TheAime.SetBool("ToRunBack", false);

                if (TheAime.enabled == true)
                    TheAime.enabled = false;

                KeepStand_IK();


            }

        }

        //if (ColdTime0 > 0)
        //{
        //    ColdTime0 -= Time.deltaTime;
        //}

        //if (ColdTime1 > 0)
        //{
        //    ColdTime1 -= Time.deltaTime;
        //}

    }


    public void CloseAllHinge()
    {
        for (int i = 0; i < TheSkins.Length; i++)
        {
            TheSkins[i].enabled = true;
        }

        for (int i = 0; i < TheBoxs.Length; i++)
        {
            TheBoxs[i].enabled = false;
        }

        //TheBone.gameObject.SetActive(true);
    }

    public void ReturnToHinge()
    {
        for (int i = 0; i < TheSkins.Length; i++)
        {
            TheSkins[i].enabled = false;
        }

        for (int i = 0; i < TheBoxs.Length; i++)
        {
            TheBoxs[i].enabled = true;
        }

        //TheBone.gameObject.SetActive(false);
    }

    [SerializeField] Transform raycastY = null;
    [SerializeField] Transform[] legs = null;
    [SerializeField] Transform[] IkPoints = null;

    public float DownForce = 5f;

    //public float ColdTime0 = 0.5f;
    //public float ColdTime1 = 0f;
    //public float MaxTime = 0.5f;

    public void KeepStand_IK()
    {



        float DistanceIk1 = Mathf.Sqrt(Mathf.Pow((legs[0].position.x - IkPoints[0].position.x), 2) + Mathf.Pow((legs[0].position.y - IkPoints[0].position.y), 2));
        float DistanceIk2 = Mathf.Sqrt(Mathf.Pow((legs[1].position.x - IkPoints[1].position.x), 2) + Mathf.Pow((legs[1].position.y - IkPoints[1].position.y), 2));

        if (DistanceIk1 > 0.1f && TheHealth.Health > 0 )
        {
            if(!TheStatus.IsCollapse)
                legs[0].position = IkPoints[0].position;
        }
        if (DistanceIk2 > 0.1f && TheHealth.Health > 0 )
        {
            if (!TheStatus.IsCollapse)
                legs[1].position = IkPoints[1].position;
        }

        if (Hight > 0.73f || Hight < 0)
        {
            OntheGround = false;
        }
        else
        {
            OntheGround = true;
        }

        float StepBetween = Mathf.Abs(legs[0].position.x - legs[1].position.x);
        float StepDistance1 = raycastY.position.x - legs[0].position.x; 
        float StepDistance2 = raycastY.position.x - legs[1].position.x;

        if ((StepDistance1 < 0f && StepDistance2 > 0f) || (StepDistance1 > 0f && StepDistance2 < 0f)) return;

        int footToMove = 0;


        int Direction = 0;
        if (StepDistance1 < 0 && StepDistance2 < 0)
        {
            Direction = -1;
        }
        else if (StepDistance1 > 0 && StepDistance2 > 0)
        {
            Direction = 1;
        }

        if (Mathf.Abs(StepDistance1) > Mathf.Abs(StepDistance2))
        {
            footToMove = 0;
        }
        else
        {
            footToMove = 1;
        }

        //if (StepBetween > 0.01f)
        //{
            Vector2 rayOrigin = legs[footToMove].position + new Vector3(Mathf.Abs(transform.position.x) * 0.6f * Direction, 0, 0);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.down, 1f);

            if (hit.collider != null)
            {
                Vector2 targetPos = hit.point;
                legs[footToMove].position = Vector2.MoveTowards(
                legs[footToMove].position,
                targetPos,
                Time.deltaTime * 5f
                                                                            );
            }
        //}


        if (Hight <= 0.75f)
        {
            legs[footToMove].position = Vector2.MoveTowards(
            legs[footToMove].position,
            raycastY.position,
            Time.deltaTime * 5f
                                                                            );
        }

    }

}
