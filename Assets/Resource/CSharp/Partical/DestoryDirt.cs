using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestoryDirt : MonoBehaviour
{
    public float DestroyTime;
    public float DetectRadio = 0.08f;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnBlood(gameObject.transform.position));
        Destroy(gameObject, DestroyTime);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //private void OnTriggerEnter2D(Collider2D collider)
    //{

    //    if (collider.gameObject.tag == gameObject.tag)
    //    {
    //        Destroy(collider.gameObject);

    //    }

    //}

    IEnumerator SpawnBlood(Vector3 position)
    {
        GameObject[] bloods = GameObject.FindGameObjectsWithTag("Dirt");

        foreach (GameObject b in bloods)
        {
            float dist = Vector3.Distance(position, b.transform.position);
            if (dist < DetectRadio && dist != 0)
            {
                Destroy(b);
            }
        }

        yield return null;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(gameObject.transform.position, DetectRadio);
    }


}
