using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.Jobs;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UnableallPanel : MonoBehaviour , IPointerClickHandler
{
    public Slotsss TheSlots;
    public PrefabList ThePrefabList;

    public int ItemSlotSelectedIndex = 0;

    public GameObject ItemInHand;

    public GetBigGun TheGetBigGun;
    public GetSmallGun TheGetSmallGun;
    public GetMeleeWeapon TheGetMeleeWeapon;

    public Player ThePlayer;

    public MagazineSlot TheMagazineSlot;

    public Sprite EmptySprite;

    public GameObject ParentPanel_MagazinePanel;
    public GameObject MagazinePanel;

    public InventoryDrag TheDrag;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        SelectItem();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
        }

    }

    public void SelectItem()
    {
        int LastIndex;

        float ScrollValue = Input.mouseScrollDelta.y;
        if (ScrollValue < 0f)
        {
            LastIndex = ItemSlotSelectedIndex;
            ItemSlotSelectedIndex++;
            if (ItemSlotSelectedIndex > TheSlots.ItemSlot.Length -1 )
            {
                ItemSlotSelectedIndex = 0;
            }

            Outline SelectedOutline = TheSlots.ItemSlot[ItemSlotSelectedIndex].gameObject.GetComponent<Outline>();
            if (SelectedOutline != null)
            {
                SelectedOutline.enabled = true;
                Outline SelectedOutline1 = TheSlots.ItemSlot[LastIndex].gameObject.GetComponent<Outline>();
                SelectedOutline1.enabled = false;
            }
            //RecordAmmo(LastIndex);
            StartCoroutine(EquipItem());

        }
        else if (ScrollValue > 0f)
        {
            LastIndex = ItemSlotSelectedIndex;
            ItemSlotSelectedIndex--;
            if (ItemSlotSelectedIndex < 0)
            {
                ItemSlotSelectedIndex = TheSlots.ItemSlot.Length - 1;
            }

            Outline SelectedOutline = TheSlots.ItemSlot[ItemSlotSelectedIndex].gameObject.GetComponent<Outline>();
            if (SelectedOutline != null)
            {
                SelectedOutline.enabled = true;
                Outline SelectedOutline1 = TheSlots.ItemSlot[LastIndex].gameObject.GetComponent<Outline>();
                SelectedOutline1.enabled = false;
            }
            //RecordAmmo(LastIndex);
            StartCoroutine(EquipItem());

        }
    }

    public IEnumerator EquipItem()
    {
        InventoryManager TheInventoryManager = ThePlayer.gameObject.GetComponent<InventoryManager>();


        InventoryManager.TheSlot slot = TheInventoryManager.AllInventory_BaseItem[ItemSlotSelectedIndex];
        if (slot == null || slot.TheThingsInSlot == null || slot.TheThingsInSlot.TheBaseItem == null)
            TheInventoryManager.AllInventory_BaseItem[ItemSlotSelectedIndex].TheThingsInSlot.TheBaseItem = new CreateNewJson.BaseItem();

        /////////////////////////////////////////////////////////////////上方代码不用管

        if (TheInventoryManager.AllInventory_BaseItem[ItemSlotSelectedIndex].TheThingsInSlot.TheBaseItem.Type == CreateNewJson.TheType.LongRangeWeapon)
        {
            DeleteAllChildren(ParentPanel_MagazinePanel);

            while (TheInventoryManager.AllInventory_BaseItem[ItemSlotSelectedIndex].TheThingsInSlot.TheObj == null)
            {
                yield return null; // 等待下一帧
            }

            if (TheInventoryManager.AllInventory_BaseItem[ItemSlotSelectedIndex].TheThingsInSlot.TheBaseItem.HandleStyle == CreateNewJson.TheHandleStyle.MainHandWeapon)
            {
                DropThing();
                TheGetBigGun.GetGun(TheInventoryManager.AllInventory_BaseItem[ItemSlotSelectedIndex].TheThingsInSlot.TheObj);
            }
            else if (TheInventoryManager.AllInventory_BaseItem[ItemSlotSelectedIndex].TheThingsInSlot.TheBaseItem.HandleStyle == CreateNewJson.TheHandleStyle.Normal)
            {
                DropThing();
                TheGetSmallGun.GetGun(TheInventoryManager.AllInventory_BaseItem[ItemSlotSelectedIndex].TheThingsInSlot.TheObj);
            }


            ShowUI(TheInventoryManager, ItemSlotSelectedIndex);
        }
        else
        {
            DeleteAllChildren(ParentPanel_MagazinePanel);
            DropThing();
        }

    }



    //public void EquipItem()
    //{

    //    InventoryManager TheInventoryManager = ThePlayer.gameObject.GetComponent<InventoryManager>();
    //    TheInventoryManager.DicOfAllPocket.TryGetValue(ItemSlotSelectedIndex, out InventoryManager.ThingsInSlot TheInform);

    //    if (TheInform != null && TheInventoryManager.DicOfAllPocket.ContainsKey(ItemSlotSelectedIndex))
    //    {
    //        Item Item1 = TheInform.TheObj.gameObject.GetComponent<Item>();
    //        if (Item1)
    //        {
    //            if (Item1.TheBasicData.Type == CreateNewJson.TheType.LongRangeWeapon)
    //            {
    //                DropThing();
    //                //TheInventoryManager.DicOfAllPocket.TryGetValue(ItemSlotSelectedIndex, out InventoryManager.ThingsInSlot TheInform);

    //                GameObject TheGun = TheInform.TheObj;

    //                if (TheGun != null)
    //                {
    //                    Weapon TheWeapon = TheGun.GetComponent<Weapon>();
    //                    if (TheWeapon != null && TheWeapon.IsMainHand == true)
    //                    {
    //                        TheGetBigGun.GetGun(TheGun);
    //                    }
    //                    else if (TheWeapon != null)
    //                    {
    //                        TheGetSmallGun.GetGun(TheGun);
    //                    }
    //                }


    //                ItemInventorySlot TheAdditionSlot = ThePlayer.ThingInHand.gameObject.GetComponent<ItemInventorySlot>();
    //                TheMagazineSlot.gameObject.SetActive(true);

    //                TheMagazineSlot.MTheAdditionSlot = TheAdditionSlot;
    //                TheMagazineSlot.ProperMagazine = TheAdditionSlot.ProperMagazine;

    //                if (TheAdditionSlot.TheMagazine == null)
    //                {
    //                    TheMagazineSlot.ClearMagazineSlot();
    //                }
    //                else
    //                {
    //                    ItemInventorySlot Theadd = TheAdditionSlot.OriginOne.gameObject.GetComponent<ItemInventorySlot>();
    //                    Magazine TheMagazine = Theadd.TheMagazine;
    //                    Item TheItem = TheMagazine.gameObject.GetComponent<Item>();

    //                    if (Theadd && TheMagazine && TheItem && TheItem.Stackable == false)//非叠加弹药
    //                    {
    //                        TheMagazineSlot.ItemImage.sprite = TheItem.TheSprite;//更新预览图
    //                        TheMagazineSlot.UniqueCode = TheItem.UniqueCode;

    //                        TheMagazineSlot.AmmoSlider.gameObject.SetActive(true);
    //                        TheMagazineSlot.TheText.gameObject.SetActive(false);

    //                        TheMagazineSlot.AmmoSlider.value = (float)TheMagazine.RemainAmmo / TheMagazine.MaxAmmo;
    //                        TheMagazineSlot.IsFull = true;

    //                    }
    //                    else if (Theadd && TheMagazine && TheItem && TheItem.Stackable == true)
    //                    {

    //                        TheMagazineSlot.ItemImage.sprite = TheItem.TheSprite;//更新预览图
    //                        TheMagazineSlot.UniqueCode = TheItem.UniqueCode;

    //                        TheMagazineSlot.TheText.gameObject.SetActive(true);
    //                        TheMagazineSlot.AmmoSlider.gameObject.SetActive(false);

    //                        TheMagazineSlot.TheText.text = TheMagazine.RemainAmmo.ToString();
    //                        TheMagazineSlot.IsFull = true;
    //                    }

    //                }

    //                return;


    //            }
    //            else if (Item1.TheBasicData.Type == CreateNewJson.TheType.Ammo)
    //            {
    //                DropThing();

    //                TheGetBigGun.GetItem(TheSlots.ItemSlot[ItemSlotSelectedIndex].UniqueCode);
    //                TheMagazineSlot.gameObject.SetActive(false);

    //            }
    //            else if (Item1.TheBasicData.Type == CreateNewJson.TheType.MeleeWeapon)
    //            {
    //                DropThing();

    //                TheInventoryManager.DicOfAllPocket.TryGetValue(ItemSlotSelectedIndex, out InventoryManager.ThingsInSlot TheGunInform);
    //                if (TheGunInform != null)
    //                {

    //                    MeleeWeapon TheWeapon = TheGunInform.TheObj.GetComponent<MeleeWeapon>();
    //                    if (TheWeapon != null)
    //                    {

    //                        TheGetMeleeWeapon.GetGun(TheGunInform.TheObj);
    //                    }
    //                }

    //                TheMagazineSlot.gameObject.SetActive(false);
    //            }
    //            else
    //            {
    //                DropThing();

    //                TheMagazineSlot.gameObject.SetActive(false);
    //            }
    //        }
    //        else
    //        {
    //            DropThing();

    //            TheMagazineSlot.gameObject.SetActive(false);
    //        }
    //    }
    //    else
    //    {
    //        DropThing();

    //        TheMagazineSlot.gameObject.SetActive(false);
    //    }

    //}

    public void ShowUI(InventoryManager The_TheInventoryManager, int The_ItemSlotSelectedIndex)
    {
        CreateNewJson.BaseItem TheBaseItem = The_TheInventoryManager.AllInventory_BaseItem[The_ItemSlotSelectedIndex].TheThingsInSlot.TheBaseItem;

        for (int i = 0; i < The_TheInventoryManager.AllInventory_BaseItem[The_ItemSlotSelectedIndex].TheThingsInSlot.TheBaseItem.InventoryObjs.Length; i++)
        {
            GameObject TheMagazineSlot = Instantiate(MagazinePanel);
            TheMagazineSlot.transform.SetParent(ParentPanel_MagazinePanel.transform);
            TheMagazineSlot.transform.localScale = new Vector3(1, 1, 1);

            ItemSlot TheItemSlot = TheMagazineSlot.GetComponent<ItemSlot>();
            TheItemSlot.TheObjData = TheBaseItem.InventoryObjs[i].TheItemData;
            TheItemSlot.TheDrag = TheDrag ;
            TheItemSlot.TheInventoryManager = The_TheInventoryManager;

            if (TheItemSlot && TheBaseItem.InventoryObjs.Length > 0) {

                for (int i2 = 0; i2 < TheBaseItem.InventoryObjs.Length; i2++) // 遍历物品的物品栏
                {
                    TheItemSlot.TheSlotType = TheBaseItem.InventoryObjs[i2].TheSlotType;

                    if (TheBaseItem.InventoryObjs[i2].TheCount <= 0) //没有东西则下一位
                        continue;

                    if (TheBaseItem.InventoryObjs[i2].TheSlotType == CreateNewJson.TheType_InSlot.ForMagazine)
                    {
                        if (TheBaseItem.InventoryObjs[i2].TheItemData.Stackable) //可叠加物品
                        {
                            TheItemSlot.TheText.gameObject.SetActive(true);
                            TheItemSlot.TheText.text = TheBaseItem.InventoryObjs[i2].TheCount.ToString();
                        }
                        else //不可叠加物品
                        {
                            if (TheBaseItem.InventoryObjs[i2].TheItemData.Type == CreateNewJson.TheType.Ammo) //当xx时露出物品使用进度条
                            {
                                TheItemSlot.AmmoSlider.gameObject.SetActive(true);

                                CreateNewJson.Ammo TheAmmo = TheBaseItem.InventoryObjs[i2].TheItemData as CreateNewJson.Ammo;
                                TheItemSlot.AmmoSlider.value = (float)TheAmmo.RemainMagazine / TheAmmo.MaxMagazine;
                            }
                        }

                        //生成物品的物品的图标

                        byte[] imageData = File.ReadAllBytes(TheBaseItem.InventoryObjs[i2].TheItemData.TheSprite_Path);
                        Texture2D texture = new Texture2D(2, 2);
                        texture.LoadImage(imageData);

                        TheItemSlot.ItemImage.sprite = ConvertWholeTextureToSprite(texture);
                    }
                    else
                    {
                        return;
                    }
                }
            }
        }

        GetItemInHand(The_TheInventoryManager);
    }

    public void DeleteAllChildren(GameObject PatherObj)
    {
        if(PatherObj.transform.childCount > 0)
        {
            foreach (Transform child in PatherObj.transform)
            {
                DestroyImmediate(child.gameObject);
            }
        }

    }

    void GetItemInHand(InventoryManager TheInventoryManager)
    {

        Item TheItem = ThePlayer.ThingInHand.gameObject.GetComponent<Item>();
        TheItem.IsEquiped = true;

        TheItem.CurrentIndex_ItemSlot = ItemSlotSelectedIndex;
        if (TheItem != null)
        {
            TheItem.TheInventoryManager = TheInventoryManager;
            int i = 0;
            foreach (Transform child in ParentPanel_MagazinePanel.transform)
            {
                ItemSlot TheItemSlot = child.gameObject.GetComponent<ItemSlot>();
                if (TheItemSlot != null)
                {
                    TheItem.CurrentIndex = i;
                    
                    ItemInventorySlot TheAddition = TheItem.gameObject.GetComponent<ItemInventorySlot>();
                    if(TheAddition != null)
                    {
                        TheAddition.TheMagazineSlot = TheItemSlot;
                    }
                }

                i++;
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

    public void DropThing()
    {
        TheGetBigGun.DropGun();
        TheGetBigGun.DropItem();
        TheGetSmallGun.DropGun();
        TheGetMeleeWeapon.DropGun();

    }

    //private void RecordAmmo(int LastIndex)
    //{
    //    if (TheMagazineSlot.IsFull == true && TheSlots.ItemSlot[LastIndex].Types == CreateNewJson.TheType.LongRangeWeapon && TheMagazineSlot.AmmoSlider.gameObject.activeSelf == true) { 

    //        TheSlots.ItemSlot[LastIndex].RecordedRemainAmmo = TheMagazineSlot.RemainAmmo;
    //        TheSlots.ItemSlot[LastIndex].RecordedMaxAmmo = TheMagazineSlot.MaxAmmo;
    //        TheSlots.ItemSlot[LastIndex].RecordedUniqueCode = TheMagazineSlot.UniqueCode;
    //        TheSlots.ItemSlot[LastIndex].StackableForMagazine = false;

    //    }
    //    else if (TheMagazineSlot.IsFull == true && TheSlots.ItemSlot[LastIndex].Types == CreateNewJson.TheType.LongRangeWeapon && TheMagazineSlot.TheText.gameObject.activeSelf == true)
    //    {
    //        TheSlots.ItemSlot[LastIndex].RecordedRemainAmmo = TheMagazineSlot.RemainAmmo;
    //        TheSlots.ItemSlot[LastIndex].RecordedMaxAmmo = TheMagazineSlot.MTheAdditionSlot.MaxStack;
    //        TheSlots.ItemSlot[LastIndex].RecordedUniqueCode = TheMagazineSlot.UniqueCode;
    //        TheSlots.ItemSlot[LastIndex].StackableForMagazine = true;
    //    }
    //    else if(TheMagazineSlot.IsFull == false && TheSlots.ItemSlot[LastIndex].Types == CreateNewJson.TheType.LongRangeWeapon && TheMagazineSlot.TheText.gameObject.activeSelf == false)
    //    {
    //        TheSlots.ItemSlot[LastIndex].RecordedRemainAmmo = 0;
    //        TheSlots.ItemSlot[LastIndex].RecordedMaxAmmo = TheMagazineSlot.MTheAdditionSlot.MaxStack;
    //        TheSlots.ItemSlot[LastIndex].RecordedUniqueCode = string.Empty;
    //        TheSlots.ItemSlot[LastIndex].StackableForMagazine = true;
    //    }
    //}
    
}
