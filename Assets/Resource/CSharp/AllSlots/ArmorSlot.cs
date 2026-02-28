using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ArmorSlot : FatherSlot , IPointerClickHandler
{
    public SpriteRenderer ArmorSprite;

    public Sprite TheArmorSprite;

    public Sprite EmptySprite;

    public Image ItemImage;
    public Sprite ItemSprite;
    public bool IsFull;

    public Slotsss TheSlots;

    public GameObject Inform;

    public bool InformOn;

    public UnableallPanel Unable;
    public GameObject DropAllButton;

    public InventoryDrag TheDrag;

    public HealthManager TheHealthManager;

    public Image ClassPNG;
    // Start is called before the first frame update
    void Start()
    {
        InformOn = false;

        Inform.SetActive(false);
        TheSlots = GameObject.Find("SlotsManager").gameObject.GetComponent<Slotsss>();
        Unable = GameObject.Find("Canvas/BagBackground").gameObject.GetComponent<UnableallPanel>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!ItemImage.sprite.Equals(TheDrag.TheEmptySprite) && ClassPNG.gameObject.activeSelf == true)
        {
            ClassPNG.gameObject.SetActive(false);
        }
        else if (ItemImage.sprite.Equals(TheDrag.TheEmptySprite) && ClassPNG.gameObject.activeSelf == false)
        {
            ClassPNG.gameObject.SetActive(true);
        }

        if (Inform.gameObject.activeSelf == true)
        {
            if (Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(0))
            {
            }

            InformOn = false;
        }

        if (!Inform.gameObject.activeSelf)
        {
            DropAllButton.SetActive(false);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {

        if (eventData.button == PointerEventData.InputButton.Right)
        {
            if (InformOn == false && IsFull == true)
            {
                MoreInformation();
            }
        }
        else if (eventData.button == PointerEventData.InputButton.Left)
        {
            for (int i = 0; i < TheSlots.ItemSlot.Length; i++)
            {
            }

            InformOn = false;

            if (TheDrag.TheItemSlot == this)
            {
                TheDrag.ClearTheDragPanel();
            }

            if (TheDrag.IsDraging == false && this.IsFull == true)
            {
                OnDrag();
            }
            else if (TheDrag.IsDraging == true && this.IsFull == false && TheDrag.TheItemSlot != null && TheDrag.ClothType == CreateClothXML.ClothType.Armor)
            {
                DragFromItemSlot();
            }
        }
        else
        {
            Inform.SetActive(false);
            InformOn = false;
        }
    }

    public void OnDrag()
    {
        TheDrag.ClothType = CreateClothXML.ClothType.Armor;

        TheDrag.IsDraging = true;

        //TheDrag.TheArmorSlot = this;

        TheDrag.ItemImage.sprite = ItemImage.sprite;

        TheDrag.ArmorSprite = TheArmorSprite;
    }

    public void DragFromItemSlot()
    {

        ItemImage.sprite = TheDrag.ItemImage.sprite;

        this.IsFull = true;

        ArmorSprite.sprite = TheDrag.ArmorSprite;

        TheArmorSprite = TheDrag.ArmorSprite;

        TheDrag.ClearClothSprites();
        TheDrag.ClearItemSlot();
        TheDrag.ClearTheDragPanel();
        IsFull = true;
        TheDrag.TheItemSlot.IsFull = false;
        TheDrag.ClearTheSlot();

        TheDrag.ClothType = CreateClothXML.ClothType.None;
    }

    public void MoreInformation()
    {

        Inform.SetActive(true);
        InformOn = true;

    }


}
