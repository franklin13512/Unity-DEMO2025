using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class CameraMove : MonoBehaviour
{
    public WatchMouse Watch;
    public bool IsForward;

    public BoxCollider2D Body;

    private Vector2 vect;
    private Vector2 vect2;

    private Vector3 PosLeft;
    private Vector3 PosRight;

    public float MoveSpeed;
    public float MoveSpeed2;

    public Vector3 Distance;
    public Vector3 Distance2;
    public Vector3 Distance3;
    public Vector2 CurPos;

    public Vector2 ReservedLeft;
    public Vector2 ReservedRight;

    public bool Changed01;
    public bool Changed02;

    public float BoundValue;
    public float MaxBound;
    public float MinBound;
    public float BoundY;

    public float ShackTime;
    public float Shacking;

    public SpriteRenderer DeepBackGround;

    private LinkedList<SpriteRenderer> Front = new LinkedList<SpriteRenderer>();
    public SpriteRenderer Front1;
    public SpriteRenderer Front2;
    public SpriteRenderer Front3;
    public SpriteRenderer CurrentFront;

    private LinkedList<SpriteRenderer> Back = new LinkedList<SpriteRenderer>();
    public SpriteRenderer Back1;
    public SpriteRenderer Back2;
    public SpriteRenderer Back3;
    public SpriteRenderer CurrentBack;

    // Start is called before the first frame update
    void Start()
    {
        IsForward = Watch.IsTurned;

        Distance = transform.position - Body.bounds.center;

        PosRight = transform.position;
        PosLeft = new Vector2(2 * Body.bounds.center.x - transform.position.x, transform.position.y);

        Distance2 = PosLeft - Body.bounds.center;
        Distance3 = PosRight - Body.bounds.center;

        Changed01 = false;
        Changed02 = true;
        CurPos = transform.position;

        Front.AddFirst(Front3);
        Front.AddFirst(Front2);
        Front.AddFirst(Front1);

        CurrentFront = Front2;

        Back.AddFirst(Back3);
        Back.AddFirst(Back2);
        Back.AddFirst(Back1);

        CurrentBack = Back2;

    }

    // Update is called once per frame
    void Update()
    {

        BoundY = Watch.TheMouseCorodinate.y;
        MaxBound = Body.bounds.center.y + BoundValue;
        MinBound = Body.bounds.center.y - BoundValue;

        IsForward = Watch.IsTurned;

        PosLeft = Distance2 + Body.bounds.center;
        PosRight = Distance3 + Body.bounds.center;

        ReservedLeft = PosLeft;
        ReservedRight = PosRight;

        DeepBackGround.gameObject.transform.position = gameObject.transform.position;



        if (Input.GetMouseButton(1))
        {
            if (Watch.TheMouseCorodinate.y >= MaxBound )
            {
                BoundY = MaxBound;
            }else if (Watch.TheMouseCorodinate.y <= MinBound)
            {
                BoundY = MinBound;
            }
            PosLeft = new Vector2(PosLeft.x, BoundY);
            PosRight = new Vector2(PosRight.x, BoundY);
        }
        else
        {
            PosLeft = ReservedLeft;
            PosRight = ReservedRight;
        }

        if (IsForward == false)
        {
            if (IsForward == false && Changed01)
            {

                Changed01 = false;
                Changed02 = true;
                transform.position = Vector2.SmoothDamp(transform.position, PosRight, ref vect, MoveSpeed);

            }

            Distance = transform.position - Body.bounds.center;
            Vector2 OldPos = transform.position;
            transform.position = Distance + Body.bounds.center;
            Vector2 NewPos = transform.position;
            transform.position = Vector2.SmoothDamp(OldPos, PosRight, ref vect, MoveSpeed);

        }else if (IsForward == true)
        {
            if (IsForward == true && Changed02)
                {

                Changed01 = true;
                Changed02 = false;

                transform.position = Vector2.SmoothDamp(transform.position, PosLeft, ref vect, MoveSpeed);
                }

            Distance = transform.position - Body.bounds.center;
            Vector2 OldPos = transform.position;
            transform.position = Distance + Body.bounds.center;
            Vector2 NewPos = transform.position;
            transform.position = Vector2.SmoothDamp(OldPos, PosLeft, ref vect, MoveSpeed);
        }
    }

    public void ShakingFunc()
    {
        StartCoroutine(WaitForShack());
    }

    IEnumerator WaitForShack()
    {
        Vector2 OldPos = transform.position;

        float StartTime = 0;
        while (StartTime < ShackTime)
        {
            float x = Random.Range(-1f, 1f) * Shacking;
            float y = Random.Range(-1f, 1f) * Shacking;

            transform.position  = new Vector2(OldPos.x + x, OldPos.y + y);
            StartTime += Time.deltaTime;

            //yield return new WaitForSeconds(0.05f);
            yield return null;
            //yield return null;
        }
    }

    private void LateUpdate()
    {
        Watch.TheMouseCorodinate = Camera.main.ScreenToWorldPoint(Input.mousePosition);


        MoveBackGround(Front, ref CurrentFront);
        MoveBackGround(Back, ref CurrentBack);

    }

    public void MoveBackGround(LinkedList<SpriteRenderer> TheList,ref SpriteRenderer Current)
    {
        float Distance = Current.transform.position.x - gameObject.transform.position.x;
        if (Mathf.Abs(Distance) > Current.bounds.size.x/2)
        {
            if (Distance <= 0)
            {
                SpriteRenderer RemovedObj = TheList.First.Value;
                TheList.RemoveFirst();
                Current = TheList.Last.Value;
                TheList.AddLast(RemovedObj);

                Vector3 NewPos = new Vector3(Current.transform.position.x + Current.bounds.size.x, Current.transform.position.y, Current.transform.position.z);
                RemovedObj.transform.position = NewPos;
            }
            else if (Distance > 0)
            {
                SpriteRenderer RemovedObj = TheList.Last.Value;
                TheList.RemoveLast();
                Current = TheList.First.Value;
                TheList.AddFirst(RemovedObj);

                Vector3 NewPos = new Vector3(Current.transform.position.x - Current.bounds.size.x, Current.transform.position.y, Current.transform.position.z);
                RemovedObj.transform.position = NewPos;
            }
        }
    }

}
