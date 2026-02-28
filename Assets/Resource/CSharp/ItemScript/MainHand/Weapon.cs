using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public float Damage;

    public enum BulletType
    {
        None,
        BackForce,
        Explosion
    }

    public bool EnableShell;//允许抛壳
    public bool EnabelFlash;//允许手电/激光瞄具
    public bool IsMainHand;//双手武器
    public bool IsHalfAuto;
    public bool EnableBullet;//允许实体子弹
    public bool IsLaserWeapon;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
