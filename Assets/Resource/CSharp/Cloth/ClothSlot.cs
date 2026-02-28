using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ClothSlot : FatherSlot, IPointerClickHandler
{
    public CreateClothXML.ClothType TheClothType;

    public CreateClothXML.ClothType[] TypesFor_Static; 
    public Image TheDefaultSprite;
    public Image ClothPreviewSprite;

    public InventoryDrag TheDrag;

    public bool IsFull;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {

            if (TheDrag.TheClothSlot == this)
            {
                TheDrag.ClearTheDragPanel();
            }


            if (TheDrag.IsDraging == true && this.IsFull == false)
            {
                //OnDragEnd();
                TheDrag.ClearTheDragPanel();
            }
            else if (TheDrag.IsDraging == false && this.IsFull == true)
            {
                DragToItem();
            }
            else
            {
                TheDrag.DropDownItem();
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
        
    }

    public void DragToItem()
    {
        TheDrag.TheClothSlot = this;
        TheDrag.IsDraging = true;

        TheDrag.UniqueCode = UniqueCode;

        TheDrag.IsFull = IsFull;
        TheDrag.ItemImage.sprite = this.ClothPreviewSprite.sprite;
        TheDrag.ClothType = TheClothType;

    }

    //public void OnDragEnd() 
    //{
    //    ClothItem TheClothItem;

    //    CreateClothXML.ClothType WhichType;
    //    InventoryManager.ThingsInSlot TheCloth = TheDrag.FindItemInInventory();//先从物品栏里获取服装

    //    if (TheCloth != null)
    //    {
    //        TheClothItem = TheCloth.TheObj.gameObject.GetComponent<ClothItem>();
    //        WhichType = TheClothItem.TheType;
    //    }
    //    else // 判断这个东西是不是服装，否则中止
    //    {
    //        TheDrag.ClearTheDragPanel();
    //        return;
    //    }

    //    InventoryManager TheInventory = TheDrag.ThePlayer.gameObject.GetComponent<InventoryManager>();
    //    Player ThePlayer = TheDrag.ThePlayer.gameObject.GetComponent<Player>();

    //    if (TheInventory != null && ThePlayer != null)
    //    {
    //        TheClothType = WhichType;

    //        if (WhichType == CreateClothXML.ClothType.Helmet && TypesFor_Static.Contains<CreateClothXML.ClothType>(WhichType))//判断服装类别
    //        {
    //            TheInventory.Helmet.TheCloth = TheCloth.TheObj;
    //            ClothPreviewSprite.sprite = TheClothItem.PreviewSprite;

    //            TheDefaultSprite.gameObject.SetActive(false);
                
    //        }
    //        else if (WhichType == CreateClothXML.ClothType.UpperCloth && TypesFor_Static.Contains<CreateClothXML.ClothType>(WhichType))
    //        {
    //            TheInventory.Upper.TheCloth = TheCloth.TheObj;
    //            ClothPreviewSprite.sprite = TheClothItem.PreviewSprite;

    //            TheDefaultSprite.gameObject.SetActive(false);
    //        }
    //        else if (WhichType == CreateClothXML.ClothType.LowerCloth && TypesFor_Static.Contains<CreateClothXML.ClothType>(WhichType))
    //        {
    //            TheInventory.Lower.TheCloth = TheCloth.TheObj;
    //            ClothPreviewSprite.sprite = TheClothItem.PreviewSprite;

    //            TheDefaultSprite.gameObject.SetActive(false);
    //        }
    //        else if (WhichType == CreateClothXML.ClothType.Skirt && TypesFor_Static.Contains<CreateClothXML.ClothType>(WhichType))
    //        {
    //            TheInventory.Hip.TheCloth = TheCloth.TheObj;
    //            ClothPreviewSprite.sprite = TheClothItem.PreviewSprite;

    //            TheDefaultSprite.gameObject.SetActive(false);
    //        }
    //        else if (WhichType == CreateClothXML.ClothType.Stocking && TypesFor_Static.Contains<CreateClothXML.ClothType>(WhichType))
    //        {
    //            TheInventory.Stocking.TheCloth = TheCloth.TheObj;
    //            ClothPreviewSprite.sprite = TheClothItem.PreviewSprite;

    //            TheDefaultSprite.gameObject.SetActive(false);
    //        }
    //        else if (WhichType == CreateClothXML.ClothType.Socks && TypesFor_Static.Contains<CreateClothXML.ClothType>(WhichType))
    //        {
    //            TheInventory.Socks.TheCloth = TheCloth.TheObj;
    //            ClothPreviewSprite.sprite = TheClothItem.PreviewSprite;

    //            TheDefaultSprite.gameObject.SetActive(false);
    //        }
    //        else if (WhichType == CreateClothXML.ClothType.Shoes && TypesFor_Static.Contains<CreateClothXML.ClothType>(WhichType))
    //        {
    //            TheInventory.Shoes.TheCloth = TheCloth.TheObj;
    //            ClothPreviewSprite.sprite = TheClothItem.PreviewSprite;

    //            TheDefaultSprite.gameObject.SetActive(false);
    //        }
    //        else if (WhichType == CreateClothXML.ClothType.Armor && TypesFor_Static.Contains<CreateClothXML.ClothType>(WhichType))
    //        {
    //            TheInventory.Armor.TheCloth = TheCloth.TheObj;
    //            ClothPreviewSprite.sprite = TheClothItem.PreviewSprite;

    //            TheDefaultSprite.gameObject.SetActive(false);
    //        }
    //        else if (WhichType == CreateClothXML.ClothType.BackPack && TypesFor_Static.Contains<CreateClothXML.ClothType>(WhichType))
    //        {

    //        }
    //        else if (WhichType == CreateClothXML.ClothType.None && TypesFor_Static.Contains<CreateClothXML.ClothType>(WhichType))
    //        {

    //        }
    //        else
    //        {
    //            TheDrag.ClearTheDragPanel();
    //            TheDrag.ClearTheSlot();

    //            return;
    //        }

    //        IsFull = true;

    //        TheDrag.ClearItemSlot();
    //        TheCloth.TheObj.transform.SetParent(ThePlayer.TheInventoryStorage.transform);
    //        TheDrag.RemoveItemInInventory();
    //        TheDrag.ClearTheSlot();
    //    }
    //    else
    //    {
    //        TheDrag.ClearTheDragPanel();
    //        TheDrag.ClearTheSlot();

    //        return;
    //    }



    //}
}
