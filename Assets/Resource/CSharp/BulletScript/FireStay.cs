using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireStay : MonoBehaviour
{
    //public GameObject FireLight;
    public float Damage;

    public float StayTime;

    //TheChance>= 1, TheChance越小，子弹引火的概率越大,等于1时必然引火
    public float TheChance;

    public bool Touched = false;
    private Rigidbody2D Rb;
    private RaycastHit2D Hit;

    public GameObject TheFire;

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
        if (Hit.collider != null && (Hit.collider.gameObject.tag == "Hostile" || Hit.collider.gameObject.tag == "Player" || Hit.collider.gameObject.tag == "Friendly"))
        {
            Rb.bodyType = RigidbodyType2D.Static;

            Rigidbody2D BulletRb = gameObject.GetComponent<Rigidbody2D>();
            BulletRb.bodyType = RigidbodyType2D.Static;

            float TheRandom = Random.Range(0, TheChance);
            if (TheRandom <= 1)
            {
                GameObject TheStayFire = Instantiate(TheFire, gameObject.transform.position, TheFire.transform.rotation);

                Rigidbody2D TheRb = Hit.collider.gameObject.GetComponent<Rigidbody2D>();

                if (TheRb != null)
                {
                    DistanceJoint2D TheFixed = TheStayFire.gameObject.AddComponent<DistanceJoint2D>();
                    TheFixed.connectedBody = TheRb;
                    TheFixed.autoConfigureDistance = false;
                    TheFixed.distance = 0;
                }

                //TheStayFire.transform.SetParent(Hit.collider.gameObject.transform);

                Destroy(TheStayFire, StayTime);
            }


            Rigidbody2D MyRb = gameObject.GetComponent<Rigidbody2D>();
            MyRb.bodyType = RigidbodyType2D.Static;

            StartCoroutine(WaitForEnd());

        }
    }


    public void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.tag != "Creature" && (collision.gameObject.tag != "Hostile" || collision.gameObject.tag != "Player"))
        {
            Rigidbody2D BulletRb = gameObject.GetComponent<Rigidbody2D>();
            BulletRb.bodyType = RigidbodyType2D.Static;


            float TheRandom = Random.Range(0, TheChance);
            if (TheRandom <= 1)
            {
                GameObject TheStayFire = Instantiate(TheFire, gameObject.transform.position, TheFire.transform.rotation);

                Rigidbody2D TheRb = TheStayFire.gameObject.GetComponent<Rigidbody2D>();
                TheRb.bodyType = RigidbodyType2D.Static;

                Destroy(TheStayFire, StayTime);
            }

            Rigidbody2D MyRb = gameObject.GetComponent<Rigidbody2D>();
            MyRb.bodyType = RigidbodyType2D.Static;

            StartCoroutine(WaitForEnd());


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

    IEnumerator WaitForDestroy(GameObject TheStayFire)
    {
        //FireLight.gameObject.SetActive(true);
        yield return new WaitForSeconds(1 / 10);
        Destroy(TheStayFire, StayTime);
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

    IEnumerator WaitForEnd()
    {
        yield return new WaitForSeconds(0.05f);
        Destroy(gameObject);
    }
}
