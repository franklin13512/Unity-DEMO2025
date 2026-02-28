using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class DestoryDirt2 : MonoBehaviour
{
    public float DestroyTime = 2;


    // Start is called before the first frame update
    void Start()
    {


        StartCoroutine(Waiting());
        Destroy(gameObject, DestroyTime);
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {

        if (collider.gameObject.tag == gameObject.tag)
        {
            Destroy(collider.gameObject);

        }

    }



    IEnumerator Waiting()
    {
        yield return new WaitForSeconds(DestroyTime + 1);
        GameObject TheP = gameObject.transform.parent.gameObject;
        if (TheP != null)
        {
            BloodDetech TheDetactor = TheP.gameObject.GetComponent<BloodDetech>();
            if (TheDetactor != null)
            {
                StartCoroutine(WaiForMinus());
            }
        }
    }

    IEnumerator WaiForMinus()
    {
        yield return new WaitForSeconds(DestroyTime - 5);

        GameObject TheP = gameObject.transform.parent.gameObject;
        if (TheP != null)
        {
            BloodDetech TheDetactor = TheP.gameObject.GetComponent<BloodDetech>();
            if (TheDetactor != null)
            {
                --TheDetactor.RemainBlood;
            }
        }


    }



}
