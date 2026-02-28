using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//public class SteelPipe : MonoBehaviour
//{
//    public GetJson TheGetJsom;
//    public Item TheItem;

//    public void OnValidate()
//    {
//        TheItem.UniqueCode = "_SteelPipe";
//        TheItem.AllType = CreateNewJson.TheType.MeleeWeapon;
//        TheItem.IsMainHand = true;
//    }

//    public MeleeAttacking TheMeleeAttacking;

//    [Serializable]
//    public class TheCrowbar : CreateNewJson.MeleeWeapon
//    {
//    }

//    public TheCrowbar ThisItem;


//    private void Awake()
//    {
//        TheGetJsom = GameObject.Find("ImportantResource/TheJson").gameObject.GetComponent<GetJson>();

//        if (TheGetJsom != null)
//        {
//            TheItem = gameObject.GetComponent<Item>();
//            ThisItem = new TheCrowbar();

//            for (int i = 0; i < TheGetJsom.MeleeWeapon.AllMeleeWeapons.Length; i++)
//            {
//                if (TheGetJsom.MeleeWeapon.AllMeleeWeapons[i].UniqueCode == "_SteelPipe")
//                {

//                    ThisItem.UniqueCode = TheGetJsom.MeleeWeapon.AllMeleeWeapons[i].UniqueCode;
//                    ThisItem.TheSprite = gameObject.GetComponent<SpriteRenderer>().sprite;
//                    ThisItem.Damage = TheGetJsom.MeleeWeapon.AllMeleeWeapons[i].Damage;
//                    ThisItem.Type = TheGetJsom.MeleeWeapon.AllMeleeWeapons[i].Type;

//                    break;
//                }
//            }


//            TheItem.AllType = CreateNewJson.TheType.MeleeWeapon;
//            TheItem.TheSprite = ThisItem.TheSprite;

//            TheItem.IsMainHand = true;

//            TheMeleeAttacking.MeleeDamage = ThisItem.Damage;
//        }
//    }

//    public void Update()
//    {
//        if (TheMeleeAttacking.TheCollision != null)
//        {
//            TakeDamage TheTakeDamage = TheMeleeAttacking.TheCollision.gameObject.GetComponent<TakeDamage>();
//            if (TheTakeDamage != null)
//            {
//                StatusManager TheStatuseManager = TheTakeDamage.TheStatuseManager;
//                if (TheStatuseManager != null)
//                {
//                    float TheChance = UnityEngine.Random.Range(0, 8);
//                    if (TheChance <= 1)
//                    {
//                        TheStatuseManager.Collapse();
//                    }
//                }
//            }

//            TheMeleeAttacking.TheCollision = null;
//        }
//    }
//}
