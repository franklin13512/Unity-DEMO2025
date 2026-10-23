using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.U2D.IK;
using UnityEngine.UI;

public class HealthManager : Player
{
    public bool IsFemale;
    public bool IsDead;
    public GameObject[] AllLegs;

    public float Health;
    public float MaxHealth;

    public bool Exploded;

    //for Player
    public HandBigGunUpLeft TheHandBigGunLeft;
    public HandBigGunUpRight TheHandBigGunRight;
    public HandGunUp TheHandGunUp;
    public HandGunUp TheHandGunUp2;
    public Staring TheStaring;
    //public Animator TheAnime;
    public PCWalk ThePCWalk;

    public GameObject[] PXP;

    public Image TheBloodImage;
    public Slider TheHealthSlider;
    public Image TheColorOfSlider;

    public Slider DamageSlider;
    public float MinusSpeed;

    public Rigidbody2D TheRb;
    public StatusManager TheStatus;
    public Player ThePlayer;

    public HingeJoint2D TheHinge0;
    public HingeJoint2D TheHinge1;

    public AiPath TheAiPath;

    // Start is called before the first frame update
    void Start()
    {
        ThePlayer = gameObject.GetComponent<Player>();
        if(ThePlayer && ThePlayer.BodyBox)
            TheRb = ThePlayer.BodyBox.gameObject.GetComponent<Rigidbody2D>();

        TheStatus = gameObject.GetComponent<StatusManager>();
        ThePCWalk = gameObject.GetComponent<PCWalk>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Exploded == true)
        {
            StartCoroutine(TurnToFalse());
        }

        if (TheAiPath && Health <= 0 && IsDead == false)
        {
            TheAiPath.Target = null;
            TheAiPath.enabled = false;
            IsDead = true;

            // 死亡交给布娃娃系统
            if (TheStatus != null)
            {
                TheStatus.Die();
                TheStatus.enabled = false;
            }
        }

        if (ThePCWalk != null && Health <= 0)
        {
            ThePCWalk.enabled = false;
            TurnAround TheTurn = ThePCWalk.gameObject.GetComponent<TurnAround>();
            if (TheTurn != null)
            {
                TheTurn.enabled = false;
            }
        }

        if (gameObject.tag == "Player")
        {
            if (Mathf.Abs(DamageSlider.value - TheHealthSlider.value) <= 0.01)
            {
                DamageSlider.value = TheHealthSlider.value;
            }

            if (DamageSlider.value > TheHealthSlider.value)
            {
                DamageSlider.value -= MinusSpeed * Time.deltaTime;
            }

            if (DamageSlider.value < TheHealthSlider.value)
            {
                DamageSlider.value += MinusSpeed * Time.deltaTime;
            }

            if (Health <= 0 && IsDead == false)
            {
                IsDead = true;

                // 死亡交给布娃娃系统：进入布娃娃 + 禁用移动/攻击/瞄准
                if (TheStatus != null)
                {
                    TheStatus.Die();
                    TheStatus.enabled = false;   // 死亡后关掉状态系统，避免昏倒/回正叠加
                }

                TheBloodImage.enabled = true;

                // 手臂铰链放宽（布娃娃下手臂更自由，保留）
                if (TheHinge0 != null)
                {
                    JointAngleLimits2D TheLimit0 = TheHinge0.limits;
                    TheLimit0.min = 0; TheLimit0.max = 360;
                    TheHinge0.limits = TheLimit0;
                }
                if (TheHinge1 != null)
                {
                    JointAngleLimits2D TheLimit1 = TheHinge1.limits;
                    TheLimit1.min = 0; TheLimit1.max = 360;
                    TheHinge1.limits = TheLimit1;
                }

                // StatusManager 保持启用（靠 IsDead 阻止回正）
                this.enabled = false;
            }else if (Health <= 50 && Health >30)
            {
                TheColorOfSlider.color = Color.yellow;
                TheBloodImage.enabled = false;
            }
            else if (Health <= 30)
            {
                TheBloodImage.enabled = true;
                TheColorOfSlider.color = Color.red;
            }
            else if (Health > 30)
            {
                TheBloodImage.enabled = false;
            }

            if (Health > 50)
            {
                TheColorOfSlider.color = Color.white;
            }

            TheHealthSlider.value = (float)Health / MaxHealth;
        }

    }

    IEnumerator TurnToFalse()
    {
        yield return new WaitForSeconds(0.1f);
        Exploded = false;
    }

    //public void LateUpdate()
    //{
    //    if (TheHealthManager.Health <= 0 && TheRb.bodyType == RigidbodyType2D.Kinematic || (TheStatus && TheStatus.IsCollapse == true))
    //    {
    //        TheRb.bodyType = RigidbodyType2D.Dynamic;
    //    }

    //}
}
