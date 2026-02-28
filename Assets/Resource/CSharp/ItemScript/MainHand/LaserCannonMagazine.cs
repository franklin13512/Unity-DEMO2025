using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//public class LaserCannonMagazine : MonoBehaviour
//{
//    public GetJson GetThisItem;
//    public Item TheItem;

//    public void OnValidate()
//    {
//        TheItem.UniqueCode = "_LaserCannonAmmo";
//        TheItem.AllType = CreateNewJson.TheType.Ammo;
//    }

//    [Serializable]
//    public class Magazine_LaserCannon : CreateNewJson.Ammo
//    {

//    }

//    public Magazine_LaserCannon ThisItem;

//    public bool IsInstalled;

//    public MatchMagazine TheMatchMagazine;


//    private void Awake()
//    {
//        TheMatchMagazine = GameObject.Find("ImportantResource/TheMatch").gameObject.GetComponent<MatchMagazine>();
//        GetThisItem = TheMatchMagazine.GetThisItem;

//        TheItem = gameObject.GetComponent<Item>();
//        ThisItem = new Magazine_LaserCannon();

//        for (int i = 0; i < GetThisItem.Ammo.AllAmmo.Length; i++)
//        {
//            if (GetThisItem.Ammo.AllAmmo[i].UniqueCode == "_LaserCannonAmmo")
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

//    // Update is called once per frame
//    private void Update()
//    {

//    }
//}
