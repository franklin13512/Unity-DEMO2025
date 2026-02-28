using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class PickUp : MonoBehaviour
{
    public bool ItemSlotFull;

    public PrefabList Prefabs;
    public WatchMouse Watch;
    public Slotsss TheSlotss;

    public MagazineSlot TheMagazine;

    private ClothItem TheClothItem;

    public GetJson TheJson;

    public AudioSource PickSound;
    public AudioClip ThePickSound;

    public UnableallPanel TheUnable;

    public InventoryManager TheInventoryManager;//管理捡到的物品

    public bool IsForward;

    // Start is called before the first frame update
    void Start()
    {
        IsForward = Watch.IsTurned;

        TheSlotss = GameObject.Find("SlotsManager").gameObject.GetComponent<Slotsss>();
        Watch = GameObject.Find("WatchMouse").gameObject.GetComponent<WatchMouse>();

        TheInventoryManager = TheUnable.ThePlayer.gameObject.GetComponent<InventoryManager>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (/*Input.GetMouseButton(0) || */Input.GetMouseButtonDown(0))
        {
            RaycastHit2D TheRay = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (TheRay.collider != null)
            {
                GameObject TheColliderObj = TheRay.collider.gameObject;
                Item TheItem = TheColliderObj.GetComponent<Item>();
                ShowUI TheShow = TheColliderObj.GetComponent<ShowUI>();
                if (TheItem != null && TheItem.IsEquiped == false)
                {
                    PickUpBaseItem(TheItem.TheBasicData, TheItem.gameObject);
                } else if (TheShow != null && !TheShow.RelatedStructure.IsOpen)
                {
                    TheShow.RelatedStructure.IsOpen = true;
                    TheShow.RelatedStructure.Interact();
                }

            }
        }
    }

    public void PickUpBaseItem_UI(CreateNewJson.BaseItem TheBaseItem, GameObject PickedItem)
    {
        Item TheItem = PickedItem.gameObject.GetComponent<Item>();
        if (TheItem.IsEquiped == false)
        {
            PickUpBaseItem(TheBaseItem, PickedItem);
        }
    }

    public void PickUpBaseItem(CreateNewJson.BaseItem TheBaseItem, GameObject PickedItem)
    {
            for(int i = 0; i < TheInventoryManager.AllInventory_BaseItem.Count; i++)
            {
                if (TheInventoryManager.AllInventory_BaseItem[i].TheThingsInSlot.TheBaseItem == null || TheInventoryManager.AllInventory_BaseItem[i].TheThingsInSlot.Count == 0)
                {

                    TheInventoryManager.AllInventory_BaseItem[i].TheThingsInSlot = new InventoryManager.ThingsInSlot(1, TheBaseItem);

                    TheInventoryManager.Capacity++;

                    TheInventoryManager.MoniterTheList();

                    //TheInventoryManager.AllInventory_BaseItem[i].TheObj = Instantiate(Resources.Load<GameObject>(TheBaseItem.ThePrefab_Path));

                    GetFirstItem(i);

                    Destroy(PickedItem);
                    return;

                }else if (TheBaseItem.Stackable && TheInventoryManager.AllInventory_BaseItem[i].TheThingsInSlot.TheBaseItem != null 
                    && TheBaseItem.UniqueCode == TheInventoryManager.AllInventory_BaseItem[i].TheThingsInSlot.TheBaseItem.UniqueCode 
                    && TheInventoryManager.AllInventory_BaseItem[i].TheThingsInSlot.Count < TheBaseItem.MaxStack && TheInventoryManager.AllInventory_BaseItem[i].TheThingsInSlot.Count > 0)
                {


                    ++TheInventoryManager.AllInventory_BaseItem[i].TheThingsInSlot.Count;

                    TheInventoryManager.MoniterTheList();

                    GetFirstItem(i);

                    Destroy(PickedItem);
                    return;
                }

            //}
        }
    }

    public void PickUpMainHandWeapon(GameObject TheObj)
    {
        //Item TheItem =  TheObj.gameObject.GetComponent<Item>();
        //for (int i = 0; i < TheSlotss.ItemSlot.Length; i++)
        //{
        //    if (TheSlotss.ItemSlot[i].IsFull == false)
        //    {
        //        GameObject Obj = Instantiate(TheObj);
        //        TheInventoryManager.AllInevntoryObj.Add(Obj);
        //        TheInventoryManager.DicOfAllPocket.Add(i, new InventoryManager.ThingsInSlot( 1, Obj) );

        //        TheSlotss.ItemSlot[i].ItemImage.sprite = TheItem.TheSprite;
        //        TheSlotss.ItemSlot[i].UniqueCode = TheItem.UniqueCode;

        //        GetFirstItem(i);

        //        Destroy(TheObj.gameObject);
        //        Obj.SetActive(false);

        //        Obj.transform.SetParent(TheUnable.ThePlayer.TheInventoryStorage.gameObject.transform);

        //        TheSlotss.ItemSlot[i].IsFull = true;
        //        return;
        //    }
        //}
    }

    public void PickUpOffHandWeapon(GameObject TheObj)
    {
        //Item TheItem = TheObj.gameObject.GetComponent<Item>();
        //for (int i = 0; i < TheSlotss.ItemSlot.Length; i++)
        //{
        //    if (TheSlotss.ItemSlot[i].IsFull == false)
        //    {
        //        GameObject Obj = Instantiate(TheObj);
        //        TheInventoryManager.AllInevntoryObj.Add(Obj);
        //        TheInventoryManager.DicOfAllPocket.Add(i, new InventoryManager.ThingsInSlot(1, Obj));

        //        TheSlotss.ItemSlot[i].ItemImage.sprite = TheItem.TheSprite;
        //        TheSlotss.ItemSlot[i].UniqueCode = TheItem.UniqueCode;

        //        GetFirstItem(i);

        //        Destroy(TheObj.gameObject);
        //        Obj.SetActive(false);

        //        Obj.transform.SetParent(TheUnable.ThePlayer.TheInventoryStorage.gameObject.transform);

        //        TheSlotss.ItemSlot[i].IsFull = true;
        //        return;
        //    }
        //}
    }

    public void PickUpMeleeWeapon(GameObject TheObj)
    {
        //Item TheItem = TheObj.gameObject.GetComponent<Item>();
        //for (int i = 0; i < TheSlotss.ItemSlot.Length; i++)
        //{
        //    if (TheSlotss.ItemSlot[i].IsFull == false)
        //    {
        //        GameObject Obj = Instantiate(TheObj);
        //        TheInventoryManager.AllInevntoryObj.Add(Obj);
        //        TheInventoryManager.DicOfAllPocket.Add(i, new InventoryManager.ThingsInSlot(1, Obj));

        //        TheSlotss.ItemSlot[i].ItemImage.sprite = TheItem.TheSprite;
        //        TheSlotss.ItemSlot[i].UniqueCode = TheItem.UniqueCode;

        //        TheSlotss.ItemSlot[i].TheStyle = Item.Style.Normal;
        //        TheSlotss.ItemSlot[i].Types = TheItem.TheBasicData.Type;


        //        GetFirstItem(i);

        //        Destroy(TheObj.gameObject);
        //        Obj.SetActive(false);

        //        Obj.transform.SetParent(TheUnable.ThePlayer.TheInventoryStorage.gameObject.transform);

        //        TheSlotss.ItemSlot[i].IsFull = true;
        //        return;
        //    }
        //}
    }

    public void PickUpAmmo(GameObject TheObj)
    {
        //Item TheItem = TheObj.gameObject.GetComponent<Item>();
        //for (int i = 0; i < TheSlotss.ItemSlot.Length; i++)
        //{
        //    if (TheSlotss.ItemSlot[i].IsFull == false || (TheItem.Stackable == true && TheItem.UniqueCode == TheSlotss.ItemSlot[i].UniqueCode && TheSlotss.TheDrag.FindItemInInventory(i).Count < TheItem.MaxStack))
        //    {

        //        TheSlotss.ItemSlot[i].ItemImage.sprite = TheItem.TheSprite;
        //        TheSlotss.ItemSlot[i].UniqueCode = TheItem.UniqueCode;

        //        TheSlotss.ItemSlot[i].TheStyle = Item.Style.Normal;
        //        TheSlotss.ItemSlot[i].Types = TheItem.AllType;
        //        GetFirstItem(i);

        //        if (TheItem.Stackable == false)
        //        {
        //            GameObject Obj = Instantiate(TheObj);
        //            TheInventoryManager.AllInevntoryObj.Add(Obj);

        //            TheSlotss.ItemSlot[i].TheText.gameObject.SetActive(false);
        //            TheSlotss.ItemSlot[i].AmmoSlider.gameObject.SetActive(true);

        //            Magazine TheMagazine = TheObj.GetComponent<Magazine>();

        //            TheSlotss.ItemSlot[i].AmmoSlider.value = (float)TheMagazine.RemainAmmo / TheMagazine.MaxAmmo;

        //            TheInventoryManager.DicOfAllPocket.Add(i, new InventoryManager.ThingsInSlot(1, Obj));

        //            Obj.transform.SetParent(TheUnable.ThePlayer.TheInventoryStorage.gameObject.transform);

        //            Destroy(TheObj.gameObject);//销毁
        //            Obj.gameObject.SetActive(false);
        //        }
        //        else
        //        {

        //            TheSlotss.ItemSlot[i].TheText.gameObject.SetActive(true);
        //            TheSlotss.ItemSlot[i].AmmoSlider.gameObject.SetActive(false);

        //            TheInventoryManager.DicOfAllPocket.TryGetValue(i, out InventoryManager.ThingsInSlot TheIform1);
        //            if (TheIform1 != null && TheItem.UniqueCode == TheSlotss.ItemSlot[i].UniqueCode && TheIform1.Count < TheItem.MaxStack && TheSlotss.ItemSlot[i].IsFull == true)
        //            {
        //                TheIform1.Count++;
        //                TheSlotss.ItemSlot[i].StackNumber++;
        //                TheSlotss.ItemSlot[i].TheText.text = TheIform1.Count.ToString();

        //                Destroy(TheObj.gameObject);

        //                return;
        //            }
        //            else
        //            {

        //                GameObject Obj = Instantiate(TheObj);
        //                TheInventoryManager.AllInevntoryObj.Add(Obj);

        //                TheSlotss.ItemSlot[i].MaxStack = TheItem.MaxStack;
        //                TheSlotss.ItemSlot[i].StackNumber++;
        //                TheInventoryManager.DicOfAllPocket.Add(i, new InventoryManager.ThingsInSlot(1, Obj));
        //                TheInventoryManager.DicOfAllPocket.TryGetValue(i, out InventoryManager.ThingsInSlot TheIform);
        //                TheSlotss.ItemSlot[i].TheText.text = TheIform.Count.ToString();

        //                Obj.transform.SetParent(TheUnable.ThePlayer.TheInventoryStorage.gameObject.transform);

        //                Obj.gameObject.SetActive(false);
        //            }

        //            Destroy(TheObj.gameObject);

        //        }

        //        Destroy(TheObj.gameObject);

        //        TheSlotss.ItemSlot[i].IsFull = true;

        //        return;
        //    }
        //}
    }

    public bool PickUpCloth(GameObject TheObj)
    {
        TheClothItem = TheObj.GetComponent<ClothItem>();
        for (int i = 0; i < TheSlotss.ItemSlot.Length; i++)
        {
            if (TheSlotss.ItemSlot[i].IsFull == false)
            {
                ////服装预览图片
                //TheSlotss.ItemSlot[i].ItemImage.sprite = TheClothItem.PreviewSprite;
                //TheSlotss.ItemSlot[i].ClothType = TheClothItem.TheType;
                //TheSlotss.ItemSlot[i].UniqueCode = TheClothItem.Name;

                ////服装图片
                //TheSlotss.ItemSlot[i].HelmetSprite = TheClothItem.HelmetSprite;

                //TheSlotss.ItemSlot[i].UpperBodySprite_Male = TheClothItem.UpperBodySprite_Male;
                //TheSlotss.ItemSlot[i].UpperBodySprite_Female = TheClothItem.UpperBodySprite_Female;
                //TheSlotss.ItemSlot[i].UpperArmSprite = TheClothItem.UpperArmSprite;
                //TheSlotss.ItemSlot[i].LowerArmSprite = TheClothItem.LowerArmSprite;

                //TheSlotss.ItemSlot[i].HipSprite = TheClothItem.HipSprite;
                //TheSlotss.ItemSlot[i].LapSprite = TheClothItem.LapSprite;
                //TheSlotss.ItemSlot[i].LegSprite = TheClothItem.LegSprite;

                //TheSlotss.ItemSlot[i].SocksSprite = TheClothItem.SocksSprite;

                //TheSlotss.ItemSlot[i].HipStockingSprite = TheClothItem.HipStockingSprite;
                //TheSlotss.ItemSlot[i].LapStockingSprite = TheClothItem.LapStockingSprite;
                //TheSlotss.ItemSlot[i].LegStockingSprite = TheClothItem.LegStockingSprite;

                //TheSlotss.ItemSlot[i].ShoesSprite = TheClothItem.ShoesSprite;

                //TheSlotss.ItemSlot[i].ArmorSprite = TheClothItem.ArmorSprite;

                //TheSlotss.ItemSlot[i].BackPackSprite = TheClothItem.BackPackSprite;

                //GetFirstItem(i);
                //TheSlotss.ItemSlot[i].IsFull = true;

                //Destroy(TheObj.gameObject);

                //return false;

                ////////////////////////////////////////////////////////////////////////////////////////////////////下面为更新后的
                //GameObject Obj = Instantiate(TheObj);
                //TheInventoryManager.AllInevntoryObj.Add(Obj);
                //TheInventoryManager.DicOfAllPocket.Add(i, new InventoryManager.ThingsInSlot(1, Obj));

                //TheSlotss.ItemSlot[i].ItemImage.sprite = TheClothItem.PreviewSprite;
                //TheSlotss.ItemSlot[i].UniqueCode = TheClothItem.Name;

                //GetFirstItem(i);

                //Destroy(TheObj.gameObject);
                //Obj.SetActive(false);

                //Obj.transform.SetParent(TheUnable.ThePlayer.TheInventoryStorage.gameObject.transform);

                //TheSlotss.ItemSlot[i].IsFull = true;
                //return false;
            }
        }

        return true;
    }

    public void GetFirstItem(int i)
    {
        if ( i == TheUnable.ItemSlotSelectedIndex)
        {
            if (TheInventoryManager.AllInventory_BaseItem[TheUnable.ItemSlotSelectedIndex].TheThingsInSlot.Count > 0)
            {
                StartCoroutine(TheUnable.EquipItem()) ;
            }
        }

    }
}
