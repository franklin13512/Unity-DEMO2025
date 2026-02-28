using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodPack : Item
{
    public float BloodToRecover;

    public override void UseTheItem()
    {
        if (TheInventoryManager)
        {
            HealthManager TheHealth = TheInventoryManager.gameObject.GetComponent<HealthManager>();
            if (TheHealth)
            {
                TheHealth.Health += BloodToRecover;
            }
        }
    }

}
