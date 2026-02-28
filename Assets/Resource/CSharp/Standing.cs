using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Standing : MonoBehaviour
{
    public float ForceToStand = 0.1f;
    public float TargetAngle;
    public Rigidbody2D RB;

    void Start()
    {
        RB = gameObject.GetComponent<Rigidbody2D>();

        StatusManager TheStatus = Player.FindComponentInAncestors<StatusManager>(transform);
        TheStatus.TheStanding.Add(this);
    }

    // Update is called once per frame
    void Update()
    {
        RB.MoveRotation(Mathf.LerpAngle(RB.rotation, TargetAngle, Time.deltaTime * ForceToStand));
    }
}
