using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.Image;

//挂载于头上，咬人脚本
public class Biting : MonoBehaviour
{
    public AudioSource TheBiteSound;

    public float TimeForNextBite;//冷却
    public float MaxColdTime;

    public float BiteDamage;

    public float BiteForce;//咬人的时候施加一个向前的力

    public float BiteDistance;//咬人检测距离（到了这个距离就开咬）

    public LayerMask BiteLayer;

    private TurnAround TheTurn;

    private HealthManager TheHealth;
    private Rigidbody2D TheRb;

    public float BiteInjuryChance;
    // Start is called before the first frame update
    void Start()
    {
        TheRb = GetComponent<Rigidbody2D>();
        TheTurn = Player.FindComponentInAncestors<TurnAround>(transform);
        TheHealth = Player.FindComponentInAncestors<HealthManager>(transform);
    }

    // Update is called once per frame
    void Update()
    {
        if (TheHealth && TheHealth.Health > 0)
        {


            if (TimeForNextBite > 0)
            {
                TimeForNextBite -= Time.deltaTime;
            }

            int IsRight;

            RaycastHit2D TheHit;

            if (TheTurn && TheTurn.MinusTimer > TheTurn.MaxTime)
            {
                TheHit = Physics2D.Raycast(
                gameObject.transform.position,
                -gameObject.transform.right,
                BiteDistance,
                BiteLayer
                );

                IsRight = -1;
                Debug.DrawRay(gameObject.transform.position, -gameObject.transform.right * BiteDistance, Color.red);
            }
            else if (TheTurn && TheTurn.PosTimer > TheTurn.MaxTime)
            {
                TheHit = Physics2D.Raycast(
                gameObject.transform.position,
                gameObject.transform.right,
                BiteDistance,
                BiteLayer
                );


                IsRight = 1;
                Debug.DrawRay(gameObject.transform.position, gameObject.transform.right * BiteDistance, Color.red);
            }
            else
            {
                return;
            }

            if (TheHit && TheHit.collider && TheHit.collider.gameObject &&  TheHealth && TheHealth.TheAiPath && TheHealth.TheAiPath.Target && TheHit.collider.gameObject.tag == TheHealth.TheAiPath.Target.tag && TimeForNextBite <= 0)
            {
                TakeDamage TheTakeDamage = TheHit.collider.gameObject.GetComponent<TakeDamage>();
                if (TheTakeDamage)
                {
                    TheRb.AddForce(gameObject.transform.right * IsRight * BiteForce, ForceMode2D.Impulse);
                    TheTakeDamage.TakingDamage(BiteDamage);
                    TimeForNextBite = MaxColdTime;

                    TheBiteSound.Play();

                    float Num = Random.value;
                    if(Num < BiteInjuryChance * 0.01)
                    {
                        TheTakeDamage.BiteInjury();
                        TheTakeDamage.BiteInjurySeverity++;
                    }

                }
            }


        }
    }
}
