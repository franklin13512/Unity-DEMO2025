using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Rendering;
using UnityEngine.U2D.Animation;

public class InventoryManager : MonoBehaviour
{
    [Serializable]
    public class ThingsInSlot//count为数量，theobj为物品
    {
        public int Count;
        public CreateNewJson.BaseItem TheBaseItem;

        public GameObject TheObj;

        public ThingsInSlot(int count, GameObject theObj)
        {
            Count = count;
            TheObj = theObj;
        }

        public ThingsInSlot(int count, CreateNewJson.BaseItem theBaseItem)
        {
            Count = count;
            TheBaseItem = theBaseItem;
        }

        public ThingsInSlot()
        {
        }
    }

    [Serializable]
    public class TheSlot
    {
        public bool PreLoadPrefab;//预加载物品
        public ThingsInSlot TheThingsInSlot;
    }

    [Serializable]
    public class ClothStorage
    {
        public GameObject TheCloth;
        public string UniqueCode;
        public bool HasEquipped;

        public ClothStorage()
        {
        }
    }

    [Serializable]
    public class ClothStorageClass<TClass>
    {
        public TClass TheCloth;
        public CreateClothXML.ClothClass TheClothClass;
        public ClothStorageClass()
        {

        }

        public ClothStorageClass(TClass theCloth, CreateClothXML.ClothClass theClothClass)
        {
            TheCloth = theCloth;
            TheClothClass = theClothClass;
        }
    }


    [Serializable]
    public class OnePartCloth : ClothStorage
    {
        public SpriteRenderer[] Sprites;
    }

    [Serializable]
    public class UpperCloth : ClothStorage
    {
        public SpriteRenderer[] UpperArm_Sprites;
        public SpriteRenderer[] LowerArm_Sprites;
        public SpriteRenderer[] Body_Sprites;
    }

    [Serializable]
    public class    LowerCloth : ClothStorage
    {
        public SpriteRenderer[] Hip_Sprites;
        public SpriteRenderer[] UpperLeg_Sprites;
        public SpriteRenderer[] LowerLeg_Sprites;
    }

    [Serializable]
    public class ArmorCloth : ClothStorage
    {
        public SpriteRenderer[] Sprites;
    }

    [Serializable]
    public class TheSocks : ClothStorage
    {
        public SpriteRenderer[] UpperLegStocing_Sprite;
        public SpriteRenderer[] LowerLegStocing_Sprite;
    }


    [Serializable]
    public class TheStocking : ClothStorage
    {
        public SpriteRenderer[] HipStocking_Sprite;
        public SpriteRenderer[] UpperLegStocking_Sprite;
        public SpriteRenderer[] LowerLegStocking_Sprite;
    }

    [Serializable]
    public class Skirt : ClothStorage
    {
    }

    [Serializable]
    public class TheAccecssory : ClothStorage
    {
    }

    public List<GameObject> AllInevntoryObj;
    public List<ThingsInSlot> AllInevntoryObj_Container;

    public List<TheSlot> AllInventory_BaseItem;
    public int MaxCapacity;
    public int Capacity;


    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    public bool IsContainer;
    ///////////////////////////////////不是容器（是生物）

    //public Tuple<int, GameObject> DicOfPocket = new SerializedDictionary<int, GameObject>();//每个格子的物品(int为数量，下面同理)
    [Space]
    public SerializedDictionary<int, ThingsInSlot> DicOfAllPocket = new SerializedDictionary<int, ThingsInSlot>();//口袋的每个格子
    public SerializedDictionary<string, SerializedDictionary<int, ThingsInSlot>> DicOfAllBagPanel = new SerializedDictionary<string, SerializedDictionary<int, ThingsInSlot>>();//不同容器（背包，胸挂等）
    public SerializedDictionary<int, ThingsInSlot> DicOfAllBag = new SerializedDictionary<int, ThingsInSlot>();//每个容器
    [Space]
    public SerializedDictionary<int, GameObject> DicOfCloth = new SerializedDictionary<int, GameObject>();//身上的装备（衣服装甲背包等）

    ///////////////////////////////////是容器
    [Space]
    [Space]
    public SerializedDictionary<int, SerializedDictionary<int, GameObject>> DicOfAllContainer = new SerializedDictionary<int, SerializedDictionary<int, GameObject>>();//容器内格子
    public SerializedDictionary<int, ThingsInSlot> DicOfContainer = new SerializedDictionary<int, ThingsInSlot>();//容器内物品
    [Space]
    [Space]
    //public ThingsInSlot[] ItemsInContainer;

    [Space]
    [Space]
    [Space]
    [SerializeField] public ClothStorageClass<OnePartCloth> Helmet;
    [SerializeField] public ClothStorageClass<UpperCloth> Upper;
    [SerializeField] public ClothStorageClass<LowerCloth> Lower;
    [SerializeField] public ClothStorageClass<Skirt> Hip;
    [SerializeField] public ClothStorageClass<ArmorCloth> Armor;
    [SerializeField] public ClothStorageClass<TheStocking> Stocking;
    [SerializeField] public ClothStorageClass<TheSocks> Socks;
    [SerializeField] public ClothStorageClass<OnePartCloth> Shoes;
    [SerializeField] public ClothStorageClass<TheAccecssory> Accessory;
    [SerializeField] public ClothStorageClass<OnePartCloth> BagPack;

    public Player ThePlayer;
    public SpriteResolver TheSpriteResolver;

    void Start()
    {
        ThePlayer = gameObject.GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        {
            EquipHelmet();
            EquipUpper();
            EquipLower();
            EquipSkirt();
            EquipStocking();
            EquipSocks();
            EquipArmor();
            EquipShoes();
        }
    }

    public void MoniterTheList()
    {
        for (int i = 0; i < AllInventory_BaseItem.Count; i++)
        {
            TheSlot CurrentSlot = AllInventory_BaseItem[i];

            if (CurrentSlot.TheThingsInSlot.Count <= 0 && CurrentSlot.TheThingsInSlot.TheObj != null)
            {
                int i2 = i;
                Destroy(CurrentSlot.TheThingsInSlot.TheObj);

                i = i2;
            }
            else if (CurrentSlot.TheThingsInSlot.Count > 0 && (CurrentSlot.PreLoadPrefab || CurrentSlot.TheThingsInSlot.TheBaseItem.Type == CreateNewJson.TheType.Cloth) && CurrentSlot.TheThingsInSlot.TheObj == null)
            {
                //Debug.Log(i);
                int i2 = i;

                Addressables.LoadAssetAsync<GameObject>(CurrentSlot.TheThingsInSlot.TheBaseItem.ThePrefab_Path.Replace("\\", "/")).Completed += handle => {

                    CurrentSlot.TheThingsInSlot.TheObj = Instantiate(handle.Result);
                    CurrentSlot.TheThingsInSlot.TheObj.SetActive(false);
                };

                //Debug.Log(i);

                i = i2;
            }
        }
    }

    public void RemoveItem()
    {

    }

    public void EquipHelmet()
    {
        if (Helmet.TheCloth.TheCloth != null && Helmet.TheCloth.HasEquipped == false)
        {
            ClothItem TheClothItem = Helmet.TheCloth.TheCloth.gameObject.GetComponent<ClothItem>();

            for (int i = 0; i < Helmet.TheCloth.Sprites.Length; i++)
            {
                Helmet.TheCloth.Sprites[i].sprite = TheClothItem.HelmetSprite;
            }

            if (TheClothItem.HideHair == true)
            {
                ThePlayer.TheHair.gameObject.SetActive(false);
            }

            Helmet.TheCloth.HasEquipped = true;
        }
    }

    public void EquipUpper()
    {
        if (Upper.TheCloth.TheCloth != null && Upper.TheCloth.HasEquipped == false)
        {

            ClothItem TheClothItem = Upper.TheCloth.TheCloth.gameObject.GetComponent<ClothItem>();

            for (int i = 0; i < Upper.TheCloth.UpperArm_Sprites.Length; i++)
            {
                Upper.TheCloth.UpperArm_Sprites[i].sprite = TheClothItem.UpperArmSprite;
            }
            for (int i = 0; i < Upper.TheCloth.LowerArm_Sprites.Length; i++)
            {
                Upper.TheCloth.LowerArm_Sprites[i].sprite = TheClothItem.LowerArmSprite;
            }
            for (int i = 0; i < Upper.TheCloth.Body_Sprites.Length; i++)
            {
               Upper.TheCloth.Body_Sprites[i].sprite = TheClothItem.UpperBodySprite_Female;
            }

            Upper.TheCloth.HasEquipped = true;
        }
    }

    public void EquipLower()
    {

        if (Lower.TheCloth.TheCloth != null && Lower.TheCloth.HasEquipped == false)
        {

            ClothItem TheClothItem = Lower.TheCloth.TheCloth.gameObject.GetComponent<ClothItem>();

            for (int i = 0; i < Lower.TheCloth.Hip_Sprites.Length; i++)
            {
                Lower.TheCloth.Hip_Sprites[i].sprite = TheClothItem.HipSprite;
            }
            for (int i = 0; i < Lower.TheCloth.UpperLeg_Sprites.Length; i++)
            {
                Lower.TheCloth.UpperLeg_Sprites[i].sprite = TheClothItem.LapSprite;
            }
            for (int i = 0; i < Lower.TheCloth.LowerLeg_Sprites.Length; i++)
            {
                Lower.TheCloth.LowerLeg_Sprites[i].sprite = TheClothItem.LegSprite;
            }

            Lower.TheCloth.HasEquipped = true;
        }
    }

    public void EquipSkirt()
    {

        if (Hip.TheCloth.TheCloth != null && Hip.TheCloth.HasEquipped == false)
        {
            ClothItem TheClothItem = Hip.TheCloth.TheCloth.gameObject.GetComponent<ClothItem>();

            TheSpriteResolver.SetCategoryAndLabel("Skirt", TheClothItem.Name);

            Hip.TheCloth.HasEquipped = true;
        }

    }

    public void EquipShoes()
    {
        if (Shoes.TheCloth.TheCloth != null && Shoes.TheCloth.HasEquipped == false)
        {
            ClothItem TheClothItem = Shoes.TheCloth.TheCloth.gameObject.GetComponent<ClothItem>();

            for (int i = 0; i < Shoes.TheCloth.Sprites.Length; i++)
            {
                Shoes.TheCloth.Sprites[i].sprite = TheClothItem.ShoesSprite;
            }

            Shoes.TheCloth.HasEquipped = true;
        }
    }

    public void EquipStocking()
    {
        if (Stocking.TheCloth.TheCloth != null && Stocking.TheCloth.HasEquipped == false)
        {
            ClothItem TheClothItem = Stocking.TheCloth.TheCloth.gameObject.GetComponent<ClothItem>();

            for (int i = 0; i < Stocking.TheCloth.HipStocking_Sprite.Length; i++)
            {
                Stocking.TheCloth.HipStocking_Sprite[i].sprite = TheClothItem.HipStockingSprite;
            }
            for (int i = 0; i < Stocking.TheCloth.UpperLegStocking_Sprite.Length; i++)
            {
                Stocking.TheCloth.UpperLegStocking_Sprite[i].sprite = TheClothItem.LapStockingSprite;
            }
            for (int i = 0; i < Stocking.TheCloth.LowerLegStocking_Sprite.Length; i++)
            {
                Stocking.TheCloth.LowerLegStocking_Sprite[i].sprite = TheClothItem.LegStockingSprite;
            }

            Stocking.TheCloth.HasEquipped = true;
        }
    }

    public void EquipSocks()
    {
        if (Socks.TheCloth.TheCloth != null && Socks.TheCloth.HasEquipped == false)
        {
            ClothItem TheClothItem = Socks.TheCloth.TheCloth.gameObject.GetComponent<ClothItem>();

            for (int i = 0; i < Socks.TheCloth.UpperLegStocing_Sprite.Length; i++)
            {
                Socks.TheCloth.UpperLegStocing_Sprite[i].sprite = TheClothItem.LapStockingSprite;
            }
            for (int i = 0; i < Socks.TheCloth.LowerLegStocing_Sprite.Length; i++)
            {
                Socks.TheCloth.LowerLegStocing_Sprite[i].sprite = TheClothItem.LegStockingSprite;
            }

            Socks.TheCloth.HasEquipped = true;
        }
    }

    public void EquipArmor()
    {
        if (Armor.TheCloth.TheCloth != null && Armor.TheCloth.HasEquipped == false)
        {
            ClothItem TheClothItem = Armor.TheCloth.TheCloth.gameObject.GetComponent<ClothItem>();

            for (int i = 0; i < Armor.TheCloth.Sprites.Length; i++)
            {
                Armor.TheCloth.Sprites[i].sprite = TheClothItem.ArmorSprite;
            }

            Armor.TheCloth.HasEquipped = true;
        }
    }
}
