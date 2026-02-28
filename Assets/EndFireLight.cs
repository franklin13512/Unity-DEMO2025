using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndFireLight : MonoBehaviour
{
    public GameObject TheEndFireLight;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Instantiate(TheEndFireLight,gameObject.transform.position, gameObject.transform.rotation);
        Destroy(gameObject);
    }

}
