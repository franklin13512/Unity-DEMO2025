using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class WeaponSlot : FatherSlot, IPointerClickHandler
{
    public Image ItemImage;
    public Sprite ItemSprite;
    public bool IsFull;

    public Slotsss TheSlots;

    public GameObject Inform;

    public bool InformOn;

    public CreateNewJson.TheType Types;
    public int Id;

    public MagazineSlot TheMagazineSlot;

    public UnableallPanel Unable;

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
        if (Inform.gameObject.activeSelf == true)
        {
            if (Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(0))
            {
            }

            InformOn = false;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            if (InformOn == false && IsFull == true)
            {
            }
        }
        else if (eventData.button == PointerEventData.InputButton.Left)
        {
            for (int i = 0; i < TheSlots.WeaponSlots.Length; i++)
            {
                TheSlots.WeaponSlots[i].Inform.SetActive(false);
                TheSlots.WeaponSlots[i].InformOn = false;
            }

            InformOn = false;
        }
        else
        {
            Inform.SetActive(false);
            InformOn = false;
        }
    }

}
