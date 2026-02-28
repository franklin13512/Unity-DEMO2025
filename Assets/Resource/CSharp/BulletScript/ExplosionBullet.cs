using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class ExplosionBullet : MonoBehaviour
{
    public float TimeToBoom;

    public GameObject TheBoomPartical;
    public float Maganification;
    public float MaxDamage;
    public float Damage_ST;

    public AudioSource ExplosionSound;
    public AudioClip Exploding;

    public float ExplosionRadius;

    public AudioSource RingingSource;
    public AudioClip TheRingSound;

    public LayerMask TheLayers;

    // Start is called before the first frame update
    void Start()
    {

        UnityEngine.Quaternion Thequaternion = new UnityEngine.Quaternion(0,0,0,0);

        StartCoroutine(WaiForExplode(Thequaternion));
        Destroy(gameObject, TimeToBoom);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator WaiForExplode(UnityEngine.Quaternion Thequaternion)
    {
        yield return new WaitForSeconds(TimeToBoom - 5);

        UnityEngine.Vector2 NePos = new UnityEngine.Vector2(transform.position.x, transform.position.y + 0.7f);
        GameObject TheBoomProject = Instantiate(TheBoomPartical, NePos, Thequaternion);

        ExplosionSound.clip = Exploding;
        ExplosionSound.Play();

        CameraMove TheCamera = GameObject.Find("ImportantResource/CameraMove").GetComponent<CameraMove>();

        if (TheCamera != null)
        {
            TheCamera.ShackTime = 0.5f;
            TheCamera.Shacking = 0.5f;
            TheCamera.ShakingFunc();
        }

        Collider2D[] TheCollider = Physics2D.OverlapCircleAll(TheBoomProject.transform.position, ExplosionRadius);

        for (int i = 0; i < TheCollider.Length; i++)
        {
            Rigidbody2D TheRb = TheCollider[i].gameObject.GetComponent<Rigidbody2D>();
            TakeDamage TheDamage = TheCollider[i].GetComponent<TakeDamage>();
            Structure TheStructure = TheCollider[i].gameObject.GetComponent<Structure>();

            if (TheRb != null )
            {
                UnityEngine.Vector2 TheDirection = TheCollider[i].transform.position - TheBoomProject.transform.position;
                TheRb.AddForce(TheDirection.normalized * Maganification, ForceMode2D.Impulse);
            }

            if (TheDamage != null)
            {
                float TheDistance = UnityEngine.Vector2.Distance(TheBoomProject.transform.position, TheCollider[i].transform.position);

                if (TheDamage.TheHealthManager.Exploded == false)
                {
                    if (TheCollider[i].gameObject.tag == "Player")
                    {
                        RingingSource.clip = TheRingSound;
                        RingingSource.Play();
                    }
                    TheDamage.TheHealthManager.Exploded = true;
                    TheDamage.TakingDamage((1 - (TheDistance / ExplosionRadius)) * MaxDamage);

                    TheDamage.TheStatuseManager.Collapse();
                    //导致内脏损伤
                    TheDamage.TheStatuseManager.IsInternalInjures = true;
                    TheDamage.TheStatuseManager.InternalInjures_TimeToRecover = 120f;
                    TheDamage.TheStatuseManager.InternalInjures_TimerStart = false;
                    TheDamage.TheStatuseManager.InternalInjures_Severity += 6 * (1 - (TheDistance / ExplosionRadius));

                }

                //导致烧伤(会使多个肢体同时造成烧伤)
                TheDamage.BurnSeverity += 1.5f * (1 - (TheDistance / ExplosionRadius));

                if (TheStructure != null)
                {

                    TheStructure.TakeDamage_ST((1 - (TheDistance / ExplosionRadius)) * Damage_ST);
                }
            }


        }
    }


}
