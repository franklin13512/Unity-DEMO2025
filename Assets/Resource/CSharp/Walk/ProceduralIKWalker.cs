using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProceduralIKWalker : MonoBehaviour
{
    // 指向玩家角色
    public GameObject playerObj;

    // 指向IK目标
    public Transform target;

    // 指向另一只脚的"FootPositioner" class
    public ProceduralIKWalker otherFoot;

    public bool isBalanced;

    // 用于迈出每一步的动作过程
    public float lerp;

    // 每一步的开始和结束位置
    private Vector3 startPos;
    private Vector3 endPos;
    private Vector3 midPos;

    // 每一步应该往前迈多少距离
    public float overShootFactor = 0.8f;

    // 脚移动的有多快
    public float stepSpeed = 3f;

    public float footDisplacementOnX = 0.25f;

    public float maxFootDistance = 1f;

    public LayerMask TerrainLayer;

    private void Start()
    {
        startPos = midPos = endPos = target.position;
    }

    private void Update()
    {
        UpdateBalance();

        // 如果身体失去平衡，并且已经踏完上一步了，那么可以迈出下一步
        //if (!isBalanced && lerp > 1)
        //{
        //    CalculateNewStep();
        //}

        //lerp += Time.deltaTime * stepSpeed;

        float footDistance = Vector3.Distance(target.position, otherFoot.target.position);
        bool feetTooFar = footDistance > maxFootDistance;

        bool thisFootCanMove = otherFoot.lerp > 1 && lerp > otherFoot.lerp;

        if ((!isBalanced || feetTooFar) && lerp > 1 && thisFootCanMove)
        {
            CalculateNewStep();
        }

        float easedLerp = EaseInOutCubic(lerp);

        // 这个lerp方法可以结合startPos, midPos，和endPos画出一个连接它们的弧线
        target.position = Vector3.Lerp(
            Vector3.Lerp(startPos, midPos, easedLerp),
            Vector3.Lerp(midPos, endPos, easedLerp),
            easedLerp
            );
        lerp += Time.deltaTime * stepSpeed;
    }

    /// <summary>
    /// 用Sigmoid方程来缓和一下动画
    /// </summary>
    private float EaseInOutCubic(float x)
    {
        return 1f / (1 + Mathf.Exp(-10 * (x - 0.5f)));
    }

    /// <summary>
    /// 计算新一步的起点和终点
    /// </summary>
    private void CalculateNewStep()
    {
        // set starting position
        startPos = target.position;

        // 这会让IK目标开始从起点往终点移动
        lerp = 0;

        // 计算移动终点
        RaycastHit2D ray = Physics2D.Raycast(playerObj.transform.position + new Vector3(footDisplacementOnX, 0, 0), Vector2.down, 10, TerrainLayer);
        Vector3 posDiff = ((Vector3)ray.point - target.position) * (1 + overShootFactor);
        endPos = target.position + posDiff;

        // midPos可以简单理解为startPos和endPos的中点，但是除此之外，还垂直往上挪了一点位置
        float stepSize = Vector3.Distance(startPos, endPos);
        midPos = startPos + posDiff / 2f + new Vector3(0, stepSize * 0.2f);
    }

    /// <summary>
    /// 这个会利用Unity Gizmo帮我们可视化脚的目标位置
    /// </summary>
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(endPos, 0.1f);
    }

    private void UpdateBalance()
    {
        // 获取角色在X轴上的重心位置
        //float centerOfMass = playerObj.transform.position.x;
        //// 如果重心在两只脚之间，那么身体将会处于平衡的状态
        //isBalanced = IsFloatInRange(centerOfMass, target.position.x, otherFoot.target.position.x);

        float centerOfMass = playerObj.transform.position.x;

        isBalanced = IsFloatInRange(centerOfMass, target.position.x - footDisplacementOnX, otherFoot.target.position.x - otherFoot.footDisplacementOnX);
    }

    /// <summary>
    /// 如果value在bound1和bound2之间，返回true，反之返回false
    /// </summary>
    bool IsFloatInRange(float value, float bound1, float bound2)
    {
        float minValue = Mathf.Min(bound1, bound2);
        float maxValue = Mathf.Max(bound1, bound2);
        return value > minValue && value < maxValue;
    }

}