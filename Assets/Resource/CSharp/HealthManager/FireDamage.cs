using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireDamage : MonoBehaviour
{
    public float Damage;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerStay2D(Collider2D collision)
    {
        TakeDamage TheTakeDamage = collision.gameObject.GetComponent<TakeDamage>();
        if (TheTakeDamage != null)
        {
            TheTakeDamage.BurnSeverity += 0.01f * Time.deltaTime;
            StartCoroutine(WaitForNextDamage(TheTakeDamage));
        }
    }

    IEnumerator WaitForNextDamage(TakeDamage TheTakeDamage)
    {
        yield return new WaitForSeconds(2f);
        TheTakeDamage.TakingDamage(Damage);
    }
}
