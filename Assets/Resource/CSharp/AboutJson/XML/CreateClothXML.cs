using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using System.IO;
using System;
using System.Net.Sockets;
using System.Net.NetworkInformation;

public class CreateClothXML : MonoBehaviour
{


    public enum ClothType 
    {
        None,
        UpperCloth,
        LowerCloth,
        Skirt,
        Helmet,
        Socks,
        Stocking,
        Shoes,
        Armor,
        BackPack
    }

    [Serializable]
    [XmlRoot("Cloth")]
    public class ClothClass : CreateNewJson.BaseItem
    {
        [XmlElement("Name")]
        public string Name;
        [XmlElement("Sprite")]
        public string SpritePath_Male;
        public string SpritePath_Female;
        [XmlElement("ClothType")]
        public ClothType TheType;

        [XmlElement("ClothStorePath")]
        public string ClothStorePath;

        [XmlElement("Attributes")]
        public float AntiPhysicsDamage;

        public ClothClass() { }

        public ClothClass(string name, string description, string spritePath_Male, string spritePath_Female,CreateNewJson.TheType baseType , ClothType theType, string clothStorePath, float antiPhysicsDamage)
        {
            UniqueCode = name;
            TheDescription = description;
            SpritePath_Male = spritePath_Male;
            SpritePath_Female = spritePath_Female;
            Type = baseType;
            TheType = theType;
            ClothStorePath = clothStorePath;
            AntiPhysicsDamage = antiPhysicsDamage;
        }

        public ClothClass(string name, string description, string spritePath_Male, string spritePath_Female, CreateNewJson.TheType baseType, ClothType theType, string clothStorePath, float antiPhysicsDamage, string PrefabPath)
        {
            UniqueCode = name;
            TheDescription = description;
            SpritePath_Male = spritePath_Male;
            SpritePath_Female = spritePath_Female;
            Type = baseType;
            TheType = theType;
            ClothStorePath = clothStorePath;
            AntiPhysicsDamage = antiPhysicsDamage;

            ThePrefab_Path = PrefabPath;
            TheSprite_Path = null;
        }
    }

    [Serializable]
    public class ClothArray
    {
        public string ClothPath = Path.Combine("Assets", "Resource", "GameAssets", "Cloth");

        public string HelmetPath = Path.Combine("Assets", "Resource", "GameAssets", "Cloth", "Helmet");
        public string UpperPath = Path.Combine("Assets", "Resource", "GameAssets", "Cloth", "Upper");
        public string LowerPath = Path.Combine("Assets", "Resource", "GameAssets", "Cloth", "Lower");
        public string BagPath = Path.Combine("Assets", "Resource", "GameAssets", "Cloth", "Bag");
        public string ArmorPath = Path.Combine("Assets", "Resource", "GameAssets", "Cloth", "Armor");
        public string SocksPath = Path.Combine("Assets", "Resource", "GameAssets", "Cloth", "Socks");
        public string ShoesPath = Path.Combine("Assets", "Resource", "GameAssets", "Cloth", "Shoes");

        public ClothClass[] AllCloth = new ClothClass[] {};
    }


    // Start is called before the first frame update
    void OnValidate()
    {
        string ClothPath = Path.Combine("Assets", "Resource", "GameAssets", "Cloth");
        string ClothPrefabPath = Path.Combine("Assets", "Resource", "Prefab", "Cloth");
        string Prefab_Armor = Path.Combine("Assets", "Resource", "Prefab", "Cloth", "Armor");
        string Prefab_Helmet = Path.Combine("Assets", "Resource", "Prefab", "Cloth", "Helmet");
        string Prefab_Lower = Path.Combine("Assets", "Resource", "Prefab", "Cloth", "Lower");
        string Prefab_Shoes = Path.Combine("Assets", "Resource", "Prefab", "Cloth", "Shoes");
        string Prefab_Socks = Path.Combine("Assets", "Resource", "Prefab", "Cloth", "Socks");
        string Prefab_Upper = Path.Combine("Assets", "Resource", "Prefab", "Cloth", "Upper");

        string HelmetPath = Path.Combine("Assets", "Resource", "GameAssets", "Cloth", "Helmet");
        string UpperPath = Path.Combine("Assets", "Resource", "GameAssets", "Cloth", "Upper");
        string LowerPath = Path.Combine("Assets", "Resource", "GameAssets", "Cloth", "Lower");
        string BagPath = Path.Combine("Assets", "Resource", "GameAssets", "Cloth", "Bag");
        string ArmorPath = Path.Combine("Assets", "Resource", "GameAssets", "Cloth", "Armor");
        string SocksPath = Path.Combine("Assets", "Resource", "GameAssets", "Cloth", "Socks");
        string ShoesPath = Path.Combine("Assets", "Resource", "GameAssets", "Cloth", "Shoes");

        ClothArray TheClothArray = new ClothArray();

        ClothClass ForestRangerUpper = new ClothClass(
            "ForestRangerUpper",
            "Description_ForestRangerUpper",
            Path.Combine(UpperPath, "ForestRangerUpper_Male.png"),
            Path.Combine(UpperPath, "ForestRangerUpper_Female.png"),
            CreateNewJson.TheType.Cloth,
            ClothType.UpperCloth,
            Path.Combine(ClothPath, "ClothXML.xml"),
            10,
            Path.Combine(Prefab_Upper, "ForestRangerUpper.prefab")
            ) ;

        ClothClass ForestRangerLower = new ClothClass(
            name:"ForestRangerLower",
            description:"Description_ForestRangerLower",
            spritePath_Male: Path.Combine(LowerPath, "ForestRangerLower.png"),
            spritePath_Female: Path.Combine(LowerPath, "ForestRangerLower.png"),
            baseType: CreateNewJson.TheType.Cloth,
            theType: ClothType.LowerCloth,
            clothStorePath: Path.Combine(ClothPath, "ClothXML.xml"),
            antiPhysicsDamage: 10,
            PrefabPath: Path.Combine(Prefab_Lower, "ForestRangerLower.prefab")
            );

        ClothClass BlackPleatedSkirt = new ClothClass(
            name: "BlackPleatedSkirt",
            description: "Description_BlackPleatedSkirt",
            spritePath_Male: Path.Combine(LowerPath, "BlackPleatedSkirt.png"),
            spritePath_Female: Path.Combine(LowerPath, "BlackPleatedSkirt.png"),
            baseType: CreateNewJson.TheType.Cloth,
            theType: ClothType.Skirt,
            clothStorePath: Path.Combine(ClothPath, "ClothXML.xml"),
            antiPhysicsDamage: 1,
            PrefabPath: Path.Combine(Prefab_Lower, "BlackPleatedSkirt.prefab")
            );

        ClothClass MedicalCoat = new ClothClass(
            name: "MedicalCoat",
            description: "Description_MedicalCoat",
            spritePath_Male: Path.Combine(UpperPath, "MedicalCoat_Male.png"),
            spritePath_Female: Path.Combine(UpperPath, "MedicalCoat_Female.png"),
            baseType: CreateNewJson.TheType.Cloth,
            theType: ClothType.UpperCloth,
            clothStorePath: Path.Combine(ClothPath, "ClothXML.xml"),
            antiPhysicsDamage: 8,
            PrefabPath: Path.Combine(Prefab_Upper, "MedicalCoat.prefab")
            );

        ClothClass MaskHelmet = new ClothClass(
            name: "MaskHelmet",
            description: "Description_MaskHelmet",
            spritePath_Male: Path.Combine(HelmetPath, "MaskHelmet.png"),
            spritePath_Female: Path.Combine(HelmetPath, "MaskHelmet.png"),
            baseType: CreateNewJson.TheType.Cloth,
            theType: ClothType.Helmet,
            clothStorePath: Path.Combine(ClothPath, "ClothXML.xml"),
            antiPhysicsDamage: 5,
            PrefabPath: Path.Combine(Prefab_Helmet, "MaskHelmet.prefab")
            );

        ClothClass MotorcycleHelmet = new ClothClass(
            name: "MotorcycleHelmet",
            description: "Description_MotorcycleHelmet",
            spritePath_Male: Path.Combine(HelmetPath, "MotorcycleHelmet.png"),
            spritePath_Female: Path.Combine(HelmetPath, "MotorcycleHelmet.png"),
            baseType: CreateNewJson.TheType.Cloth,
            theType: ClothType.Helmet,
            clothStorePath: Path.Combine(ClothPath, "ClothXML.xml"),
            antiPhysicsDamage: 2.5f,
            PrefabPath: Path.Combine(Prefab_Helmet, "MotorcycleHelmet.prefab")
            );

        ClothClass BlackCanvasShoes = new ClothClass(
            name: "BlackCanvasShoes",
            description: "Description_BlackCanvasShoes",
            spritePath_Male: Path.Combine(ShoesPath, "BlackCanvasShoes.png"),
            spritePath_Female: Path.Combine(ShoesPath, "BlackCanvasShoes.png"),
            baseType: CreateNewJson.TheType.Cloth,
            theType: ClothType.Shoes,
            clothStorePath: Path.Combine(ClothPath, "ClothXML.xml"),
            antiPhysicsDamage: 1f,
            PrefabPath: Path.Combine(Prefab_Shoes, "BlackCanvasShoes.prefab")
            );

        ClothClass FormalSuitUpper = new ClothClass(
            name: "FormalSuitUpper",
            description: "Description_FormalSuitUpper",
            spritePath_Male: Path.Combine(UpperPath, "FormalSuitUpper_Male.png"),
            spritePath_Female: Path.Combine(UpperPath, "FormalSuitUpper_Female.png"),
            baseType: CreateNewJson.TheType.Cloth,
            theType: ClothType.UpperCloth,
            clothStorePath: Path.Combine(ClothPath, "ClothXML.xml"),
            antiPhysicsDamage: 3.5f,
            PrefabPath: Path.Combine(Prefab_Upper, "FormalSuitUpper.prefab")
            );

        ClothClass SimpleArmor = new ClothClass(
            name: "SimpleArmor",
            description: "Description_SimpleArmor",
            spritePath_Male: Path.Combine(ArmorPath, "SimpleArmor.png"),
            spritePath_Female: Path.Combine(ArmorPath, "SimpleArmor.png"),
            baseType: CreateNewJson.TheType.Cloth,
            theType: ClothType.Armor,
            clothStorePath: Path.Combine(ClothPath, "ClothXML.xml"),
            antiPhysicsDamage: 12f,
            PrefabPath: Path.Combine(Prefab_Armor, "SimpleArmor.prefab")
            );

        ClothClass GreyShirt = new ClothClass(
            name: "GreyShirt",
            description: "Description_GreyShirt",
            spritePath_Male: Path.Combine(UpperPath, "GreyShirt_Male.png"),
            spritePath_Female: Path.Combine(UpperPath, "GreyShirt_Female.png"),
            baseType: CreateNewJson.TheType.Cloth,
            theType: ClothType.UpperCloth,
            clothStorePath: Path.Combine(ClothPath, "ClothXML.xml"),
            antiPhysicsDamage: 3f,
            PrefabPath: Path.Combine(Prefab_Upper, "GreyShirt.prefab")
            );

        ClothClass RedShirt = new ClothClass(
            name: "RedShirt",
            description: "Description_RedShirt",
            spritePath_Male: Path.Combine(UpperPath, "RedShirt_Male.png"),
            spritePath_Female: Path.Combine(UpperPath, "RedShirt_Female.png"),
            baseType: CreateNewJson.TheType.Cloth,
            theType: ClothType.UpperCloth,
            clothStorePath: Path.Combine(ClothPath, "ClothXML.xml"),
            antiPhysicsDamage: 3f,
            PrefabPath: Path.Combine(Prefab_Upper, "RedShirt.prefab")
            );

        ClothClass BlueShirt = new ClothClass(
            name: "BlueShirt",
            description: "Description_BlueShirt",
            spritePath_Male: Path.Combine(UpperPath, "BlueShirt_Male.png"),
            spritePath_Female: Path.Combine(UpperPath, "BlueShirt_Female.png"),
            baseType: CreateNewJson.TheType.Cloth,
            theType: ClothType.UpperCloth,
            clothStorePath: Path.Combine(ClothPath, "ClothXML.xml"),
            antiPhysicsDamage: 3f,
            PrefabPath: Path.Combine(Prefab_Upper, "BlueShirt.prefab")
            );

        ClothClass Jeans = new ClothClass(
            name: "Jeans",
            description: "Description_Jeans",
            spritePath_Male: Path.Combine(LowerPath, "Jeans.png"),
            spritePath_Female: Path.Combine(LowerPath, "Jeans.png"),
            baseType: CreateNewJson.TheType.Cloth,
            theType: ClothType.LowerCloth,
            clothStorePath: Path.Combine(ClothPath, "ClothXML.xml"),
            antiPhysicsDamage: 4f,
            PrefabPath: Path.Combine(Prefab_Lower, "Jeans.prefab")
            );

        ClothClass GreyTrouser = new ClothClass(
            name: "GreyTrouser",
            description: "Description_GreyTrouser",
            spritePath_Male: Path.Combine(LowerPath, "GreyTrouser.png"),
            spritePath_Female: Path.Combine(LowerPath, "GreyTrouser.png"),
            baseType: CreateNewJson.TheType.Cloth,
            theType: ClothType.LowerCloth,
            clothStorePath: Path.Combine(ClothPath, "ClothXML.xml"),
            antiPhysicsDamage: 3.5f,
            PrefabPath: Path.Combine(Prefab_Lower, "GreyTrouser.prefab")
            );

        ClothClass FormalSuitLower = new ClothClass(
            name: "FormalSuitLower",
            description: "Description_FormalSuitLower",
            spritePath_Male: Path.Combine(LowerPath, "FormalSuitLower.png"),
            spritePath_Female: Path.Combine(LowerPath, "FormalSuitLower.png"),
            baseType: CreateNewJson.TheType.Cloth,
            theType: ClothType.LowerCloth,
            clothStorePath: Path.Combine(ClothPath, "ClothXML.xml"),
            antiPhysicsDamage: 4f,
            PrefabPath: Path.Combine(Prefab_Lower, "FormalSuitLower.prefab")
            );

        ClothClass YellowShorts = new ClothClass(
            name: "YellowShorts",
            description: "Description_YellowShorts",
            spritePath_Male: Path.Combine(LowerPath, "YellowShorts.png"),
            spritePath_Female: Path.Combine(LowerPath, "YellowShorts.png"),
            baseType: CreateNewJson.TheType.Cloth,
            theType: ClothType.LowerCloth,
            clothStorePath: Path.Combine(ClothPath, "ClothXML.xml"),
            antiPhysicsDamage: 2f,
            PrefabPath: Path.Combine(Prefab_Lower, "YellowShorts.prefab")
            );

        ClothClass GreenShorts = new ClothClass(
            name: "GreenShorts",
            description: "Description_GreenShorts",
            spritePath_Male: Path.Combine(LowerPath, "GreenShorts.png"),
            spritePath_Female: Path.Combine(LowerPath, "GreenShorts.png"),
            baseType: CreateNewJson.TheType.Cloth,
            theType: ClothType.LowerCloth,
            clothStorePath: Path.Combine(ClothPath, "ClothXML.xml"),
            antiPhysicsDamage: 2f,
            PrefabPath: Path.Combine(Prefab_Lower, "GreenShorts.prefab")
            );

        ClothClass BlackStocking_Thin = new ClothClass(
            name: "BlackStocking_Thin",
            description: "Description_BlackStocking_Thin",
            spritePath_Male: Path.Combine(SocksPath, "BlackStocking_Thin.png"),
            spritePath_Female: Path.Combine(SocksPath, "BlackStocking_Thin.png"),
            baseType: CreateNewJson.TheType.Cloth,
            theType: ClothType.Stocking,
            clothStorePath: Path.Combine(ClothPath, "ClothXML.xml"),
            antiPhysicsDamage: 0.2f,
            PrefabPath: Path.Combine(Prefab_Socks, "BlackStocking_Thin.prefab")
            );

        ClothClass BlackStocking_Mid = new ClothClass(
            name: "BlackStocking_Mid",
            description: "Description_BlackStocking_Mid",
            spritePath_Male: Path.Combine(SocksPath, "BlackStocking_Mid.png"),
            spritePath_Female: Path.Combine(SocksPath, "BlackStocking_Mid.png"),
            baseType: CreateNewJson.TheType.Cloth,
            theType: ClothType.Stocking,
            clothStorePath: Path.Combine(ClothPath, "ClothXML.xml"),
            antiPhysicsDamage: 0.3f,
            PrefabPath: Path.Combine(Prefab_Socks, "BlackStocking_Mid.prefab")
            );

        ClothClass BlackStocking_Thick = new ClothClass(
            name: "BlackStocking_Thick",
            description: "Description_BlackStocking_Thick",
            spritePath_Male: Path.Combine(SocksPath, "BlackStocking_Thick.png"),
            spritePath_Female: Path.Combine(SocksPath, "BlackStocking_Thick.png"),
            baseType: CreateNewJson.TheType.Cloth,
            theType: ClothType.Stocking,
            clothStorePath: Path.Combine(ClothPath, "ClothXML.xml"),
            antiPhysicsDamage: 0.3f,
            PrefabPath: Path.Combine(Prefab_Socks, "BlackStocking_Thick.prefab")
            );

        ClothClass YellowSweater = new ClothClass(
            name: "YellowSweater",
            description: "Description_YellowSweater",
            spritePath_Male: Path.Combine(UpperPath, "YellowSweater_Male.png"),
            spritePath_Female: Path.Combine(UpperPath, "YellowSweater_Female.png"),
            baseType: CreateNewJson.TheType.Cloth,
            theType: ClothType.UpperCloth,
            clothStorePath: Path.Combine(ClothPath, "ClothXML.xml"),
            antiPhysicsDamage: 5f,
            PrefabPath: Path.Combine(Prefab_Upper, "YellowSweater.prefab")
            );

        ClothClass WhiteShortSocks = new ClothClass(
            name: "WhiteShortSocks",
            description: "Description_WhiteShortSocks",
            spritePath_Male: Path.Combine(SocksPath, "WhiteShortSocks.png"),
            spritePath_Female: Path.Combine(SocksPath, "WhiteShortSocks.png"),
            baseType: CreateNewJson.TheType.Cloth,
            theType: ClothType.Socks,
            clothStorePath: Path.Combine(ClothPath, "ClothXML.xml"),
            antiPhysicsDamage: 0.2f,
            PrefabPath: Path.Combine(Prefab_Socks, "WhiteShortSocks.prefab")
            );

        ClothClass GreyShortSocks = new ClothClass(
            name: "GreyShortSocks",
            description: "Description_GreyShortSocks",
            spritePath_Male: Path.Combine(SocksPath, "GreyShortSocks.png"),
            spritePath_Female: Path.Combine(SocksPath, "GreyShortSocks.png"),
            baseType: CreateNewJson.TheType.Cloth,
            theType: ClothType.Socks,
            clothStorePath: Path.Combine(ClothPath, "ClothXML.xml"),
            antiPhysicsDamage: 0.2f,
            PrefabPath: Path.Combine(Prefab_Socks, "GreyShortSocks.prefab")
            );

        ClothClass RedShortSocks = new ClothClass(
            name: "RedShortSocks",
            description: "Description_RedShortSocks",
            spritePath_Male: Path.Combine(SocksPath, "RedShortSocks.png"),
            spritePath_Female: Path.Combine(SocksPath, "RedShortSocks.png"),
            baseType: CreateNewJson.TheType.Cloth,
            theType: ClothType.Socks,
            clothStorePath: Path.Combine(ClothPath, "ClothXML.xml"),
            antiPhysicsDamage: 0.2f,
            PrefabPath: Path.Combine(Prefab_Socks, "RedShortSocks.prefab")
            );

        ClothClass PoliceUniformUpper = new ClothClass(
            name: "PoliceUniformUpper",
            description: "Description_PoliceUniformUpper",
            spritePath_Male: Path.Combine(UpperPath, "PoliceUniformUpper_Male.png"),
            spritePath_Female: Path.Combine(UpperPath, "PoliceUniformUpper_Female.png"),
            baseType: CreateNewJson.TheType.Cloth,
            theType: ClothType.UpperCloth,
            clothStorePath: Path.Combine(ClothPath, "ClothXML.xml"),
            antiPhysicsDamage: 7f,
            PrefabPath: Path.Combine(Prefab_Upper, "PoliceUniformUpper.prefab")
            );

        ClothClass PoliceUniformLower = new ClothClass(
            name: "PoliceUniformLower",
            description: "Description_PoliceUniformLower",
            spritePath_Male: Path.Combine(LowerPath, "PoliceUniformLower.png"),
            spritePath_Female: Path.Combine(LowerPath, "PoliceUniformLower.png"),
            baseType: CreateNewJson.TheType.Cloth,
            theType: ClothType.LowerCloth,
            clothStorePath: Path.Combine(ClothPath, "ClothXML.xml"),
            antiPhysicsDamage: 6.5f,
            PrefabPath: Path.Combine(Prefab_Lower, "PoliceUniformLower.prefab")
            );

        ClothClass WhiteTrouser = new ClothClass(
            name: "WhiteTrouser",
            description: "Description_WhiteTrouser",
            spritePath_Male: Path.Combine(LowerPath, "WhiteTrouser.png"),
            spritePath_Female: Path.Combine(LowerPath, "WhiteTrouser.png"),
            baseType: CreateNewJson.TheType.Cloth,
            theType: ClothType.LowerCloth,
            clothStorePath: Path.Combine(ClothPath, "ClothXML.xml"),
            antiPhysicsDamage: 4f,
            PrefabPath: Path.Combine(Prefab_Lower, "WhiteTrouser.prefab")
            );

        ClothClass DownCoat = new ClothClass(
            name: "DownCoat",
            description: "Description_DownCoat",
            spritePath_Male: Path.Combine(UpperPath, "DownCoat_Male.png"),
            spritePath_Female: Path.Combine(UpperPath, "DownCoat_Female.png"),
            baseType: CreateNewJson.TheType.Cloth,
            theType: ClothType.UpperCloth,
            clothStorePath: Path.Combine(ClothPath, "ClothXML.xml"),
            antiPhysicsDamage: 11f,
            PrefabPath: Path.Combine(Prefab_Upper, "DownCoat.prefab")
            );

        ClothClass YellowLeatherJacket = new ClothClass(
            name: "YellowLeatherJacket",
            description: "Description_YellowLeatherJacket",
            spritePath_Male: Path.Combine(UpperPath, "YellowLeatherJacket_Male.png"),
            spritePath_Female: Path.Combine(UpperPath, "YellowLeatherJacket_Female.png"),
            baseType: CreateNewJson.TheType.Cloth,
            theType: ClothType.UpperCloth,
            clothStorePath: Path.Combine(ClothPath, "ClothXML.xml"),
            antiPhysicsDamage: 8f,
            PrefabPath: Path.Combine(Prefab_Upper, "YellowLeatherJacket.prefab")
            );

        ClothClass RedLeatherJacket = new ClothClass(
            name: "RedLeatherJacket",
            description: "Description_RedLeatherJacket",
            spritePath_Male: Path.Combine(UpperPath, "RedLeatherJacket_Male.png"),
            spritePath_Female: Path.Combine(UpperPath, "RedLeatherJacket_Female.png"),
            baseType: CreateNewJson.TheType.Cloth,
            theType: ClothType.UpperCloth,
            clothStorePath: Path.Combine(ClothPath, "ClothXML.xml"),
            antiPhysicsDamage: 8f,
            PrefabPath: Path.Combine(Prefab_Upper, "RedLeatherJacket.prefab")
            );

        ClothClass GreenStripedJacket = new ClothClass(
            name: "GreenStripedJacket",
            description: "Description_GreenStripedJacket",
            spritePath_Male: Path.Combine(UpperPath, "GreenStripedJacket_Male.png"),
            spritePath_Female: Path.Combine(UpperPath, "GreenStripedJacket_Female.png"),
            baseType: CreateNewJson.TheType.Cloth,
            theType: ClothType.UpperCloth,
            clothStorePath: Path.Combine(ClothPath, "ClothXML.xml"),
            antiPhysicsDamage: 5f,
            PrefabPath: Path.Combine(Prefab_Upper, "GreenStripedJacket.prefab")
            );

        ClothClass YellowWoolenSweater = new ClothClass(
            name: "YellowWoolenSweater",
            description: "Description_YellowWoolenSweater",
            spritePath_Male: Path.Combine(UpperPath, "YellowWoolenSweater_Male.png"),
            spritePath_Female: Path.Combine(UpperPath, "YellowWoolenSweater_Female.png"),
            baseType: CreateNewJson.TheType.Cloth,
            theType: ClothType.UpperCloth,
            clothStorePath: Path.Combine(ClothPath, "ClothXML.xml"),
            antiPhysicsDamage: 5f,
            PrefabPath: Path.Combine(Prefab_Upper, "YellowWoolenSweater.prefab")
            );

        ClothClass GreyWoolenSweater = new ClothClass(
            name: "GreyWoolenSweater",
            description: "Description_GreyWoolenSweater",
            spritePath_Male: Path.Combine(UpperPath, "GreyWoolenSweater_Male.png"),
            spritePath_Female: Path.Combine(UpperPath, "GreyWoolenSweater_Female.png"),
            baseType: CreateNewJson.TheType.Cloth,
            theType: ClothType.UpperCloth,
            clothStorePath: Path.Combine(ClothPath, "ClothXML.xml"),
            antiPhysicsDamage: 5f,
            PrefabPath: Path.Combine(Prefab_Upper, "GreyWoolenSweater.prefab")
            );

        ClothClass RedWoolenSweater = new ClothClass(
            name: "RedWoolenSweater",
            description: "Description_RedWoolenSweater",
            spritePath_Male: Path.Combine(UpperPath, "RedWoolenSweater_Male.png"),
            spritePath_Female: Path.Combine(UpperPath, "RedWoolenSweater_Female.png"),
            baseType: CreateNewJson.TheType.Cloth,
            theType: ClothType.UpperCloth,
            clothStorePath: Path.Combine(ClothPath, "ClothXML.xml"),
            antiPhysicsDamage: 5f,
            PrefabPath: Path.Combine(Prefab_Upper, "RedWoolenSweater.prefab")
            );

        ClothClass SailorSuit = new ClothClass(
            name: "SailorSuit",
            description: "Description_SailorSuit",
            spritePath_Male: Path.Combine(UpperPath, "SailorSuit_Male.png"),
            spritePath_Female: Path.Combine(UpperPath, "SailorSuit_Female.png"),
            baseType: CreateNewJson.TheType.Cloth,
            theType: ClothType.UpperCloth,
            clothStorePath: Path.Combine(ClothPath, "ClothXML.xml"),
            antiPhysicsDamage: 4f,
            PrefabPath: Path.Combine(Prefab_Upper, "SailorSuit.prefab")
            );

        ClothClass BlackSailorSuit = new ClothClass(
            name: "BlackSailorSuit",
            description: "Description_BlackSailorSuit",
            spritePath_Male: Path.Combine(UpperPath, "BlackSailorSuit_Male.png"),
            spritePath_Female: Path.Combine(UpperPath, "BlackSailorSuit_Female.png"),
            baseType: CreateNewJson.TheType.Cloth,
            theType: ClothType.UpperCloth,
            clothStorePath: Path.Combine(ClothPath, "ClothXML.xml"),
            antiPhysicsDamage: 4f,
            PrefabPath: Path.Combine(Prefab_Upper, "BlackSailorSuit.prefab")
            );

        ClothClass ChemSuit_Grey = new ClothClass(
            name: "ChemSuitUpper_Grey",
            description: "Description_ChemSuit",
            spritePath_Male: Path.Combine(UpperPath, "ChemSuitUpper_Grey_Female.png"),
            spritePath_Female: Path.Combine(UpperPath, "ChemSuitUpper_Grey_Female.png"),
            baseType: CreateNewJson.TheType.Cloth,
            theType: ClothType.UpperCloth,
            clothStorePath: Path.Combine(ClothPath, "ClothXML.xml"),
            antiPhysicsDamage: 10f,
            PrefabPath: Path.Combine(Prefab_Upper, "ChemSuitUpper_Grey.prefab")
            );

        ClothClass ChemSuitHelmet = new ClothClass(
            name: "ChemSuitHelmet",
            description: "Description_ChemSuitHelmet",
            spritePath_Male: Path.Combine(HelmetPath, "ChemSuitHelmet.png"),
            spritePath_Female: Path.Combine(HelmetPath, "ChemSuitHelmet.png"),
            baseType: CreateNewJson.TheType.Cloth,
            theType: ClothType.Helmet,
            clothStorePath: Path.Combine(ClothPath, "ClothXML.xml"),
            antiPhysicsDamage: 6f,
            PrefabPath: Path.Combine(Prefab_Helmet, "ChemSuitHelmet Variant.prefab")
            );

        ClothClass ChemSuitLower_Grey = new ClothClass(
            name: "ChemSuitLower_Grey",
            description: "Description_ChemSuit",
            spritePath_Male: Path.Combine(LowerPath, "ChemSuitLower_Grey.png"),
            spritePath_Female: Path.Combine(LowerPath, "ChemSuitLower_Grey.png"),
            baseType: CreateNewJson.TheType.Cloth,
            theType: ClothType.LowerCloth,
            clothStorePath: Path.Combine(ClothPath, "ClothXML.xml"),
            antiPhysicsDamage: 6f,
            PrefabPath: Path.Combine(Prefab_Lower, "ChemSuitLower_Grey.prefab")
            );

        ClothClass CamoUpper = new ClothClass(
            name: "CamoUpper",
            description: "Description_Camo",
            spritePath_Male: Path.Combine(UpperPath, "CamoUpper_Female.png"),
            spritePath_Female: Path.Combine(UpperPath, "CamoUpper_Female.png"),
            baseType: CreateNewJson.TheType.Cloth,
            theType: ClothType.UpperCloth,
            clothStorePath: Path.Combine(ClothPath, "ClothXML.xml"),
            antiPhysicsDamage: 8f,
            PrefabPath: Path.Combine(Prefab_Upper, "CamoUpper.prefab")
            );

        ClothClass CamoLower = new ClothClass(
            name: "CamoLower",
            description: "Description_Camo",
            spritePath_Male: Path.Combine(LowerPath, "CamoLower.png"),
            spritePath_Female: Path.Combine(LowerPath, "CamoLower.png"),
            baseType: CreateNewJson.TheType.Cloth,
            theType: ClothType.LowerCloth,
            clothStorePath: Path.Combine(ClothPath, "ClothXML.xml"),
            antiPhysicsDamage: 7f,
            PrefabPath: Path.Combine(Prefab_Lower, "CamoLower.prefab")
            );

        ClothClass CottonTrouser = new ClothClass(
            name: "CottonTrouser",
            description: "Description_CottonTrouser",
            spritePath_Male: Path.Combine(LowerPath, "CottonTrouser.png"),
            spritePath_Female: Path.Combine(LowerPath, "CottonTrouser.png"),
            baseType: CreateNewJson.TheType.Cloth,
            theType: ClothType.LowerCloth,
            clothStorePath: Path.Combine(ClothPath, "ClothXML.xml"),
            antiPhysicsDamage: 7f,
            PrefabPath: Path.Combine(Prefab_Lower, "CottonTrouser.prefab")
            );

        ClothClass FirefightingJacket = new ClothClass(
            name: "FirefightingJacket",
            description: "Description_FirefightingJacket",
            spritePath_Male: Path.Combine(UpperPath, "FirefightingJacket_Female.png"),
            spritePath_Female: Path.Combine(UpperPath, "FirefightingJacket_Female.png"),
            baseType: CreateNewJson.TheType.Cloth,
            theType: ClothType.UpperCloth,
            clothStorePath: Path.Combine(ClothPath, "ClothXML.xml"),
            antiPhysicsDamage: 16f,
            PrefabPath: Path.Combine(Prefab_Upper, "FirefightingJacket.prefab")
            );

        ClothClass FirefightingLower = new ClothClass(
            name: "FirefightingSuitLower",
            description: "Description_FirefightingJacket",
            spritePath_Male: Path.Combine(LowerPath, "FirefightingSuitLower.png"),
            spritePath_Female: Path.Combine(LowerPath, "FirefightingSuitLower.png"),
            baseType: CreateNewJson.TheType.Cloth,
            theType: ClothType.LowerCloth,
            clothStorePath: Path.Combine(ClothPath, "ClothXML.xml"),
            antiPhysicsDamage: 9f,
            PrefabPath: Path.Combine(Prefab_Lower, "FirefightingSuitLower.prefab")
            );

        //ClothClass GreenSweater = new ClothClass(
        //    name: "GreenSweater",
        //    description: "Description_GreenSweater",
        //    spritePath_Male: Path.Combine(UpperPath, "GreenSweater_Male.png"),
        //    spritePath_Female: Path.Combine(UpperPath, "GreenSweater_Female.png"),
        //    baseType: CreateNewJson.TheType.Cloth,
        //    theType: ClothType.UpperCloth,
        //    clothStorePath: Path.Combine(ClothPath, "ClothXML.xml"),
        //    antiPhysicsDamage: 5f,
        //    PrefabPath: Path.Combine(Prefab_Upper, "YellowSweater.prefab")
        //    );

        //TheClothArray.AllCloth = new[] { ForestRangerUpper, ForestRangerLower, MaskHelmet, MotorcycleHelmet, BlackCanvasShoes, SimpleArmor, GreyShirt, 
        // YellowLeatherJacket,RedLeatherJacket,GreenStripedJacket,MedicalCoat,YellowWoolenSweater,GreyWoolenSweater,RedWoolenSweater,SailorSuit,
        //    BlackSailorSuit,ChemSuit_Grey,ChemSuitHelmet,ChemSuitLower_Grey,CamoUpper,CamoLower,CottonTrouser,FirefightingJacket,FirefightingLower};


        TheClothArray.AllCloth = new[] { ForestRangerUpper, ForestRangerLower, BlackPleatedSkirt, MedicalCoat, MaskHelmet, MotorcycleHelmet, BlackCanvasShoes,
                                                 FormalSuitUpper, SimpleArmor, GreyShirt, RedShirt, BlueShirt, Jeans, GreyTrouser, FormalSuitLower, YellowShorts,
                                                 GreenShorts, BlackStocking_Thin, BlackStocking_Mid, BlackStocking_Thick, YellowSweater, WhiteShortSocks, PoliceUniformUpper,
                                                 GreyShortSocks, RedShortSocks, PoliceUniformLower, WhiteTrouser, DownCoat, YellowLeatherJacket, RedLeatherJacket,
                                                 GreenStripedJacket, YellowWoolenSweater, GreyWoolenSweater, RedWoolenSweater, SailorSuit, BlackSailorSuit,
                                                 ChemSuit_Grey, ChemSuitHelmet, ChemSuitLower_Grey, CamoUpper, CamoLower, CottonTrouser, FirefightingJacket, FirefightingLower};

        XmlSerializer TheXmlSerializer = new XmlSerializer(typeof(ClothArray));
        StringWriter TheWriter = new StringWriter();
        TheXmlSerializer.Serialize(TheWriter, TheClothArray);
        string TheClothXMLText = TheWriter.ToString();
        string XMLPath = Path.Combine(ClothPath, "ClothXML.xml");
        //Debug.Log(XMLPath);
        File.WriteAllText(XMLPath, TheClothXMLText);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
