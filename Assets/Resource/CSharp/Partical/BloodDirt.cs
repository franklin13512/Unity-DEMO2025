using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BloodDirt : MonoBehaviour
{
    public GameObject TheBloodSprite;
    private List<ParticleCollisionEvent> TheCollison;
    ParticleSystem TheParticle;

    public StatusManager TheStatus;
    // Start is called before the first frame update
    void Start()
    {
        TheCollison = new List<ParticleCollisionEvent>();
        TheParticle = gameObject.GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.transform.parent != null && TheStatus != null)
        {
            if (TheStatus.StartBleeding == false)
            {
                Destroy(gameObject);
            }
        }
    }

    private void OnParticleCollision(GameObject other)
    {

        int NumOfEvents = ParticlePhysicsExtensions.GetCollisionEvents(TheParticle, other, TheCollison);

        for (int i = 0; i < NumOfEvents; i++)
        {
            float WhetherSpawn = Random.Range(0, 35);
            Vector2 HitPoint = TheCollison[i].intersection;

            if (WhetherSpawn < 1)
            {

                GameObject TheBlood = Instantiate(TheBloodSprite, HitPoint, new Quaternion(0, 0, 0, 0));
                TheBlood.transform.SetParent(other.transform);

                //BloodDetech TheDetactor = collision.transform.GetComponent<BloodDetech>();

                if (TheBlood.transform.parent.gameObject.layer != LayerMask.NameToLayer("Default"))
                {
                    SpriteRenderer TheSprite = TheBlood.GetComponent<SpriteRenderer>();
                    if (TheSprite != null)
                    {
                        TheSprite.sortingLayerName = "BloodDirt";
                    }
                }

            }
        }

        //Destroy(gameObject);
    }
}

//    public void (Collider2D collision)
//    {
//        float WhetherSpawn = Random.Range(0, 70);
//        if (WhetherSpawn < 1)
//        {
//            GameObject TheBlood = Instantiate(TheBloodSprite, transform.position, new Quaternion(0, 0, 0, 0));
//            TheBlood.transform.SetParent(collision.transform);

//            //BloodDetech TheDetactor = collision.transform.GetComponent<BloodDetech>();

//            if (TheBlood.transform.parent.gameObject.layer != LayerMask.NameToLayer("Default"))
//            {
//                SpriteRenderer TheSprite = TheBlood.GetComponent<SpriteRenderer>();
//                if (TheSprite != null)
//                {
//                    TheSprite.sortingLayerName = "BloodDirt";
//                }
//            }

//            //if (TheDetactor != null)
//            //{
//            //    TheDetactor.RemainBlood++;
//            //    TheDetactor.TheSpriteMask.enabled = true;
//            //}
//}
//        Destroy(gameObject);
//    }
//}
