using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Detech : MonoBehaviour
{
    public Rigidbody2D Bullet;

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
        Bullet.bodyType = RigidbodyType2D.Static;
    }
}
