using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using Unity.VisualScripting;
using UnityEngine;

public class ClothItem : Item
{
    public string Name;
    public CreateClothXML.ClothType TheType;
    public Sprite PreviewSprite;
    public string Description;
    public GetXML TheGetXML;

    public bool HideHair;

    public float AntiPhysicalDamage;
    public float HeatRetention;

    [HideInInspector]
    public CreateClothXML.ClothArray TheCloth;

    [Space]
    ////////////////////////////////////////当类型为头盔
    [HideInInspector] public Sprite HelmetSprite;
    [Space]
    ////////////////////////////////////////当类型为上身服装
    [HideInInspector] public Sprite UpperBodySprite_Male;
/*    [HideInInspector]*/ public Sprite UpperBodySprite_Female;
    [HideInInspector] public Sprite UpperArmSprite;
    [HideInInspector] public Sprite LowerArmSprite;
    [Space]
    ////////////////////////////////////////当类型为下身服装
    [HideInInspector] public Sprite HipSprite;
    [HideInInspector] public Sprite LapSprite;
    [HideInInspector] public Sprite LegSprite;
    [Space]
    ////////////////////////////////////////当类型为袜子
    [HideInInspector] public Sprite SocksSprite;
    [Space]
    ////////////////////////////////////////当类型为裤袜
    [HideInInspector] public Sprite HipStockingSprite;
    [HideInInspector] public Sprite LapStockingSprite;
    [HideInInspector] public Sprite LegStockingSprite;
    [Space]
    ////////////////////////////////////////当类型为鞋子
    [HideInInspector] public Sprite ShoesSprite;
    [Space]
    ////////////////////////////////////////当类型为护甲
    [HideInInspector] public Sprite ArmorSprite;
    [Space]
    ////////////////////////////////////////当类型为背包
    [HideInInspector] public Sprite BackPackSprite;

    public override void OnValidate()
    {
        ReadClothXML();

        if (AllType == CreateNewJson.TheType.Cloth)
        {
            foreach (CreateClothXML.ClothClass TheObj in TheCloth.AllCloth)
            {
                if (TheObj.UniqueCode == Name)
                {
                    TheBasicData = new CreateClothXML.ClothClass(
                        TheObj.UniqueCode,
                        TheObj.TheDescription,
                        TheObj.SpritePath_Male,
                        TheObj.SpritePath_Female,
                        TheObj.Type,
                        TheObj.TheType,
                        TheObj.ClothStorePath,
                        TheObj.AntiPhysicsDamage,
                        TheObj.ThePrefab_Path
                        );

                    GetSprite_NoXML(TheBasicData as CreateClothXML.ClothClass);
                }
            }
        }
    }

    private void Start()
    {
        OnValidate();
    }

    public void ReadClothXML()
    {
        string PathOfClothXML = Path.Combine("Assets", "Resource", "GameAssets", "Cloth", "ClothXML.xml");

        string PathOfWeaponXML = Path.Combine("Assets", "Resource", "GameAssets", "Weapon", "RangedWeapon", "RangedWeaponXML.xml");

        TheCloth = DeserializeXML<CreateClothXML.ClothArray>(PathOfClothXML);
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

    public static Sprite GetSprite_NoXML_WithReturn(CreateClothXML.ClothClass TheObj)
    {
            Sprite PreviewSprite;

            if (TheObj.TheType == CreateClothXML.ClothType.UpperCloth)
            {
                byte[] TheBytes1 = File.ReadAllBytes(TheObj.SpritePath_Female);
                byte[] TheBytes2 = File.ReadAllBytes(TheObj.SpritePath_Male);

                //Debug.Log(TheObj.SpritePath_Female);

                Texture2D PNG_UpperBodySprite_Male = new Texture2D(500, 500);
                PNG_UpperBodySprite_Male.LoadImage(TheBytes2);
                Texture2D PNG_UpperBodySprite_Female = new Texture2D(500, 500);
                PNG_UpperBodySprite_Female.LoadImage(TheBytes1);

                Rect MaleBody = new Rect(500, 0, -100, 175);
                Rect FemaleBody = new Rect(500, 0, -100, 350);
                Rect UpperArm = new Rect(0, 85, 47, 90);
                Rect LowerArm = new Rect(0, 0, 44, 86);
                Rect Preview = new Rect(500, 500, -100, -100);
                Vector2 Pivot = new Vector2(0.5f, 0.5f);
                Vector2 PivotFemale = new Vector2(0.485f, 0.73f);

                PreviewSprite = Sprite.Create(PNG_UpperBodySprite_Female, Preview, Pivot);


            return PreviewSprite;

            }
            else if (TheObj.TheType == CreateClothXML.ClothType.LowerCloth)
            {
                byte[] TheBytes1 = File.ReadAllBytes(TheObj.SpritePath_Female);
                byte[] TheBytes2 = File.ReadAllBytes(TheObj.SpritePath_Male);

                Texture2D PNG_UpperBodySprite_Male = new Texture2D(500, 500);
                PNG_UpperBodySprite_Male.LoadImage(TheBytes2);
                Texture2D PNG_UpperBodySprite_Female = new Texture2D(500, 500);
                PNG_UpperBodySprite_Female.LoadImage(TheBytes1);

                Rect Lap = new Rect(0, 115, 53, 105);
                Rect Leg = new Rect(0, 0, 53, 115);
                Rect Hip = new Rect(500, 0, -53, 40);
                Rect Preview = new Rect(500, 500, -100, -100);
                Vector2 Pivot = new Vector2(0.5f, 0.5f);

                PreviewSprite = Sprite.Create(PNG_UpperBodySprite_Female, Preview, Pivot);

                return PreviewSprite;
            }
            else if (TheObj.TheType == CreateClothXML.ClothType.Skirt)
            {
                byte[] TheBytes1 = File.ReadAllBytes(TheObj.SpritePath_Female);
                byte[] TheBytes2 = File.ReadAllBytes(TheObj.SpritePath_Male);

                Texture2D PNG_UpperBodySprite_Male = new Texture2D(500, 500);
                PNG_UpperBodySprite_Male.LoadImage(TheBytes2);
                Texture2D PNG_UpperBodySprite_Female = new Texture2D(500, 500);
                PNG_UpperBodySprite_Female.LoadImage(TheBytes1);

                Rect Lap = new Rect(0, 115, 53, 105);
                Rect Leg = new Rect(0, 0, 53, 115);
                Rect Hip = new Rect(500, 0, -200, 200);
                Rect Preview = new Rect(500, 500, -100, -100);
                Vector2 Pivot = new Vector2(0.52f, 0.66f);

                PreviewSprite = Sprite.Create(PNG_UpperBodySprite_Female, Preview, Pivot);

                return PreviewSprite;
            }
            else if (TheObj.TheType == CreateClothXML.ClothType.Helmet)
            {
                byte[] TheBytes1 = File.ReadAllBytes(TheObj.SpritePath_Female);
                byte[] TheBytes2 = File.ReadAllBytes(TheObj.SpritePath_Male);

                Texture2D PNG_UpperBodySprite_Male = new Texture2D(500, 500);
                PNG_UpperBodySprite_Male.LoadImage(TheBytes2);
                Texture2D PNG_UpperBodySprite_Female = new Texture2D(500, 500);
                PNG_UpperBodySprite_Female.LoadImage(TheBytes1);

                Rect Helmet = new Rect(0, 0, 150, 150);
                Rect Preview = new Rect(500, 500, -100, -100);
                Vector2 Pivot = new Vector2(0.5f, 0.5f);

                PreviewSprite = Sprite.Create(PNG_UpperBodySprite_Female, Preview, Pivot);

                return PreviewSprite;
            }
            else if (TheObj.TheType == CreateClothXML.ClothType.Socks)
            {
                byte[] TheBytes1 = File.ReadAllBytes(TheObj.SpritePath_Female);
                byte[] TheBytes2 = File.ReadAllBytes(TheObj.SpritePath_Male);

                Texture2D PNG_UpperBodySprite_Male = new Texture2D(500, 600);
                PNG_UpperBodySprite_Male.LoadImage(TheBytes2);
                Texture2D PNG_UpperBodySprite_Female = new Texture2D(500, 600);
                PNG_UpperBodySprite_Female.LoadImage(TheBytes1);

                Rect Lap = new Rect(0, 0, 500, 500);
                Rect Leg = new Rect(0, 0, 500, 500);
                Rect Hip = new Rect(0, 0, 500, 500);
                Rect Preview = new Rect(500, 600, -100, -100);
                Vector2 Pivot = new Vector2(0.5f, 0.5f);

                PreviewSprite = Sprite.Create(PNG_UpperBodySprite_Female, Preview, Pivot);

                return PreviewSprite;
            }
            else if (TheObj.TheType == CreateClothXML.ClothType.Stocking)
            {
                byte[] TheBytes1 = File.ReadAllBytes(TheObj.SpritePath_Female);
                byte[] TheBytes2 = File.ReadAllBytes(TheObj.SpritePath_Male);

                Texture2D PNG_UpperBodySprite_Male = new Texture2D(500, 600);
                PNG_UpperBodySprite_Male.LoadImage(TheBytes2);
                Texture2D PNG_UpperBodySprite_Female = new Texture2D(500, 600);
                PNG_UpperBodySprite_Female.LoadImage(TheBytes1);

                Rect Hip = new Rect(0, 0, 500, 500);
                Rect Preview = new Rect(500, 600, -100, -100);
                Vector2 Pivot = new Vector2(0.5f, 0.5f);


                PreviewSprite = Sprite.Create(PNG_UpperBodySprite_Female, Preview, Pivot);

                return PreviewSprite;
            }
            else if (TheObj.TheType == CreateClothXML.ClothType.Armor)
            {
                byte[] TheBytes1 = File.ReadAllBytes(TheObj.SpritePath_Female);
                byte[] TheBytes2 = File.ReadAllBytes(TheObj.SpritePath_Male);

                Texture2D PNG_UpperBodySprite_Male = new Texture2D(500, 500);
                PNG_UpperBodySprite_Male.LoadImage(TheBytes2);
                Texture2D PNG_UpperBodySprite_Female = new Texture2D(500, 500);
                PNG_UpperBodySprite_Female.LoadImage(TheBytes1);

                Rect Armor = new Rect(0, 0, 80, 180);
                Rect Preview = new Rect(500, 500, -100, -100);
                Vector2 Pivot = new Vector2(0.5f, 0.5f);

                PreviewSprite = Sprite.Create(PNG_UpperBodySprite_Female, Preview, Pivot);

                return PreviewSprite;
            }
            else if (TheObj.TheType == CreateClothXML.ClothType.BackPack)
            {
                return null;
            }
            else if (TheObj.TheType == CreateClothXML.ClothType.Shoes)
            {
                byte[] TheBytes1 = File.ReadAllBytes(TheObj.SpritePath_Female);
                byte[] TheBytes2 = File.ReadAllBytes(TheObj.SpritePath_Male);

                Texture2D PNG_UpperBodySprite_Male = new Texture2D(500, 500);
                PNG_UpperBodySprite_Male.LoadImage(TheBytes2);
                Texture2D PNG_UpperBodySprite_Female = new Texture2D(500, 500);
                PNG_UpperBodySprite_Female.LoadImage(TheBytes1);

                Rect Shoes = new Rect(0, 0, 55, 115);
                Rect Preview = new Rect(500, 500, -100, -100);
                Vector2 Pivot = new Vector2(0.5f, 0.1f);


                PreviewSprite = Sprite.Create(PNG_UpperBodySprite_Female, Preview, Pivot);

                return PreviewSprite;
            }
            else
            {
                return null;
            }
       
    }

    public void GetSprite_NoXML(CreateClothXML.ClothClass TheObj)
    {

        if (Name == TheObj.UniqueCode)
            {

            if (TheObj.TheType == CreateClothXML.ClothType.UpperCloth)
                {
                    byte[] TheBytes1 = File.ReadAllBytes(TheObj.SpritePath_Female);
                    byte[] TheBytes2 = File.ReadAllBytes(TheObj.SpritePath_Male);

                    Texture2D PNG_UpperBodySprite_Male = new Texture2D(500, 500);
                    PNG_UpperBodySprite_Male.LoadImage(TheBytes2);
                    Texture2D PNG_UpperBodySprite_Female = new Texture2D(500, 500);
                    PNG_UpperBodySprite_Female.LoadImage(TheBytes1);

                    Rect MaleBody = new Rect(500, 0, -100, 175);
                    Rect FemaleBody = new Rect(500, 0, -100, 350);
                    Rect UpperArm = new Rect(0, 85, 47, 90);
                    Rect LowerArm = new Rect(0, 0, 44, 86);
                    Rect Preview = new Rect(500, 500, -100, -100);
                    Vector2 Pivot = new Vector2(0.5f, 0.5f);
                    Vector2 PivotFemale = new Vector2(0.485f, 0.73f);

                    UpperBodySprite_Male = Sprite.Create(PNG_UpperBodySprite_Male, MaleBody, Pivot);
                    UpperBodySprite_Female = Sprite.Create(PNG_UpperBodySprite_Female, FemaleBody, PivotFemale);

                    UpperArmSprite = Sprite.Create(PNG_UpperBodySprite_Female, UpperArm, Pivot);
                    LowerArmSprite = Sprite.Create(PNG_UpperBodySprite_Female, LowerArm, Pivot);
                    PreviewSprite = Sprite.Create(PNG_UpperBodySprite_Female, Preview, Pivot);

                }
                else if (TheObj.TheType == CreateClothXML.ClothType.LowerCloth)
                {
                    byte[] TheBytes1 = File.ReadAllBytes(TheObj.SpritePath_Female);
                    byte[] TheBytes2 = File.ReadAllBytes(TheObj.SpritePath_Male);

                    Texture2D PNG_UpperBodySprite_Male = new Texture2D(500, 500);
                    PNG_UpperBodySprite_Male.LoadImage(TheBytes2);
                    Texture2D PNG_UpperBodySprite_Female = new Texture2D(500, 500);
                    PNG_UpperBodySprite_Female.LoadImage(TheBytes1);

                    Rect Lap = new Rect(0, 115, 53, 105);
                    Rect Leg = new Rect(0, 0, 53, 115);
                    Rect Hip = new Rect(500, 0, -53, 40);
                    Rect Preview = new Rect(500, 500, -100, -100);
                    Vector2 Pivot = new Vector2(0.5f, 0.5f);

                    HipSprite = Sprite.Create(PNG_UpperBodySprite_Female, Hip, Pivot);

                    LapSprite = Sprite.Create(PNG_UpperBodySprite_Female, Lap, Pivot);
                    LegSprite = Sprite.Create(PNG_UpperBodySprite_Female, Leg, Pivot);
                    PreviewSprite = Sprite.Create(PNG_UpperBodySprite_Female, Preview, Pivot);
                }
                else if (TheObj.TheType == CreateClothXML.ClothType.Skirt)
                {
                    byte[] TheBytes1 = File.ReadAllBytes(TheObj.SpritePath_Female);
                    byte[] TheBytes2 = File.ReadAllBytes(TheObj.SpritePath_Male);

                    Texture2D PNG_UpperBodySprite_Male = new Texture2D(500, 500);
                    PNG_UpperBodySprite_Male.LoadImage(TheBytes2);
                    Texture2D PNG_UpperBodySprite_Female = new Texture2D(500, 500);
                    PNG_UpperBodySprite_Female.LoadImage(TheBytes1);

                    Rect Lap = new Rect(0, 115, 53, 105);
                    Rect Leg = new Rect(0, 0, 53, 115);
                    Rect Hip = new Rect(500, 0, -200, 200);
                    Rect Preview = new Rect(500, 500, -100, -100);
                    Vector2 Pivot = new Vector2(0.52f, 0.66f);


                    HipSprite = Sprite.Create(PNG_UpperBodySprite_Female, Hip, Pivot);

                    LapSprite = Sprite.Create(PNG_UpperBodySprite_Female, Lap, Pivot);
                    LegSprite = Sprite.Create(PNG_UpperBodySprite_Female, Leg, Pivot);
                    PreviewSprite = Sprite.Create(PNG_UpperBodySprite_Female, Preview, Pivot);
                }
                else if (TheObj.TheType == CreateClothXML.ClothType.Helmet)
                {
                    byte[] TheBytes1 = File.ReadAllBytes(TheObj.SpritePath_Female);
                    byte[] TheBytes2 = File.ReadAllBytes(TheObj.SpritePath_Male);

                    Texture2D PNG_UpperBodySprite_Male = new Texture2D(500, 500);
                    PNG_UpperBodySprite_Male.LoadImage(TheBytes2);
                    Texture2D PNG_UpperBodySprite_Female = new Texture2D(500, 500);
                    PNG_UpperBodySprite_Female.LoadImage(TheBytes1);

                    Rect Helmet = new Rect(0, 0, 150, 150);
                    Rect Preview = new Rect(500, 500, -100, -100);
                    Vector2 Pivot = new Vector2(0.5f, 0.5f);

                    HelmetSprite = Sprite.Create(PNG_UpperBodySprite_Female, Helmet, Pivot);

                    PreviewSprite = Sprite.Create(PNG_UpperBodySprite_Female, Preview, Pivot);
                }
                else if (TheObj.TheType == CreateClothXML.ClothType.Socks)
                {
                    byte[] TheBytes1 = File.ReadAllBytes(TheObj.SpritePath_Female);
                    byte[] TheBytes2 = File.ReadAllBytes(TheObj.SpritePath_Male);

                    Texture2D PNG_UpperBodySprite_Male = new Texture2D(500, 600);
                    PNG_UpperBodySprite_Male.LoadImage(TheBytes2);
                    Texture2D PNG_UpperBodySprite_Female = new Texture2D(500, 600);
                    PNG_UpperBodySprite_Female.LoadImage(TheBytes1);

                    Rect Lap = new Rect(0, 0, 500, 500);
                    Rect Leg = new Rect(0, 0, 500, 500);
                    Rect Hip = new Rect(0, 0, 500, 500);
                    Rect Preview = new Rect(500, 600, -100, -100);
                    Vector2 Pivot = new Vector2(0.5f, 0.5f);

                    HipStockingSprite = Sprite.Create(PNG_UpperBodySprite_Female, Hip, Pivot);

                    LapStockingSprite = Sprite.Create(PNG_UpperBodySprite_Female, Lap, Pivot);
                    LegStockingSprite = Sprite.Create(PNG_UpperBodySprite_Female, Leg, Pivot);
                    PreviewSprite = Sprite.Create(PNG_UpperBodySprite_Female, Preview, Pivot);
                }
                else if (TheObj.TheType == CreateClothXML.ClothType.Stocking)
                {
                    byte[] TheBytes1 = File.ReadAllBytes(TheObj.SpritePath_Female);
                    byte[] TheBytes2 = File.ReadAllBytes(TheObj.SpritePath_Male);

                    Texture2D PNG_UpperBodySprite_Male = new Texture2D(500, 600);
                    PNG_UpperBodySprite_Male.LoadImage(TheBytes2);
                    Texture2D PNG_UpperBodySprite_Female = new Texture2D(500, 600);
                    PNG_UpperBodySprite_Female.LoadImage(TheBytes1);

                    //Rect Lap = new Rect(0, 0, 500, 500);
                    //Rect Leg = new Rect(0, 0, 500, 500);
                    Rect Hip = new Rect(0, 0, 500, 500);
                    Rect Preview = new Rect(500, 600, -100, -100);
                    Vector2 Pivot = new Vector2(0.5f, 0.5f);

                    HipStockingSprite = Sprite.Create(PNG_UpperBodySprite_Female, Hip, Pivot);

                    //LapStockingSprite = Sprite.Create(PNG_UpperBodySprite_Female, Lap, Pivot);
                    //LegStockingSprite = Sprite.Create(PNG_UpperBodySprite_Female, Leg, Pivot);
                    PreviewSprite = Sprite.Create(PNG_UpperBodySprite_Female, Preview, Pivot);
                }
                else if (TheObj.TheType == CreateClothXML.ClothType.Armor)
                {
                    byte[] TheBytes1 = File.ReadAllBytes(TheObj.SpritePath_Female);
                    byte[] TheBytes2 = File.ReadAllBytes(TheObj.SpritePath_Male);

                    Texture2D PNG_UpperBodySprite_Male = new Texture2D(500, 500);
                    PNG_UpperBodySprite_Male.LoadImage(TheBytes2);
                    Texture2D PNG_UpperBodySprite_Female = new Texture2D(500, 500);
                    PNG_UpperBodySprite_Female.LoadImage(TheBytes1);

                    Rect Armor = new Rect(0, 0, 80, 180);
                    Rect Preview = new Rect(500, 500, -100, -100);
                    Vector2 Pivot = new Vector2(0.5f, 0.5f);

                    ArmorSprite = Sprite.Create(PNG_UpperBodySprite_Female, Armor, Pivot);

                    PreviewSprite = Sprite.Create(PNG_UpperBodySprite_Female, Preview, Pivot);
                }
                else if (TheObj.TheType == CreateClothXML.ClothType.BackPack)
                {

                }
                else if (TheObj.TheType == CreateClothXML.ClothType.Shoes)
                {
                    byte[] TheBytes1 = File.ReadAllBytes(TheObj.SpritePath_Female);
                    byte[] TheBytes2 = File.ReadAllBytes(TheObj.SpritePath_Male);

                    Texture2D PNG_UpperBodySprite_Male = new Texture2D(500, 500);
                    PNG_UpperBodySprite_Male.LoadImage(TheBytes2);
                    Texture2D PNG_UpperBodySprite_Female = new Texture2D(500, 500);
                    PNG_UpperBodySprite_Female.LoadImage(TheBytes1);

                    Rect Shoes = new Rect(0, 0, 55, 115);
                    Rect Preview = new Rect(500, 500, -100, -100);
                    Vector2 Pivot = new Vector2(0.5f, 0.1f);

                    ShoesSprite = Sprite.Create(PNG_UpperBodySprite_Female, Shoes, Pivot);

                    PreviewSprite = Sprite.Create(PNG_UpperBodySprite_Female, Preview, Pivot);
                }
            }
    }

    public void GetSprite(GetXML TheGetXML)
    {

        for (int i = 0; i < TheGetXML.TheClothArray.AllCloth.Length; i++)
        {
            if (Name == TheGetXML.TheClothArray.AllCloth[i].Name)
            {
                Name = TheGetXML.TheClothArray.AllCloth[i].Name;
                TheType = TheGetXML.TheClothArray.AllCloth[i].TheType;
                Description = TheGetXML.TheClothArray.AllCloth[i].TheDescription;

                AntiPhysicalDamage = TheGetXML.TheClothArray.AllCloth[i].AntiPhysicsDamage;

                if (TheType == CreateClothXML.ClothType.UpperCloth)
                {
                    byte[] TheBytes1 = File.ReadAllBytes(TheGetXML.TheClothArray.AllCloth[i].SpritePath_Female);
                    byte[] TheBytes2 = File.ReadAllBytes(TheGetXML.TheClothArray.AllCloth[i].SpritePath_Male);

                    Texture2D PNG_UpperBodySprite_Male = new Texture2D(500, 500);
                    PNG_UpperBodySprite_Male.LoadImage(TheBytes2);
                    Texture2D PNG_UpperBodySprite_Female = new Texture2D(500, 500);
                    PNG_UpperBodySprite_Female.LoadImage(TheBytes1);

                    Rect MaleBody = new Rect(500, 0, -100, 175);
                    Rect FemaleBody = new Rect(500, 0, -100, 350);
                    Rect UpperArm = new Rect(0, 85, 47, 90);
                    Rect LowerArm = new Rect(0, 0, 44, 86);
                    Rect Preview = new Rect(500, 500, -100, -100);
                    Vector2 Pivot = new Vector2(0.5f, 0.5f);
                    Vector2 PivotFemale = new Vector2(0.485f, 0.73f);

                    UpperBodySprite_Male = Sprite.Create(PNG_UpperBodySprite_Male, MaleBody, Pivot);
                    UpperBodySprite_Female = Sprite.Create(PNG_UpperBodySprite_Female, FemaleBody, PivotFemale);

                    UpperArmSprite = Sprite.Create(PNG_UpperBodySprite_Female, UpperArm, Pivot);
                    LowerArmSprite = Sprite.Create(PNG_UpperBodySprite_Female, LowerArm, Pivot);
                    PreviewSprite = Sprite.Create(PNG_UpperBodySprite_Female, Preview, Pivot);

                }
                else if (TheType == CreateClothXML.ClothType.LowerCloth)
                {
                    byte[] TheBytes1 = File.ReadAllBytes(TheGetXML.TheClothArray.AllCloth[i].SpritePath_Female);
                    byte[] TheBytes2 = File.ReadAllBytes(TheGetXML.TheClothArray.AllCloth[i].SpritePath_Male);

                    Texture2D PNG_UpperBodySprite_Male = new Texture2D(500, 500);
                    PNG_UpperBodySprite_Male.LoadImage(TheBytes2);
                    Texture2D PNG_UpperBodySprite_Female = new Texture2D(500, 500);
                    PNG_UpperBodySprite_Female.LoadImage(TheBytes1);

                    Rect Lap = new Rect(0, 115, 53, 105);
                    Rect Leg = new Rect(0, 0, 53, 115);
                    Rect Hip = new Rect(500, 0, -53, 40);
                    Rect Preview = new Rect(500, 500, -100, -100);
                    Vector2 Pivot = new Vector2(0.5f, 0.5f);

                    HipSprite = Sprite.Create(PNG_UpperBodySprite_Female, Hip, Pivot);

                    LapSprite = Sprite.Create(PNG_UpperBodySprite_Female, Lap, Pivot);
                    LegSprite = Sprite.Create(PNG_UpperBodySprite_Female, Leg, Pivot);
                    PreviewSprite = Sprite.Create(PNG_UpperBodySprite_Female, Preview, Pivot);
                }
                else if (TheType == CreateClothXML.ClothType.Skirt)
                {
                    byte[] TheBytes1 = File.ReadAllBytes(TheGetXML.TheClothArray.AllCloth[i].SpritePath_Female);
                    byte[] TheBytes2 = File.ReadAllBytes(TheGetXML.TheClothArray.AllCloth[i].SpritePath_Male);

                    Texture2D PNG_UpperBodySprite_Male = new Texture2D(500, 500);
                    PNG_UpperBodySprite_Male.LoadImage(TheBytes2);
                    Texture2D PNG_UpperBodySprite_Female = new Texture2D(500, 500);
                    PNG_UpperBodySprite_Female.LoadImage(TheBytes1);

                    Rect Lap = new Rect(0, 115, 53, 105);
                    Rect Leg = new Rect(0, 0, 53, 115);
                    Rect Hip = new Rect(500, 0, -200, 200);
                    Rect Preview = new Rect(500, 500, -100, -100);
                    Vector2 Pivot = new Vector2(0.52f, 0.66f);


                    HipSprite = Sprite.Create(PNG_UpperBodySprite_Female, Hip, Pivot);

                    LapSprite = Sprite.Create(PNG_UpperBodySprite_Female, Lap, Pivot);
                    LegSprite = Sprite.Create(PNG_UpperBodySprite_Female, Leg, Pivot);
                    PreviewSprite = Sprite.Create(PNG_UpperBodySprite_Female, Preview, Pivot);
                }
                else if (TheType == CreateClothXML.ClothType.Helmet)
                {
                    byte[] TheBytes1 = File.ReadAllBytes(TheGetXML.TheClothArray.AllCloth[i].SpritePath_Female);
                    byte[] TheBytes2 = File.ReadAllBytes(TheGetXML.TheClothArray.AllCloth[i].SpritePath_Male);

                    Texture2D PNG_UpperBodySprite_Male = new Texture2D(500, 500);
                    PNG_UpperBodySprite_Male.LoadImage(TheBytes2);
                    Texture2D PNG_UpperBodySprite_Female = new Texture2D(500, 500);
                    PNG_UpperBodySprite_Female.LoadImage(TheBytes1);

                    Rect Helmet = new Rect(0, 0, 150, 150);
                    Rect Preview = new Rect(500, 500, -100, -100);
                    Vector2 Pivot = new Vector2(0.5f, 0.5f);

                    HelmetSprite = Sprite.Create(PNG_UpperBodySprite_Female, Helmet, Pivot);

                    PreviewSprite = Sprite.Create(PNG_UpperBodySprite_Female, Preview, Pivot);
                }
                else if (TheType == CreateClothXML.ClothType.Socks)
                {
                    byte[] TheBytes1 = File.ReadAllBytes(TheGetXML.TheClothArray.AllCloth[i].SpritePath_Female);
                    byte[] TheBytes2 = File.ReadAllBytes(TheGetXML.TheClothArray.AllCloth[i].SpritePath_Male);

                    Texture2D PNG_UpperBodySprite_Male = new Texture2D(500, 600);
                    PNG_UpperBodySprite_Male.LoadImage(TheBytes2);
                    Texture2D PNG_UpperBodySprite_Female = new Texture2D(500, 600);
                    PNG_UpperBodySprite_Female.LoadImage(TheBytes1);

                    Rect Lap = new Rect(0, 0, 500, 500);
                    Rect Leg = new Rect(0, 0, 500, 500);
                    Rect Hip = new Rect(0, 0, 500, 500);
                    Rect Preview = new Rect(500, 600, -100, -100);
                    Vector2 Pivot = new Vector2(0.5f, 0.5f);

                    HipStockingSprite = Sprite.Create(PNG_UpperBodySprite_Female, Hip, Pivot);

                    LapStockingSprite = Sprite.Create(PNG_UpperBodySprite_Female, Lap, Pivot);
                    LegStockingSprite = Sprite.Create(PNG_UpperBodySprite_Female, Leg, Pivot);
                    PreviewSprite = Sprite.Create(PNG_UpperBodySprite_Female, Preview, Pivot);
                }
                else if (TheType == CreateClothXML.ClothType.Stocking)
                {
                    byte[] TheBytes1 = File.ReadAllBytes(TheGetXML.TheClothArray.AllCloth[i].SpritePath_Female);
                    byte[] TheBytes2 = File.ReadAllBytes(TheGetXML.TheClothArray.AllCloth[i].SpritePath_Male);

                    Texture2D PNG_UpperBodySprite_Male = new Texture2D(500, 600);
                    PNG_UpperBodySprite_Male.LoadImage(TheBytes2);
                    Texture2D PNG_UpperBodySprite_Female = new Texture2D(500, 600);
                    PNG_UpperBodySprite_Female.LoadImage(TheBytes1);

                    //Rect Lap = new Rect(0, 0, 500, 500);
                    //Rect Leg = new Rect(0, 0, 500, 500);
                    Rect Hip = new Rect(0, 0, 500, 500);
                    Rect Preview = new Rect(500, 600, -100, -100);
                    Vector2 Pivot = new Vector2(0.5f, 0.5f);

                    HipStockingSprite = Sprite.Create(PNG_UpperBodySprite_Female, Hip, Pivot);

                    //LapStockingSprite = Sprite.Create(PNG_UpperBodySprite_Female, Lap, Pivot);
                    //LegStockingSprite = Sprite.Create(PNG_UpperBodySprite_Female, Leg, Pivot);
                    PreviewSprite = Sprite.Create(PNG_UpperBodySprite_Female, Preview, Pivot);
                }
                else if (TheType == CreateClothXML.ClothType.Armor)
                {
                    byte[] TheBytes1 = File.ReadAllBytes(TheGetXML.TheClothArray.AllCloth[i].SpritePath_Female);
                    byte[] TheBytes2 = File.ReadAllBytes(TheGetXML.TheClothArray.AllCloth[i].SpritePath_Male);

                    Texture2D PNG_UpperBodySprite_Male = new Texture2D(500, 500);
                    PNG_UpperBodySprite_Male.LoadImage(TheBytes2);
                    Texture2D PNG_UpperBodySprite_Female = new Texture2D(500, 500);
                    PNG_UpperBodySprite_Female.LoadImage(TheBytes1);

                    Rect Armor = new Rect(0, 0, 80, 180);
                    Rect Preview = new Rect(500, 500, -100, -100);
                    Vector2 Pivot = new Vector2(0.5f, 0.5f);

                    ArmorSprite = Sprite.Create(PNG_UpperBodySprite_Female, Armor, Pivot);

                    PreviewSprite = Sprite.Create(PNG_UpperBodySprite_Female, Preview, Pivot);
                }
                else if (TheType == CreateClothXML.ClothType.BackPack)
                {

                }
                else if (TheType == CreateClothXML.ClothType.Shoes)
                {
                    byte[] TheBytes1 = File.ReadAllBytes(TheGetXML.TheClothArray.AllCloth[i].SpritePath_Female);
                    byte[] TheBytes2 = File.ReadAllBytes(TheGetXML.TheClothArray.AllCloth[i].SpritePath_Male);

                    Texture2D PNG_UpperBodySprite_Male = new Texture2D(500, 500);
                    PNG_UpperBodySprite_Male.LoadImage(TheBytes2);
                    Texture2D PNG_UpperBodySprite_Female = new Texture2D(500, 500);
                    PNG_UpperBodySprite_Female.LoadImage(TheBytes1);

                    Rect Shoes = new Rect(0, 0, 55, 115);
                    Rect Preview = new Rect(500, 500, -100, -100);
                    Vector2 Pivot = new Vector2(0.5f, 0.1f);

                    ShoesSprite = Sprite.Create(PNG_UpperBodySprite_Female, Shoes, Pivot);

                    PreviewSprite = Sprite.Create(PNG_UpperBodySprite_Female, Preview, Pivot);
                }
            }
        }
    }

    IEnumerator WaitForOneFrame()
    {
        yield return new WaitForSeconds(0.3f);

        if (TheGetXML != null)
        {
            for (int i = 0; i < TheGetXML.TheClothArray.AllCloth.Length; i++)
            {
                if (Name == TheGetXML.TheClothArray.AllCloth[i].Name)
                {
                    Name = TheGetXML.TheClothArray.AllCloth[i].Name;
                    TheType = TheGetXML.TheClothArray.AllCloth[i].TheType;
                    Description = TheGetXML.TheClothArray.AllCloth[i].TheDescription;

                    AntiPhysicalDamage = TheGetXML.TheClothArray.AllCloth[i].AntiPhysicsDamage;

                    if (TheType == CreateClothXML.ClothType.UpperCloth)
                    {
                        byte[] TheBytes1 = File.ReadAllBytes(TheGetXML.TheClothArray.AllCloth[i].SpritePath_Female);
                        byte[] TheBytes2 = File.ReadAllBytes(TheGetXML.TheClothArray.AllCloth[i].SpritePath_Male);

                        Texture2D PNG_UpperBodySprite_Male = new Texture2D(500, 500);
                        PNG_UpperBodySprite_Male.LoadImage(TheBytes2);
                        Texture2D PNG_UpperBodySprite_Female = new Texture2D(500, 500);
                        PNG_UpperBodySprite_Female.LoadImage(TheBytes1);

                        Rect MaleBody = new Rect(500, 0, -100, 175);
                        Rect FemaleBody = new Rect(500, 0, -100, 350);
                        Rect UpperArm = new Rect(0, 85, 47, 90);
                        Rect LowerArm = new Rect(0, 0, 44, 86);
                        Rect Preview = new Rect(500, 500, -100, -100);
                        Vector2 Pivot = new Vector2(0.5f, 0.5f);
                        Vector2 PivotFemale = new Vector2(0.485f, 0.73f);

                        UpperBodySprite_Male = Sprite.Create(PNG_UpperBodySprite_Male, MaleBody, Pivot);
                        UpperBodySprite_Female = Sprite.Create(PNG_UpperBodySprite_Female, FemaleBody, PivotFemale);

                        UpperArmSprite = Sprite.Create(PNG_UpperBodySprite_Female, UpperArm, Pivot);
                        LowerArmSprite = Sprite.Create(PNG_UpperBodySprite_Female, LowerArm, Pivot);
                        PreviewSprite = Sprite.Create(PNG_UpperBodySprite_Female, Preview, Pivot);

                    }
                    else if (TheType == CreateClothXML.ClothType.LowerCloth)
                    {
                        byte[] TheBytes1 = File.ReadAllBytes(TheGetXML.TheClothArray.AllCloth[i].SpritePath_Female);
                        byte[] TheBytes2 = File.ReadAllBytes(TheGetXML.TheClothArray.AllCloth[i].SpritePath_Male);

                        Texture2D PNG_UpperBodySprite_Male = new Texture2D(500, 500);
                        PNG_UpperBodySprite_Male.LoadImage(TheBytes2);
                        Texture2D PNG_UpperBodySprite_Female = new Texture2D(500, 500);
                        PNG_UpperBodySprite_Female.LoadImage(TheBytes1);

                        Rect Lap = new Rect(0, 115, 53, 105);
                        Rect Leg = new Rect(0, 0, 53, 115);
                        Rect Hip = new Rect(500, 0, -53, 40);
                        Rect Preview = new Rect(500, 500, -100, -100);
                        Vector2 Pivot = new Vector2(0.5f, 0.5f);

                        HipSprite = Sprite.Create(PNG_UpperBodySprite_Female, Hip, Pivot);

                        LapSprite = Sprite.Create(PNG_UpperBodySprite_Female, Lap, Pivot);
                        LegSprite = Sprite.Create(PNG_UpperBodySprite_Female, Leg, Pivot);
                        PreviewSprite = Sprite.Create(PNG_UpperBodySprite_Female, Preview, Pivot);
                    }
                    else if (TheType == CreateClothXML.ClothType.Skirt)
                    {
                        byte[] TheBytes1 = File.ReadAllBytes(TheGetXML.TheClothArray.AllCloth[i].SpritePath_Female);
                        byte[] TheBytes2 = File.ReadAllBytes(TheGetXML.TheClothArray.AllCloth[i].SpritePath_Male);

                        Texture2D PNG_UpperBodySprite_Male = new Texture2D(500, 500);
                        PNG_UpperBodySprite_Male.LoadImage(TheBytes2);
                        Texture2D PNG_UpperBodySprite_Female = new Texture2D(500, 500);
                        PNG_UpperBodySprite_Female.LoadImage(TheBytes1);

                        Rect Lap = new Rect(0, 115, 53, 105);
                        Rect Leg = new Rect(0, 0, 53, 115);
                        Rect Hip = new Rect(500, 0, -200, 200);
                        Rect Preview = new Rect(500, 500, -100, -100);
                        Vector2 Pivot = new Vector2(0.52f, 0.66f);


                        HipSprite = Sprite.Create(PNG_UpperBodySprite_Female, Hip, Pivot);

                        LapSprite = Sprite.Create(PNG_UpperBodySprite_Female, Lap, Pivot);
                        LegSprite = Sprite.Create(PNG_UpperBodySprite_Female, Leg, Pivot);
                        PreviewSprite = Sprite.Create(PNG_UpperBodySprite_Female, Preview, Pivot);
                    }
                    else if (TheType == CreateClothXML.ClothType.Helmet)
                    {
                        byte[] TheBytes1 = File.ReadAllBytes(TheGetXML.TheClothArray.AllCloth[i].SpritePath_Female);
                        byte[] TheBytes2 = File.ReadAllBytes(TheGetXML.TheClothArray.AllCloth[i].SpritePath_Male);

                        Texture2D PNG_UpperBodySprite_Male = new Texture2D(500, 500);
                        PNG_UpperBodySprite_Male.LoadImage(TheBytes2);
                        Texture2D PNG_UpperBodySprite_Female = new Texture2D(500, 500);
                        PNG_UpperBodySprite_Female.LoadImage(TheBytes1);

                        Rect Helmet = new Rect(0, 0, 150, 150);
                        Rect Preview = new Rect(500, 500, -100, -100);
                        Vector2 Pivot = new Vector2(0.5f, 0.5f);

                        HelmetSprite = Sprite.Create(PNG_UpperBodySprite_Female, Helmet, Pivot);

                        PreviewSprite = Sprite.Create(PNG_UpperBodySprite_Female, Preview, Pivot);
                    }
                    else if (TheType == CreateClothXML.ClothType.Socks)
                    {
                        byte[] TheBytes1 = File.ReadAllBytes(TheGetXML.TheClothArray.AllCloth[i].SpritePath_Female);
                        byte[] TheBytes2 = File.ReadAllBytes(TheGetXML.TheClothArray.AllCloth[i].SpritePath_Male);

                        Texture2D PNG_UpperBodySprite_Male = new Texture2D(500, 600);
                        PNG_UpperBodySprite_Male.LoadImage(TheBytes2);
                        Texture2D PNG_UpperBodySprite_Female = new Texture2D(500, 600);
                        PNG_UpperBodySprite_Female.LoadImage(TheBytes1);

                        Rect Lap = new Rect(0, 0, 500, 500);
                        Rect Leg = new Rect(0, 0, 500, 500);
                        Rect Hip = new Rect(0, 0, 500, 500);
                        Rect Preview = new Rect(500, 600, -100, -100);
                        Vector2 Pivot = new Vector2(0.5f, 0.5f);

                        HipStockingSprite = Sprite.Create(PNG_UpperBodySprite_Female, Hip, Pivot);

                        LapStockingSprite = Sprite.Create(PNG_UpperBodySprite_Female, Lap, Pivot);
                        LegStockingSprite = Sprite.Create(PNG_UpperBodySprite_Female, Leg, Pivot);
                        PreviewSprite = Sprite.Create(PNG_UpperBodySprite_Female, Preview, Pivot);
                    }
                    else if (TheType == CreateClothXML.ClothType.Stocking)
                    {
                        byte[] TheBytes1 = File.ReadAllBytes(TheGetXML.TheClothArray.AllCloth[i].SpritePath_Female);
                        byte[] TheBytes2 = File.ReadAllBytes(TheGetXML.TheClothArray.AllCloth[i].SpritePath_Male);

                        Texture2D PNG_UpperBodySprite_Male = new Texture2D(500, 600);
                        PNG_UpperBodySprite_Male.LoadImage(TheBytes2);
                        Texture2D PNG_UpperBodySprite_Female = new Texture2D(500, 600);
                        PNG_UpperBodySprite_Female.LoadImage(TheBytes1);

                        //Rect Lap = new Rect(0, 0, 500, 500);
                        //Rect Leg = new Rect(0, 0, 500, 500);
                        Rect Hip = new Rect(0, 0, 500, 500);
                        Rect Preview = new Rect(500, 600, -100, -100);
                        Vector2 Pivot = new Vector2(0.5f, 0.5f);

                        HipStockingSprite = Sprite.Create(PNG_UpperBodySprite_Female, Hip, Pivot);

                        //LapStockingSprite = Sprite.Create(PNG_UpperBodySprite_Female, Lap, Pivot);
                        //LegStockingSprite = Sprite.Create(PNG_UpperBodySprite_Female, Leg, Pivot);
                        PreviewSprite = Sprite.Create(PNG_UpperBodySprite_Female, Preview, Pivot);
                    }
                    else if (TheType == CreateClothXML.ClothType.Armor)
                    {
                        byte[] TheBytes1 = File.ReadAllBytes(TheGetXML.TheClothArray.AllCloth[i].SpritePath_Female);
                        byte[] TheBytes2 = File.ReadAllBytes(TheGetXML.TheClothArray.AllCloth[i].SpritePath_Male);

                        Texture2D PNG_UpperBodySprite_Male = new Texture2D(500, 500);
                        PNG_UpperBodySprite_Male.LoadImage(TheBytes2);
                        Texture2D PNG_UpperBodySprite_Female = new Texture2D(500, 500);
                        PNG_UpperBodySprite_Female.LoadImage(TheBytes1);

                        Rect Armor = new Rect(0, 0, 80, 180);
                        Rect Preview = new Rect(500, 500, -100, -100);
                        Vector2 Pivot = new Vector2(0.5f, 0.5f);

                        ArmorSprite = Sprite.Create(PNG_UpperBodySprite_Female, Armor, Pivot);

                        PreviewSprite = Sprite.Create(PNG_UpperBodySprite_Female, Preview, Pivot);
                    }
                    else if (TheType == CreateClothXML.ClothType.BackPack)
                    {

                    }
                    else if (TheType == CreateClothXML.ClothType.Shoes)
                    {
                        byte[] TheBytes1 = File.ReadAllBytes(TheGetXML.TheClothArray.AllCloth[i].SpritePath_Female);
                        byte[] TheBytes2 = File.ReadAllBytes(TheGetXML.TheClothArray.AllCloth[i].SpritePath_Male);

                        Texture2D PNG_UpperBodySprite_Male = new Texture2D(500, 500);
                        PNG_UpperBodySprite_Male.LoadImage(TheBytes2);
                        Texture2D PNG_UpperBodySprite_Female = new Texture2D(500, 500);
                        PNG_UpperBodySprite_Female.LoadImage(TheBytes1);

                        Rect Shoes = new Rect(0, 0, 55, 115);
                        Rect Preview = new Rect(500, 500, -100, -100);
                        Vector2 Pivot = new Vector2(0.5f, 0.1f);

                        ShoesSprite = Sprite.Create(PNG_UpperBodySprite_Female, Shoes, Pivot);

                        PreviewSprite = Sprite.Create(PNG_UpperBodySprite_Female, Preview, Pivot);
                    }
                }
            }
        }
    }
}
