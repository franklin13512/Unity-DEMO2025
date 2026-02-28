using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class ExplosionLight : MonoBehaviour
{
    public Light2D TheLight;
    public float LightingTime;

    public float DestroyTime;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, DestroyTime);
        TheLight.enabled = true;
        StartCoroutine(TurnOnLight());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator TurnOnLight()
    {
        yield return new WaitForSeconds(LightingTime);
        TheLight.enabled = false;
    }
}
