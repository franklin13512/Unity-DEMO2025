using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttacking : MonoBehaviour
{
    public float MeleeDamage;
    public float ReservedDamage;
    public GetMeleeWeapon TheGetMeleeWeapon;

    public bool AttackFinished;

    public AudioSource TheStrikeSound;

    public GameObject TheCollision;

    // Start is called before the first frame update
    void Start()
    {
        ReservedDamage = MeleeDamage;
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Rigidbody2D Rb = gameObject.GetComponent<Rigidbody2D>();

        if (Rb && Rb.velocity.magnitude > 1 && TheGetMeleeWeapon != null)
        {

            Debug.Log(Rb.velocity.magnitude);
            if (collision.gameObject.tag != TheGetMeleeWeapon.MasterTag)
            {
                TakeDamage TheTakeDamage = collision.gameObject.GetComponent<TakeDamage>();
                if (TheTakeDamage != null && AttackFinished == false)
                {

                    if (gameObject.transform.parent != null)
                    {
                        if (gameObject.transform.parent.parent != null)
                        {
                            TakeDamage HanderTakeDamage = gameObject.transform.parent.parent.GetComponent<TakeDamage>();
                            if (HanderTakeDamage != null)
                            {
                                //此处添加武器持有者的buff
                                MeleeDamage = ReservedDamage * HanderTakeDamage.TheStatuseManager.ExcitementSeverity;
                            }
                        }
                    }

                    StartCoroutine(WaitForNextAttack());
                    float RandomPitch = Random.Range(0.5f, 1f);
                    TheStrikeSound.pitch = RandomPitch;
                    TheStrikeSound.Play();
                    //Debug.Log(MeleeDamage);
                    TheTakeDamage.TakingDamage(MeleeDamage);

                    StatusManager TheStatuseManager = TheTakeDamage.TheStatuseManager;
                    if (TheStatuseManager != null)
                    {
                        TheCollision = collision.gameObject;
                    }

                }
            }
        }
    }


    IEnumerator WaitForNextAttack()
    {
        AttackFinished = true;
        yield return new WaitForSeconds(0.6f);
        AttackFinished = false;
    }
}
