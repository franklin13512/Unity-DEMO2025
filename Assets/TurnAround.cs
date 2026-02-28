using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TurnAround : MonoBehaviour
{
    public Rigidbody2D TheRb;//负责移动的部位
    public Vector2 TheNormalScale;
    public bool IsTurned;
    public BoxCollider2D BC;

    public Vector2 TargetCorodinate;
    public bool Turning;

    public float MinusTimer;
    public float PosTimer;
    public float MaxTime;

    // Start is called before the first frame update
    void Start()
    {
        TheNormalScale = gameObject.transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        if (TheRb.velocity.x > 0)
        {
            PosTimer += Time.deltaTime;
            if (PosTimer > MaxTime)
            {
                MinusTimer = 0;
            }
        }
        else if(TheRb.velocity.x < 0)
        {
            MinusTimer += Time.deltaTime;
            if (MinusTimer > MaxTime)
            {
                PosTimer = 0;
            }
        }



        TargetCorodinate = BC.bounds.center;
        //Debug.Log("1: " + TheRb.velocity.x);

        if (PosTimer > MaxTime && IsTurned)
        {


            IsTurned = false;
            GameObject NewPlayer = new GameObject("NPC00");

            NewPlayer.transform.position = new Vector2(TargetCorodinate.x, 0);

            gameObject.transform.SetParent(NewPlayer.transform);
            NewPlayer.transform.localScale = new Vector2(-TheNormalScale.x, TheNormalScale.y);

            gameObject.transform.SetParent(null);
            Destroy(NewPlayer);

        }
        else if (MinusTimer > MaxTime && !IsTurned)
        {

            IsTurned = true;
            GameObject NewPlayer = new GameObject("NPC00");

            NewPlayer.transform.position = new Vector2(TargetCorodinate.x, 0);

            gameObject.transform.SetParent(NewPlayer.transform);
            NewPlayer.transform.localScale = new Vector2(-TheNormalScale.x, TheNormalScale.y);

            gameObject.transform.SetParent(null);
            Destroy(NewPlayer);

        }

    }
}
