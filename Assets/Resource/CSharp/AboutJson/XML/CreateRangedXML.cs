using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;

public class CreateRangedXML : MonoBehaviour
{

    public enum RangedWeaponType
    {
        None,
        MainHand,
        OffHand
    }

    [Serializable]
    [XmlRoot("RangedWeapon")]
    public class RangedWeapon
    {
        [XmlElement("Name")]
        public string Name;
        [XmlElement("Description")]
        public string Description;
        [XmlElement("Sprite")]
        public string WeaponSprite;
        public string WeaponSpritePreview;
        //[XmlElement("SpriteSize")]
        //public float Width;
        //public float Height;
        [XmlElement("Type")]
        public RangedWeaponType TheType;

        [XmlElement("ClothStorePath")]
        public string RangedWeaponStorePath;

        [XmlElement("Attributes")]
        public float Damage;
        public float ShootSpeed;//Ô½µÍÔ½¿ì
        public float BackwardForce;
    }

    [Serializable]
    public class WeaponArray
    {
        public RangedWeapon[] AllWeapon = new RangedWeapon[] { };
    }

    private void Awake()
    {
        WeaponArray TheWeaponArray = new WeaponArray();

        RangedWeapon TheHunter = new RangedWeapon();
        TheHunter.Name = "TheHunter";
        TheHunter.Description = "Description_TheHunter";
        TheHunter.WeaponSprite = Path.Combine("");
        TheHunter.WeaponSpritePreview = Path.Combine();
        TheHunter.TheType = RangedWeaponType.MainHand;
        TheHunter.RangedWeaponStorePath = Path.Combine();
        TheHunter.Damage = 30f;
        TheHunter.ShootSpeed = 0.5f;
        TheHunter.BackwardForce = 1f;

        TheWeaponArray.AllWeapon = new[] { TheHunter };

        XmlSerializer TheXmlSerializer = new XmlSerializer(typeof(WeaponArray));
        StringWriter TheWriter = new StringWriter();
        TheXmlSerializer.Serialize(TheWriter, TheWeaponArray);
        string TheRangedWeaponXMLText = TheWriter.ToString();
        string XMLPath = Path.Combine(Application.dataPath, "GameAssets", "Weapon", "RangedWeapon", "RangedWeaponXML.xml");
        File.WriteAllText(XMLPath, TheRangedWeaponXMLText);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
