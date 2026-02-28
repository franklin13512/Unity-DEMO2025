using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//public class BigFireJar : MonoBehaviour
//{
//    public GetJson GetThisItem;
//    public Item TheItem;

//    [Serializable]
//    public class Magazine_BigFire : CreateNewJson.Ammo
//    {

//    }

//    public Magazine_BigFire ThisItem;

//    public bool IsInstalled;

//    public MatchMagazine TheMatchMagazine;

//    private void Awake()
//    {
//        TheMatchMagazine = GameObject.Find("ImportantResource/TheMatch").gameObject.GetComponent<MatchMagazine>();
//        GetThisItem = TheMatchMagazine.GetThisItem;

//        TheItem = gameObject.GetComponent<Item>();
//        ThisItem = new Magazine_BigFire();

//        for (int i = 0; i < GetThisItem.Ammo.AllAmmo.Length; i++)
//        {
//            if (GetThisItem.Ammo.AllAmmo[i].UniqueCode == "_BigFireJar")
//            {
//                ThisItem.UniqueCode = GetThisItem.Ammo.AllAmmo[i].UniqueCode;
//                ThisItem.TheSprite = gameObject.GetComponent<SpriteRenderer>().sprite;
//                ThisItem.MaxMagazine = GetThisItem.Ammo.AllAmmo[i].MaxMagazine;
//                ThisItem.Type = GetThisItem.Ammo.AllAmmo[i].Type;
//                ThisItem.RemainMagazine = GetThisItem.Ammo.AllAmmo[i].RemainMagazine;

//                break;
//            }
//        }
//        TheItem.AllType = CreateNewJson.TheType.Ammo;
//        TheItem.TheSprite = ThisItem.TheSprite;

//        TheItem.MaxAmmo = ThisItem.MaxMagazine;
//        TheItem.RemainAmmo = ThisItem.RemainMagazine;
//    }
//    // Start is called before the first frame update
//    void Start()
//    {
//    }

//    private void Update()
//    {

//    }
//}
