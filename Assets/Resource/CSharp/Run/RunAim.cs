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

    /// <summary>
    /// 静止时的处理。脚的摆放已统一交给 ProceduralIKWalker，
    /// 这里只负责维护“离地高度 / 是否在地面”的判断，不再移动 legs。
    /// </summary>
    public void KeepStand_IK()
    {
        if (Hight > 0.73f || Hight < 0)
        {
            OntheGround = false;
        }
        else
        {
            OntheGround = true;
        }
    }

}
