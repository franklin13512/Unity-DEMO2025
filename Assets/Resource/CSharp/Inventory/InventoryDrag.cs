using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static InventoryManager;

public class InventoryDrag : MonoBehaviour
{
    public ItemSlot TheItemSlot;
    //public ArmorSlot TheArmorSlot;
    public MagazineSlot TheMagazineSlot;
    public MainHandSlot TheMainHandSlot;
    public MeleeSlot TheMeleeSlot;
    public OffHandSlot TheOffHandSlot;
    public OffHandEquippedSlot TheOffHandEquippedSlot;
    public Slotsss TheSlotsss;
    public WeaponSlot TheWeaponSlot;

    public ClothSlot TheClothSlot;

    public FatherSlot TheFatherSlot;

    //public ClothUpperSlot TheClothUpperSlot;
    //public ClothLowerSlot TheClothLowerSlot;
    //public HelmetSlot TheHelmetSlot;
    //public ShoesSlot TheShoesSlot;
    //public SocksSlot TheSocksSlot;

    public int TheIndex_Drag;
    //移动物品时需要储存的物品数据
    public float AmmoSliderValue;
    public int RemainAmmo;
    public int MaxAmmo;

    public Image ItemImage;
    public Sprite ItemSprite;
    public string UniqueCode;
    public bool IsFull;

    public CreateNewJson.TheType Types;
    public int Id;

    public UnableallPanel Unable;
    public int StackNumber;
    
    public bool StackOpen;

    public PrefabList ThePrefabs;
    public int MaxStack;

    public bool IsDraging;

    public GameObject TheDragSprite;

    public Sprite TheEmptySprite;
    public bool SliderOpen;

    public Canvas TheCanvas;

    public Player ThePlayer;

    GameObject NewObj;

    private bool IsDroping;

    public InventoryManager TheInventoryManager_ForItemSlot;

    [Space]
    ////////////////////////////////////////当类型为头盔
    public Sprite HelmetSprite;
    ////////////////////////////////////////当类型为上身服装
    public Sprite UpperBodySprite_Male;
    public Sprite UpperBodySprite_Female;
    public Sprite UpperArmSprite;
    public Sprite LowerArmSprite;
    ////////////////////////////////////////当类型为下身服装
    public Sprite HipSprite;
    public Sprite LapSprite;
    public Sprite LegSprite;
    ////////////////////////////////////////当类型为袜子
    public Sprite SocksSprite;
    ////////////////////////////////////////当类型为裤袜
    public Sprite HipStockingSprite;
    public Sprite LapStockingSprite;
    public Sprite LegStockingSprite;
    ////////////////////////////////////////当类型为鞋子
    public Sprite ShoesSprite;
    ////////////////////////////////////////当类型为护甲
    public Sprite ArmorSprite;
    ////////////////////////////////////////当类型为背包
    public Sprite BackPackSprite;

    public CreateClothXML.ClothType ClothType;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(1))
        {
            ClearTheDragPanel();
            ClearTheSlot();
        }

        if (IsDraging == true)
        {
            Vector2 TheMousePos1 = Input.mousePosition;
            Vector2 TheMousePos2;

            RectTransformUtility.ScreenPointToLocalPointInRectangle(TheCanvas.transform as RectTransform, TheMousePos1, null, out TheMousePos2);

            Image TheImage = TheDragSprite.GetComponent<Image>();
            TheImage.sprite = this.ItemImage.sprite;

            TheDragSprite.gameObject.SetActive(true);
            TheDragSprite.transform.localPosition = TheMousePos2;
        }

        if (TheItemSlot != null && TheMagazineSlot != null)
        {
            ClearTheDragPanel();
            TheMagazineSlot = null;
            TheItemSlot = null;
        }

        if (Input.GetMouseButton(1) || Input.mouseScrollDelta.y != 0)
        {
            ClearTheDragPanel();
            ClearTheSlot();
        }


    }

    public void ClearTheDragPanel()
    {

            IsDraging = false;

            AmmoSliderValue = default(int);
            RemainAmmo = 0;
            MaxAmmo = 0;

            Types = CreateNewJson.TheType.None;
            Id = default(int);
            StackNumber = 0;
            StackOpen = false;

            IsFull = false;
            MaxStack = default(int);
            TheInventoryManager_ForItemSlot = null;
            //ItemImage.sprite = TheEmptySprite;

        Image TheImage = TheDragSprite.GetComponent<Image>();
        if (TheImage && ItemImage)
        {
            TheImage.sprite = this.ItemImage.sprite;

            TheDragSprite.gameObject.SetActive(false);
        }


            //TheItemSlot = null;
            //TheMagazineSlot = null;
    }

    public void ClearTheSlot()
    {
        TheItemSlot = null;
        TheMagazineSlot = null;
        TheClothSlot = null;
        //TheClothUpperSlot = null;
        //TheClothLowerSlot = null;
        //TheHelmetSlot = null;
        //TheShoesSlot = null;
        //TheArmorSlot = null;

    }

    public void ClearItemSlot()
    {
        TheItemSlot.AmmoSlider.value = default(int);
        TheItemSlot.AmmoSlider.gameObject.SetActive(false);
        TheItemSlot.RemainAmmo = 0;
        TheItemSlot.MaxAmmo = 0;

        TheItemSlot.RecordedMaxAmmo = 0;
        TheItemSlot.RecordedRemainAmmo = 0;
        TheItemSlot.RecordedUniqueCode = string.Empty;

        TheItemSlot.Types = CreateNewJson.TheType.None;
        TheItemSlot.Id = default(int);
        TheItemSlot.UniqueCode = "";
        TheItemSlot.StackNumber = 0;
        TheItemSlot.StackOpen = false;
        TheItemSlot.TheText.gameObject.SetActive(false);

        TheItemSlot.IsFull = false;
        TheItemSlot.MaxStack = default(int);
        TheItemSlot.ItemImage.sprite = TheEmptySprite;

        RemoveItemFromHand();

        //TheItemSlot = null;
        //TheMagazineSlot = null;
    }


    public void ClearMagazineSlot()
    {
        TheMagazineSlot.AmmoSlider.value = default(int);
        TheMagazineSlot.AmmoSlider.gameObject.SetActive(false);
        TheMagazineSlot.RemainAmmo = 0;
        TheMagazineSlot.MaxAmmo = 0;

        TheMagazineSlot.Types = CreateNewJson.TheType.None;
        TheMagazineSlot.Id = default(int);
        TheMagazineSlot.UniqueCode = default(string);
        TheMagazineSlot.StackNumber = 0;
        TheMagazineSlot.StackOpen = false;
        TheMagazineSlot.TheText.gameObject.SetActive(false);
       
        TheMagazineSlot.IsFull = false;
        TheMagazineSlot.MaxStack = default(int);
        TheMagazineSlot.ItemImage.sprite = TheEmptySprite;

        //TheItemSlot = null;
        //TheMagazineSlot = null;
    }

    public void ClearClothSprites()
    {

        TheClothSlot.ClothPreviewSprite.sprite = TheEmptySprite;
        TheClothSlot.TheDefaultSprite.gameObject.SetActive(true);
        TheClothSlot.TheClothType = CreateClothXML.ClothType.None;
        TheClothSlot.IsFull = false;

        ClothType = CreateClothXML.ClothType.None;
    }

    public IEnumerator DropDownItem()
    {

        if(TheFatherSlot != null)
        {
            ItemSlot TheItemSlot = TheFatherSlot as ItemSlot;
            if (TheItemSlot != null)
            {
                if(TheItemSlot.TheSlotType == CreateNewJson.TheType_InSlot.None)
                {
                    for (int i = 0; i < TheItemSlot.TheInventoryManager.AllInventory_BaseItem[TheItemSlot.TheIndex].TheThingsInSlot.Count; i++)
                    {

                        int i2 = i;

                        InventoryManager.ThingsInSlot TheThingsInSlot = TheItemSlot.TheInventoryManager.AllInventory_BaseItem[TheItemSlot.TheIndex].TheThingsInSlot;

                        GameObject TheObj = new GameObject();

                        Addressables.LoadAssetAsync<GameObject>(TheThingsInSlot.TheBaseItem.ThePrefab_Path.Replace("\\", "/")).Completed += handle =>
                        {

                            TheObj = Instantiate(handle.Result);
                            TheObj.transform.localPosition = Vector3.zero;
                            TheObj.transform.localRotation = Quaternion.identity;
                            TheObj.transform.position = ThePlayer.BodyBox.bounds.center;

                        };

                        while (TheObj.GetComponents<Component>().Length == 1)
                        {
                            yield return null; // 等待下一帧
                        }

                        Item TheItem = TheObj.GetComponent<Item>();
                        if (TheItem != null)
                        {
                            TheItem.TheBasicData = TheThingsInSlot.TheBaseItem;

                        }

                        if (TheItemSlot.TheInventoryManager.AllInventory_BaseItem[TheItemSlot.TheIndex].TheThingsInSlot.TheObj != null)
                            Destroy(TheItemSlot.TheInventoryManager.AllInventory_BaseItem[TheItemSlot.TheIndex].TheThingsInSlot.TheObj);

                        TheItemSlot.TheText.gameObject.SetActive(false);
                        TheItemSlot.AmmoSlider.gameObject.SetActive(false);

                        TheItemSlot.TheInventoryManager.AllInventory_BaseItem[TheItemSlot.TheIndex].TheThingsInSlot.Count = 0;
                        TheItemSlot.TheInventoryManager.AllInventory_BaseItem[TheItemSlot.TheIndex].TheThingsInSlot.TheBaseItem = new CreateNewJson.BaseItem();


                        TheItemSlot.ItemImage.sprite = TheEmptySprite;
                        TheFatherSlot = null;


                        //Debug.Log(TheItem.TheBasicData.InventoryObjs[0].TheItemData.UniqueCode);

                        i = i2;
                    }
                }
                else if (TheItemSlot.TheSlotType == CreateNewJson.TheType_InSlot.ForMagazine)
                {
                    for (int i = 0; i < TheItemSlot.TheInventoryManager.AllInventory_BaseItem[Unable.ItemSlotSelectedIndex].TheThingsInSlot.TheBaseItem.InventoryObjs[TheItemSlot.TheIndex].TheCount; i++)
                    {

                        int i2 = i;

                        InventoryManager.ThingsInSlot TheThingsInSlot = TheItemSlot.TheInventoryManager.AllInventory_BaseItem[Unable.ItemSlotSelectedIndex].TheThingsInSlot;

                        GameObject TheObj = new GameObject();

                        Addressables.LoadAssetAsync<GameObject>(TheThingsInSlot.TheBaseItem.InventoryObjs[TheItemSlot.TheIndex].TheItemData.ThePrefab_Path.Replace("\\", "/")).Completed += handle =>
                        {

                            TheObj = Instantiate(handle.Result);
                            TheObj.transform.localPosition = Vector3.zero;
                            TheObj.transform.localRotation = Quaternion.identity;
                            TheObj.transform.position = ThePlayer.BodyBox.bounds.center;

                        };

                        while (TheObj.GetComponents<Component>().Length == 1)
                        {
                            yield return null; // 等待下一帧
                        }

                        Item TheItem = TheObj.GetComponent<Item>();
                        if (TheItem != null)
                        {
                            TheItem.TheBasicData = TheThingsInSlot.TheBaseItem.InventoryObjs[TheItemSlot.TheIndex].TheItemData;

                        }

                        TheItemSlot.TheText.gameObject.SetActive(false);
                        TheItemSlot.AmmoSlider.gameObject.SetActive(false);

                        TheItemSlot.TheInventoryManager.AllInventory_BaseItem[Unable.ItemSlotSelectedIndex].TheThingsInSlot.TheBaseItem.InventoryObjs[TheItemSlot.TheIndex].TheCount = 0;
                        TheItemSlot.TheInventoryManager.AllInventory_BaseItem[Unable.ItemSlotSelectedIndex].TheThingsInSlot.TheBaseItem.InventoryObjs[TheItemSlot.TheIndex].TheItemData = new CreateNewJson.BaseItem();


                        TheItemSlot.ItemImage.sprite = TheEmptySprite;
                        TheFatherSlot = null;


                        //Debug.Log(TheItem.TheBasicData.InventoryObjs[0].TheItemData.UniqueCode);

                        i = i2;
                    }
                }
                else if (TheItemSlot.TheSlotType == CreateNewJson.TheType_InSlot.ForCloth)
                {
                    CreateClothXML.ClothClass TheClothClass = new CreateClothXML.ClothClass();

                    if (TheItemSlot.ClothType == CreateClothXML.ClothType.UpperCloth)
                    {
                        Destroy(TheItemSlot.TheInventoryManager.Upper.TheCloth.TheCloth);
                        TheItemSlot.TheInventoryManager.Upper.TheCloth.TheCloth = null;


                        for (int i = 0; i < TheItemSlot.TheInventoryManager.Upper.TheCloth.UpperArm_Sprites.Length; i++)
                        {
                            TheItemSlot.TheInventoryManager.Upper.TheCloth.UpperArm_Sprites[i].sprite = TheEmptySprite;
                        }

                        for (int i = 0; i < TheItemSlot.TheInventoryManager.Upper.TheCloth.LowerArm_Sprites.Length; i++)
                        {
                            TheItemSlot.TheInventoryManager.Upper.TheCloth.LowerArm_Sprites[i].sprite = TheEmptySprite;
                        }

                        for (int i = 0; i < TheItemSlot.TheInventoryManager.Upper.TheCloth.Body_Sprites.Length; i++)
                        {
                            TheItemSlot.TheInventoryManager.Upper.TheCloth.Body_Sprites[i].sprite = TheEmptySprite;
                        }

                        TheClothClass = TheItemSlot.TheInventoryManager.Upper.TheClothClass;

                        TheItemSlot.ItemImage.sprite = TheEmptySprite;
                        TheItemSlot.ClassicPNG.gameObject.SetActive(true);
                        TheItemSlot.TheInventoryManager.Upper.TheCloth.HasEquipped = false;
                        TheItemSlot.TheInventoryManager.Upper.TheClothClass = null;

                    }
                    else if (TheItemSlot.ClothType == CreateClothXML.ClothType.Helmet)
                    {
                        Destroy(TheItemSlot.TheInventoryManager.Helmet.TheCloth.TheCloth);
                        TheItemSlot.TheInventoryManager.Helmet.TheCloth.TheCloth = null;

                        for (int i = 0; i < TheItemSlot.TheInventoryManager.Helmet.TheCloth.Sprites.Length; i++)
                        {
                            TheItemSlot.TheInventoryManager.Helmet.TheCloth.Sprites[i].sprite = TheEmptySprite;
                        }

                        TheClothClass = TheItemSlot.TheInventoryManager.Helmet.TheClothClass;

                        TheItemSlot.ItemImage.sprite = TheEmptySprite;
                        TheItemSlot.ClassicPNG.gameObject.SetActive(true);
                        TheItemSlot.TheInventoryManager.Helmet.TheCloth.HasEquipped = false;
                        TheItemSlot.TheInventoryManager.Helmet.TheClothClass = null;

                        TheItemSlot.TheInventoryManager.ThePlayer.TheHair.gameObject.SetActive(true);

                    }
                    else if (TheItemSlot.ClothType == CreateClothXML.ClothType.LowerCloth && TheItemSlot.TheInventoryManager.Lower.TheCloth.HasEquipped)
                    {
                        Destroy(TheItemSlot.TheInventoryManager.Lower.TheCloth.TheCloth);
                        TheItemSlot.TheInventoryManager.Lower.TheCloth.TheCloth=null;

                        for (int i = 0; i < TheItemSlot.TheInventoryManager.Lower.TheCloth.UpperLeg_Sprites.Length; i++)
                        {
                            TheItemSlot.TheInventoryManager.Lower.TheCloth.UpperLeg_Sprites[i].sprite = TheEmptySprite;
                        }

                        for (int i = 0; i < TheItemSlot.TheInventoryManager.Lower.TheCloth.LowerLeg_Sprites.Length; i++)
                        {
                            TheItemSlot.TheInventoryManager.Lower.TheCloth.LowerLeg_Sprites[i].sprite = TheEmptySprite;
                        }

                        for (int i = 0; i < TheItemSlot.TheInventoryManager.Lower.TheCloth.Hip_Sprites.Length; i++)
                        {
                            TheItemSlot.TheInventoryManager.Lower.TheCloth.Hip_Sprites[i].sprite = TheEmptySprite;
                        }

                        TheClothClass = TheItemSlot.TheInventoryManager.Lower.TheClothClass;

                        TheItemSlot.ItemImage.sprite = TheEmptySprite;
                        TheItemSlot.ClassicPNG.gameObject.SetActive(true);
                        TheItemSlot.TheInventoryManager.Lower.TheCloth.HasEquipped = false;
                        TheItemSlot.TheInventoryManager.Lower.TheClothClass = null;

                    }
                    else if (TheItemSlot.ClothType == CreateClothXML.ClothType.LowerCloth && TheItemSlot.TheInventoryManager.Hip.TheCloth.HasEquipped)
                    {
                        Destroy(TheItemSlot.TheInventoryManager.Hip.TheCloth.TheCloth);
                        TheItemSlot.TheInventoryManager.Hip.TheCloth.TheCloth = null;

                        InventoryManager TheInventoryManager = ThePlayer.gameObject.GetComponent<InventoryManager>();
                        TheInventoryManager.TheSpriteResolver.SetCategoryAndLabel("Skirt", "Nothing");

                        TheClothClass = TheItemSlot.TheInventoryManager.Hip.TheClothClass;

                        TheItemSlot.ItemImage.sprite = TheEmptySprite;
                        TheItemSlot.ClassicPNG.gameObject.SetActive(true);
                        TheItemSlot.TheInventoryManager.Hip.TheCloth.HasEquipped = false;
                        TheItemSlot.TheInventoryManager.Hip.TheClothClass = null;

                    }
                    else if (TheItemSlot.ClothType == CreateClothXML.ClothType.Socks && TheItemSlot.TheInventoryManager.Socks.TheCloth.HasEquipped)
                    {
                        Destroy(TheItemSlot.TheInventoryManager.Socks.TheCloth.TheCloth);
                        TheItemSlot.TheInventoryManager.Socks.TheCloth.TheCloth = null;

                        for (int i = 0; i < TheItemSlot.TheInventoryManager.Socks.TheCloth.UpperLegStocing_Sprite.Length; i++)
                        {
                            TheItemSlot.TheInventoryManager.Socks.TheCloth.UpperLegStocing_Sprite[i].sprite = TheEmptySprite;
                        }

                        for (int i = 0; i < TheItemSlot.TheInventoryManager.Socks.TheCloth.LowerLegStocing_Sprite.Length; i++)
                        {
                            TheItemSlot.TheInventoryManager.Socks.TheCloth.LowerLegStocing_Sprite[i].sprite = TheEmptySprite;
                        }

                        TheClothClass = TheItemSlot.TheInventoryManager.Socks.TheClothClass;

                        TheItemSlot.ItemImage.sprite = TheEmptySprite;
                        TheItemSlot.ClassicPNG.gameObject.SetActive(true);
                        TheItemSlot.TheInventoryManager.Socks.TheCloth.HasEquipped = false;
                        TheItemSlot.TheInventoryManager.Socks.TheClothClass = null;

                    }
                    else if (TheItemSlot.ClothType == CreateClothXML.ClothType.Socks && TheItemSlot.TheInventoryManager.Stocking.TheCloth.HasEquipped)
                    {
                        Destroy(TheItemSlot.TheInventoryManager.Stocking.TheCloth.TheCloth);
                        TheItemSlot.TheInventoryManager.Stocking.TheCloth.TheCloth = null;

                        for (int i = 0; i < TheItemSlot.TheInventoryManager.Stocking.TheCloth.HipStocking_Sprite.Length; i++)
                        {
                            TheItemSlot.TheInventoryManager.Stocking.TheCloth.HipStocking_Sprite[i].sprite = TheEmptySprite;
                        }

                        for (int i = 0; i < TheItemSlot.TheInventoryManager.Stocking.TheCloth.UpperLegStocking_Sprite.Length; i++)
                        {
                            TheItemSlot.TheInventoryManager.Stocking.TheCloth.UpperLegStocking_Sprite[i].sprite = TheEmptySprite;
                        }

                        for (int i = 0; i < TheItemSlot.TheInventoryManager.Stocking.TheCloth.LowerLegStocking_Sprite.Length; i++)
                        {
                            TheItemSlot.TheInventoryManager.Stocking.TheCloth.LowerLegStocking_Sprite[i].sprite = TheEmptySprite;
                        }

                        TheClothClass = TheItemSlot.TheInventoryManager.Stocking.TheClothClass;

                        TheItemSlot.ItemImage.sprite = TheEmptySprite;
                        TheItemSlot.ClassicPNG.gameObject.SetActive(true);
                        TheItemSlot.TheInventoryManager.Stocking.TheCloth.HasEquipped = false;
                        TheItemSlot.TheInventoryManager.Stocking.TheClothClass = null;

                    }
                    else if (TheItemSlot.ClothType == CreateClothXML.ClothType.Shoes)
                    {
                        Destroy(TheItemSlot.TheInventoryManager.Shoes.TheCloth.TheCloth);
                        TheItemSlot.TheInventoryManager.Shoes.TheCloth.TheCloth = null;

                        for (int i = 0; i < TheItemSlot.TheInventoryManager.Shoes.TheCloth.Sprites.Length; i++)
                        {
                            TheItemSlot.TheInventoryManager.Shoes.TheCloth.Sprites[i].sprite = TheEmptySprite;
                        }

                        TheClothClass = TheItemSlot.TheInventoryManager.Shoes.TheClothClass;

                        TheItemSlot.ItemImage.sprite = TheEmptySprite;
                        TheItemSlot.ClassicPNG.gameObject.SetActive(true);
                        TheItemSlot.TheInventoryManager.Shoes.TheCloth.HasEquipped = false;
                        TheItemSlot.TheInventoryManager.Shoes.TheClothClass = null;

                    }
                    else if (TheItemSlot.ClothType == CreateClothXML.ClothType.Armor)
                    {
                        Destroy(TheItemSlot.TheInventoryManager.Armor.TheCloth.TheCloth);
                        TheItemSlot.TheInventoryManager.Armor.TheCloth.TheCloth = null;

                        for (int i = 0; i < TheItemSlot.TheInventoryManager.Armor.TheCloth.Sprites.Length; i++)
                        {
                            TheItemSlot.TheInventoryManager.Armor.TheCloth.Sprites[i].sprite = TheEmptySprite;
                        }

                        TheClothClass = TheItemSlot.TheInventoryManager.Armor.TheClothClass;

                        TheItemSlot.ItemImage.sprite = TheEmptySprite;
                        TheItemSlot.ClassicPNG.gameObject.SetActive(true);
                        TheItemSlot.TheInventoryManager.Armor.TheCloth.HasEquipped = false;
                        TheItemSlot.TheInventoryManager.Armor.TheClothClass = null;

                    }

                    Addressables.LoadAssetAsync<GameObject>(TheClothClass.ThePrefab_Path.Replace("\\", "/")).Completed += handle =>
                    {

                        GameObject TheObj = Instantiate(handle.Result);
                        TheObj.transform.localPosition = Vector3.zero;
                        TheObj.transform.localRotation = Quaternion.identity;
                        TheObj.transform.position = ThePlayer.BodyBox.bounds.center;

                    };
                }
            }
        }

        Unable.DropThing();
        StartCoroutine(Unable.EquipItem());
        ClearTheSlot();
        ClearTheDragPanel();
    }

    public void RemoveItemFromHand()
    {

        Outline TheOutline = TheItemSlot.gameObject.GetComponent<Outline>();
        if (TheOutline.enabled == true && TheOutline != null)
        {
            TheSlotsss.UnablePanel.EquipItem();
        }
    }

    IEnumerator WaitForMagazineReset(GameObject NewObj, int Remain, int Max)
    {
        yield return null;
        yield return null;
        Magazine TheMagazine = NewObj.gameObject.GetComponent<Magazine>();
        TheMagazine.RemainAmmo = Remain;
        TheMagazine.MaxAmmo = Max;
    }

    public InventoryManager.ThingsInSlot FindItemInInventory()
    {
        if (String.IsNullOrEmpty(TheItemSlot.BagName))
        {
            InventoryManager TheInventoryManager = ThePlayer.gameObject.GetComponent<InventoryManager>();

            TheInventoryManager.DicOfAllPocket.TryGetValue(TheItemSlot.TheIndex, out InventoryManager.ThingsInSlot TheInform);
            return TheInform;
        }
        else
        {
            InventoryManager TheInventoryManager = ThePlayer.gameObject.GetComponent<InventoryManager>();

            TheInventoryManager.DicOfAllBagPanel.TryGetValue(TheItemSlot.BagName, out UnityEngine.Rendering.SerializedDictionary<int, InventoryManager.ThingsInSlot> TheDic);
            TheDic.TryGetValue(TheItemSlot.TheIndex, out InventoryManager.ThingsInSlot TheInform);
            return TheInform;
        }
    }

    public InventoryManager.ThingsInSlot RemoveItemInInventory()
    {
        if (TheItemSlot == null)
        {
            Debug.Log("出现过错误，原因未知");
            return null;
        }

        if (String.IsNullOrEmpty(TheItemSlot.BagName))
        {
            //InventoryManager TheInventoryManager = ThePlayer.gameObject.GetComponent<InventoryManager>();
            InventoryManager TheInventoryManager = TheItemSlot.TheInventoryManager;

            if(TheInventoryManager_ForItemSlot == TheItemSlot.TheInventoryManager && !IsDroping)//单个容器内部
            {
                if (TheInventoryManager.IsContainer)
                {

                    TheInventoryManager.DicOfContainer.Remove(TheItemSlot.TheIndex, out InventoryManager.ThingsInSlot TheInform);
                    //TheInventoryManager.AllInevntoryObj_Container.Remove(TheInform);

                    return TheInform;
                }
                else
                {
                    TheInventoryManager.DicOfAllPocket.Remove(TheItemSlot.TheIndex, out InventoryManager.ThingsInSlot TheInform);
                    //TheInventoryManager.AllInevntoryObj.Remove(TheInform.TheObj);

                    return TheInform;
                }
            }
            else if (IsDroping || TheInventoryManager_ForItemSlot != TheItemSlot.TheInventoryManager)//跨容器
            {
                if (TheInventoryManager.IsContainer)
                {
                    TheInventoryManager.DicOfContainer.Remove(TheItemSlot.TheIndex, out InventoryManager.ThingsInSlot TheInform);
                    //TheInventoryManager.AllInevntoryObj_Container.Remove(TheInform);

                    return TheInform;
                }
                else
                {
                    TheInventoryManager.DicOfAllPocket.Remove(TheItemSlot.TheIndex, out InventoryManager.ThingsInSlot TheInform);
                    TheInventoryManager.AllInevntoryObj.Remove(TheInform.TheObj);

                    return TheInform;
                }
            }
            else
            {
                return null;
            }

        }
        else
        {
            InventoryManager TheInventoryManager = TheItemSlot.TheInventoryManager;

            TheInventoryManager.DicOfAllBagPanel.TryGetValue(TheItemSlot.BagName, out UnityEngine.Rendering.SerializedDictionary<int, InventoryManager.ThingsInSlot> TheDic);
            TheDic.Remove(TheItemSlot.TheIndex, out InventoryManager.ThingsInSlot TheInform);
            TheInventoryManager.AllInevntoryObj.Remove(TheInform.TheObj);
            return TheInform;
        }


    }

    public InventoryManager.ThingsInSlot FindItemInInventory(int TIndex)
    {
        if (true)
        {
            InventoryManager TheInventoryManager = ThePlayer.gameObject.GetComponent<InventoryManager>();

            TheInventoryManager.DicOfAllPocket.TryGetValue(TIndex, out InventoryManager.ThingsInSlot TheInform);
            return TheInform;
        }
        else
        {
            InventoryManager TheInventoryManager = ThePlayer.gameObject.GetComponent<InventoryManager>();

            TheInventoryManager.DicOfAllBagPanel.TryGetValue(TheItemSlot.BagName, out UnityEngine.Rendering.SerializedDictionary<int, InventoryManager.ThingsInSlot> TheDic);
            TheDic.TryGetValue(TIndex, out InventoryManager.ThingsInSlot TheInform);
            return TheInform;
        }
    }

    public InventoryManager.ThingsInSlot FindItemInInventory(int TIndex, string BagName)
    {
        if (String.IsNullOrEmpty(BagName))
        {
            InventoryManager TheInventoryManager = ThePlayer.gameObject.GetComponent<InventoryManager>();

            TheInventoryManager.DicOfAllPocket.TryGetValue(TIndex, out InventoryManager.ThingsInSlot TheInform);
            return TheInform;
        }
        else
        {
            InventoryManager TheInventoryManager = ThePlayer.gameObject.GetComponent<InventoryManager>();

            TheInventoryManager.DicOfAllBagPanel.TryGetValue(TheItemSlot.BagName, out UnityEngine.Rendering.SerializedDictionary<int, InventoryManager.ThingsInSlot> TheDic);
            TheDic.TryGetValue(TIndex, out InventoryManager.ThingsInSlot TheInform);
            return TheInform;
        }
    }
}
