using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//public class ExpandableBatonCs : MonoBehaviour
//{
//    public GetJson TheGetJsom;
//    public Item TheItem;

//    public void OnValidate()
//    {
//        TheItem.UniqueCode = "_ExpandableBaton";
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
//                if (TheGetJsom.MeleeWeapon.AllMeleeWeapons[i].UniqueCode == "_ExpandableBaton")
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
//    // Start is called before the first frame update
//    void Start()
//    {


//    }



//    // Update is called once per frame
//    void Update()
//    {

//    }
//}
