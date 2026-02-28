using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodPart : MonoBehaviour
{
    public GameObject BloodBlock;
    public GameObject[] BloodPoint;
    public GameObject TheBlood;

    public Rigidbody2D BloodRig;

    public float Duration;
    public bool Bleeding;
    public Vector2 Magnification;

    public float DestroyTime;
    public float MaintainTime;

    public bool BloodPermission;

    public bool IsBleedable;
    // Start is called before the first frame update
    void Start()
    {
        if (gameObject.transform.parent != null)
        {
            TakeDamage TheTakeDamage = gameObject.transform.parent.gameObject.GetComponent<TakeDamage>();
            if (TheTakeDamage != null)
            {
                IsBleedable = true;
            }
        }

        Bleeding = false;
        BloodRig = BloodBlock.gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        //当流血恢复或者死亡时，停止生成血液
        if (IsBleedable)
        {
            TakeDamage TheTakeDamage = gameObject.transform.parent.gameObject.GetComponent<TakeDamage>();
            if ((TheTakeDamage.TheStatuseManager.BleedingTime <= 0 || TheTakeDamage.TheHealthManager.Health <= 0) && TheTakeDamage != null)
            {
                Destroy(gameObject);
            }
        }

        if (Bleeding == false )
        {
            StartCoroutine(ContinueBleed());
        }
    }

    IEnumerator ContinueBleed()
    {
        Bleeding = true;

        StartCoroutine(WaitForNextBlood());

        while (BloodPermission == false)
        {
            for (int i = 0; i < BloodPoint.Length; i++)
            {
                TheBlood = Instantiate(BloodBlock, BloodPoint[i].transform.position, BloodPoint[i].transform.rotation);
                Rigidbody2D TheBloodRig = TheBlood.gameObject.GetComponent<Rigidbody2D>();
                TheBloodRig.velocity = Magnification;


                yield return new WaitForSeconds(Duration);
                Destroy(TheBlood, DestroyTime);
            }
        }

        //Bleeding = true;

    }

    IEnumerator WaitForNextBlood()
    {

        yield return new WaitForSeconds(MaintainTime);
        BloodPermission = true;
    }
}
