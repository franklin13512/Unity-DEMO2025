using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Player : MonoBehaviour
{
    public GameObject ThingInHand;

    public PrefabList ThePrefabList;
    public WatchMouse Watch;
    public HealthManager TheHealthManager;

    public GameObject TItem;
    public Item TheItem;

    public GameObject RightHandPos;
    public BoxCollider2D HandPos;
    public Rigidbody2D RightHandRig;
    public FixedJoint2D TheJoin2D;

    public BoxCollider2D BodyBox;

    public GameObject TheInventoryStorage;
    public GameObject TheHair;//发型图层，在戴帽子时应当控制

    public Rigidbody2D[] TheLimbRbs;

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

        return null; // 所有父物体都没有找到
    }

    private void OnValidate()
    {
        TheHealthManager = gameObject.GetComponent<HealthManager>();

    }

    public void Update()
    {
        if (TheHealthManager.ThingInHand != ThingInHand)
        {
            ThingInHand = TheHealthManager.ThingInHand;

            if (ThingInHand != null)
            {
                FixedJoint2D TheJoint = ThingInHand.GetComponent<FixedJoint2D>();
                if (TheJoint && ThingInHand)
                {
                    TheJoin2D = TheJoint;
                }
            }

        }

        if (ThingInHand != null && TheJoin2D)
        {
            ThingInHand.transform.position = TheJoin2D.transform.position;
        }
    }



}
