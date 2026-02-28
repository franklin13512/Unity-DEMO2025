using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletStay : MonoBehaviour
{
    //public GameObject FireLight;
    public float Damage;

    public float StayTime;
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
    void Update()
    {
        Vector2 BoxSize = new Vector2(0.4f, 0.4f);
        Vector2 Direction = Rb.velocity.normalized;
        float Distance = Rb.velocity.magnitude * Time.deltaTime;

        Hit = Physics2D.BoxCast(transform.position, BoxSize, 0, Direction);
        if (Hit.collider != null && Hit.collider.gameObject.tag == "Hostile")
        {
            Rb.bodyType = RigidbodyType2D.Static;

            Rigidbody2D BulletRb = gameObject.GetComponent<Rigidbody2D>();
            BulletRb.bodyType = RigidbodyType2D.Static;
            gameObject.transform.SetParent(Hit.collider.gameObject.transform);

            BoxCollider2D BulletBox = gameObject.GetComponent<BoxCollider2D>();
            BulletBox.enabled = false;

            Destroy(gameObject,StayTime);
        }
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag != "Creature" && (collision.gameObject.tag != "Hostile" || collision.gameObject.tag != "Player"))
        {
            Rigidbody2D BulletRb = gameObject.GetComponent<Rigidbody2D>();
            BulletRb.bodyType = RigidbodyType2D.Static;
            gameObject.transform.SetParent(collision.gameObject.transform);

            BoxCollider2D BulletBox = gameObject.GetComponent<BoxCollider2D>();
            BulletBox.enabled = false;
            StartCoroutine(WaitForDestroy());
        }
        else if (collision.gameObject.tag == "Hostile")
        {
            StartCoroutine(WaitForHitHuman());

        }
        else
        {
            StartCoroutine(WaitForDestroy_NoLight());
        }
    }

    IEnumerator WaitForDestroy()
    {
        //FireLight.gameObject.SetActive(true);
        yield return new WaitForSeconds(1 / 10);
        Destroy(gameObject, StayTime);
    }

    IEnumerator WaitForHitHuman()
    {
        //FireLight.gameObject.SetActive(true);
        yield return new WaitForSeconds(1 / 10);
        Destroy(gameObject, StayTime);
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
