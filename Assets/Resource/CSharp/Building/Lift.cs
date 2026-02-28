using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class Lift : Structure
{
    public GameObject[] TheTargetPoint;
    public GameObject CurrentTarget;
    public int PresentTarget;//当前所在层
    public bool Finished;//到达指定层

    public GameObject Canvas;
    public GameObject UIPrefab;

    public GameObject PrefabInScene;

    public float TheRadius;//超出范围ui关闭
    public LayerMask PlayerLayer;

    public Rigidbody2D Rb;

    // Start is called before the first frame update
    void Start()
    {
        if(TheTargetPoint.Length > 0)
            PresentTarget = 0;

        Finished = true;
        IsOpen = false;

        Rb = gameObject.GetComponent<Rigidbody2D>();
        if (Rb != null )
        {
            Rb.bodyType = RigidbodyType2D.Kinematic;
        }
    }

    private void Update()
    {
        Collider2D[] TheCollier = Physics2D.OverlapCircleAll(gameObject.transform.position, TheRadius, PlayerLayer);

        bool playerFound = false;

        foreach (Collider2D col in TheCollier)
        {
            if (col.CompareTag("Player"))
            {
                playerFound = true;
                break;
            }
        }

        if ((!playerFound && IsOpen) || Input.GetKeyDown(KeyCode.Escape))
        {

            IsOpen = false;
            if (PrefabInScene != null)
            {
                Destroy(PrefabInScene);
            }

        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!Finished && CurrentTarget != null)
        {
            MoveTheLift(CurrentTarget.transform);
        }
    }

    public void MoveTheLift(Transform ThePoint)
    {
        if (Rb != null)
        {
            // 平滑移动：逐渐靠近目标
            Vector2 newPos = Vector2.MoveTowards(
                Rb.position,
                ThePoint.position,
                2f * Time.fixedDeltaTime // 速度可调
            );

            Rb.MovePosition(newPos);

            // 判断是否到达
            if (Vector2.Distance(Rb.position, ThePoint.position) < 0.01f)
            {
                Finished = true;
                CurrentTarget = null;
            }
        }
    }

    public void PressUp()
    {
        if (PresentTarget + 1 < TheTargetPoint.Length)
        {
            CurrentTarget = TheTargetPoint[++PresentTarget];
            Finished = false;
        }
    }

    public void PressDown()
    {
        if (PresentTarget > 0)
        {
            CurrentTarget = TheTargetPoint[--PresentTarget];
            Finished = false;
        }
    }

    public override void Interact() 
    {
        // 在 Canvas 下生成
        GameObject UI = Instantiate(UIPrefab, Canvas.transform);
        PrefabInScene = UI;
        RectTransform rect = UI.GetComponent<RectTransform>();
        //// 设置锚点到中心
        //rect.anchorMin = new Vector2(0.5f, 0.5f);
        //rect.anchorMax = new Vector2(0.5f, 0.5f);

        // 设置位置为 (0,0)
        rect.anchoredPosition = Vector2.zero;


        LiftUI TheLiftUI = UI.GetComponent<LiftUI>();
        TheLiftUI.UpButton.onClick.AddListener(PressUp);
        TheLiftUI.DownButton.onClick.AddListener(PressDown);

        return;
    }

}
