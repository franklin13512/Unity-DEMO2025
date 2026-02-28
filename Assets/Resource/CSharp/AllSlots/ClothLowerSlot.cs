using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.U2D;
using UnityEngine.U2D.Animation;
using System.IO;

public class ClothLowerSlot : FatherSlot, IPointerClickHandler
{
    public SpriteRenderer HipSprite;
    public SpriteRenderer HipSprite_Trou;
    public SpriteRenderer[] LapSprite;
    public SpriteRenderer[] LegSprite;

    public Sprite LowerBodySprite;

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

    public GetXML TheGetXML;
    // Start is called before the first frame update

    void Start()
    {
        InformOn = false;

        Inform.SetActive(false);
        TheSlots = GameObject.Find("SlotsManager").gameObject.GetComponent<Slotsss>();
        Unable = GameObject.Find("Canvas/BagBackground").gameObject.GetComponent<UnableallPanel>();

        StartCoroutine(ClothTest());    
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
        if (eventData.button == PointerEventData.InputButton.Left)
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
            else if (TheDrag.IsDraging == true && this.IsFull == false && TheDrag.TheItemSlot != null && (TheDrag.ClothType == CreateClothXML.ClothType.LowerCloth || (TheDrag.ClothType == CreateClothXML.ClothType.Skirt)))
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
        SpriteResolver TheResolver = HipSprite.gameObject.GetComponent<SpriteResolver>();
        if (TheResolver && TheResolver.GetLabel() != "Nothing")
        {
            TheDrag.ClothType = CreateClothXML.ClothType.Skirt;
        }
        else
        {
            TheDrag.ClothType = CreateClothXML.ClothType.LowerCloth;
        }

        TheDrag.UniqueCode = UniqueCode;

        TheDrag.IsDraging = true;

        //TheDrag.TheClothLowerSlot = this;

        TheDrag.ItemImage.sprite = ItemImage.sprite;

        TheDrag.HipSprite = LowerBodySprite;

        TheDrag.LapSprite = LapSprite[0].sprite;
        TheDrag.LegSprite = LegSprite[0].sprite;
    }

    public void DragFromItemSlot()
    {
        UniqueCode = TheDrag.UniqueCode;
        ItemImage.sprite = TheDrag.ItemImage.sprite;
        this.IsFull = true;

        if(TheDrag.ClothType == CreateClothXML.ClothType.Skirt)
        {
            SpriteResolver TheResolver = HipSprite.gameObject.GetComponent<SpriteResolver>();
            bool GetCloth = TheResolver.SetCategoryAndLabel("Skirt", TheDrag.UniqueCode);
            if (!GetCloth)
                Debug.Log("No such Cloth");
            else
                HipSprite_Trou.sprite = TheDrag.TheEmptySprite;
        }
        else
        {
            HipSprite_Trou.sprite = TheDrag.HipSprite;

            HipSprite.sprite = TheDrag.TheEmptySprite;
        }


        for (int i = 0; i < LapSprite.Length; i++)
        {
            LapSprite[i].sprite = TheDrag.LapSprite;
        }

        for (int i = 0; i < LegSprite.Length; i++)
        {
            LegSprite[i].sprite = TheDrag.LegSprite;
        }

        
        LowerBodySprite = TheDrag.HipSprite;

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

    IEnumerator ClothTest()
    {
        yield return new WaitForSeconds(0.3f);

        Sprite PreviewSprite;

        for (int i = 0; i < TheGetXML.TheClothArray.AllCloth.Length; i++)
        {

            if (TheGetXML.TheClothArray.AllCloth[i].Name == "CamoLower")
            {
                UniqueCode = TheGetXML.TheClothArray.AllCloth[i].Name;
                CreateClothXML.ClothType TheType = TheGetXML.TheClothArray.AllCloth[i].TheType;

                if (TheType == CreateClothXML.ClothType.LowerCloth)
                {
                    byte[] TheBytes1 = File.ReadAllBytes(TheGetXML.TheClothArray.AllCloth[i].SpritePath_Female);
                    byte[] TheBytes2 = File.ReadAllBytes(TheGetXML.TheClothArray.AllCloth[i].SpritePath_Male);

                    Texture2D PNG_UpperBodySprite_Male = new Texture2D(500, 500);
                    PNG_UpperBodySprite_Male.LoadImage(TheBytes2);
                    Texture2D PNG_UpperBodySprite_Female = new Texture2D(500, 500);
                    PNG_UpperBodySprite_Female.LoadImage(TheBytes1);

                    Rect Lap = new Rect(0, 115, 53, 105);
                    Rect Leg = new Rect(0, 0, 53, 115);
                    Rect Hip = new Rect(500, 0, -53, 40);
                    Rect Preview = new Rect(500, 500, -100, -100);
                    Vector2 Pivot = new Vector2(0.5f, 0.5f);

                    this.HipSprite_Trou.sprite = Sprite.Create(PNG_UpperBodySprite_Female, Hip, Pivot);

                    PreviewSprite = Sprite.Create(PNG_UpperBodySprite_Female, Preview, Pivot);

                    for (int im = 0; im < this.LapSprite.Length; im++)
                    {
                        this.LapSprite[im].sprite = Sprite.Create(PNG_UpperBodySprite_Female, Lap, Pivot);
                        this.LegSprite[im].sprite = Sprite.Create(PNG_UpperBodySprite_Female, Leg, Pivot);
                    }
                }
            }
        }
    }

}
