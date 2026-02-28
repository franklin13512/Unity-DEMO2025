using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using UnityEngine;

public class GetXML : MonoBehaviour
{
    public PrefabList ThePrefabs;

    public CreateClothXML.ClothArray TheClothArray;
    public CreateRangedXML.WeaponArray TheWeaponArray;

    //public void OnValidate()
    //{
    //    ReadClothXML();

    //}

    //void Awake()
    //{
    //    EquipmentToDic();
    //}

    //private void Start()
    //{


    //    StartCoroutine(WaitForOneFrame());
    //}

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ReadClothXML()
    {
        string PathOfClothXML = Path.Combine("Assets", "Resource", "GameAssets", "Cloth", "ClothXML.xml");

        string PathOfWeaponXML = Path.Combine("Assets", "Resource", "GameAssets", "Weapon", "RangedWeapon", "RangedWeaponXML.xml");

        TheClothArray = DeserializeXML<CreateClothXML.ClothArray>(PathOfClothXML);
        //TheWeaponArray = DeserializeXML<CreateRangedXML.WeaponArray>(PathOfWeaponXML);
    }

    public T DeserializeXML<T>(string TheXML)
    {
        XmlSerializer TheXmlSerializer = new XmlSerializer(typeof(T));
        using (StreamReader TheReader = new StreamReader(TheXML))
        {
            return (T)TheXmlSerializer.Deserialize(TheReader);
        }
    }

    public void EquipmentToDic()
    {
        for (int i = 0; i < ThePrefabs.EquipmentPrefab.Length; i++)
        {
            ClothItem TheClothItem = ThePrefabs.EquipmentPrefab[i].gameObject.GetComponent<ClothItem>();
            if (TheClothItem != null)
            {
                //GameObject NewObj = ThePrefabs.EquipmentPrefab[i].gameObject;
                TheClothItem.GetSprite(this);

                ThePrefabs.EquipmentDic.Add(TheClothItem.Name, ThePrefabs.EquipmentPrefab[i].gameObject);
            }
        }
    }



    IEnumerator WaitForOneFrame()
    {
        yield return new WaitForSeconds(1);

    }
}
