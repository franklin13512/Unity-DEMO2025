using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Player : MonoBehaviour
{
    public PrefabList ThePrefabList;
    public WatchMouse Watch;
    public HealthManager TheHealthManager;

    public GameObject TItem;
    public Item TheItem;

    public GameObject TheInventoryStorage;
    public GameObject TheHair;   // 头发图层，戴帽子时应隐藏

    public Rigidbody2D[] TheLimbRbs;

    // ---- 兼容旧接口：物品与身体数据现在都存放在 HealthManager 上 ----
    // 这样 NPC / 敌人（没有 Player 组件）也能持有物品、拥有身体碰撞体。
    public GameObject ThingInHand
    {
        get { return TheHealthManager != null ? TheHealthManager.ThingInHand : null; }
        set { if (TheHealthManager != null) TheHealthManager.ThingInHand = value; }
    }

    public BoxCollider2D BodyBox
    {
        get { return TheHealthManager != null ? TheHealthManager.BodyBox : null; }
        set { if (TheHealthManager != null) TheHealthManager.BodyBox = value; }
    }

    public static T FindComponentInAncestors<T>(Transform start) where T : Component
    {
        Transform current = start.parent;

        while (current != null)
        {
            T found = current.GetComponent<T>();
            if (found != null)
                return found;

            current = current.parent;
        }

        return null;   // 所有父级都没有找到
    }

    private void OnValidate()
    {
        TheHealthManager = gameObject.GetComponent<HealthManager>();
    }
}

