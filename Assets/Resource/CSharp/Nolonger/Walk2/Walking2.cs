using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Walking2 : MonoBehaviour
{
    public GameObject CenterPoint;
    public GameObject RightPoint;
    public GameObject LeftPoint;

    public float Angle;
    public float WalkSpeed;

    public float Longa;
    public float Shortb;

    public Rigidbody2D RightLeg;
    public Rigidbody2D LeftLeg;

    private Rigidbody2D RightPointRig;
    private Rigidbody2D LeftPointRig;

    private Vector2 StartPosRight;
    private Vector2 StartPosLeft;

    public bool HasRecovered1;
    public bool HasRecovered2;

    public bool Walking1Finished;

    public TargetJoint2D TargetJointLeft;
    public TargetJoint2D TargetJointRight;


    // Start is called before the first frame update
    void Start()
    {
        Walking1Finished = false;

        RightPointRig = RightPoint.GetComponent<Rigidbody2D>();
        LeftPointRig = LeftPoint.GetComponent<Rigidbody2D>();

        StartPosRight = new Vector2(CenterPoint.transform.position.x + Longa, CenterPoint.transform.position.y );
        RightPoint.transform.position = StartPosRight;

        StartPosLeft = new Vector2(CenterPoint.transform.position.x - Longa, CenterPoint.transform.position.y );
        LeftPoint.transform.position = StartPosLeft;

        Angle = -Mathf.PI / 2;
        Angle += WalkSpeed * Time.deltaTime;
    }

    // Update is called once per frame
    void Update()
    {
        TargetJointLeft.target = LeftPoint.transform.position;
        TargetJointRight.target = RightPoint.transform.position;

        if (Input.GetButton("Horizontal") && !Input.GetKeyDown(KeyCode.LeftShift))
        {
            if (Walking1Finished == false)
            {
                Walking01();
            }else if (Walking1Finished == true)
            {
                Walking02();
            }
        }
        else
        {
            Walking1Finished = false;

            StartPosRight = new Vector2(CenterPoint.transform.position.x + Longa, CenterPoint.transform.position.y );
            RightPoint.transform.position = StartPosRight;

            StartPosLeft = new Vector2(CenterPoint.transform.position.x - Longa, CenterPoint.transform.position.y );
            LeftPoint.transform.position = StartPosLeft;
        }
    }

    public void Walking01()
    {

        StartPosRight = new Vector2(CenterPoint.transform.position.x + Longa, CenterPoint.transform.position.y );
        StartPosLeft = new Vector2(CenterPoint.transform.position.x - Longa, CenterPoint.transform.position.y );

        if (HasRecovered1 == false)
        {
            RightPoint.transform.position = StartPosRight;
            LeftPoint.transform.position = StartPosLeft;
            HasRecovered1 = true;
            HasRecovered2 = false;
        }

        Vector2 OldPos = LeftPoint.transform.position;

        Angle += WalkSpeed * Time.deltaTime;

        Vector2 NewLeftPos = new Vector2(Mathf.Sin(Angle) * Longa, Mathf.Cos(Angle) * Shortb);
        LeftPoint.transform.position = (Vector2)CenterPoint.transform.position + NewLeftPos;

        float Distance = LeftPoint.transform.position.x - OldPos.x;

        RightPoint.transform.position = Vector2.MoveTowards(RightPoint.transform.position, StartPosLeft,0.2f * WalkSpeed * Time.deltaTime);


        if (LeftPoint.transform.position.x >= StartPosRight.x - 0.1 && RightPoint.transform.position.x <= StartPosLeft.x + 0.1)
        {
            Walking1Finished = true;
            Angle = -Mathf.PI / 2;
        }
    }



    public void Walking02() 
    {

        StartPosLeft = new Vector2(CenterPoint.transform.position.x + Longa, CenterPoint.transform.position.y );
        StartPosRight = new Vector2(CenterPoint.transform.position.x - Longa, CenterPoint.transform.position.y );

        if (HasRecovered2 == false)
        {
            RightPoint.transform.position = StartPosRight;
            LeftPoint.transform.position = StartPosLeft;
            HasRecovered2 = true;
            HasRecovered1 = false;
        }

        Vector2 OldPos2 = RightPoint.transform.position;

        Angle += WalkSpeed * Time.deltaTime;

        Vector2 NewRightPos = new Vector2(Mathf.Sin(Angle) * Longa, Mathf.Cos(Angle) * Shortb);
        RightPoint.transform.position = (Vector2)CenterPoint.transform.position + NewRightPos;

        float Distance2 = RightPoint.transform.position.x - OldPos2.x;

        LeftPoint.transform.position = Vector2.MoveTowards(LeftPoint.transform.position, StartPosRight, 0.2f * WalkSpeed * Time.deltaTime);

        if (LeftPoint.transform.position.x <= StartPosRight.x + 0.1 && RightPoint.transform.position.x >= StartPosLeft.x )
        {
            Debug.Log(100);
            Walking1Finished = false;
            Angle = -Mathf.PI / 2;
        }
    }
}
