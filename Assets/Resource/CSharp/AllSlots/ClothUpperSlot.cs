using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.IO;

public class ClothUpperSlot : FatherSlot, IPointerClickHandler
{
    public SpriteRenderer UpperBodySprite;
    public SpriteRenderer[] UpperArmSprite;
    public SpriteRenderer[] LowerArmSprite;

    public Sprite UpperBodySpriteMale;
    public Sprite UpperBodySpriteFemale;

    public Sprite EmptySprite;

    public Image ItemImage;
    public Sprite ItemSprite;
    public bool IsFull;

    public Slotsss TheSlots;

    //public GameObject Inform;

    //public bool InformOn;

    public UnableallPanel Unable;
    //public GameObject DropAllButton;

    public InventoryDrag TheDrag;

    public HealthManager TheHealthManager;

    public Image ClassPNG;

    public GetXML TheGetXML;
    // Start is called before the first frame update
    void Start()
    {
        //InformOn = false;

        //Inform.SetActive(false);
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

    }

    public void OnPointerClick(PointerEventData eventData)
    {

        if (eventData.button == PointerEventData.InputButton.Right)
        {
        }
        else if (eventData.button == PointerEventData.InputButton.Left)
        {
            for (int i = 0; i < TheSlots.ItemSlot.Length; i++)
            {
            }

            if (TheDrag.TheItemSlot == this)
            {
                TheDrag.ClearTheDragPanel();
            }

            if (TheDrag.IsDraging == false && this.IsFull == true)
            {
                OnDrag();
            }
            else if (TheDrag.IsDraging == true && this.IsFull == false && TheDrag.TheItemSlot != null && TheDrag.ClothType == CreateClothXML.ClothType.UpperCloth)
            {
                DragFromItemSlot();
            }
        }
    }

    public void OnDrag()
    {
        TheDrag.ClothType = CreateClothXML.ClothType.UpperCloth;

        TheDrag.IsDraging = true;

        //TheDrag.TheClothUpperSlot = this;

        TheDrag.ItemImage.sprite = ItemImage.sprite;

        TheDrag.UpperBodySprite_Female = UpperBodySpriteFemale;
        TheDrag.UpperBodySprite_Male = UpperBodySpriteMale;

        TheDrag.UpperArmSprite = UpperArmSprite[0].sprite;
        TheDrag.LowerArmSprite = LowerArmSprite[0].sprite;
    }

    public void DragFromItemSlot()
    {
        {//ItemImage.sprite = TheDrag.ItemImage.sprite;

            //this.IsFull = true;

            //if (TheHealthManager.IsFemale == true)
            //{
            //    UpperBodySprite.sprite = TheDrag.UpperBodySprite_Female;
            //}
            //else
            //{
            //    UpperBodySprite.sprite = TheDrag.UpperBodySprite_Male;
            //}

            //for (int i = 0; i < UpperArmSprite.Length; i++)
            //{
            //    UpperArmSprite[i].sprite = TheDrag.UpperArmSprite;
            //}

            //for (int i = 0; i < LowerArmSprite.Length; i++)
            //{
            //    LowerArmSprite[i].sprite = TheDrag.LowerArmSprite;
            //}

            //UpperBodySpriteMale = TheDrag.UpperBodySprite_Male;
            //UpperBodySpriteFemale = TheDrag.UpperBodySprite_Female;

            //TheDrag.ClearClothSprites();
            //TheDrag.ClearItemSlot();
            //TheDrag.ClearTheDragPanel();
            //IsFull = true;
            //TheDrag.TheItemSlot.IsFull = false;
            //TheDrag.TheItemSlot = null;

            //TheDrag.ClothType = CreateClothXML.ClothType.None;
        } //·ÏÆú´úÂë

        InventoryManager TheManager = Unable.ThePlayer.gameObject.GetComponent<InventoryManager>();
        if (TheManager != null)
        {

        }
    }


    IEnumerator ClothTest()
    {
        yield return new WaitForSeconds(0.3f);

        Sprite PreviewSprite;

        for (int i = 0; i < TheGetXML.TheClothArray.AllCloth.Length; i++)
        {

            if (TheGetXML.TheClothArray.AllCloth[i].Name == "CamoUpper")
            {
                UniqueCode = TheGetXML.TheClothArray.AllCloth[i].Name;
                CreateClothXML.ClothType TheType = TheGetXML.TheClothArray.AllCloth[i].TheType;

                if (TheType == CreateClothXML.ClothType.UpperCloth)
                {
                    byte[] TheBytes1 = File.ReadAllBytes(TheGetXML.TheClothArray.AllCloth[i].SpritePath_Female);
                    byte[] TheBytes2 = File.ReadAllBytes(TheGetXML.TheClothArray.AllCloth[i].SpritePath_Male);

                    Texture2D PNG_UpperBodySprite_Male = new Texture2D(500, 500);
                    PNG_UpperBodySprite_Male.LoadImage(TheBytes2);
                    Texture2D PNG_UpperBodySprite_Female = new Texture2D(500, 500);
                    PNG_UpperBodySprite_Female.LoadImage(TheBytes1);

                    Rect MaleBody = new Rect(500, 0, -100, 175);
                    Rect FemaleBody = new Rect(500, 0, -100, 350);
                    Rect UpperArm = new Rect(0, 85, 47, 90);
                    Rect LowerArm = new Rect(0, 0, 44, 86);
                    Rect Preview = new Rect(500, 500, -100, -100);
                    Vector2 Pivot = new Vector2(0.5f, 0.5f);
                    Vector2 PivotFemale = new Vector2(0.485f, 0.73f);

                    UpperBodySprite.sprite = Sprite.Create(PNG_UpperBodySprite_Female, FemaleBody, PivotFemale);

                    for (int im = 0; im < 2; im++)
                    {
                        this.UpperArmSprite[im].sprite = Sprite.Create(PNG_UpperBodySprite_Female, UpperArm, Pivot);
                        this.LowerArmSprite[im].sprite = Sprite.Create(PNG_UpperBodySprite_Female, LowerArm, Pivot);
                    }

                    PreviewSprite = Sprite.Create(PNG_UpperBodySprite_Female, Preview, Pivot);

                }
            }
        }
    }
}
