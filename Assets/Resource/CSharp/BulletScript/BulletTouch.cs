using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class BulletTouch : MonoBehaviour
{
    public GameObject FireLight;
    public float Damage;
    public bool Touched = false;
    private Rigidbody2D Rb;
    private RaycastHit2D Hit;

    // Start is called before the first frame update
    private void Awake()
    {
        Rb = gameObject.GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector2 BoxSize = new Vector2(0.4f, 0.4f);
        Vector2 Direction = Rb.velocity.normalized;
        float Distance = Rb.velocity.magnitude * Time.deltaTime;

        Hit = Physics2D.BoxCast(transform.position, BoxSize, 0, Direction);
        if (Hit.collider != null && Hit.collider.gameObject.tag == "Hostile")
        {
            Rb.bodyType = RigidbodyType2D.Static;
            Destroy(gameObject);
        }


    }

    //public void (Collider2D collision)
    //{
    //    //if (collision.gameObject.tag != "Creature" && (collision.gameObject.tag != "Hostile" || collision.gameObject.tag != "Player"))
    //    //{
    //    //    StartCoroutine(WaitForDestroy());
    //    //}
    //    //else if (collision.gameObject.tag == "Hostile")
    //    //{
    //    //    StartCoroutine(WaitForHitHuman());

    //    //}
    //    //else
    //    //{
    //    //    StartCoroutine(WaitForDestroy_NoLight());
    //    //}



    //}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        StartCoroutine(WaitForHitHuman(collision.gameObject));
    }

    //public void OnCollisionEnter2D(UnityEngine.Collision2D collision)
    //{

    //}

    IEnumerator WaitForDestroy()
    {
        FireLight.gameObject.SetActive(true);
        yield return new WaitForSeconds(1 / 10);
        Destroy(gameObject);
    }

    IEnumerator WaitForHitHuman(GameObject TheObj)
    {
        FireLight.gameObject.SetActive(true);
        yield return new WaitForSeconds(1 / 10);

        if(TheObj)
        {
            TakeDamage TheTakeDamage = TheObj.gameObject.GetComponent<TakeDamage>();
            if (TheTakeDamage != null)
            {
                TheTakeDamage.TakingDamage(Damage);
            }
        }


        Destroy(gameObject);
    }

    IEnumerator WaitForDestroy_NoLight()
    {
        yield return new WaitForSeconds(1 / 10);
        Destroy(gameObject);
    }

    IEnumerator Unfreeze(Rigidbody2D Rb)
    {
        yield return new WaitForSeconds(0.0001f);
        Rb.bodyType = RigidbodyType2D.Dynamic;
    }
}

