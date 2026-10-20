using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainDetector : MonoBehaviour
{
    [SerializeField] private LayerMask terrainLayer;
    [SerializeField] private float raycastDistance = 2f;

    // 检测指定位置的地面高度
    public Vector2 GetGroundPoint(Vector2 origin)
    {
        RaycastHit2D hit = Physics2D.Raycast(origin, Vector2.down, raycastDistance, terrainLayer);
        if (hit.collider != null)
            return hit.point;

        // 没检测到地面，返回虚拟远点（会触发步态）
        return origin + Vector2.down * float.MaxValue;
    }

    // 获取地面法线，用于脚部旋转
    public Vector2 GetGroundNormal(Vector2 origin)
    {
        RaycastHit2D hit = Physics2D.Raycast(origin, Vector2.down, raycastDistance, terrainLayer);
        return hit.normal;
    }
}
