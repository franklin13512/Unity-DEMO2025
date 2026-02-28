using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopForWhile : MonoBehaviour
{
    public GameObject CurrentPos;
    public Rigidbody2D RB;

    void Start()
    {
        StartCoroutine(StopForAWhile());
        StopAllCoroutines();
    }


    IEnumerator StopForAWhile()
    {
        RB.bodyType = RigidbodyType2D.Static;

        yield return new WaitForSeconds(3);

        RB.bodyType = RigidbodyType2D.Dynamic;

    }

}
