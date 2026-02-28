using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.EventSystems;
using UnityEngine.U2D.Animation;
using UnityEngine.UI;

public class ItemSlot : FatherSlot, IPointerClickHandler
{
    public string BagName;

    public Item.Style TheStyle;

    public Slider AmmoSlider;
    public int RemainAmmo;
    public int MaxAmmo;

    //记录临时弹药信息
    public int RecordedRemainAmmo;
    public int RecordedMaxAmmo;
    public string RecordedUniqueCode;

    public Slotsss TheSlots;

    public CreateNewJson.TheType Types;
    public int Id;

    public int StackNumber;
    public Text TheText;

    public bool StackOpen;

    public PrefabList ThePrefabs;
    public int MaxStack;

    public bool StackableForMagazine;
    ///////////////////////////////////////////////////////////

    public Image ItemImage;
    public Sprite ItemSprite;

    public bool IsFull;
    public bool IsPlayer;
    public InventoryDrag TheDrag;
    public InventoryManager TheInventoryManager;
    public CreateNewJson.TheType_InSlot TheSlotType;

    private int LastInt;

    public Image ClassicPNG;
    //[Space]
    //////////////////////////////////////////当类型为头盔
    //public Sprite HelmetSprite;
    //////////////////////////////////////////当类型为上身服装
    //public Sprite UpperBodySprite_Male;
    //public Sprite UpperBodySprite_Female;
    //public Sprite UpperArmSprite;
    //public Sprite LowerArmSprite;
    //////////////////////////////////////////当类型为下身服装
    //public Sprite HipSprite;
    //public Sprite LapSprite;
    //public Sprite LegSprite;
    //////////////////////////////////////////当类型为袜子
    //public Sprite SocksSprite;
    //////////////////////////////////////////当类型为裤袜
    //public Sprite HipStockingSprite;
    //public Sprite LapStockingSprite;
    //public Sprite LegStockingSprite;
    //////////////////////////////////////////当类型为鞋子
    //public Sprite ShoesSprite;
    //////////////////////////////////////////当类型为护甲
    //public Sprite ArmorSprite;
    //////////////////////////////////////////当类型为背包
    //public Sprite BackPackSprite;

    public CreateClothXML.ClothType ClothType;

    private InventoryManager.ThingsInSlot _moniteringTheSlot;
    public InventoryManager.ThingsInSlot MoniteringTheSlot
    {
        get => _moniteringTheSlot;
        set
        {
            if (_moniteringTheSlot != value)
            {
                _moniteringTheSlot = value;

                UpdateSlotData();
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (true)
        {
            if (TheInventoryManager.AllInventory_BaseItem[TheIndex].TheThingsInSlot.Count > 0)
               MoniteringTheSlot = TheInventoryManager.AllInventory_BaseItem[TheIndex].TheThingsInSlot;
        }

        if (TheInventoryManager && TheInventoryManager.AllInventory_BaseItem[TheIndex].TheThingsInSlot.Count > 0 && TheInventoryManager.AllInventory_BaseItem[TheIndex].TheThingsInSlot.TheBaseItem.Stackable == true)
        {
            if (LastInt != TheInventoryManager.AllInventory_BaseItem[TheIndex].TheThingsInSlot.Count && TheSlotType != CreateNewJson.TheType_InSlot.ForCloth)
            {
                LastInt = TheInventoryManager.AllInventory_BaseItem[TheIndex].TheThingsInSlot.Count;
                TheText.text = TheInventoryManager.AllInventory_BaseItem[TheIndex].TheThingsInSlot.Count.ToString();
            }

        }
    }

    public void UpdateSlotData()
    {
        if (TheInventoryManager.AllInventory_BaseItem[TheIndex].TheThingsInSlot.Count > 0)
        {
            if (TheInventoryManager.AllInventory_BaseItem[TheIndex].TheThingsInSlot.TheBaseItem.Type == CreateNewJson.TheType.Cloth && this.TheSlotType != CreateNewJson.TheType_InSlot.ForCloth && this.TheSlotType != CreateNewJson.TheType_InSlot.ForMagazine)
            {
                ItemImage.sprite = ClothItem.GetSprite_NoXML_WithReturn(TheInventoryManager.AllInventory_BaseItem[TheIndex].TheThingsInSlot.TheBaseItem as CreateClothXML.ClothClass);
            }
            else
            {
                if (TheSlotType == CreateNewJson.TheType_InSlot.None)
                {
                    byte[] imageData = File.ReadAllBytes(TheInventoryManager.AllInventory_BaseItem[TheIndex].TheThingsInSlot.TheBaseItem.TheSprite_Path);
                    Texture2D texture = new Texture2D(2, 2);
                    texture.LoadImage(imageData);

                    ItemImage.sprite = ConvertWholeTextureToSprite(texture);
                }

            }

            if (TheSlotType == CreateNewJson.TheType_InSlot.None)
                {
                    if (TheInventoryManager.AllInventory_BaseItem[TheIndex].TheThingsInSlot.TheBaseItem.Type == CreateNewJson.TheType.Ammo
                    && TheInventoryManager.AllInventory_BaseItem[TheIndex].TheThingsInSlot.TheBaseItem.Stackable == false)
                {
                    CreateNewJson.Ammo TheAmmo = TheInventoryManager.AllInventory_BaseItem[TheIndex].TheThingsInSlot.TheBaseItem as CreateNewJson.Ammo;
                    AmmoSlider.gameObject.SetActive(true);
                    AmmoSlider.value = (float)TheAmmo.RemainMagazine/TheAmmo.MaxMagazine;
                }
                else if (TheInventoryManager.AllInventory_BaseItem[TheIndex].TheThingsInSlot.TheBaseItem.Stackable == true)
                {
                    TheText.gameObject.SetActive(true);
                    TheText.text = TheInventoryManager.AllInventory_BaseItem[TheIndex].TheThingsInSlot.Count.ToString();
                }
            }
        }
    }

    public Sprite ConvertWholeTextureToSprite(Texture2D texture)
    {
        return Sprite.Create(
            texture,
            new Rect(0, 0, texture.width, texture.height), // 整张图区域
            new Vector2(0.5f, 0.5f)                        // 锚点居中
        );
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left && TheDrag.IsDraging == false)
        {
            GetItem();
        }
        else if (eventData.button == PointerEventData.InputButton.Left && TheDrag.IsDraging == true)
        {
            PutItem();
        }
    }

    public void GetItem()
    {
        TheDrag.TheFatherSlot = this;
        TheDrag.ItemImage.gameObject.SetActive(true);
        TheDrag.ItemImage.sprite = ItemImage.sprite;

        TheDrag.IsDraging = true;
    }

    public void PutItem()
    {

        TheDrag.ItemImage.gameObject.SetActive(false);
        TheDrag.ItemImage.sprite = TheDrag.TheEmptySprite;


        //////////////////////////////////////////////////////////下面为重新放置物品
        ItemSlot TheItemSlot = TheDrag.TheFatherSlot as ItemSlot;
        if (TheItemSlot != null)
        {
            if (TheSlotType == CreateNewJson.TheType_InSlot.None) // 默认物品栏
            {
                if (TheItemSlot.TheSlotType == CreateNewJson.TheType_InSlot.None )
                {


                    if ( TheItemSlot.TheInventoryManager.AllInventory_BaseItem[TheItemSlot.TheIndex].TheThingsInSlot.TheBaseItem.Stackable == false && TheInventoryManager.AllInventory_BaseItem[TheIndex].TheThingsInSlot.Count <= 0)
                    {

                        TheInventoryManager.AllInventory_BaseItem[TheIndex].TheThingsInSlot = new InventoryManager.ThingsInSlot(
                            TheItemSlot.TheInventoryManager.AllInventory_BaseItem[TheItemSlot.TheIndex].TheThingsInSlot.Count,
                            TheItemSlot.TheInventoryManager.AllInventory_BaseItem[TheItemSlot.TheIndex].TheThingsInSlot.TheBaseItem);

                        if (TheItemSlot.TheInventoryManager.AllInventory_BaseItem[TheItemSlot.TheIndex].TheThingsInSlot.TheBaseItem.Type == CreateNewJson.TheType.Ammo
                        )
                        {
                            CreateNewJson.Ammo TheAmmo = TheItemSlot.TheInventoryManager.AllInventory_BaseItem[TheItemSlot.TheIndex].TheThingsInSlot.TheBaseItem as CreateNewJson.Ammo;
                            TheItemSlot.AmmoSlider.gameObject.SetActive(true);
                            TheItemSlot.AmmoSlider.value = (float)TheAmmo.RemainMagazine / TheAmmo.MaxMagazine;
                        }

                        TheItemSlot.ItemImage.sprite = TheDrag.TheEmptySprite;
                        TheItemSlot.TheText.gameObject.SetActive(false);
                        TheItemSlot.AmmoSlider.gameObject.SetActive(false);
                        TheItemSlot.TheInventoryManager.AllInventory_BaseItem[TheItemSlot.TheIndex].TheThingsInSlot.Count = 0;
                        TheItemSlot.TheInventoryManager.AllInventory_BaseItem[TheItemSlot.TheIndex].TheThingsInSlot.TheBaseItem = null;
                    }
                    else if(TheItemSlot.TheInventoryManager.AllInventory_BaseItem[TheItemSlot.TheIndex].TheThingsInSlot.TheBaseItem.Stackable == true)
                    {

                        if (TheInventoryManager.AllInventory_BaseItem[TheIndex].TheThingsInSlot.Count <= 0)
                        {
                            TheText.gameObject.SetActive(true);
                            TheText.text = TheItemSlot.TheInventoryManager.AllInventory_BaseItem[TheItemSlot.TheIndex].TheThingsInSlot.Count.ToString();
                            TheInventoryManager.AllInventory_BaseItem[TheIndex].TheThingsInSlot.TheBaseItem = TheItemSlot.TheInventoryManager.AllInventory_BaseItem[TheItemSlot.TheIndex].TheThingsInSlot.TheBaseItem;
                            TheInventoryManager.AllInventory_BaseItem[TheIndex].TheThingsInSlot.Count = TheItemSlot.TheInventoryManager.AllInventory_BaseItem[TheItemSlot.TheIndex].TheThingsInSlot.Count;
                            ItemImage.sprite = TheItemSlot.ItemImage.sprite;

                            TheItemSlot.ItemImage.sprite = TheDrag.TheEmptySprite;
                            TheItemSlot.TheText.gameObject.SetActive(false);
                            TheItemSlot.AmmoSlider.gameObject.SetActive(false);
                            TheItemSlot.TheInventoryManager.AllInventory_BaseItem[TheItemSlot.TheIndex].TheThingsInSlot.Count = 0;
                            TheItemSlot.TheInventoryManager.AllInventory_BaseItem[TheItemSlot.TheIndex].TheThingsInSlot.TheBaseItem = null;
                        }
                        else
                        {

                            if (TheInventoryManager.AllInventory_BaseItem[TheIndex].TheThingsInSlot.TheBaseItem.UniqueCode == TheItemSlot.TheInventoryManager.AllInventory_BaseItem[TheItemSlot.TheIndex].TheThingsInSlot.TheBaseItem.UniqueCode &&
                                TheInventoryManager.AllInventory_BaseItem[TheIndex].TheThingsInSlot.Count < TheInventoryManager.AllInventory_BaseItem[TheIndex].TheThingsInSlot.TheBaseItem.MaxStack
                                )
                            {
                                TheInventoryManager.AllInventory_BaseItem[TheIndex].TheThingsInSlot.Count += TheItemSlot.TheInventoryManager.AllInventory_BaseItem[TheItemSlot.TheIndex].TheThingsInSlot.Count;
                                if (TheInventoryManager.AllInventory_BaseItem[TheIndex].TheThingsInSlot.Count > TheInventoryManager.AllInventory_BaseItem[TheIndex].TheThingsInSlot.TheBaseItem.MaxStack)
                                {
                                    int Num = TheInventoryManager.AllInventory_BaseItem[TheIndex].TheThingsInSlot.TheBaseItem.MaxStack - TheInventoryManager.AllInventory_BaseItem[TheIndex].TheThingsInSlot.Count;
                                    TheInventoryManager.AllInventory_BaseItem[TheIndex].TheThingsInSlot.Count = TheInventoryManager.AllInventory_BaseItem[TheIndex].TheThingsInSlot.TheBaseItem.MaxStack;
                                    TheItemSlot.TheInventoryManager.AllInventory_BaseItem[TheItemSlot.TheIndex].TheThingsInSlot.Count = Math.Abs(Num);

                                    TheItemSlot.TheText.text = TheItemSlot.TheInventoryManager.AllInventory_BaseItem[TheItemSlot.TheIndex].TheThingsInSlot.Count.ToString();
                                    TheText.text = TheInventoryManager.AllInventory_BaseItem[TheItemSlot.TheIndex].TheThingsInSlot.Count.ToString();
                                }
                                else
                                {
                                    TheText.text = TheInventoryManager.AllInventory_BaseItem[TheItemSlot.TheIndex].TheThingsInSlot.Count.ToString();

                                    TheItemSlot.ItemImage.sprite = TheDrag.TheEmptySprite;
                                    TheItemSlot.TheText.gameObject.SetActive(false);
                                    TheItemSlot.AmmoSlider.gameObject.SetActive(false);
                                    TheItemSlot.TheInventoryManager.AllInventory_BaseItem[TheItemSlot.TheIndex].TheThingsInSlot.Count = 0;
                                    TheItemSlot.TheInventoryManager.AllInventory_BaseItem[TheItemSlot.TheIndex].TheThingsInSlot.TheBaseItem = null;
                                }
                            }
                        }


                    }



                }
                else if(TheItemSlot.TheSlotType == CreateNewJson.TheType_InSlot.ForMagazine)
                {
                    if (TheInventoryManager.AllInventory_BaseItem[TheIndex].TheThingsInSlot.Count > 0 &&
                        TheItemSlot.TheInventoryManager.AllInventory_BaseItem[TheDrag.Unable.ItemSlotSelectedIndex].TheThingsInSlot.TheBaseItem.InventoryObjs[TheItemSlot.TheIndex].TheCount + TheInventoryManager.AllInventory_BaseItem[TheIndex].TheThingsInSlot.Count > TheInventoryManager.AllInventory_BaseItem[TheIndex].TheThingsInSlot.TheBaseItem.MaxStack &&
                        TheItemSlot.TheInventoryManager.AllInventory_BaseItem[TheDrag.Unable.ItemSlotSelectedIndex].TheThingsInSlot.TheBaseItem.InventoryObjs[TheItemSlot.TheIndex].TheItemData.UniqueCode == TheInventoryManager.AllInventory_BaseItem[TheIndex].TheThingsInSlot.TheBaseItem.UniqueCode)
                    {
                        int Num = TheInventoryManager.AllInventory_BaseItem[TheIndex].TheThingsInSlot.Count + TheItemSlot.TheInventoryManager.AllInventory_BaseItem[TheDrag.Unable.ItemSlotSelectedIndex].TheThingsInSlot.TheBaseItem.InventoryObjs[TheItemSlot.TheIndex].TheCount - TheInventoryManager.AllInventory_BaseItem[TheIndex].TheThingsInSlot.TheBaseItem.MaxStack;

                        //TheInventoryManager.AllInventory_BaseItem[TheIndex].TheThingsInSlot = new InventoryManager.ThingsInSlot(
                        //    TheItemSlot.TheInventoryManager.AllInventory_BaseItem[TheDrag.Unable.ItemSlotSelectedIndex].TheThingsInSlot.TheBaseItem.InventoryObjs[TheItemSlot.TheIndex].TheCount,
                        //    TheItemSlot.TheInventoryManager.AllInventory_BaseItem[TheDrag.Unable.ItemSlotSelectedIndex].TheThingsInSlot.TheBaseItem.InventoryObjs[TheItemSlot.TheIndex].TheItemData);

                        if (TheItemSlot.TheInventoryManager.AllInventory_BaseItem[TheDrag.Unable.ItemSlotSelectedIndex].TheThingsInSlot.TheBaseItem.InventoryObjs[TheItemSlot.TheIndex].TheItemData.Type == CreateNewJson.TheType.Ammo
                            && TheItemSlot.TheInventoryManager.AllInventory_BaseItem[TheDrag.Unable.ItemSlotSelectedIndex].TheThingsInSlot.TheBaseItem.InventoryObjs[TheItemSlot.TheIndex].TheItemData.Stackable == false)
                        {

                            CreateNewJson.Ammo TheAmmo = TheItemSlot.TheInventoryManager.AllInventory_BaseItem[TheDrag.Unable.ItemSlotSelectedIndex].TheThingsInSlot.TheBaseItem.InventoryObjs[TheItemSlot.TheIndex].TheItemData as CreateNewJson.Ammo;
                            TheItemSlot.AmmoSlider.gameObject.SetActive(true);
                            TheItemSlot.AmmoSlider.value = (float)TheAmmo.RemainMagazine / TheAmmo.MaxMagazine;
                        }
                        else if (TheItemSlot.TheInventoryManager.AllInventory_BaseItem[TheDrag.Unable.ItemSlotSelectedIndex].TheThingsInSlot.TheBaseItem.InventoryObjs[TheItemSlot.TheIndex].TheItemData.Stackable == true)
                        {
                            TheItemSlot.TheText.gameObject.SetActive(true);
                            TheItemSlot.TheText.text = TheItemSlot.TheInventoryManager.AllInventory_BaseItem[TheDrag.Unable.ItemSlotSelectedIndex].TheThingsInSlot.TheBaseItem.InventoryObjs[TheItemSlot.TheIndex].TheCount.ToString();
                        }

                        TheInventoryManager.AllInventory_BaseItem[TheIndex].TheThingsInSlot.Count = TheInventoryManager.AllInventory_BaseItem[TheIndex].TheThingsInSlot.TheBaseItem.MaxStack;
                        TheItemSlot.TheInventoryManager.AllInventory_BaseItem[TheDrag.Unable.ItemSlotSelectedIndex].TheThingsInSlot.TheBaseItem.InventoryObjs[TheItemSlot.TheIndex].TheCount = Math.Abs(Num);

                    }
                    else if (TheInventoryManager.AllInventory_BaseItem[TheIndex].TheThingsInSlot.Count > 0 &&
                        TheItemSlot.TheInventoryManager.AllInventory_BaseItem[TheDrag.Unable.ItemSlotSelectedIndex].TheThingsInSlot.TheBaseItem.InventoryObjs[TheItemSlot.TheIndex].TheCount + TheInventoryManager.AllInventory_BaseItem[TheIndex].TheThingsInSlot.Count <= TheInventoryManager.AllInventory_BaseItem[TheIndex].TheThingsInSlot.TheBaseItem.MaxStack &&
                        TheItemSlot.TheInventoryManager.AllInventory_BaseItem[TheDrag.Unable.ItemSlotSelectedIndex].TheThingsInSlot.TheBaseItem.InventoryObjs[TheItemSlot.TheIndex].TheItemData.UniqueCode == TheInventoryManager.AllInventory_BaseItem[TheIndex].TheThingsInSlot.TheBaseItem.UniqueCode)
                    {
                        if (TheItemSlot.TheInventoryManager.AllInventory_BaseItem[TheDrag.Unable.ItemSlotSelectedIndex].TheThingsInSlot.TheBaseItem.InventoryObjs[TheItemSlot.TheIndex].TheItemData.Type == CreateNewJson.TheType.Ammo
                            && TheItemSlot.TheInventoryManager.AllInventory_BaseItem[TheDrag.Unable.ItemSlotSelectedIndex].TheThingsInSlot.TheBaseItem.InventoryObjs[TheItemSlot.TheIndex].TheItemData.Stackable == false)
                        {

                            CreateNewJson.Ammo TheAmmo = TheItemSlot.TheInventoryManager.AllInventory_BaseItem[TheDrag.Unable.ItemSlotSelectedIndex].TheThingsInSlot.TheBaseItem.InventoryObjs[TheItemSlot.TheIndex].TheItemData as CreateNewJson.Ammo;
                            TheItemSlot.AmmoSlider.gameObject.SetActive(true);
                            TheItemSlot.AmmoSlider.value = (float)TheAmmo.RemainMagazine / TheAmmo.MaxMagazine;
                        }
                        else if (TheItemSlot.TheInventoryManager.AllInventory_BaseItem[TheDrag.Unable.ItemSlotSelectedIndex].TheThingsInSlot.TheBaseItem.InventoryObjs[TheItemSlot.TheIndex].TheItemData.Stackable == true)
                        {
                            TheItemSlot.TheText.gameObject.SetActive(true);
                            TheItemSlot.TheText.text = TheItemSlot.TheInventoryManager.AllInventory_BaseItem[TheDrag.Unable.ItemSlotSelectedIndex].TheThingsInSlot.TheBaseItem.InventoryObjs[TheItemSlot.TheIndex].TheCount.ToString();
                        }

                        TheInventoryManager.AllInventory_BaseItem[TheIndex].TheThingsInSlot.Count = TheInventoryManager.AllInventory_BaseItem[TheIndex].TheThingsInSlot.Count + TheItemSlot.TheInventoryManager.AllInventory_BaseItem[TheDrag.Unable.ItemSlotSelectedIndex].TheThingsInSlot.TheBaseItem.InventoryObjs[TheItemSlot.TheIndex].TheCount;

                        TheItemSlot.ItemImage.sprite = TheDrag.TheEmptySprite;
                        TheItemSlot.TheText.gameObject.SetActive(false);
                        TheItemSlot.AmmoSlider.gameObject.SetActive(false);
                        TheItemSlot.TheInventoryManager.AllInventory_BaseItem[TheDrag.Unable.ItemSlotSelectedIndex].TheThingsInSlot.TheBaseItem.InventoryObjs[TheItemSlot.TheIndex].TheCount = 0;
                        TheItemSlot.TheInventoryManager.AllInventory_BaseItem[TheDrag.Unable.ItemSlotSelectedIndex].TheThingsInSlot.TheBaseItem.InventoryObjs[TheItemSlot.TheIndex].TheItemData = null;
                    }
                    else
                    {
                        TheInventoryManager.AllInventory_BaseItem[TheIndex].TheThingsInSlot = new InventoryManager.ThingsInSlot(
                            TheItemSlot.TheInventoryManager.AllInventory_BaseItem[TheDrag.Unable.ItemSlotSelectedIndex].TheThingsInSlot.TheBaseItem.InventoryObjs[TheItemSlot.TheIndex].TheCount,
                            TheItemSlot.TheInventoryManager.AllInventory_BaseItem[TheDrag.Unable.ItemSlotSelectedIndex].TheThingsInSlot.TheBaseItem.InventoryObjs[TheItemSlot.TheIndex].TheItemData);

                        if (TheItemSlot.TheInventoryManager.AllInventory_BaseItem[TheDrag.Unable.ItemSlotSelectedIndex].TheThingsInSlot.TheBaseItem.InventoryObjs[TheItemSlot.TheIndex].TheItemData.Type == CreateNewJson.TheType.Ammo
                            && TheItemSlot.TheInventoryManager.AllInventory_BaseItem[TheDrag.Unable.ItemSlotSelectedIndex].TheThingsInSlot.TheBaseItem.InventoryObjs[TheItemSlot.TheIndex].TheItemData.Stackable == false)
                        {

                            CreateNewJson.Ammo TheAmmo = TheItemSlot.TheInventoryManager.AllInventory_BaseItem[TheDrag.Unable.ItemSlotSelectedIndex].TheThingsInSlot.TheBaseItem.InventoryObjs[TheItemSlot.TheIndex].TheItemData as CreateNewJson.Ammo;
                            TheItemSlot.AmmoSlider.gameObject.SetActive(true);
                            TheItemSlot.AmmoSlider.value = (float)TheAmmo.RemainMagazine / TheAmmo.MaxMagazine;
                        }
                        else if (TheItemSlot.TheInventoryManager.AllInventory_BaseItem[TheDrag.Unable.ItemSlotSelectedIndex].TheThingsInSlot.TheBaseItem.InventoryObjs[TheItemSlot.TheIndex].TheItemData.Stackable == true)
                        {
                            TheItemSlot.TheText.gameObject.SetActive(true);
                            TheItemSlot.TheText.text = TheItemSlot.TheInventoryManager.AllInventory_BaseItem[TheDrag.Unable.ItemSlotSelectedIndex].TheThingsInSlot.TheBaseItem.InventoryObjs[TheItemSlot.TheIndex].TheCount.ToString();
                        }

                        TheItemSlot.ItemImage.sprite = TheDrag.TheEmptySprite;
                        TheItemSlot.TheText.gameObject.SetActive(false);
                        TheItemSlot.AmmoSlider.gameObject.SetActive(false);
                        TheItemSlot.TheInventoryManager.AllInventory_BaseItem[TheDrag.Unable.ItemSlotSelectedIndex].TheThingsInSlot.TheBaseItem.InventoryObjs[TheItemSlot.TheIndex].TheCount = 0;
                        TheItemSlot.TheInventoryManager.AllInventory_BaseItem[TheDrag.Unable.ItemSlotSelectedIndex].TheThingsInSlot.TheBaseItem.InventoryObjs[TheItemSlot.TheIndex].TheItemData = null;
                    }
                }
                else if(TheItemSlot.TheSlotType == CreateNewJson.TheType_InSlot.ForCloth && TheInventoryManager.AllInventory_BaseItem[TheIndex].TheThingsInSlot.Count <= 0)//从装备栏放回物品栏
                {
                    if (TheItemSlot.ClothType == CreateClothXML.ClothType.UpperCloth)
                    {
                        Destroy(TheItemSlot.TheInventoryManager.Upper.TheCloth.TheCloth);
                        TheItemSlot.TheInventoryManager.Upper.TheCloth.TheCloth = null;

                        TheInventoryManager.AllInventory_BaseItem[TheIndex].TheThingsInSlot = new InventoryManager.ThingsInSlot(
                            1,
                            TheItemSlot.TheInventoryManager.Upper.TheClothClass);

                        for (int i = 0; i < TheItemSlot.TheInventoryManager.Upper.TheCloth.UpperArm_Sprites.Length; i++)
                        {
                            TheItemSlot.TheInventoryManager.Upper.TheCloth.UpperArm_Sprites[i].sprite = TheDrag.TheEmptySprite;
                        }

                        for (int i = 0; i < TheItemSlot.TheInventoryManager.Upper.TheCloth.LowerArm_Sprites.Length; i++)
                        {
                            TheItemSlot.TheInventoryManager.Upper.TheCloth.LowerArm_Sprites[i].sprite = TheDrag.TheEmptySprite;
                        }

                        for (int i = 0; i < TheItemSlot.TheInventoryManager.Upper.TheCloth.Body_Sprites.Length; i++)
                        {
                            TheItemSlot.TheInventoryManager.Upper.TheCloth.Body_Sprites[i].sprite = TheDrag.TheEmptySprite;
                        }

                        TheItemSlot.ItemImage.sprite = TheDrag.TheEmptySprite;
                        TheItemSlot.ClassicPNG.gameObject.SetActive(true);
                        TheItemSlot.TheInventoryManager.Upper.TheCloth.HasEquipped = false;
                        TheItemSlot.TheInventoryManager.Upper.TheClothClass = null;



                    }
                    else if (TheItemSlot.ClothType == CreateClothXML.ClothType.Helmet)
                    {
                        Destroy(TheItemSlot.TheInventoryManager.Helmet.TheCloth.TheCloth);
                        TheItemSlot.TheInventoryManager.Helmet.TheCloth.TheCloth = null;

                        TheInventoryManager.AllInventory_BaseItem[TheIndex].TheThingsInSlot = new InventoryManager.ThingsInSlot(
                            1,
                            TheItemSlot.TheInventoryManager.Helmet.TheClothClass);

                        for (int i = 0; i < TheItemSlot.TheInventoryManager.Helmet.TheCloth.Sprites.Length; i++)
                        {
                            TheItemSlot.TheInventoryManager.Helmet.TheCloth.Sprites[i].sprite = TheDrag.TheEmptySprite;
                        }

                        TheItemSlot.ItemImage.sprite = TheDrag.TheEmptySprite;
                        TheItemSlot.ClassicPNG.gameObject.SetActive(true);
                        TheItemSlot.TheInventoryManager.Helmet.TheCloth.HasEquipped = false;
                        TheItemSlot.TheInventoryManager.Helmet.TheClothClass = null;

                        TheItemSlot.TheInventoryManager.ThePlayer.TheHair.gameObject.SetActive(true);

                    }
                    else if (TheItemSlot.ClothType == CreateClothXML.ClothType.LowerCloth && TheItemSlot.TheInventoryManager.Lower.TheCloth.HasEquipped)
                    {
                        Destroy(TheItemSlot.TheInventoryManager.Lower.TheCloth.TheCloth);
                        TheItemSlot.TheInventoryManager.Lower.TheCloth.TheCloth = null;

                        TheInventoryManager.AllInventory_BaseItem[TheIndex].TheThingsInSlot = new InventoryManager.ThingsInSlot(
                            1,
                            TheItemSlot.TheInventoryManager.Lower.TheClothClass);

                        for (int i = 0; i < TheItemSlot.TheInventoryManager.Lower.TheCloth.UpperLeg_Sprites.Length; i++)
                        {
                            TheItemSlot.TheInventoryManager.Lower.TheCloth.UpperLeg_Sprites[i].sprite = TheDrag.TheEmptySprite;
                        }

                        for (int i = 0; i < TheItemSlot.TheInventoryManager.Lower.TheCloth.LowerLeg_Sprites.Length; i++)
                        {
                            TheItemSlot.TheInventoryManager.Lower.TheCloth.LowerLeg_Sprites[i].sprite = TheDrag.TheEmptySprite;
                        }

                        for (int i = 0; i < TheItemSlot.TheInventoryManager.Lower.TheCloth.Hip_Sprites.Length; i++)
                        {
                            TheItemSlot.TheInventoryManager.Lower.TheCloth.Hip_Sprites[i].sprite = TheDrag.TheEmptySprite;
                        }


                        TheItemSlot.ItemImage.sprite = TheDrag.TheEmptySprite;
                        TheItemSlot.ClassicPNG.gameObject.SetActive(true);
                        TheItemSlot.TheInventoryManager.Lower.TheCloth.HasEquipped = false;
                        TheItemSlot.TheInventoryManager.Lower.TheClothClass = null;

                    }
                    else if (TheItemSlot.ClothType == CreateClothXML.ClothType.LowerCloth && TheItemSlot.TheInventoryManager.Hip.TheCloth.HasEquipped)
                    {
                        Destroy(TheItemSlot.TheInventoryManager.Hip.TheCloth.TheCloth);
                        TheItemSlot.TheInventoryManager.Hip.TheCloth.TheCloth = null;

                        TheInventoryManager.AllInventory_BaseItem[TheIndex].TheThingsInSlot = new InventoryManager.ThingsInSlot(
                            1,
                            TheItemSlot.TheInventoryManager.Hip.TheClothClass);

                        TheInventoryManager.TheSpriteResolver.SetCategoryAndLabel("Skirt", "Nothing");

                        TheItemSlot.ItemImage.sprite = TheDrag.TheEmptySprite;
                        TheItemSlot.ClassicPNG.gameObject.SetActive(true);
                        TheItemSlot.TheInventoryManager.Hip.TheCloth.HasEquipped = false;
                        TheItemSlot.TheInventoryManager.Hip.TheClothClass = null;

                    }
                    else if (TheItemSlot.ClothType == CreateClothXML.ClothType.Socks && TheItemSlot.TheInventoryManager.Socks.TheCloth.HasEquipped)
                    {
                        Destroy(TheItemSlot.TheInventoryManager.Socks.TheCloth.TheCloth);
                        TheItemSlot.TheInventoryManager.Socks.TheCloth.TheCloth = null;

                        TheInventoryManager.AllInventory_BaseItem[TheIndex].TheThingsInSlot = new InventoryManager.ThingsInSlot(
                            1,
                            TheItemSlot.TheInventoryManager.Socks.TheClothClass);

                        for (int i = 0; i < TheItemSlot.TheInventoryManager.Socks.TheCloth.UpperLegStocing_Sprite.Length; i++)
                        {
                            TheItemSlot.TheInventoryManager.Socks.TheCloth.UpperLegStocing_Sprite[i].sprite = TheDrag.TheEmptySprite;
                        }

                        for (int i = 0; i < TheItemSlot.TheInventoryManager.Socks.TheCloth.LowerLegStocing_Sprite.Length; i++)
                        {
                            TheItemSlot.TheInventoryManager.Socks.TheCloth.LowerLegStocing_Sprite[i].sprite = TheDrag.TheEmptySprite;
                        }


                        TheItemSlot.ItemImage.sprite = TheDrag.TheEmptySprite;
                        TheItemSlot.ClassicPNG.gameObject.SetActive(true);
                        TheItemSlot.TheInventoryManager.Socks.TheCloth.HasEquipped = false;
                        TheItemSlot.TheInventoryManager.Socks.TheClothClass = null;

                    }
                    else if (TheItemSlot.ClothType == CreateClothXML.ClothType.Socks && TheItemSlot.TheInventoryManager.Stocking.TheCloth.HasEquipped)
                    {
                        Destroy(TheItemSlot.TheInventoryManager.Stocking.TheCloth.TheCloth);
                        TheItemSlot.TheInventoryManager.Stocking.TheCloth.TheCloth = null;

                        TheInventoryManager.AllInventory_BaseItem[TheIndex].TheThingsInSlot = new InventoryManager.ThingsInSlot(
                            1,
                            TheItemSlot.TheInventoryManager.Stocking.TheClothClass);

                        for (int i = 0; i < TheItemSlot.TheInventoryManager.Stocking.TheCloth.HipStocking_Sprite.Length; i++)
                        {
                            TheItemSlot.TheInventoryManager.Stocking.TheCloth.HipStocking_Sprite[i].sprite = TheDrag.TheEmptySprite;
                        }

                        for (int i = 0; i < TheItemSlot.TheInventoryManager.Stocking.TheCloth.UpperLegStocking_Sprite.Length; i++)
                        {
                            TheItemSlot.TheInventoryManager.Stocking.TheCloth.UpperLegStocking_Sprite[i].sprite = TheDrag.TheEmptySprite;
                        }

                        for (int i = 0; i < TheItemSlot.TheInventoryManager.Stocking.TheCloth.LowerLegStocking_Sprite.Length; i++)
                        {
                            TheItemSlot.TheInventoryManager.Stocking.TheCloth.LowerLegStocking_Sprite[i].sprite = TheDrag.TheEmptySprite;
                        }


                        TheItemSlot.ItemImage.sprite = TheDrag.TheEmptySprite;
                        TheItemSlot.ClassicPNG.gameObject.SetActive(true);
                        TheItemSlot.TheInventoryManager.Stocking.TheCloth.HasEquipped = false;
                        TheItemSlot.TheInventoryManager.Stocking.TheClothClass = null;

                    }
                    else if (TheItemSlot.ClothType == CreateClothXML.ClothType.Shoes)
                    {
                        Destroy(TheItemSlot.TheInventoryManager.Shoes.TheCloth.TheCloth);
                        TheItemSlot.TheInventoryManager.Shoes.TheCloth.TheCloth = null;

                        TheInventoryManager.AllInventory_BaseItem[TheIndex].TheThingsInSlot = new InventoryManager.ThingsInSlot(
                            1,
                            TheItemSlot.TheInventoryManager.Shoes.TheClothClass);

                        for (int i = 0; i < TheItemSlot.TheInventoryManager.Shoes.TheCloth.Sprites.Length; i++)
                        {
                            TheItemSlot.TheInventoryManager.Shoes.TheCloth.Sprites[i].sprite = TheDrag.TheEmptySprite;
                        }


                        TheItemSlot.ItemImage.sprite = TheDrag.TheEmptySprite;
                        TheItemSlot.ClassicPNG.gameObject.SetActive(true);
                        TheItemSlot.TheInventoryManager.Shoes.TheCloth.HasEquipped = false;
                        TheItemSlot.TheInventoryManager.Shoes.TheClothClass = null;

                    }
                    else if (TheItemSlot.ClothType == CreateClothXML.ClothType.Armor)
                    {
                        Destroy(TheItemSlot.TheInventoryManager.Armor.TheCloth.TheCloth);
                        TheItemSlot.TheInventoryManager.Armor.TheCloth.TheCloth = null;

                        TheInventoryManager.AllInventory_BaseItem[TheIndex].TheThingsInSlot = new InventoryManager.ThingsInSlot(
                            1,
                            TheItemSlot.TheInventoryManager.Armor.TheClothClass);

                        for (int i = 0; i < TheItemSlot.TheInventoryManager.Armor.TheCloth.Sprites.Length; i++)
                        {
                            TheItemSlot.TheInventoryManager.Armor.TheCloth.Sprites[i].sprite = TheDrag.TheEmptySprite;
                        }

                        TheItemSlot.ItemImage.sprite = TheDrag.TheEmptySprite;
                        TheItemSlot.ClassicPNG.gameObject.SetActive(true);
                        TheItemSlot.TheInventoryManager.Armor.TheCloth.HasEquipped = false;
                        TheItemSlot.TheInventoryManager.Armor.TheClothClass = null;

                    }

                }


                if (TheInventoryManager.Equals(TheItemSlot.TheInventoryManager))
                    TheInventoryManager.MoniterTheList();
                else
                {
                    TheInventoryManager.MoniterTheList();
                    TheItemSlot.TheInventoryManager.MoniterTheList();
                }

                StartCoroutine(TheDrag.Unable.EquipItem());

            }
            else if (TheSlotType == CreateNewJson.TheType_InSlot.ForMagazine) //弹药物品栏
            {
                CreateNewJson.LongRangeWeapon TheLongRange = /*TheItemSlot.*/TheInventoryManager.AllInventory_BaseItem[TheDrag.Unable.ItemSlotSelectedIndex].TheThingsInSlot.TheBaseItem as CreateNewJson.LongRangeWeapon;
                CreateNewJson.Ammo TheAmmo = TheItemSlot.TheInventoryManager.AllInventory_BaseItem[TheItemSlot.TheIndex].TheThingsInSlot.TheBaseItem as CreateNewJson.Ammo;

                if (TheLongRange.AmmoCode.Contains<string>(TheAmmo.UniqueCode) && (TheLongRange.InventoryObjs[0].TheCount <= 0 || TheLongRange.InventoryObjs[0].TheItemData.Stackable == true))
                {
                    if (TheItemSlot.TheInventoryManager.AllInventory_BaseItem[TheItemSlot.TheIndex].TheThingsInSlot.TheBaseItem.Stackable == false)
                    {
                        /*TheItemSlot.*/TheInventoryManager.AllInventory_BaseItem[TheDrag.Unable.ItemSlotSelectedIndex].TheThingsInSlot.TheBaseItem.InventoryObjs[0].TheCount =
                            1;

                        //Debug.Log(TheItemSlot.TheInventoryManager.AllInventory_BaseItem[TheDrag.Unable.ItemSlotSelectedIndex].TheThingsInSlot.TheBaseItem.UniqueCode);

                        /*TheItemSlot.*/TheInventoryManager.AllInventory_BaseItem[TheDrag.Unable.ItemSlotSelectedIndex].TheThingsInSlot.TheBaseItem.InventoryObjs[0].TheItemData =
                            TheItemSlot.TheInventoryManager.AllInventory_BaseItem[TheItemSlot.TheIndex].TheThingsInSlot.TheBaseItem;

                        TheItemSlot.ItemImage.sprite = TheDrag.TheEmptySprite;
                        TheItemSlot.TheText.gameObject.SetActive(false);
                        TheItemSlot.AmmoSlider.gameObject.SetActive(false);
                        TheItemSlot.TheInventoryManager.AllInventory_BaseItem[TheItemSlot.TheIndex].TheThingsInSlot.Count = 0;
                        TheItemSlot.TheInventoryManager.AllInventory_BaseItem[TheItemSlot.TheIndex].TheThingsInSlot.TheBaseItem = null;

                        TheItemSlot.TheInventoryManager.MoniterTheList();

                        StartCoroutine(TheDrag.Unable.EquipItem());
                    }
                    else
                    {
                        if ((/*TheItemSlot.*/TheInventoryManager.AllInventory_BaseItem[TheDrag.Unable.ItemSlotSelectedIndex].TheThingsInSlot.TheBaseItem as CreateNewJson.LongRangeWeapon).MaxAmmoStack - /*TheItemSlot.*/TheInventoryManager.AllInventory_BaseItem[TheDrag.Unable.ItemSlotSelectedIndex].TheThingsInSlot.TheBaseItem.InventoryObjs[0].TheCount >= TheItemSlot.TheInventoryManager.AllInventory_BaseItem[TheItemSlot.TheIndex].TheThingsInSlot.Count)
                        {
                            TheInventoryManager.AllInventory_BaseItem[TheDrag.Unable.ItemSlotSelectedIndex].TheThingsInSlot.TheBaseItem.InventoryObjs[0].TheCount =
                                TheItemSlot.TheInventoryManager.AllInventory_BaseItem[TheItemSlot.TheIndex].TheThingsInSlot.Count + /*TheItemSlot.*/TheInventoryManager.AllInventory_BaseItem[TheDrag.Unable.ItemSlotSelectedIndex].TheThingsInSlot.TheBaseItem.InventoryObjs[0].TheCount;

                            TheInventoryManager.AllInventory_BaseItem[TheDrag.Unable.ItemSlotSelectedIndex].TheThingsInSlot.TheBaseItem.InventoryObjs[0].TheItemData =
                                TheItemSlot.TheInventoryManager.AllInventory_BaseItem[TheItemSlot.TheIndex].TheThingsInSlot.TheBaseItem;

                            TheItemSlot.ItemImage.sprite = TheDrag.TheEmptySprite;
                            TheItemSlot.TheText.gameObject.SetActive(false);
                            TheItemSlot.AmmoSlider.gameObject.SetActive(false);
                            TheItemSlot.TheInventoryManager.AllInventory_BaseItem[TheItemSlot.TheIndex].TheThingsInSlot.Count = 0;
                            TheItemSlot.TheInventoryManager.AllInventory_BaseItem[TheItemSlot.TheIndex].TheThingsInSlot.TheBaseItem = null;

                            TheItemSlot.TheInventoryManager.MoniterTheList();

                            StartCoroutine(TheDrag.Unable.EquipItem());
                        }
                        else if ((/*TheItemSlot.*/TheInventoryManager.AllInventory_BaseItem[TheDrag.Unable.ItemSlotSelectedIndex].TheThingsInSlot.TheBaseItem as CreateNewJson.LongRangeWeapon).MaxAmmoStack - /*TheItemSlot.*/TheInventoryManager.AllInventory_BaseItem[TheDrag.Unable.ItemSlotSelectedIndex].TheThingsInSlot.TheBaseItem.InventoryObjs[0].TheCount < TheItemSlot.TheInventoryManager.AllInventory_BaseItem[TheItemSlot.TheIndex].TheThingsInSlot.Count)
                        {
                            int Num = (/*TheItemSlot.*/TheInventoryManager.AllInventory_BaseItem[TheDrag.Unable.ItemSlotSelectedIndex].TheThingsInSlot.TheBaseItem as CreateNewJson.LongRangeWeapon).MaxAmmoStack - /*TheItemSlot.*/TheInventoryManager.AllInventory_BaseItem[TheDrag.Unable.ItemSlotSelectedIndex].TheThingsInSlot.TheBaseItem.InventoryObjs[0].TheCount;

                            /*TheItemSlot.*/TheInventoryManager.AllInventory_BaseItem[TheDrag.Unable.ItemSlotSelectedIndex].TheThingsInSlot.TheBaseItem.InventoryObjs[0].TheCount =
                                (/*TheItemSlot.*/TheInventoryManager.AllInventory_BaseItem[TheDrag.Unable.ItemSlotSelectedIndex].TheThingsInSlot.TheBaseItem as CreateNewJson.LongRangeWeapon).MaxAmmoStack;

                            /*TheItemSlot.*/TheInventoryManager.AllInventory_BaseItem[TheDrag.Unable.ItemSlotSelectedIndex].TheThingsInSlot.TheBaseItem.InventoryObjs[0].TheItemData =
                                TheItemSlot.TheInventoryManager.AllInventory_BaseItem[TheItemSlot.TheIndex].TheThingsInSlot.TheBaseItem;

                            TheItemSlot.TheInventoryManager.AllInventory_BaseItem[TheItemSlot.TheIndex].TheThingsInSlot.Count -= Num;

                            TheItemSlot.TheInventoryManager.MoniterTheList();

                            StartCoroutine(TheDrag.Unable.EquipItem());
                        }
                    }
                }

            }
            else if (TheSlotType == CreateNewJson.TheType_InSlot.ForCloth)
            {

                if ((TheItemSlot.TheInventoryManager.AllInventory_BaseItem[TheItemSlot.TheIndex].TheThingsInSlot.TheBaseItem as CreateClothXML.ClothClass).TheType == ClothType || 
                    ((TheItemSlot.TheInventoryManager.AllInventory_BaseItem[TheItemSlot.TheIndex].TheThingsInSlot.TheBaseItem as CreateClothXML.ClothClass).TheType == CreateClothXML.ClothType.Skirt && ClothType == CreateClothXML.ClothType.LowerCloth) ||
                    ((TheItemSlot.TheInventoryManager.AllInventory_BaseItem[TheItemSlot.TheIndex].TheThingsInSlot.TheBaseItem as CreateClothXML.ClothClass).TheType == CreateClothXML.ClothType.Stocking && ClothType == CreateClothXML.ClothType.Socks))
                {


                    if ((TheItemSlot.TheInventoryManager.AllInventory_BaseItem[TheItemSlot.TheIndex].TheThingsInSlot.TheBaseItem as CreateClothXML.ClothClass).TheType == CreateClothXML.ClothType.UpperCloth)
                    {

                        TheInventoryManager.Upper.TheClothClass = TheItemSlot.TheInventoryManager.AllInventory_BaseItem[TheItemSlot.TheIndex].TheThingsInSlot.TheBaseItem as CreateClothXML.ClothClass;

                        Addressables.LoadAssetAsync<GameObject>(TheInventoryManager.Upper.TheClothClass.ThePrefab_Path.Replace("\\", "/")).Completed += handle => {

                            TheInventoryManager.Upper.TheCloth.TheCloth = Instantiate(handle.Result);
                            TheInventoryManager.Upper.TheCloth.TheCloth.SetActive(false);
                        };

                    }
                    else if ((TheItemSlot.TheInventoryManager.AllInventory_BaseItem[TheItemSlot.TheIndex].TheThingsInSlot.TheBaseItem as CreateClothXML.ClothClass).TheType == CreateClothXML.ClothType.LowerCloth)
                    {
                        TheInventoryManager.Lower.TheClothClass = TheItemSlot.TheInventoryManager.AllInventory_BaseItem[TheItemSlot.TheIndex].TheThingsInSlot.TheBaseItem as CreateClothXML.ClothClass;

                        Addressables.LoadAssetAsync<GameObject>(TheInventoryManager.Lower.TheClothClass.ThePrefab_Path.Replace("\\", "/")).Completed += handle => {

                            TheInventoryManager.Lower.TheCloth.TheCloth = Instantiate(handle.Result);
                            TheInventoryManager.Lower.TheCloth.TheCloth.SetActive(false);
                        };
                    }
                    else if ((TheItemSlot.TheInventoryManager.AllInventory_BaseItem[TheItemSlot.TheIndex].TheThingsInSlot.TheBaseItem as CreateClothXML.ClothClass).TheType == CreateClothXML.ClothType.Helmet)
                    {
                        TheInventoryManager.Helmet.TheClothClass = TheItemSlot.TheInventoryManager.AllInventory_BaseItem[TheItemSlot.TheIndex].TheThingsInSlot.TheBaseItem as CreateClothXML.ClothClass;

                        Addressables.LoadAssetAsync<GameObject>(TheInventoryManager.Helmet.TheClothClass.ThePrefab_Path.Replace("\\", "/")).Completed += handle => {

                            TheInventoryManager.Helmet.TheCloth.TheCloth = Instantiate(handle.Result);
                            TheInventoryManager.Helmet.TheCloth.TheCloth.SetActive(false);
                        };
                    }
                    else if ((TheItemSlot.TheInventoryManager.AllInventory_BaseItem[TheItemSlot.TheIndex].TheThingsInSlot.TheBaseItem as CreateClothXML.ClothClass).TheType == CreateClothXML.ClothType.Shoes)
                    {
                        TheInventoryManager.Shoes.TheClothClass = TheItemSlot.TheInventoryManager.AllInventory_BaseItem[TheItemSlot.TheIndex].TheThingsInSlot.TheBaseItem as CreateClothXML.ClothClass;

                        Addressables.LoadAssetAsync<GameObject>(TheInventoryManager.Shoes.TheClothClass.ThePrefab_Path.Replace("\\", "/")).Completed += handle => {

                            TheInventoryManager.Shoes.TheCloth.TheCloth = Instantiate(handle.Result);
                            TheInventoryManager.Shoes.TheCloth.TheCloth.SetActive(false);
                        };
                    }
                    else if ((TheItemSlot.TheInventoryManager.AllInventory_BaseItem[TheItemSlot.TheIndex].TheThingsInSlot.TheBaseItem as CreateClothXML.ClothClass).TheType == CreateClothXML.ClothType.Skirt)
                    {

                        TheInventoryManager.Hip.TheClothClass = TheItemSlot.TheInventoryManager.AllInventory_BaseItem[TheItemSlot.TheIndex].TheThingsInSlot.TheBaseItem as CreateClothXML.ClothClass;

                        Addressables.LoadAssetAsync<GameObject>(TheInventoryManager.Hip.TheClothClass.ThePrefab_Path.Replace("\\", "/")).Completed += handle => {

                            TheInventoryManager.Hip.TheCloth.TheCloth = Instantiate(handle.Result);
                            TheInventoryManager.Hip.TheCloth.TheCloth.SetActive(false);
                        };
                    }
                    else if ((TheItemSlot.TheInventoryManager.AllInventory_BaseItem[TheItemSlot.TheIndex].TheThingsInSlot.TheBaseItem as CreateClothXML.ClothClass).TheType == CreateClothXML.ClothType.Stocking)
                    {
                        TheInventoryManager.Stocking.TheClothClass = TheItemSlot.TheInventoryManager.AllInventory_BaseItem[TheItemSlot.TheIndex].TheThingsInSlot.TheBaseItem as CreateClothXML.ClothClass;

                        Addressables.LoadAssetAsync<GameObject>(TheInventoryManager.Stocking.TheClothClass.ThePrefab_Path.Replace("\\", "/")).Completed += handle => {

                            TheInventoryManager.Stocking.TheCloth.TheCloth = Instantiate(handle.Result);
                            TheInventoryManager.Stocking.TheCloth.TheCloth.SetActive(false);
                        };
                    }
                    else if ((TheItemSlot.TheInventoryManager.AllInventory_BaseItem[TheItemSlot.TheIndex].TheThingsInSlot.TheBaseItem as CreateClothXML.ClothClass).TheType == CreateClothXML.ClothType.Socks)
                    {
                        TheInventoryManager.Socks.TheClothClass = TheItemSlot.TheInventoryManager.AllInventory_BaseItem[TheItemSlot.TheIndex].TheThingsInSlot.TheBaseItem as CreateClothXML.ClothClass;

                        Addressables.LoadAssetAsync<GameObject>(TheInventoryManager.Socks.TheClothClass.ThePrefab_Path.Replace("\\", "/")).Completed += handle => {

                            TheInventoryManager.Socks.TheCloth.TheCloth = Instantiate(handle.Result);
                            TheInventoryManager.Socks.TheCloth.TheCloth.SetActive(false);
                        };
                    }
                    else if ((TheItemSlot.TheInventoryManager.AllInventory_BaseItem[TheItemSlot.TheIndex].TheThingsInSlot.TheBaseItem as CreateClothXML.ClothClass).TheType == CreateClothXML.ClothType.Armor)
                    {
                        TheInventoryManager.Armor.TheClothClass = TheItemSlot.TheInventoryManager.AllInventory_BaseItem[TheItemSlot.TheIndex].TheThingsInSlot.TheBaseItem as CreateClothXML.ClothClass;

                        Addressables.LoadAssetAsync<GameObject>(TheInventoryManager.Armor.TheClothClass.ThePrefab_Path.Replace("\\", "/")).Completed += handle => {

                            TheInventoryManager.Armor.TheCloth.TheCloth = Instantiate(handle.Result);
                            TheInventoryManager.Armor.TheCloth.TheCloth.SetActive(false);
                        };
                    }

                    ItemImage.sprite = TheItemSlot.ItemImage.sprite; 
                    ClassicPNG.gameObject.SetActive(false);

                    TheItemSlot.ItemImage.sprite = TheDrag.TheEmptySprite;
                    TheItemSlot.TheText.gameObject.SetActive(false);
                    TheItemSlot.AmmoSlider.gameObject.SetActive(false);
                    TheItemSlot.TheInventoryManager.AllInventory_BaseItem[TheItemSlot.TheIndex].TheThingsInSlot.Count = 0;
                    TheItemSlot.TheInventoryManager.AllInventory_BaseItem[TheItemSlot.TheIndex].TheThingsInSlot.TheBaseItem = null;
                }

            }
        }

        //////////////////////////////////////////////////////////上面为重新放置物品
        TheDrag.IsDraging = false;
        TheDrag.TheFatherSlot = null;
    }

}
