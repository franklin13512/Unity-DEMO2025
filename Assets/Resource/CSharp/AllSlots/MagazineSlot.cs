using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MagazineSlot : FatherSlot, IPointerClickHandler
{
    public string[] ProperMagazine;
    public ItemInventorySlot MTheAdditionSlot;

    [HideInInspector]
    public Slider AmmoSlider;
    public Image ItemImage;
    public Sprite ItemSprite;
    public bool IsFull;

    public Slotsss TheSlots;

    public GameObject Inform;

    public bool InformOn;

    public CreateNewJson.TheType Types;
    public int Id;

    public UnableallPanel Unable;

    public int RemainAmmo = 0;
    public int MaxAmmo = 0;

    public Text TheText;
    public int StackNumber;
    public int MaxStack;

    public bool StackOpen;

    public InventoryDrag TheDrag;

    public Weapon.BulletType TheBulletType;
    public float Damage;
    public float Damage_ST;

    //爆炸弹药效果
    public float ExplosionRadius;
    public float Maganification;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
        }
        else if (eventData.button == PointerEventData.InputButton.Left)
        {

            InformOn = false;

            if (TheDrag.TheMagazineSlot == this)
            {
                TheDrag.ClearTheDragPanel();
            }

            if (TheDrag.IsDraging == true && (this.IsFull == false || this.TheText.gameObject.activeSelf == true))
            {
                DragToMagazine();
                TheDrag.ClearTheDragPanel();
            }
            else if (TheDrag.IsDraging == true && (this.StackNumber < this.MaxStack && this.IsFull == true) && TheDrag.StackOpen == true)
            {
                //DragToMagazineForStackable();
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

    public void DragToItem()//从magazine拖到item
    {
        TheDrag.TheMagazineSlot = this;
        TheDrag.IsDraging = true;
        TheDrag.RemainAmmo = RemainAmmo;
        TheDrag.MaxAmmo = MaxAmmo;

        TheDrag.ItemImage.sprite = this.ItemImage.sprite;
    }


    public void DragToMagazine() //从item拖到magazine
    {
        for (int i = 0; i < ProperMagazine.Length; i++)
        {
            if (TheDrag.TheItemSlot.UniqueCode == ProperMagazine[i])
            {

                UniqueCode = TheDrag.TheItemSlot.UniqueCode;
                ItemImage.sprite = TheDrag.TheItemSlot.ItemImage.sprite;

                TheDrag.IsDraging = false;

                ItemInventorySlot TheAdditionSlot = TheDrag.Unable.ThePlayer.ThingInHand.gameObject.GetComponent<ItemInventorySlot>();
                InventoryManager TheInventoryManager = TheDrag.Unable.ThePlayer.gameObject.GetComponent<InventoryManager>();
                if (TheDrag.TheItemSlot.AmmoSlider.gameObject.activeSelf == true && TheAdditionSlot && TheInventoryManager)
                {
                    GameObject TheInform1 = TheDrag.FindItemInInventory().TheObj;

                    Magazine TheMagaine = TheInform1.gameObject.GetComponent<Magazine>();
                    TheAdditionSlot.TheMagazine = TheMagaine;

                    ItemInventorySlot TheAdditionSlot2 = TheAdditionSlot.OriginOne;
                    TheAdditionSlot2.TheMagazine = TheAdditionSlot.TheMagazine;//把储存的武器与手中的武器同步弹药

                    AmmoSlider.value = (float)TheAdditionSlot.TheMagazine.RemainAmmo / TheAdditionSlot.TheMagazine.MaxAmmo;
                    AmmoSlider.gameObject.SetActive(true);

                    TheDrag.RemoveItemInInventory().TheObj.transform.SetParent(MTheAdditionSlot.OriginOne.gameObject.transform);

                    TheDrag.ClearItemSlot();
                    TheDrag.TheItemSlot = null;

                    IsFull = true;
                    break;
                }else if (TheDrag.TheItemSlot.TheText.gameObject.activeSelf == true && TheAdditionSlot && TheInventoryManager)
                {
                    int Num;
                    bool HasMagazine = false;

                    if (TheAdditionSlot.TheMagazine == null)
                        HasMagazine = false;
                    else
                        HasMagazine = true;

                    GameObject TheInform1 = TheDrag.FindItemInInventory().TheObj;

                    Magazine TheMagaine = TheInform1.gameObject.GetComponent<Magazine>();
                    ItemInventorySlot TheAdditionSlot2 = TheAdditionSlot.OriginOne;

                    if (IsFull == false && TheAdditionSlot2)
                    {
                        TheAdditionSlot.TheMagazine = TheMagaine;
                        TheAdditionSlot2.TheMagazine = TheAdditionSlot.TheMagazine;//把储存的武器与手中的武器同步弹药

                        Num = TheAdditionSlot.TheMagazine.RemainAmmo;
                        TheAdditionSlot.TheMagazine.RemainAmmo = TheDrag.FindItemInInventory().Count/* TheDrag.TheItemSlot.StackNumber*/;
                        TheText.gameObject.gameObject.SetActive(true);
                    }
                    else
                    {
                        Num = TheAdditionSlot.TheMagazine.RemainAmmo;
                        TheAdditionSlot.TheMagazine.RemainAmmo += TheDrag.FindItemInInventory().Count/* TheDrag.TheItemSlot.StackNumber*/;
                        TheText.gameObject.gameObject.SetActive(true);
                    }

                    if (TheAdditionSlot.TheMagazine.RemainAmmo > TheAdditionSlot.MaxStack)
                    {
                        if (HasMagazine == false)
                        {
                            GameObject NewObj = Instantiate(TheAdditionSlot.TheMagazine.gameObject);
                            NewObj.transform.SetParent(MTheAdditionSlot.OriginOne.gameObject.transform);

                            TheAdditionSlot2.TheMagazine = NewObj.GetComponent<Magazine>();
                            TheAdditionSlot.TheMagazine = TheAdditionSlot2.TheMagazine;
                        }


                        TheAdditionSlot.TheMagazine.RemainAmmo = MTheAdditionSlot.MaxStack;
                        TheDrag.FindItemInInventory().Count -= (TheAdditionSlot.TheMagazine.RemainAmmo - Num);

                        TheDrag.TheItemSlot.TheText.text = TheDrag.FindItemInInventory().Count.ToString();

                        TheText.text = TheAdditionSlot.TheMagazine.RemainAmmo.ToString();

                        IsFull = true;


                        return;
                    }
                    else
                    {
                        if (IsFull == false)
                            TheDrag.RemoveItemInInventory().TheObj.transform.SetParent(MTheAdditionSlot.OriginOne.gameObject.transform);
                        else
                            Destroy(TheDrag.RemoveItemInInventory().TheObj);

                        TheDrag.ClearItemSlot();
                        //TheDrag.RemoveItemInInventory().TheObj.transform.SetParent(MTheAdditionSlot.OriginOne.gameObject.transform);
                    }

                    TheText.text = TheAdditionSlot.TheMagazine.RemainAmmo.ToString();

                    TheDrag.TheItemSlot = null;

                    IsFull = true;
                    break;
                }

                IsFull = true;
            }
        }

    }

    public void RecordAmmoState()
    {

    }

    public void ClearMagazineSlot()
    {
        AmmoSlider.value = default(int);
        AmmoSlider.gameObject.SetActive(false);
        RemainAmmo = 0;
        MaxAmmo = 0;

        Types = CreateNewJson.TheType.None;
        Id = default(int);
        UniqueCode = default(string);
        StackNumber = 0;
        StackOpen = false;
        TheText.gameObject.SetActive(false);

        ProperMagazine.Initialize();
        IsFull = false;
        MaxStack = default(int);
        ItemImage.sprite = TheDrag.TheEmptySprite;

        //TheItemSlot = null;
        //TheMagazineSlot = null;
    }

}


