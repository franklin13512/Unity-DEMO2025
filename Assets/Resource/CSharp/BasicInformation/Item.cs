
using LitJson;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using static CreateNewJson;

public class Item : MonoBehaviour
{
    public enum Style
    {
        Normal,
        MainHandWeapon,
    }

    public int Id;


    public Sprite TheSprite;
    public string UniqueCode;

    public bool IsEquiped;
    public bool Usable;

    public InventoryManager TheInventoryManager;
    public int CurrentIndex;//物品的物品栏的index
    public int CurrentIndex_ItemSlot;//对应装备栏的index

    [HideInInspector] public MagazineSlot TheMagazine;

    //当物品为可叠加的，勾选stackable并设置max stacke
    public bool Stackable;
    public int MaxStack;

    //当该物品为弹匣时，记录其剩余弹药
    [HideInInspector] public int RemainAmmo;
    [HideInInspector] public int MaxAmmo;

    //在武器专门脚本内选择其是否为主武器
    public bool IsMainHand;

    [HideInInspector]
    public CreateNewJson.TheConsumable Consumable;
    [HideInInspector]
    public CreateNewJson.TheAmmo Ammo;
    //[HideInInspector]
    //public CreateClothXML.ClothArray Cloth;
    [HideInInspector]
    public CreateNewJson.TheLongRangeWeapon LongRangeWeapon;
    [HideInInspector]
    public CreateNewJson.TheMeleeWeapon MeleeWeapon;

    public CreateNewJson.TheType AllType;
    [SerializeField]
    public CreateNewJson.BaseItem TheBasicData;

    public virtual void OnValidate()
    {
        GetMeleeWeapon();
        GetLongRangeWeapon();
        GetAmmo();

        if (AllType == TheType.MeleeWeapon)//初始化近战武器数据
        {
            foreach (CreateNewJson.MeleeWeapon TheObj in MeleeWeapon.AllMeleeWeapons)
            {
                if (TheObj.UniqueCode == UniqueCode)
                {
                    TheBasicData = new CreateNewJson.MeleeWeapon(
                        TheObj.UniqueCode,
                        TheObj.Type,
                        TheObj.HandleStyle,
                        TheObj.TheDescription,
                        TheObj.Damage,
                        TheObj.TimeForNextAttack,
                        TheObj.ChanceToBleed,
                        TheObj.Stackable,
                        TheObj.InventoryObjs,

                        TheObj.TheSprite_Path,
                        TheObj.ThePrefab_Path
                        );
                }
            }
        }
        else if (AllType == TheType.LongRangeWeapon)//初始化远程武器数据
        {
            foreach (CreateNewJson.LongRangeWeapon TheObj in LongRangeWeapon.AllLongRangeWeapons)
            {
                if (TheObj.UniqueCode == UniqueCode)
                {
                    TheBasicData = new CreateNewJson.LongRangeWeapon(
                        TheObj.UniqueCode,
                        TheObj.Type,
                        TheObj.HandleStyle,
                        TheObj.MaxAmmoStack,
                        TheObj.TheDescription,
                        TheObj.AmmoCode,
                        TheObj.DamageMuti,
                        TheObj.IsAuto,
                        TheObj.EnableShell,
                        TheObj.EnableBullet,
                        TheObj.IsLaserWeapon,
                        TheObj.BackwardForce,
                        TheObj.ShootSpeed,
                        TheObj.Stackable,
                        TheObj.InventoryObjs,

                        TheObj.TheSprite_Path,
                        TheObj.ThePrefab_Path
                        );
                }
            }
        }
        else if (AllType == TheType.Ammo)
        {
            foreach (CreateNewJson.Ammo TheObj in Ammo.AllAmmo)//初始化弹药数据
            {
                if (TheObj.UniqueCode == UniqueCode)
                {
                    TheBasicData = new CreateNewJson.Ammo(
                        TheObj.UniqueCode,
                        TheObj.Type,
                        TheObj.HandleStyle,
                        TheObj.TheBulletType,
                        TheObj.TheDescription,
                        TheObj.Damage,
                        TheObj.MaxMagazine,
                        TheObj.RemainMagazine,
                        TheObj.Stackable,
                        TheObj.MaxStack,

                        TheObj.TheSprite_Path,
                        TheObj.ThePrefab_Path
                        );
                }
            }
        }

    }

    // Start is called before the first frame update
    void Start()
    {
        OnValidate();
    }

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

    public virtual void UseTheItem()
    {
        return;
    }
}
