using LitJson;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;

public class GetJson : MonoBehaviour
{

    public PrefabList ThePrefabs;
    private Item TheItem;

    public CreateNewJson.TheConsumable Consumable;
    public CreateNewJson.TheAmmo Ammo;
    public CreateNewJson.TheCloth Cloth;
    public CreateNewJson.TheLongRangeWeapon LongRangeWeapon;
    public CreateNewJson.TheMeleeWeapon MeleeWeapon;

    void OnValidate()
    {

        GetMeleeWeapon();
        GetLongRangeWeapon();
        GetAmmo();
        GetEquipment();
        GetConsumable();
    }

    //private void Awake()
    //{
    //    GetSpriteForLongRangeWeapon();
    //    GetSpriteForAmmo();
    //    GetSpriteForMeleeWeapon();

    //    LongRangeWeaponToDic();
    //    MagazineToDic();
    //    MeleeWeaponToDic();
    //    //StartCoroutine(WaitForOneFrame(););
    //}

    // Update is called once per frame
    void Update()
    {

    }

    public void GetMeleeWeapon()
    {
        string PathOfJson = Path.Combine(Application.dataPath, "Resource", "Json", "MeleeWeapon.json");
        string JsonContent = File.ReadAllText(PathOfJson);

        MeleeWeapon = new CreateNewJson.TheMeleeWeapon();
        MeleeWeapon = JsonMapper.ToObject<CreateNewJson.TheMeleeWeapon>(JsonContent);
    }

    public void GetLongRangeWeapon()
    {
        string PathOfJson = Path.Combine(Application.dataPath, "Resource", "Json", "LongRangeWeapon.json");
        string JsonContent = File.ReadAllText(PathOfJson);

        LongRangeWeapon = new CreateNewJson.TheLongRangeWeapon();
        LongRangeWeapon = JsonMapper.ToObject<CreateNewJson.TheLongRangeWeapon>(JsonContent);
    }

    public void GetAmmo()
    {
        string PathOfJson = Path.Combine(Application.dataPath, "Resource", "Json", "Ammo.json");
        string JsonContent = File.ReadAllText(PathOfJson);

        Ammo = new CreateNewJson.TheAmmo();
        Ammo = JsonMapper.ToObject<CreateNewJson.TheAmmo>(JsonContent);
    }

    public void GetEquipment()
    {

    }

    public void GetConsumable()
    {

    }

    //public void GetSpriteForLongRangeWeapon()
    //{
    //    for (int i = 0; i < LongRangeWeapon.AllLongRangeWeapons.Length; i++)
    //    {
    //        for (int j = 0; j < ThePrefabs.RangeWeaponPrefab.Length; j++)
    //        {
    //            TheItem = ThePrefabs.RangeWeaponPrefab[j].gameObject.GetComponent<Item>();
    //            if (TheItem.UniqueCode == LongRangeWeapon.AllLongRangeWeapons[i].UniqueCode)
    //            {
    //                LongRangeWeapon.AllLongRangeWeapons[i].TheSprite = TheItem.TheSprite;
    //            }
    //        }
    //    }
    //}

    //public void GetSpriteForAmmo()
    //{
    //    for (int i = 0; i < Ammo.AllAmmo.Length; i++)
    //    {
    //        for (int j = 0; j < ThePrefabs.RangeWeaponPrefab.Length; j++)
    //        {
    //            TheItem = ThePrefabs.RangeWeaponPrefab[j].gameObject.GetComponent<Item>();
    //            if (TheItem.UniqueCode == Ammo.AllAmmo[i].UniqueCode)
    //            {
    //                Ammo.AllAmmo[i].TheSprite = TheItem.TheSprite;
    //            }
    //        }
    //    }
    //}

    //public void GetSpriteForConsumable()
    //{
    //    for (int i = 0; i < Consumable.AllConsumables.Length; i++)
    //    {
    //        for (int j = 0; j < ThePrefabs.ConsumablePrefab.Length; j++)
    //        {
    //            TheItem = ThePrefabs.ConsumablePrefab[j].gameObject.GetComponent<Item>();
    //            if (TheItem.UniqueCode == Consumable.AllConsumables[i].UniqueCode)
    //            {
    //                Consumable.AllConsumables[i].TheSprite = TheItem.TheSprite;
    //            }
    //        }
    //    }
    //}

    //public void GetSpriteForMeleeWeapon()
    //{
    //    for (int i = 0; i < MeleeWeapon.AllMeleeWeapons.Length; i++)
    //    {
    //        for (int j = 0; j < ThePrefabs.MeleeWeaponsPrefab.Length; j++)
    //        {
    //            TheItem = ThePrefabs.MeleeWeaponsPrefab[j].gameObject.GetComponent<Item>();
    //            if (TheItem.UniqueCode == MeleeWeapon.AllMeleeWeapons[i].UniqueCode)
    //            {
    //                MeleeWeapon.AllMeleeWeapons[i].TheSprite = TheItem.TheSprite;
    //            }
    //        }
    //    }
    //}

    //public void LongRangeWeaponToDic()
    //{
    //    for (int i = 0; i < ThePrefabs.RangeWeaponPrefab.Length; i++)
    //    {
    //        TheItem = ThePrefabs.RangeWeaponPrefab[i].gameObject.GetComponent<Item>();
    //        if (TheItem != null)
    //        {
    //            ThePrefabs.RangeWeaponDic.Add(TheItem.UniqueCode, ThePrefabs.RangeWeaponPrefab[i]);
    //        }
    //    }
    //}

    //public void MagazineToDic()
    //{
    //    for (int i = 0; i < ThePrefabs.AllMagazine.Length; i++)
    //    {
    //        TheItem = ThePrefabs.AllMagazine[i].gameObject.GetComponent<Item>();
    //        if (TheItem != null)
    //        {
    //            ThePrefabs.AllMagazineDic.Add(TheItem.UniqueCode, ThePrefabs.AllMagazine[i]);
    //        }
    //    }
    //}

    //public void MeleeWeaponToDic()
    //{
    //    for (int i = 0; i < ThePrefabs.MeleeWeaponsPrefab.Length; i++)
    //    {
    //        TheItem = ThePrefabs.MeleeWeaponsPrefab[i].gameObject.GetComponent<Item>();
    //        if (TheItem != null)
    //        {
    //            MeleeWeapon TheMeleeWeapon = ThePrefabs.MeleeWeaponsPrefab[i].gameObject.GetComponent<MeleeWeapon>();
    //            if (TheMeleeWeapon)
    //            {
    //                TheMeleeWeapon.Fake_Awake(this);
    //            }
    //            ThePrefabs.MeleeWeaponsDic.Add(TheItem.UniqueCode, ThePrefabs.MeleeWeaponsPrefab[i]);
    //        }
    //    }
    //}


    //IEnumerator WaitForOneFrame()
    //{
    //    yield return new WaitForSeconds(1);
    //    GetSpriteForLongRangeWeapon();
    //    GetSpriteForAmmo();
    //    GetSpriteForMeleeWeapon();

    //    LongRangeWeaponToDic();
    //    MagazineToDic();
    //    MeleeWeaponToDic();
    //}
}
