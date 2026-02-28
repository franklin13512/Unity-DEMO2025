using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkirtChecker : MonoBehaviour
{

    public float DistanceToKeep = 0.3f;
    public float RotateSpeed = 1f;
    public float MinAngle = 33f;
    public float DeadAngel = 1f;

    public bool IsForward;
    [SerializeField] Transform SkirtCheckPoint;
    [SerializeField] Transform[] LegCheckPoint;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float Distance01 = Mathf.Sqrt(Mathf.Pow((SkirtCheckPoint.position.x - LegCheckPoint[0].position.x),2) + Mathf.Pow((SkirtCheckPoint.position.y - LegCheckPoint[0].position.y),2));
        float Distance02 = Mathf.Sqrt(Mathf.Pow((SkirtCheckPoint.position.x - LegCheckPoint[1].position.x), 2) + Mathf.Pow((SkirtCheckPoint.position.y - LegCheckPoint[1].position.y), 2));

        float currentZAngle = Mathf.DeltaAngle(-90f, transform.eulerAngles.z);

        if (IsForward == true)
        {
            if (Distance01 <= DistanceToKeep || Distance02 <= DistanceToKeep && currentZAngle < MinAngle + DeadAngel)
            {
                transform.Rotate(0f, 0f, RotateSpeed * Time.deltaTime);
            }
            else
            {
                if (Distance01 > DistanceToKeep && Distance02 > DistanceToKeep && currentZAngle > MinAngle)
                {
                    transform.Rotate(0f, 0f, -RotateSpeed * Time.deltaTime);

                }
            }
        }else if (IsForward == false)
        {
            if (Distance01 <= DistanceToKeep || Distance02 <= DistanceToKeep && currentZAngle > MinAngle - DeadAngel)
            {
                transform.Rotate(0f, 0f, -RotateSpeed * Time.deltaTime);
            }
            else
            {
                if (Distance01 > DistanceToKeep && Distance02 > DistanceToKeep && currentZAngle < MinAngle)
                {
                    transform.Rotate(0f, 0f, RotateSpeed * Time.deltaTime);

                }
            }
        }

    }
}
