using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;

public class CreatePath : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        string RootPath = Application.dataPath;

        string ThePath = Path.Combine(RootPath, "GameAssets");
        Directory.CreateDirectory(ThePath);

        ////////////////////////////////////////////////////////////
        string TheClothPath = Path.Combine(ThePath, "Cloth");
        Directory.CreateDirectory(TheClothPath);

        string TheWeaponPath = Path.Combine(ThePath, "Weapon");
        Directory.CreateDirectory(TheWeaponPath);

        ////////////////////////////////////////////////////////////

        string HelmetPath = Path.Combine(TheClothPath, "Helmet");
        Directory.CreateDirectory(HelmetPath);

        string LowerClothPath = Path.Combine(TheClothPath, "Lower");
        Directory.CreateDirectory(LowerClothPath);

        string HairPath = Path.Combine(TheClothPath, "Hair");
        Directory.CreateDirectory(HairPath);

        string UpperClothPath = Path.Combine(TheClothPath, "Upper");
        Directory.CreateDirectory(UpperClothPath);

        string SocksPath = Path.Combine(TheClothPath, "Socks");
        Directory.CreateDirectory(SocksPath);

        string ShoesPath = Path.Combine(TheClothPath, "Shoes");
        Directory.CreateDirectory(ShoesPath);

        string ArmorPath = Path.Combine(TheClothPath, "Armor");
        Directory.CreateDirectory(ArmorPath);

        string BagPath = Path.Combine(TheClothPath, "Bag");
        Directory.CreateDirectory(BagPath);

        ////////////////////////////////////////////////////////////

        string RangedWeapon = Path.Combine(TheWeaponPath, "RangedWeapon");
        Directory.CreateDirectory(RangedWeapon);

        string Magazine = Path.Combine(RangedWeapon, "Magazine");
        Directory.CreateDirectory(Magazine);

        string MeleeWeapon = Path.Combine(TheWeaponPath, "MeleeWeapon");
        Directory.CreateDirectory(MeleeWeapon);
    }
}
