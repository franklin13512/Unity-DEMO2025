using UnityEngine;
using LitJson;
using System;
using System.IO;
using Pathfinding;
using UnityEngine.Rendering.VirtualTexturing;
using UnityEditor.MemoryProfiler;
using static CreateRangedXML;
using static UnityEditor.Rendering.ShadowCascadeGUI;
using UnityEngine.Rendering;

public class CreateNewJson : MonoBehaviour
{
    public PrefabList ThePrefabs;
    private Item TheItem;

    public enum TheType //物品类型
    {
        None,
        MeleeWeapon,
        LongRangeWeapon,
        Ammo,
        Cloth,
        Consumable,
        Medical,
        Structure,
        Other
    }

    public enum TheHandleStyle
    {
        None,
        Normal,
        MainHandWeapon,
    }

    public enum TheType_InSlot //物品的储物栏的用处
    {
        None,
        ForMagazine,
        ForInventory,
        ForCloth
    }

    [Serializable]
    public class BaseItem
    {
        public string UniqueCode;
        public string TheDescription;
        public string TheSprite_Path;
        public string ThePrefab_Path;
        public TheType Type;
        public TheHandleStyle HandleStyle;

        public bool Stackable;
        public int MaxStack = 1;//默认1.stackable的物体在下面另外设置

        public ThingsInInventory[] InventoryObjs;
    }

    //||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||近战武器区
    [Serializable]
    public class MeleeWeapon : BaseItem
    {
        public float Damage;
        public float ChanceToBleed;
        public float TimeForNextAttack;

        public MeleeWeapon() { }

        public MeleeWeapon(string uniqueCode, TheType type, float damage, float durability, float timeForNextAttack)
        {
            UniqueCode = uniqueCode;
            Type = type;
            Damage = damage;
            TimeForNextAttack = timeForNextAttack;
        }

        public MeleeWeapon(string uniqueCode, TheType type, float damage, float durability, float timeForNextAttack, float chanceToBleed)
        {
            UniqueCode = uniqueCode;
            Type = type;
            Damage = damage;
            TimeForNextAttack = timeForNextAttack;
            ChanceToBleed = chanceToBleed;
        }
        public MeleeWeapon(string uniqueCode, TheType type,TheHandleStyle style, string Des, float damage, float timeForNextAttack, float chanceToBleed, bool stackable, ThingsInInventory[] TheInventory, string spritePath, string prefabPath)//有预制体
        {
            UniqueCode = uniqueCode;
            Type = type;
            HandleStyle = style;
            TheDescription = Des;
            Damage = damage;
            TimeForNextAttack = timeForNextAttack;
            ChanceToBleed = chanceToBleed;
            Stackable = stackable;
            InventoryObjs = TheInventory;

            TheSprite_Path = spritePath;
            ThePrefab_Path = prefabPath;
        }

        public MeleeWeapon(string uniqueCode, TheType type, TheHandleStyle style, string Des, float damage, float timeForNextAttack, float chanceToBleed, bool stackable, ThingsInInventory[] TheInventory, string spritePath)//无预制体
        {
            UniqueCode = uniqueCode;
            Type = type;
            HandleStyle = style;
            Damage = damage;
            TheDescription = Des;
            TimeForNextAttack = timeForNextAttack;
            ChanceToBleed = chanceToBleed;
            Stackable = stackable;
            InventoryObjs = TheInventory;

            TheSprite_Path = spritePath;
        }
    }

    [Serializable]
    public class TheMeleeWeapon
    {
        public MeleeWeapon[] AllMeleeWeapons;
    }
    public TheMeleeWeapon AllMelee = new TheMeleeWeapon();


    //||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||远程武器区
    [Serializable]
    public class LongRangeWeapon : BaseItem
    {
        public bool IsAuto;
        public bool EnableShell;
        public bool EnableBullet;
        public bool IsLaserWeapon;

        public float BackwardForce;
        public float DamageMuti;
        public float ShootSpeed;
        public int MaxAmmoStack;
        public string[] AmmoCode;

        public LongRangeWeapon()
        {

        }

        public LongRangeWeapon(string uniqueCode, TheType type, TheHandleStyle style, int MaxAmmo, string Des, string[] AmmoUnique, float damageM,bool AutoShoot, bool Shell, bool Bullet, bool Laser, float Backward,float shootspeed , bool stackable,ThingsInInventory[] TheInventory, string spritePath, string prefabPath)//有预制体
        {
            UniqueCode = uniqueCode;
            Type = type;
            HandleStyle = style;
            MaxAmmoStack = MaxAmmo;
            TheDescription = Des;
            AmmoCode = AmmoUnique;
            DamageMuti = damageM;
            IsAuto = AutoShoot;
            EnableShell = Shell;
            EnableBullet = Bullet;
            IsLaserWeapon = Laser;
            BackwardForce = shootspeed;
            BackwardForce = Backward;
            ShootSpeed = shootspeed;
            Stackable = stackable;
            InventoryObjs = TheInventory;

            TheSprite_Path = spritePath;
            ThePrefab_Path = prefabPath;
        }

        public LongRangeWeapon(string uniqueCode, TheType type, TheHandleStyle style, int MaxAmmo, string Des, string[] AmmoUnique, float damageM, bool AutoShoot, bool Shell, bool Bullet, bool Laser, float Backward, float shootspeed, bool stackable, ThingsInInventory[] TheInventory, string spritePath)//无预制体
        {
            UniqueCode = uniqueCode;
            Type = type;
            HandleStyle = style;
            MaxAmmoStack = MaxAmmo;
            TheDescription = Des;
            AmmoCode = AmmoUnique;
            IsAuto = AutoShoot;
            EnableShell = Shell;
            EnableBullet = Bullet;
            IsLaserWeapon = Laser;
            BackwardForce = shootspeed;
            DamageMuti = damageM;
            BackwardForce = Backward;
            ShootSpeed = shootspeed;
            Stackable = stackable;
            InventoryObjs = TheInventory;


            TheSprite_Path = spritePath;
        }
    }

    [Serializable]
    public class TheLongRangeWeapon
    {
        public LongRangeWeapon[] AllLongRangeWeapons;
    }
    public TheLongRangeWeapon AllRange = new TheLongRangeWeapon();


    //||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||弹药区
    [Serializable]
    public class Ammo : BaseItem
    {
        public int MaxMagazine;
        public int RemainMagazine;
        public float Damage;

        public Weapon.BulletType TheBulletType;

        public Ammo() { }

        public Ammo(string uniqueCode, TheType type, TheHandleStyle style, Weapon.BulletType BulletEffect,  string Des, float damage, int MaxMag, int RemainMag, bool stackable, string spritePath)
        {
            UniqueCode = uniqueCode;
            Type = type;
            HandleStyle = style;
            TheDescription = Des;
            Damage = damage;
            MaxMagazine = MaxMag;
            RemainMagazine = RemainMag;
            Stackable = stackable;
            TheBulletType = BulletEffect;

            TheSprite_Path = spritePath;
        }

        public Ammo(string uniqueCode, TheType type, TheHandleStyle style, Weapon.BulletType BulletEffect, string Des, float damage, int MaxMag, int RemainMag, bool stackable, string spritePath, string prefabPath)
        {
            UniqueCode = uniqueCode;
            Type = type;
            HandleStyle = style;
            TheDescription = Des;
            Damage = damage;
            MaxMagazine = MaxMag;
            RemainMagazine = RemainMag;
            Stackable= stackable;
            TheBulletType = BulletEffect;

            TheSprite_Path = spritePath;
            ThePrefab_Path = prefabPath;
        }

        public Ammo(string uniqueCode, TheType type, TheHandleStyle style, Weapon.BulletType BulletEffect, string Des, float damage, int MaxMag, int RemainMag, bool stackable,int maxStack, string spritePath, string prefabPath)
        {
            UniqueCode = uniqueCode;
            Type = type;
            HandleStyle = style;
            TheDescription = Des;
            Damage = damage;
            MaxMagazine = MaxMag;
            RemainMagazine = RemainMag;
            Stackable = stackable;
            MaxStack = maxStack;
            TheBulletType = BulletEffect;

            TheSprite_Path = spritePath;
            ThePrefab_Path = prefabPath;
        }
    }

    [Serializable]
    public class TheAmmo
    {
        public Ammo[] AllAmmo;
    }
    public TheAmmo AllAmmos = new TheAmmo();


    //||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||装备区
    [Serializable]
    public class Cloth : BaseItem
    {
        public float Defence;
    }

    [Serializable]
    public class TheCloth
    {
        public Cloth[] AllEquipment;
    }
    public TheCloth AllEquip = new TheCloth();


    //||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||消耗品区
    [Serializable]
    public class Consumable : BaseItem
    {

    }

    [Serializable]
    public class TheConsumable
    {
        public Consumable[] AllConsumables;
    }
    public TheConsumable AllConsum = new TheConsumable();

    //||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||药品
    [Serializable]
    public class Drug : BaseItem
    {

    }

    [Serializable]
    public class TheDrug
    {
        public Drug[] AllDrugs;
    }
    public TheDrug AllDrug = new TheDrug();

    [Serializable]
    public class ThingsInInventory {
        public int TheCount;
        public TheType_InSlot TheSlotType;
        public BaseItem TheItemData;

       public ThingsInInventory() //空物体
        {
            TheCount = 0;
            TheSlotType = TheType_InSlot.None;
            TheItemData = null;
        }

       public ThingsInInventory(int count, TheType_InSlot TheType, BaseItem TheData)
        {
            TheCount = count;
            TheSlotType = TheType;
            TheItemData = TheData;
        }

        public ThingsInInventory(TheType_InSlot TheType) //只设置类型
        {
            TheCount = 0;
            TheSlotType = TheType;
            TheItemData = null;
        }
    }


    //public int TheId;

    [Space]
    public string MainPath;
    public string Path_Melee;
    public string MainPath_LongRange;
    public string MainPath_Magazine;
    [Space]
    public string MainPath_Sprite;
    public string Path_Melee_Sprite;
    public string MainPath_LongRange_Sprite;
    public string MainPath_Magazine_Sprite;
    [Space]
    public string Melee_Sprite;
    public string Melee_Prefab;

    void OnValidate()
    {
        MainPath = System.IO.Path.Combine("Assets", "Resource", "Prefab");
        Path_Melee = System.IO.Path.Combine(MainPath, "MeleeWeapon");
        MainPath_LongRange = System.IO.Path.Combine(MainPath, "WeaponAndMa", "RangeWeapon");
        MainPath_Magazine = System.IO.Path.Combine(MainPath, "WeaponAndMa", "Magazine");

        MainPath_Sprite = System.IO.Path.Combine("Assets", "Resource", "GameAssets");
        Path_Melee_Sprite = System.IO.Path.Combine(MainPath_Sprite, "Weapon","MeleeWeapon");
        MainPath_LongRange_Sprite = System.IO.Path.Combine(MainPath_Sprite, "Weapon", "RangedWeapon");
        MainPath_Magazine_Sprite = System.IO.Path.Combine("Assets", "Resource", "Png", "GunAndMa", "Magazine");

        Melee_Sprite = System.IO.Path.Combine("Assets", "Resource", "GameAssets", "Weapon", "MeleeWeapon");
        Melee_Prefab = System.IO.Path.Combine("Assets", "Resource", "Prefab", "MeleeWeapon");

        //TheId = 0;
        CreateMeleeWeapon();
        CreateLongRangeWeapon();
        CreateAmmo();
        CreateEquipment();
        CreateConsumable();

    }



    void Update()
    {

    }

    public void CreateMeleeWeapon()
    {
        MeleeWeapon Crowbar = new MeleeWeapon("_Crowbar", TheType.MeleeWeapon,25,100,1f,40);

        MeleeWeapon BaseBallBat = new MeleeWeapon("_BaseBallBat", TheType.MeleeWeapon, 20,100,1f, 10);

        MeleeWeapon FireAxe = new MeleeWeapon("_FireAxe",TheType.MeleeWeapon, 40, 200,1.5f, 30);

        MeleeWeapon Axe = new MeleeWeapon("_Axe", TheType.MeleeWeapon, 35, 150,1.2f, 25);

        MeleeWeapon Machete = new MeleeWeapon("_Machete", TheType.MeleeWeapon, 30, 150, 1.2f, 10);

        MeleeWeapon Hoe = new MeleeWeapon("_Hoe", TheType.MeleeWeapon, 25, 150, 1.2f, 15);

        MeleeWeapon Pickaxe = new MeleeWeapon("_Pickaxe", TheType.MeleeWeapon, 40, 150, 1.2f, 10);

        MeleeWeapon FireHammer = new MeleeWeapon("_FireHammer", TheType.MeleeWeapon, 30, 150, 1.2f, 10);

        MeleeWeapon LargeMachete = new MeleeWeapon("_LargeMachete", TheType.MeleeWeapon, 25, 150, 1.2f, 30);

        MeleeWeapon Handsaw = new MeleeWeapon("_Handsaw", TheType.MeleeWeapon, 15, 150, 1.2f, 50);

        MeleeWeapon SteelPipe = new MeleeWeapon("_SteelPipe", TheType.MeleeWeapon, 20, 150, 1.2f, 3);

        MeleeWeapon Hammer = new MeleeWeapon("_Hammer", TheType.MeleeWeapon, 15, 150, 1.2f, 7);

        MeleeWeapon ExpandableBaton = new MeleeWeapon("_ExpandableBaton", TheType.MeleeWeapon, 35, 150, 1.2f, 5);

        MeleeWeapon KitchenKnife = new MeleeWeapon("_KitchenKnife", TheType.MeleeWeapon, 22, 150, 1.2f, 30);

        MeleeWeapon Baton = new MeleeWeapon("_Baton", TheType.MeleeWeapon, 30, 150, 1.2f, 10);

        MeleeWeapon Hatchet = new MeleeWeapon("_Hatchet", TheType.MeleeWeapon, 32, 150, 1.2f, 35);

        MeleeWeapon Shovel = new MeleeWeapon("_Shovel", TheType.MeleeWeapon, 22, 150, 1.2f, 10);

        MeleeWeapon Mace = new MeleeWeapon("_Mace", TheType.MeleeWeapon, 65, 150, 1.2f, 60);


        //生成json文件
        AllMelee.AllMeleeWeapons = new MeleeWeapon[]{Crowbar, BaseBallBat, FireAxe, Axe, Machete, Hoe, Pickaxe, FireHammer, LargeMachete, Handsaw, SteelPipe, Hammer, ExpandableBaton,
        KitchenKnife, Baton, Hatchet, Shovel, Mace};

        string MeleeWeaponJson = JsonMapper.ToJson(AllMelee);
        string PathOfRange = System.IO.Path.Combine(Application.dataPath, "Resource", "Json", "MeleeWeapon.json");

        File.WriteAllText(PathOfRange, MeleeWeaponJson);
    }

    public void CreateLongRangeWeapon()
    {
        LongRangeWeapon BasicPistol = new LongRangeWeapon(
            "_BasicPistol",
            TheType.LongRangeWeapon,
            TheHandleStyle.Normal,
            MaxAmmo:1,
            "_BasicPistol_Des",
            new string[] { "_NormalPistolAmmo", "_ExtendedPistolMagazine" },
            1,
            AutoShoot:false,
            Shell: true,
            Bullet: false,
            Laser: false,
            Backward:0.4f,
            0.3f,
            false,

            new ThingsInInventory[1] {
                    new ThingsInInventory(TheType_InSlot.ForMagazine)
            },

            System.IO.Path.Combine(MainPath_LongRange_Sprite, "Pistol_Pre.png"),
            System.IO.Path.Combine(MainPath_LongRange, "pistol.prefab")
        );

        LongRangeWeapon UZISMG = new LongRangeWeapon(
            uniqueCode: "_UZISMG",
            type: TheType.LongRangeWeapon,
            style: TheHandleStyle.Normal,
            MaxAmmo: 1,
            Des: "_UZISMG_Des",
            AmmoUnique: new string[] { "_SubmachineGunAmmo" },
            damageM: 1,
            AutoShoot: true,
            Shell: true,
            Bullet: false,
            Laser: false,
            Backward: 0.8f,
            shootspeed: 0.01f,
            stackable: false,

            TheInventory: new ThingsInInventory[1] {
                            new ThingsInInventory(TheType_InSlot.ForMagazine)
            },

            spritePath: System.IO.Path.Combine(MainPath_LongRange_Sprite, "UZISMG_Pre.png"),
            prefabPath: System.IO.Path.Combine(MainPath_LongRange, "UZISMG.prefab")
            );

        LongRangeWeapon AS416 = new LongRangeWeapon(
            "_AS416",
            TheType.LongRangeWeapon,
            TheHandleStyle.MainHandWeapon,
            MaxAmmo: 1,
            "_AS416_Des",
            new string[] { "_AS416Ammo" },
            1,
            AutoShoot: true,
            Shell: true,
            Bullet: false,
            Laser: false,
            Backward: 1.3f,
            0.5f,
            false,

            new ThingsInInventory[1] {
                    new ThingsInInventory(TheType_InSlot.ForMagazine)
            },

            System.IO.Path.Combine(MainPath_LongRange_Sprite, "AS416_Pre.png"),
            System.IO.Path.Combine(MainPath_LongRange, "AS416.prefab")
            );

        LongRangeWeapon M414ShotGun = new LongRangeWeapon(
            uniqueCode: "_M414ShotGun",
            type: TheType.LongRangeWeapon,
            style: TheHandleStyle.MainHandWeapon,
            MaxAmmo: 5,
            Des: "_M414ShotGun_Des",
            AmmoUnique: new string[] { "_NormalShotGunAmmo" },
            damageM: 1,
            AutoShoot: false,
            Shell: true,
            Bullet: false,
            Laser: false,
            Backward: 4f,
            shootspeed: 1.85f,
            stackable: false,

            TheInventory: new ThingsInInventory[1] {
                                    new ThingsInInventory(TheType_InSlot.ForMagazine)
            },

            spritePath: System.IO.Path.Combine(MainPath_LongRange_Sprite, "M414ShotGun_Pre.png"),
            prefabPath: System.IO.Path.Combine(MainPath_LongRange, "M414ShotGun.prefab")
            );

        LongRangeWeapon S10ShotGun = new LongRangeWeapon(
            uniqueCode: "_S10ShotGun",
            type: TheType.LongRangeWeapon,
            style: TheHandleStyle.MainHandWeapon,
            MaxAmmo: 1,
            Des: "_S10ShotGun_Des",
            AmmoUnique: new string[] { "_ShotGunAmmo" },
            damageM: 0.9f,
            AutoShoot: false,
            Shell: true,
            Bullet: false,
            Laser: false,
            Backward: 5f,
            shootspeed: 1,
            stackable: false,

            TheInventory: new ThingsInInventory[1] {
                                            new ThingsInInventory(TheType_InSlot.ForMagazine)
            },

            spritePath: System.IO.Path.Combine(MainPath_LongRange_Sprite, "S10ShotGun_Pre.png"),
            prefabPath: System.IO.Path.Combine(MainPath_LongRange, "S10ShotGun.prefab")
            );

        LongRangeWeapon UMG45SMG = new LongRangeWeapon(
            uniqueCode: "_UMG45SMG",
            type: TheType.LongRangeWeapon,
            style: TheHandleStyle.MainHandWeapon,
            MaxAmmo: 1,
            Des: "_UMG45SMG_Des",
            AmmoUnique: new string[] { "_HeavySMGAmmo", "_SubmachineGunAmmo" },
            damageM: 1.2f,
            AutoShoot: true,
            Shell: true,
            Bullet: false,
            Laser: false,
            Backward: 1.6f,
            shootspeed: 0.06f,
            stackable: false,

            TheInventory: new ThingsInInventory[1] {
                                            new ThingsInInventory(TheType_InSlot.ForMagazine)
            },

            spritePath: System.IO.Path.Combine(MainPath_LongRange_Sprite, "UMG45SMG_Pre.png"),
            prefabPath: System.IO.Path.Combine(MainPath_LongRange, "UMG45SMG.prefab")
            );

        LongRangeWeapon AWMSR = new LongRangeWeapon(
            uniqueCode: "_AWMSR",
            type: TheType.LongRangeWeapon,
            style: TheHandleStyle.MainHandWeapon,
            MaxAmmo: 1,
            Des: "_AWMSR_Des",
            AmmoUnique: new string[] { "_SniperAmmo" },
            damageM: 1f,
            AutoShoot: false,
            Shell: true,
            Bullet: false,
            Laser: false,
            Backward: 10f,
            shootspeed: 5.5f,
            stackable: false,

            TheInventory: new ThingsInInventory[1] {
                                                    new ThingsInInventory(TheType_InSlot.ForMagazine)
            },

            spritePath: System.IO.Path.Combine(MainPath_LongRange_Sprite, "AWMSR_Pre.png"),
            prefabPath: System.IO.Path.Combine(MainPath_LongRange, "AWMSR.prefab")
            );

        LongRangeWeapon MK1941LMG = new LongRangeWeapon(
            uniqueCode: "_MK1941LMG",
            type: TheType.LongRangeWeapon,
            style: TheHandleStyle.MainHandWeapon,
            MaxAmmo: 1,
            Des: "_MK1941LMG_Des",
            AmmoUnique: new string[] { "_Type2LMGAmmo" },
            damageM: 1.1f,
            AutoShoot: true,
            Shell: true,
            Bullet: false,
            Laser: false,
            Backward: 2.5f,
            shootspeed: 0.035f,
            stackable: false,

            TheInventory: new ThingsInInventory[1] {
                                                            new ThingsInInventory(TheType_InSlot.ForMagazine)
            },

            spritePath: System.IO.Path.Combine(MainPath_LongRange_Sprite, "MK1941LMG_Pre.png"),
            prefabPath: System.IO.Path.Combine(MainPath_LongRange, "MK1941LMG.prefab")
            );

        LongRangeWeapon TP249LMG = new LongRangeWeapon(
            uniqueCode: "_TP249LMG",
            type: TheType.LongRangeWeapon,
            style: TheHandleStyle.MainHandWeapon,
            MaxAmmo: 1,
            Des: "_TP249LMG_Des",
            AmmoUnique: new string[] { "_LMGAmmo" },
            damageM: 1.3f,
            AutoShoot: true,
            Shell: true,
            Bullet: false,
            Laser: false,
            Backward: 2f,
            shootspeed: 0.08f,
            stackable: false,

            TheInventory: new ThingsInInventory[1] {
                                                                    new ThingsInInventory(TheType_InSlot.ForMagazine)
            },

            spritePath: System.IO.Path.Combine(MainPath_LongRange_Sprite, "TP249LMG_Pre.png"),
            prefabPath: System.IO.Path.Combine(MainPath_LongRange, "TP249LMG.prefab")
            );

        LongRangeWeapon C98Pistol = new LongRangeWeapon(
            uniqueCode: "_C98Pistol",
            type: TheType.LongRangeWeapon,
            style: TheHandleStyle.Normal,
            MaxAmmo: 1,
            Des: "_C98Pistol_Des",
            AmmoUnique: new string[] { "_HeavyPistolAmmo" },
            damageM: 1.2f,
            AutoShoot: false,
            Shell: true,
            Bullet: false,
            Laser: false,
            Backward: 1.5f,
            shootspeed: 0.7f,
            stackable: false,

            TheInventory: new ThingsInInventory[1] {
                                                                            new ThingsInInventory(TheType_InSlot.ForMagazine)
            },

            spritePath: System.IO.Path.Combine(MainPath_LongRange_Sprite, "C98Pistol_Pre.png"),
            prefabPath: System.IO.Path.Combine(MainPath_LongRange, "C98Pistol.prefab")
            );

        LongRangeWeapon GrenadeLauncher = new LongRangeWeapon(
            uniqueCode: "_GrenadeLauncher",
            type: TheType.LongRangeWeapon,
            style: TheHandleStyle.MainHandWeapon,
            MaxAmmo: 4,
            Des: "_GrenadeLauncher_Des",
            AmmoUnique: new string[] { "_GrenadeLauncherAmmo" },
            damageM: 1f,
            AutoShoot: false,
            Shell: false,
            Bullet: true,
            Laser: false,
            Backward: 4f,
            shootspeed: 0.8f,
            stackable: false,

            TheInventory: new ThingsInInventory[1] {
                       new ThingsInInventory(TheType_InSlot.ForMagazine)
            },

            spritePath: System.IO.Path.Combine(MainPath_LongRange_Sprite, "GrenadeLauncher_Pre.png"),
            prefabPath: System.IO.Path.Combine(MainPath_LongRange, "GrenadeLauncher.prefab")
            );

        LongRangeWeapon MK1907 = new LongRangeWeapon(
            uniqueCode: "_MK1907",
            type: TheType.LongRangeWeapon,
            style: TheHandleStyle.MainHandWeapon,
            MaxAmmo: 4,
            Des: "_MK1907_Des",
            AmmoUnique: new string[] { "_MK1907Ammo" },
            damageM: 1f,
            AutoShoot: true,
            Shell: true,
            Bullet: false,
            Laser: false,
            Backward: 1f,
            shootspeed: 0.03f,
            stackable: false,

            TheInventory: new ThingsInInventory[1] {
                               new ThingsInInventory(TheType_InSlot.ForMagazine)
            },

            spritePath: System.IO.Path.Combine(MainPath_LongRange_Sprite, "MK1907_Pre.png"),
            prefabPath: System.IO.Path.Combine(MainPath_LongRange, "MK1907.prefab")
            );

        LongRangeWeapon NailGun = new LongRangeWeapon(
            uniqueCode: "_NailGun",
            type: TheType.LongRangeWeapon,
            style: TheHandleStyle.Normal,
            MaxAmmo: 8,
            Des: "_NailGun_Des",
            AmmoUnique: new string[] { "_Nail" },
            damageM: 1f,
            AutoShoot: false,
            Shell: false,
            Bullet: true,
            Laser: false,
            Backward: 2f,
            shootspeed: 0.5f,
            stackable: false,

            TheInventory: new ThingsInInventory[1] {
                                       new ThingsInInventory(TheType_InSlot.ForMagazine)
            },

            spritePath: System.IO.Path.Combine(MainPath_LongRange_Sprite, "NailGun_Pre.png"),
            prefabPath: System.IO.Path.Combine(MainPath_LongRange, "NailGun.prefab")
            );

        LongRangeWeapon Revolver = new LongRangeWeapon(
            uniqueCode: "_Revolver",
            type: TheType.LongRangeWeapon,
            style: TheHandleStyle.Normal,
            MaxAmmo: 6,
            Des: "_Revolver_Des",
            AmmoUnique: new string[] { "_RevolverAmmo" },
            damageM: 1.3f,
            AutoShoot: false,
            Shell: false,
            Bullet: false,
            Laser: false,
            Backward: 2f,
            shootspeed: 0.4f,
            stackable: false,

            TheInventory: new ThingsInInventory[1] {
                                               new ThingsInInventory(TheType_InSlot.ForMagazine)
            },

            spritePath: System.IO.Path.Combine(MainPath_LongRange_Sprite, "Revolver_Pre.png"),
            prefabPath: System.IO.Path.Combine(MainPath_LongRange, "Revolver.prefab")
            );

        LongRangeWeapon LS416 = new LongRangeWeapon(
            uniqueCode: "_LS416",
            type: TheType.LongRangeWeapon,
            style: TheHandleStyle.MainHandWeapon,
            MaxAmmo: 1,
            Des: "_LS416_Des",
            AmmoUnique: new string[] { "_LightAssaultRifleAmmo", "_AS416Ammo" },
            damageM: 0.85f,
            AutoShoot: true,
            Shell: true,
            Bullet: false,
            Laser: false,
            Backward: 0.8f,
            shootspeed: 0.07f,
            stackable: false,

            TheInventory: new ThingsInInventory[1] {
                                                       new ThingsInInventory(TheType_InSlot.ForMagazine)
            },

            spritePath: System.IO.Path.Combine(MainPath_LongRange_Sprite, "LS416_Pre.png"),
            prefabPath: System.IO.Path.Combine(MainPath_LongRange, "LS416.prefab")
            );

        LongRangeWeapon AirGun = new LongRangeWeapon(
            uniqueCode: "_AirGun",
            type: TheType.LongRangeWeapon,
            style: TheHandleStyle.MainHandWeapon,
            MaxAmmo: 1,
            Des: "_AirGun_Des",
            AmmoUnique: new string[] { "_AirGunAmmo" },
            damageM: 1f,
            AutoShoot: true,
            Shell: false,
            Bullet: false,
            Laser: false,
            Backward: 0.7f,
            shootspeed: 0.06f,
            stackable: false,

            TheInventory: new ThingsInInventory[1] {
                                                               new ThingsInInventory(TheType_InSlot.ForMagazine)
            },

            spritePath: System.IO.Path.Combine(MainPath_LongRange_Sprite, "AirGun_Pre.png"),
            prefabPath: System.IO.Path.Combine(MainPath_LongRange, "AirGun.prefab")
            );

        LongRangeWeapon H1894Rifle = new LongRangeWeapon(
            uniqueCode: "_H1894Rifle",
            type: TheType.LongRangeWeapon,
            style: TheHandleStyle.MainHandWeapon,
            MaxAmmo: 6,
            Des: "_H1894Rifle_Des",
            AmmoUnique: new string[] { "_HighVelocityRifleAmmo" },
            damageM: 1f,
            AutoShoot: false,
            Shell: true,
            Bullet: false,
            Laser: false,
            Backward: 0.7f,
            shootspeed: 0.06f,
            stackable: false,

            TheInventory: new ThingsInInventory[1] {
                                                                       new ThingsInInventory(TheType_InSlot.ForMagazine)
            },

            spritePath: System.IO.Path.Combine(MainPath_LongRange_Sprite, "H1894Rifle_Pre.png"),
            prefabPath: System.IO.Path.Combine(MainPath_LongRange, "H1894Rifle.prefab")
            );

        LongRangeWeapon AutoPistol = new LongRangeWeapon(
            uniqueCode: "_AutoPistol",
            type: TheType.LongRangeWeapon,
            style: TheHandleStyle.Normal,
            MaxAmmo: 6,
            Des: "_AutoPistol_Des",
            AmmoUnique: new string[] { "_ExtendedPistolMagazine", "_NormalPistolAmmo" },
            damageM: 0.85f,
            AutoShoot: true,
            Shell: true,
            Bullet: false,
            Laser: false,
            Backward: 0.5f,
            shootspeed: 0.03f,
            stackable: false,

            TheInventory: new ThingsInInventory[1] {
                                                                               new ThingsInInventory(TheType_InSlot.ForMagazine)
            },

            spritePath: System.IO.Path.Combine(MainPath_LongRange_Sprite, "AutoPistol_Pre.png"),
            prefabPath: System.IO.Path.Combine(MainPath_LongRange, "AutoPistol.prefab")
            );

        LongRangeWeapon MultiAssaultRifle = new LongRangeWeapon(
            uniqueCode: "_MultiAssaultRifle",
            type: TheType.LongRangeWeapon,
            style: TheHandleStyle.MainHandWeapon,
            MaxAmmo: 1,
            Des: "_MultiAssaultRifle_Des",
            AmmoUnique: new string[] { "_MultiAssaultRifleAmmo", "_AS416Ammo" },
            damageM: 1f,
            AutoShoot: true,
            Shell: true,
            Bullet: false,
            Laser: false,
            Backward: 3f,
            shootspeed: 0.2f,
            stackable: false,

            TheInventory: new ThingsInInventory[1] {
                                                                                       new ThingsInInventory(TheType_InSlot.ForMagazine)
            },

            spritePath: System.IO.Path.Combine(MainPath_LongRange_Sprite, "MultiAssaultRifle_Pre.png"),
            prefabPath: System.IO.Path.Combine(MainPath_LongRange, "MultiAssaultRifle.prefab")
            );

        LongRangeWeapon BattleFlareGun = new LongRangeWeapon(
            uniqueCode: "_BattleFlareGun",
            type: TheType.LongRangeWeapon,
            style: TheHandleStyle.Normal,
            MaxAmmo: 1,
            Des: "_BattleFlareGun_Des",
            AmmoUnique: new string[] { "_SignalFlare" },
            damageM: 1f,
            AutoShoot: false,
            Shell: false,
            Bullet: true,
            Laser: false,
            Backward: 2f,
            shootspeed: 0.1f,
            stackable: false,

            TheInventory: new ThingsInInventory[1] {
                                                                                               new ThingsInInventory(TheType_InSlot.ForMagazine)
            },

            spritePath: System.IO.Path.Combine(MainPath_LongRange_Sprite, "BattleFlareGun_Pre.png"),
            prefabPath: System.IO.Path.Combine(MainPath_LongRange, "BattleFlareGun.prefab")
            );

        LongRangeWeapon LaserPistol = new LongRangeWeapon(
            uniqueCode: "_LaserPistol",
            type: TheType.LongRangeWeapon,
            style: TheHandleStyle.Normal,
            MaxAmmo: 1,
            Des: "_LaserPistol_Des",
            AmmoUnique: new string[] { "_LaserPistolAmmo" },
            damageM: 1f,
            AutoShoot: false,
            Shell: false,
            Bullet: true,
            Laser: true,
            Backward: 2f,
            shootspeed: 0.3f,
            stackable: false,

            TheInventory: new ThingsInInventory[1] {
                                                                                                       new ThingsInInventory(TheType_InSlot.ForMagazine)
            },

            spritePath: System.IO.Path.Combine(MainPath_LongRange_Sprite, "LaserPistol_Pre.png"),
            prefabPath: System.IO.Path.Combine(MainPath_LongRange, "LaserPistol.prefab")
            );

        LongRangeWeapon LaserCannon = new LongRangeWeapon(
            uniqueCode: "_LaserCannon",
            type: TheType.LongRangeWeapon,
            style: TheHandleStyle.MainHandWeapon,
            MaxAmmo: 1,
            Des: "_LaserCannon_Des",
            AmmoUnique: new string[] { "_LaserCannonAmmo" },
            damageM: 1f,
            AutoShoot: false,
            Shell: false,
            Bullet: false,
            Laser: true,
            Backward: 5f,
            shootspeed: 0.7f,
            stackable: false,

            TheInventory: new ThingsInInventory[1] {
                                                                                                               new ThingsInInventory(TheType_InSlot.ForMagazine)
            },

            spritePath: System.IO.Path.Combine(MainPath_LongRange_Sprite, "LaserCannon_Pre.png"),
            prefabPath: System.IO.Path.Combine(MainPath_LongRange, "LaserCannon.prefab")
            );

        LongRangeWeapon LaserAssaultRifle = new LongRangeWeapon(
            uniqueCode: "_LaserAssaultRifle",
            type: TheType.LongRangeWeapon,
            style: TheHandleStyle.MainHandWeapon,
            MaxAmmo: 1,
            Des: "_LaserAssaultRifle_Des",
            AmmoUnique: new string[] { "_AS416Ammo" },
            damageM: 1.2f,
            AutoShoot: true,
            Shell: false,
            Bullet: false,
            Laser: true,
            Backward: 3f,
            shootspeed: 0.4f,
            stackable: false,

            TheInventory: new ThingsInInventory[1] {
                                     new ThingsInInventory(TheType_InSlot.ForMagazine)
            },

            spritePath: System.IO.Path.Combine(MainPath_LongRange_Sprite, "LaserAssaultRifle_Pre.png"),
            prefabPath: System.IO.Path.Combine(MainPath_LongRange, "LaserAssaultRifle.prefab")
            );

        LongRangeWeapon HP9ShotGun = new LongRangeWeapon(
            uniqueCode: "_HP9ShotGun",
            type: TheType.LongRangeWeapon,
            style: TheHandleStyle.Normal,
            MaxAmmo: 2,
            Des: "_HP9ShotGun_Des",
            AmmoUnique: new string[] { "_NormalShotGunAmmo" },
            damageM: 0.75f,
            AutoShoot: false,
            Shell: false,
            Bullet: false,
            Laser: false,
            Backward: 3f,
            shootspeed: 0.5f,
            stackable: false,

            TheInventory: new ThingsInInventory[1] {
                                             new ThingsInInventory(TheType_InSlot.ForMagazine)
            },

            spritePath: System.IO.Path.Combine(MainPath_LongRange_Sprite, "HP9ShotGun_Pre.png"),
            prefabPath: System.IO.Path.Combine(MainPath_LongRange, "HP9ShotGun.prefab")
            );

        LongRangeWeapon TP2Revolver = new LongRangeWeapon(
            uniqueCode: "_TP2Revolver",
            type: TheType.LongRangeWeapon,
            style: TheHandleStyle.Normal,
            MaxAmmo: 4,
            Des: "_TP2Revolver_Des",
            AmmoUnique: new string[] { "_RevolverAmmo" },
            damageM: 1.3f,
            AutoShoot: false,
            Shell: false,
            Bullet: false,
            Laser: false,
            Backward: 3f,
            shootspeed: 0.4f,
            stackable: false,

            TheInventory: new ThingsInInventory[1] {
                                                     new ThingsInInventory(TheType_InSlot.ForMagazine)
            },

            spritePath: System.IO.Path.Combine(MainPath_LongRange_Sprite, "TP2Revolver_Pre.png"),
            prefabPath: System.IO.Path.Combine(MainPath_LongRange, "TP2Revolver.prefab")
            );

        LongRangeWeapon TP28AssaultPistol = new LongRangeWeapon(
            uniqueCode: "_TP28AssaultPistol",
            type: TheType.LongRangeWeapon,
            style: TheHandleStyle.Normal,
            MaxAmmo: 1,
            Des: "_TP28AssaultPistol_Des",
            AmmoUnique: new string[] { "_ExtendedPistolMagazine", "_SubmachineGunAmmo", "_NormalPistolAmmo" },
            damageM: 0.9f,
            AutoShoot: true,
            Shell: true,
            Bullet: false,
            Laser: false,
            Backward: 0.5f,
            shootspeed: 0.025f,
            stackable: false,

            TheInventory: new ThingsInInventory[1] {
                                                             new ThingsInInventory(TheType_InSlot.ForMagazine)
            },

            spritePath: System.IO.Path.Combine(MainPath_LongRange_Sprite, "TP28AssaultPistol_Pre.png"),
            prefabPath: System.IO.Path.Combine(MainPath_LongRange, "TP28AssaultPistol.prefab")
            );

        LongRangeWeapon AntiTankSR = new LongRangeWeapon(
            uniqueCode: "_AntiTankSR",
            type: TheType.LongRangeWeapon,
            style: TheHandleStyle.MainHandWeapon,
            MaxAmmo: 1,
            Des: "_AntiTankSR_Des",
            AmmoUnique: new string[] { "_SniperAmmo" },
            damageM: 2.5f,
            AutoShoot: false,
            Shell: true,
            Bullet: false,
            Laser: false,
            Backward: 15f,
            shootspeed: 7.5f,
            stackable: false,

            TheInventory: new ThingsInInventory[1] {
                                                                     new ThingsInInventory(TheType_InSlot.ForMagazine)
            },

            spritePath: System.IO.Path.Combine(MainPath_LongRange_Sprite, "AntiTankSR_Pre.png"),
            prefabPath: System.IO.Path.Combine(MainPath_LongRange, "AntiTankSR.prefab")
            );

        LongRangeWeapon TheHunter = new LongRangeWeapon(
            uniqueCode: "_TheHunter",
            type: TheType.LongRangeWeapon,
            style: TheHandleStyle.MainHandWeapon,
            MaxAmmo: 2,
            Des: "_TheHunter_Des",
            AmmoUnique: new string[] { "_NormalShotGunAmmo" },
            damageM: 1.5f,
            AutoShoot: false,
            Shell: false,
            Bullet: false,
            Laser: false,
            Backward: 5f,
            shootspeed: 0.5f,
            stackable: false,

            TheInventory: new ThingsInInventory[1] {
                               new ThingsInInventory(TheType_InSlot.ForMagazine)
            },

            spritePath: System.IO.Path.Combine(MainPath_LongRange_Sprite, "TheHunter_Pre.png"),
            prefabPath: System.IO.Path.Combine(MainPath_LongRange, "TheHunter.prefab")
            );

        LongRangeWeapon TP2BR = new LongRangeWeapon(
            uniqueCode: "_TP2BR",
            type: TheType.LongRangeWeapon,
            style: TheHandleStyle.MainHandWeapon,
            MaxAmmo: 1,
            Des: "_TP2BR_Des",
            AmmoUnique: new string[] { "_MK1907Ammo" },
            damageM: 1.2f,
            AutoShoot: true,
            Shell: true,
            Bullet: false,
            Laser: false,
            Backward: 1.7f,
            shootspeed: 0.08f,
            stackable: false,

            TheInventory: new ThingsInInventory[1] {
                                       new ThingsInInventory(TheType_InSlot.ForMagazine)
            },

            spritePath: System.IO.Path.Combine(MainPath_LongRange_Sprite, "TP2BR_Pre.png"),
            prefabPath: System.IO.Path.Combine(MainPath_LongRange, "TP2BR.prefab")
            );

        LongRangeWeapon TP2MR = new LongRangeWeapon(
            uniqueCode: "_TP2MR",
            type: TheType.LongRangeWeapon,
            style: TheHandleStyle.MainHandWeapon,
            MaxAmmo: 1,
            Des: "_TP2MR_Des",
            AmmoUnique: new string[] { "_HalfAutoAmmo" },
            damageM: 1.2f,
            AutoShoot: true,
            Shell: true,
            Bullet: false,
            Laser: false,
            Backward: 1.5f,
            shootspeed: 0.4f,
            stackable: false,

            TheInventory: new ThingsInInventory[1] {
                                               new ThingsInInventory(TheType_InSlot.ForMagazine)
            },

            spritePath: System.IO.Path.Combine(MainPath_LongRange_Sprite, "TP2MR_Pre.png"),
            prefabPath: System.IO.Path.Combine(MainPath_LongRange, "TP2MR.prefab")
            );

        LongRangeWeapon HalfAutoRifle = new LongRangeWeapon(
            uniqueCode: "_HalfAutoRifle",
            type: TheType.LongRangeWeapon,
            style: TheHandleStyle.MainHandWeapon,
            MaxAmmo: 1,
            Des: "_HalfAutoRifle_Des",
            AmmoUnique: new string[] { "_HalfAutoAmmo" },
            damageM: 1f,
            AutoShoot: true,
            Shell: true,
            Bullet: false,
            Laser: false,
            Backward: 1.1f,
            shootspeed: 0.5f,
            stackable: false,

            TheInventory: new ThingsInInventory[1] {
                                                       new ThingsInInventory(TheType_InSlot.ForMagazine)
            },

            spritePath: System.IO.Path.Combine(MainPath_LongRange_Sprite, "HalfAutoRifle_Pre.png"),
            prefabPath: System.IO.Path.Combine(MainPath_LongRange, "HalfAutoRifle.prefab")
            );

        LongRangeWeapon HighPressureNailGun = new LongRangeWeapon(
            uniqueCode: "_HighPressureNailGun",
            type: TheType.LongRangeWeapon,
            style: TheHandleStyle.MainHandWeapon,
            MaxAmmo: 10,
            Des: "_HighPressureNailGun_Des",
            AmmoUnique: new string[] { "_Nail" },
            damageM: 1.7f,
            AutoShoot: false,
            Shell: false,
            Bullet: true,
            Laser: false,
            Backward: 3f,
            shootspeed: 0.7f,
            stackable: false,

            TheInventory: new ThingsInInventory[1] {
                                                               new ThingsInInventory(TheType_InSlot.ForMagazine)
            },

            spritePath: System.IO.Path.Combine(MainPath_LongRange_Sprite, "HighPressureNailGu_Pre.png"),
            prefabPath: System.IO.Path.Combine(MainPath_LongRange, "HighPressureNailGun.prefab")
            );

        LongRangeWeapon NightShadowSMG = new LongRangeWeapon(
            uniqueCode: "_NightShadowSMG",
            type: TheType.LongRangeWeapon,
            style: TheHandleStyle.MainHandWeapon,
            MaxAmmo: 1,
            Des: "_NightShadowSMG_Des",
            AmmoUnique: new string[] { "_HeavySMGAmmo" },
            damageM: 1.5f,
            AutoShoot: true,
            Shell: true,
            Bullet: false,
            Laser: false,
            Backward: 1f,
            shootspeed: 0.04f,
            stackable: false,

            TheInventory: new ThingsInInventory[1] {
                                                                       new ThingsInInventory(TheType_InSlot.ForMagazine)
            },

            spritePath: System.IO.Path.Combine(MainPath_LongRange_Sprite, "NightShadowSMG_Pre.png"),
            prefabPath: System.IO.Path.Combine(MainPath_LongRange, "NightShadowSubMachineGun.prefab")
            );

        LongRangeWeapon TP96LMG = new LongRangeWeapon(
            uniqueCode: "_TP96LMG",
            type: TheType.LongRangeWeapon,
            style: TheHandleStyle.MainHandWeapon,
            MaxAmmo: 1,
            Des: "_TP96LMG_Des",
            AmmoUnique: new string[] { "_Type2LMGAmmo" },
            damageM: 1.1f,
            AutoShoot: true,
            Shell: true,
            Bullet: false,
            Laser: false,
            Backward: 1.7f,
            shootspeed: 0.11f,
            stackable: false,

            TheInventory: new ThingsInInventory[1] {
                                   new ThingsInInventory(TheType_InSlot.ForMagazine)
            },

            spritePath: System.IO.Path.Combine(MainPath_LongRange_Sprite, "TP96LMG_Pre.png"),
            prefabPath: System.IO.Path.Combine(MainPath_LongRange, "TP96LMG.prefab")
            );

        LongRangeWeapon QZ95AS = new LongRangeWeapon(
            uniqueCode: "_QZ95AS",
            type: TheType.LongRangeWeapon,
            style: TheHandleStyle.MainHandWeapon,
            MaxAmmo: 1,
            Des: "_QZ95AS_Des",
            AmmoUnique: new string[] { "_AS416Ammo" },
            damageM: 1.1f,
            AutoShoot: true,
            Shell: true,
            Bullet: false,
            Laser: false,
            Backward: 1.2f,
            shootspeed: 0.07f,
            stackable: false,

            TheInventory: new ThingsInInventory[1] {
                                           new ThingsInInventory(TheType_InSlot.ForMagazine)
            },

            spritePath: System.IO.Path.Combine(MainPath_LongRange_Sprite, "QZ95AS_Pre.png"),
            prefabPath: System.IO.Path.Combine(MainPath_LongRange, "QZ95AS.prefab")
            );

        LongRangeWeapon OneHandGrenadeLauncher = new LongRangeWeapon(
            uniqueCode: "_SimpleGrenadeLauncher",
            type: TheType.LongRangeWeapon,
            style: TheHandleStyle.Normal,
            MaxAmmo: 1,
            Des: "_SimpleGrenadeLauncher_Des",
            AmmoUnique: new string[] { "_GrenadeLauncherAmmo" },
            damageM: 0.95f,
            AutoShoot: false,
            Shell: false,
            Bullet: false,
            Laser: false,
            Backward: 4f,
            shootspeed: 0.8f,
            stackable: false,

            TheInventory: new ThingsInInventory[1] {
                                                   new ThingsInInventory(TheType_InSlot.ForMagazine)
            },

            spritePath: System.IO.Path.Combine(MainPath_LongRange_Sprite, "SimpleGrenadeLauncher_Pre.png"),
            prefabPath: System.IO.Path.Combine(MainPath_LongRange, "SimpleGrenadeLauncher.prefab")
            );

        LongRangeWeapon StabilizedSMG = new LongRangeWeapon(
            uniqueCode: "_StabilizedSMG",
            type: TheType.LongRangeWeapon,
            style: TheHandleStyle.Normal,
            MaxAmmo: 1,
            Des: "_StabilizedSMG_Des",
            AmmoUnique: new string[] { "_SubmachineGunAmmo" },
            damageM: 0.85f,
            AutoShoot: true,
            Shell: true,
            Bullet: false,
            Laser: false,
            Backward: 0.6f,
            shootspeed: 0.07f,
            stackable: false,

            TheInventory: new ThingsInInventory[1] {
                                                           new ThingsInInventory(TheType_InSlot.ForMagazine)
            },

            spritePath: System.IO.Path.Combine(MainPath_LongRange_Sprite, "StabilizedSMG_Pre.png"),
            prefabPath: System.IO.Path.Combine(MainPath_LongRange, "StabilizedSMG.prefab")
            );

        LongRangeWeapon HunterPistol = new LongRangeWeapon(
            uniqueCode: "_HunterPistol",
            type: TheType.LongRangeWeapon,
            style: TheHandleStyle.Normal,
            MaxAmmo: 1,
            Des: "_HunterPistol_Des",
            AmmoUnique: new string[] { "_HeavyPistolAmmo" },
            damageM: 1.5f,
            AutoShoot: false,
            Shell: true,
            Bullet: false,
            Laser: false,
            Backward: 2f,
            shootspeed: 0.8f,
            stackable: false,

            TheInventory: new ThingsInInventory[1] {
                               new ThingsInInventory(TheType_InSlot.ForMagazine)
            },

            spritePath: System.IO.Path.Combine(MainPath_LongRange_Sprite, "HunterPistol_Pre.png"),
            prefabPath: System.IO.Path.Combine(MainPath_LongRange, "HunterPistol.prefab")
            );

        LongRangeWeapon AH12ShotGunn = new LongRangeWeapon(
            uniqueCode: "_AH12ShotGun",
            type: TheType.LongRangeWeapon,
            style: TheHandleStyle.MainHandWeapon,
            MaxAmmo: 1,
            Des: "_AH12ShotGun_Des",
            AmmoUnique: new string[] { "_ShotGunAmmo" },
            damageM: 0.8f,
            AutoShoot: true,
            Shell: true,
            Bullet: false,
            Laser: false,
            Backward: 3f,
            shootspeed: 0.6f,
            stackable: false,

            TheInventory: new ThingsInInventory[1] {
                                        new ThingsInInventory(TheType_InSlot.ForMagazine)
            },

            spritePath: System.IO.Path.Combine(MainPath_LongRange_Sprite, "AH12ShotGun_Pre.png"),
            prefabPath: System.IO.Path.Combine(MainPath_LongRange, "AH12ShotGun.prefab")
            );

        LongRangeWeapon Chainsaw = new LongRangeWeapon(
            uniqueCode: "_Chainsaw",
            type: TheType.LongRangeWeapon,
            style: TheHandleStyle.MainHandWeapon,
            MaxAmmo: 1,
            Des: "_Chainsaw_Des",
            AmmoUnique: new string[] { "_BigFireJar" },
            damageM: 1f,
            AutoShoot: false,
            Shell: false,
            Bullet: false,
            Laser: false,
            Backward: 0f,
            shootspeed: 0f,
            stackable: false,

            TheInventory: new ThingsInInventory[1] {
                                                new ThingsInInventory(TheType_InSlot.ForMagazine)
            },

            spritePath: System.IO.Path.Combine(Melee_Sprite, "Chainsaw_Pre.png"),
            prefabPath: System.IO.Path.Combine(Melee_Prefab, "Chainsaw.prefab")
            );

        LongRangeWeapon Circularsaw = new LongRangeWeapon(
            uniqueCode: "_Circularsaw",
            type: TheType.LongRangeWeapon,
            style: TheHandleStyle.Normal,
            MaxAmmo: 1,
            Des: "_Circularsaw_Des",
            AmmoUnique: new string[] { "_BigFireJar" },
            damageM: 0.5f,
            AutoShoot: false,
            Shell: false,
            Bullet: false,
            Laser: false,
            Backward: 0f,
            shootspeed: 0f,
            stackable: false,

            TheInventory: new ThingsInInventory[1] {
                                                        new ThingsInInventory(TheType_InSlot.ForMagazine)
            },

            spritePath: System.IO.Path.Combine(Melee_Sprite, "Circularsaw_Pre.png"),
            prefabPath: System.IO.Path.Combine(Melee_Prefab, "Circularsaw.prefab")
            );

        //LongRangeWeapon Firethrower = new LongRangeWeapon();
        //Firethrower.UniqueCode = "_Firethrower";
        //Firethrower.Damage = 2;
        //Firethrower.MaxMagazine = 800;
        //Firethrower.AmmoTypeId = 0;
        //Firethrower.ShootSpeed = 8;
        //Firethrower.Type = TheType.LongRangeWeapon;


        //生成json文件
        //AllRange.AllLongRangeWeapons = new LongRangeWeapon[]{BasicPistol, UZISMG, AS416, S10ShotGun, HalfAutoRifle, AWMSR, UMG45SMG, C98Pistol ,TP249LMG, MK1907,
        //NailGun, M414ShotGun, Firethrower, Revolver, GrenadeLauncher, LS416, AirGun, H1894Rifle, AutoPistol, MK1941LMG, MultiAssaultRifle, BattleFlareGun,
        //LaserPistol, LaserCannon, LaserAssaultRifle, HP9ShotGun, TP2Revolver, TP28AssaultPistol, AntiTankSR, TheHunter, TP2BR, TP2MR,
        //HighPressureNailGun, NightShadowSMG, TP96LMG, QZ95AS, OneHandGrenadeLauncher, StabilizedSMG, HunterPistol, AH12ShotGunn, Chainsaw, Circularsaw};

        AllRange.AllLongRangeWeapons = new LongRangeWeapon[]{BasicPistol, AS416, UZISMG, M414ShotGun, S10ShotGun, UMG45SMG, AWMSR, MK1941LMG, TP249LMG, C98Pistol, GrenadeLauncher, MK1907, NailGun,
                                                                           Revolver, LS416, AirGun, H1894Rifle, AutoPistol, MultiAssaultRifle, BattleFlareGun, LaserPistol, LaserCannon, LaserAssaultRifle,
                                                                           HP9ShotGun, TP2Revolver, TP28AssaultPistol, AntiTankSR, TheHunter, TP2BR, TP2MR, HalfAutoRifle, HighPressureNailGun, NightShadowSMG,
                                                                           TP96LMG, QZ95AS, OneHandGrenadeLauncher, StabilizedSMG, HunterPistol, AH12ShotGunn, Chainsaw, Circularsaw};

        string LongRangeWeaponJson = JsonMapper.ToJson(AllRange);
        string PathOfRange = System.IO.Path.Combine(Application.dataPath, "Resource", "Json", "LongRangeWeapon.json");
        
        File.WriteAllText(PathOfRange, LongRangeWeaponJson);

    }

    public void CreateAmmo()
    {
        Ammo NormalPistolAmmo = new Ammo(
            "_NormalPistolAmmo",
            TheType.Ammo,
            TheHandleStyle.Normal,
            Weapon.BulletType.BackForce,
            "_NormalPistolAmmo_Des",
            20,
            10,
            10,
            false,
            System.IO.Path.Combine(MainPath_Magazine_Sprite, "PistolMagazine.png"),
            System.IO.Path.Combine(MainPath_Magazine, "NormalPistolMagazine 1.prefab")
            );

        Ammo SubmachineGunAmmo = new Ammo(
            uniqueCode: "_SubmachineGunAmmo",
            type: TheType.Ammo,
            style: TheHandleStyle.Normal,
            BulletEffect: Weapon.BulletType.BackForce,
            Des: "_SubmachineGunAmmo_Des",
            damage: 10,
            MaxMag: 25,
            RemainMag: 25,
            stackable: false,
            spritePath: System.IO.Path.Combine(MainPath_Magazine_Sprite, "SubMachineMagazine.png"),
            prefabPath: System.IO.Path.Combine(MainPath_Magazine, "SubmachineMagazine.prefab")

            );

        Ammo AS416Ammo = new Ammo(
            uniqueCode: "_AS416Ammo",
            type: TheType.Ammo,
            style: TheHandleStyle.Normal,
            BulletEffect: Weapon.BulletType.BackForce,
            Des: "_AS416Ammo_Des",
            damage: 12,
            MaxMag: 30,
            RemainMag: 30,
            stackable: false,
            spritePath: System.IO.Path.Combine(MainPath_Magazine_Sprite, "AssaultRifleMagazine.png"),
            prefabPath: System.IO.Path.Combine(MainPath_Magazine, "AS416_Magazine.prefab")

            ) ;

        Ammo NormalShotGunAmmo = new Ammo(
            uniqueCode: "_NormalShotGunAmmo",
            type: TheType.Ammo,
            style: TheHandleStyle.Normal,
            BulletEffect: Weapon.BulletType.BackForce,
            Des: "_NormalShotGunAmmo_Des",
            damage: 8,
            MaxMag: 0,
            RemainMag: 0,
            stackable: true,
            maxStack: 8,
            spritePath: System.IO.Path.Combine(MainPath_Magazine_Sprite, "NormalShotGunAmmo.png"),
            prefabPath: System.IO.Path.Combine(MainPath_Magazine, "NormalShotGunAmmo.prefab")
            );

        Ammo ShotGunAmmo = new Ammo(
            uniqueCode: "_ShotGunAmmo",
            type: TheType.Ammo,
            style: TheHandleStyle.Normal,
            BulletEffect: Weapon.BulletType.BackForce,
            Des: "_ShotGunAmmo_Des",
            damage: 7,
            MaxMag: 6,
            RemainMag: 6,
            stackable: false,
            spritePath: System.IO.Path.Combine(MainPath_Magazine_Sprite, "ShotGunMagazine.png"),
            prefabPath: System.IO.Path.Combine(MainPath_Magazine, "ShotGunMagazine.prefab")

            );

        Ammo HeavySMGAmmo = new Ammo(
            uniqueCode: "_HeavySMGAmmo",
            type: TheType.Ammo,
            style: TheHandleStyle.Normal,
            BulletEffect: Weapon.BulletType.BackForce,
            Des: "_HeavySMGAmmo_Des",
            damage: 11,
            MaxMag: 40,
            RemainMag: 40,
            stackable: false,
            spritePath: System.IO.Path.Combine(MainPath_Magazine_Sprite, "HeavySMGMagazine.png"),
            prefabPath: System.IO.Path.Combine(MainPath_Magazine, "HeavySMGMagazine.prefab")

            );

        Ammo SniperAmmo = new Ammo(
            uniqueCode: "_SniperAmmo",
            type: TheType.Ammo,
            style: TheHandleStyle.Normal,
            BulletEffect: Weapon.BulletType.BackForce,
            Des: "_SniperAmmo_Des",
            damage: 70,
            MaxMag: 7,
            RemainMag: 7,
            stackable: false,
            spritePath: System.IO.Path.Combine(MainPath_Magazine_Sprite, "SniperMagazine.png"),
            prefabPath: System.IO.Path.Combine(MainPath_Magazine, "SniperMagazine.prefab")

            );

        Ammo Type2LMGAmmo = new Ammo(
            uniqueCode: "_Type2LMGAmmo",
            type: TheType.Ammo,
            style: TheHandleStyle.Normal,
            BulletEffect: Weapon.BulletType.BackForce,
            Des: "_Type2LMGAmmo_Des",
            damage: 15,
            MaxMag: 100,
            RemainMag: 100,
            stackable: false,
            spritePath: System.IO.Path.Combine(MainPath_Magazine_Sprite, "Type2LMGMagazine.png"),
            prefabPath: System.IO.Path.Combine(MainPath_Magazine, "Type2LMGMagazine.prefab")

            );

        Ammo LMGAmmo = new Ammo(
            uniqueCode: "_LMGAmmo",
            type: TheType.Ammo,
            style: TheHandleStyle.Normal,
            BulletEffect: Weapon.BulletType.BackForce,
            Des: "_LMGAmmo_Des",
            damage: 15,
            MaxMag: 120,
            RemainMag: 120,
            stackable: false,
            spritePath: System.IO.Path.Combine(MainPath_Magazine_Sprite, "LMGMagazine.png"),
            prefabPath: System.IO.Path.Combine(MainPath_Magazine, "LMGMagazine.prefab")

            );

        Ammo HeavyPistolAmmo = new Ammo(
            uniqueCode: "_HeavyPistolAmmo",
            type: TheType.Ammo,
            style: TheHandleStyle.Normal,
            BulletEffect: Weapon.BulletType.BackForce,
            Des: "_HeavyPistolAmmo_Des",
            damage: 30,
            MaxMag: 8,
            RemainMag: 8,
            stackable: false,
            spritePath: System.IO.Path.Combine(MainPath_Magazine_Sprite, "HeavyPistolMagazine.png"),
            prefabPath: System.IO.Path.Combine(MainPath_Magazine, "HeavyPistolMagazine.prefab")

            );

        Ammo GrenadeLauncherAmmo = new Ammo(
            uniqueCode: "_GrenadeLauncherAmmo",
            type: TheType.Ammo,
            style: TheHandleStyle.Normal,
            BulletEffect: Weapon.BulletType.BackForce,
            Des: "_GrenadeLauncherAmmo_Des",
            damage: 150,
            MaxMag: 0,
            RemainMag: 0,
            stackable: true,
            maxStack: 5,

            spritePath: System.IO.Path.Combine(MainPath_Magazine_Sprite, "GrenadeLauncherAmmo.png"),
            prefabPath: System.IO.Path.Combine(MainPath_Magazine, "GrenadeLauncherAmmo.prefab")
            );

        Ammo MK1907Ammo = new Ammo(
            uniqueCode: "_MK1907Ammo",
            type: TheType.Ammo,
            style: TheHandleStyle.Normal,
            BulletEffect: Weapon.BulletType.BackForce,
            Des: "_MK1907Ammo_Des",
            damage: 20,
            MaxMag: 21,
            RemainMag: 21,
            stackable: false,

            spritePath: System.IO.Path.Combine(MainPath_Magazine_Sprite, "BattleRifleMagazine.png"),
            prefabPath: System.IO.Path.Combine(MainPath_Magazine, "MK1907_Magazine.prefab")
            );

        Ammo Nail = new Ammo(
            uniqueCode: "_Nail",
            type: TheType.Ammo,
            style: TheHandleStyle.Normal,
            BulletEffect: Weapon.BulletType.BackForce,
            Des: "_Nail_Des",
            damage: 11,
            MaxMag: 0,
            RemainMag: 0,
            stackable: true,
            maxStack:16,

            spritePath: System.IO.Path.Combine(MainPath_Magazine_Sprite, "Nail.png"),
            prefabPath: System.IO.Path.Combine(MainPath_Magazine, "Nail.prefab")
            );

        Ammo RevolverAmmo = new Ammo(
            uniqueCode: "_RevolverAmmo",
            type: TheType.Ammo,
            style: TheHandleStyle.Normal,
            BulletEffect: Weapon.BulletType.BackForce,
            Des: "_RevolverAmmo_Des",
            damage: 45,
            MaxMag: 0,
            RemainMag: 0,
            stackable: true,
            maxStack: 12,

            spritePath: System.IO.Path.Combine(MainPath_Magazine_Sprite, "RevolverAmmo.png"),
            prefabPath: System.IO.Path.Combine(MainPath_Magazine, "RevolverAmmo.prefab")
            );

        Ammo LightAssaultRifleAmmo = new Ammo(
            uniqueCode: "_LightAssaultRifleAmmo",
            type: TheType.Ammo,
            style: TheHandleStyle.Normal,
            BulletEffect: Weapon.BulletType.BackForce,
            Des: "_LightAssaultRifleAmmo_Des",
            damage: 10,
            MaxMag: 40,
            RemainMag: 40,
            stackable: false,

            spritePath: System.IO.Path.Combine(MainPath_Magazine_Sprite, "LAssaultRifleMagazine.png"),
            prefabPath: System.IO.Path.Combine(MainPath_Magazine, "LAssaultRifleMagzine.prefab")
            );

        Ammo AirGunAmmo = new Ammo(
            uniqueCode: "_AirGunAmmo",
            type: TheType.Ammo,
            style: TheHandleStyle.Normal,
            BulletEffect: Weapon.BulletType.BackForce,
            Des: "_AirGunAmmo_Des",
            damage: 7,
            MaxMag: 25,
            RemainMag: 25,
            stackable: false,

            spritePath: System.IO.Path.Combine(MainPath_Magazine_Sprite, "AirGunMagazine.png"),
            prefabPath: System.IO.Path.Combine(MainPath_Magazine, "AirGunMagazine.prefab")
            );

        Ammo HighVelocityRifleAmmo = new Ammo(
            uniqueCode: "_HighVelocityRifleAmmo",
            type: TheType.Ammo,
            style: TheHandleStyle.Normal,
            BulletEffect: Weapon.BulletType.BackForce,
            Des: "_HighVelocityRifleAmmo_Des",
            damage: 34f,
            MaxMag: 0,
            RemainMag: 0,
            stackable: true,
            maxStack: 10,

            spritePath: System.IO.Path.Combine(MainPath_Magazine_Sprite, "LeverActionRifleAmmo.png"),
            prefabPath: System.IO.Path.Combine(MainPath_Magazine, "HighVelocityRifleAmmo.prefab")
            );

        Ammo ExtendedPistolMagazine = new Ammo(
            uniqueCode: "_ExtendedPistolMagazine",
            type: TheType.Ammo,
            style: TheHandleStyle.Normal,
            BulletEffect: Weapon.BulletType.BackForce,
            Des: "_ExtendedPistolMagazine_Des",
            damage: 15f,
            MaxMag: 26,
            RemainMag: 26,
            stackable: false,

            spritePath: System.IO.Path.Combine(MainPath_Magazine_Sprite, "AutoPistolMagazine.png"),
            prefabPath: System.IO.Path.Combine(MainPath_Magazine, "ExtendedPistolMagazine.prefab")
            );

        Ammo MultiAssaultRifleAmmo = new Ammo(
            uniqueCode: "_MultiAssaultRifleAmmo",
            type: TheType.Ammo,
            style: TheHandleStyle.Normal,
            BulletEffect: Weapon.BulletType.BackForce,
            Des: "_MultiAssaultRifleAmmo_Des",
            damage: 7f,
            MaxMag: 30,
            RemainMag: 30,
            stackable: false,

            spritePath: System.IO.Path.Combine(MainPath_Magazine_Sprite, "MultiAssaultRifleMagazine.png"),
            prefabPath: System.IO.Path.Combine(MainPath_Magazine, "MultiAssaultRifleMagazine.prefab")
            );

        Ammo SignalFlare = new Ammo(
            uniqueCode: "_SignalFlare",
            type: TheType.Ammo,
            style: TheHandleStyle.Normal,
            BulletEffect: Weapon.BulletType.BackForce,
            Des: "_SignalFlare_Des",
            damage: 90f,
            MaxMag: 0,
            RemainMag: 0,
            stackable: true,
            maxStack: 2,

            spritePath: System.IO.Path.Combine(MainPath_Magazine_Sprite, "FlareGunAmmo.png"),
            prefabPath: System.IO.Path.Combine(MainPath_Magazine, "SignalFlare.prefab")
            );

        Ammo LaserPistolAmmo = new Ammo(
            uniqueCode: "_LaserPistolAmmo",
            type: TheType.Ammo,
            style: TheHandleStyle.Normal,
            BulletEffect: Weapon.BulletType.BackForce,
            Des: "_LaserPistolAmmo_Des",
            damage: 60f,
            MaxMag: 10,
            RemainMag: 10,
            stackable: false,

            spritePath: System.IO.Path.Combine(MainPath_Magazine_Sprite, "LaserPistolMagazine.png"),
            prefabPath: System.IO.Path.Combine(MainPath_Magazine, "LaserPistolMagazine.prefab")
            );

        Ammo LaserCannoAmmo = new Ammo(
            uniqueCode: "_LaserCannonAmmo",
            type: TheType.Ammo,
            style: TheHandleStyle.Normal,
            BulletEffect: Weapon.BulletType.Explosion,
            Des: "_LaserCannonAmmo_Des",
            damage: 80f,
            MaxMag: 10,
            RemainMag: 10,
            stackable: false,

            spritePath: System.IO.Path.Combine(MainPath_Magazine_Sprite, "LaserCannonMagazine.png"),
            prefabPath: System.IO.Path.Combine(MainPath_Magazine, "LaserCannonMagazine.prefab")
            );

        Ammo HalfAutoAmmo = new Ammo(
            uniqueCode: "_HalfAutoAmmo",
            type: TheType.Ammo,
            style: TheHandleStyle.Normal,
            BulletEffect: Weapon.BulletType.Explosion,
            Des: "_HalfAutoAmmo_Des",
            damage: 60f,
            MaxMag: 10,
            RemainMag: 10,
            stackable: false,

            spritePath: System.IO.Path.Combine(MainPath_Magazine_Sprite, "HalfAutoMagazine.png"),
            prefabPath: System.IO.Path.Combine(MainPath_Magazine, "HalfAutoMagazine.prefab")
            );

        Ammo BigFireJar = new Ammo(
            uniqueCode: "_BigFireJar",
            type: TheType.Ammo,
            style: TheHandleStyle.Normal,
            BulletEffect: Weapon.BulletType.None,
            Des: "_BigFireJar_Des",
            damage: 1f,
            MaxMag: 800,
            RemainMag: 800,
            stackable: false,

            spritePath: System.IO.Path.Combine(MainPath_Magazine_Sprite, "FireJar.png"),
            prefabPath: System.IO.Path.Combine(MainPath_Magazine, "FireJar.prefab")
            );

        AllAmmos.AllAmmo = new Ammo[] { NormalPistolAmmo, AS416Ammo, NormalShotGunAmmo, SubmachineGunAmmo, ShotGunAmmo, HeavySMGAmmo, SniperAmmo, Type2LMGAmmo, LMGAmmo, HeavyPistolAmmo,
                                                GrenadeLauncherAmmo,MK1907Ammo, Nail,RevolverAmmo, LightAssaultRifleAmmo, AirGunAmmo, HighVelocityRifleAmmo, ExtendedPistolMagazine, MultiAssaultRifleAmmo,
                                                SignalFlare, LaserPistolAmmo, LaserCannoAmmo, HalfAutoAmmo, BigFireJar};


        string AmmoJson = JsonMapper.ToJson(AllAmmos);
        string PathOfAmmo = System.IO.Path.Combine(@"D:\u3dpro", "Demo2024", "Assets", "Resource", "Json", "Ammo.json");

        File.WriteAllText(PathOfAmmo, AmmoJson);
    }

    public void CreateEquipment()
    {

    }

    public void CreateConsumable()
    {

    }



}
