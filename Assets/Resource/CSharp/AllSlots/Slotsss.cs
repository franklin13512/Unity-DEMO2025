using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Slotsss : MonoBehaviour
{

    public GameObject Background;
    public bool BagIsOpened;

    public ItemSlot[] ItemSlot;
    public WeaponSlot[] WeaponSlots;
    public OffHandSlot[] OffHandSlots;
    public MagazineSlot[] MagazineSlots;
    public ClothSlot[] ClothSlots;
    public MeleeSlot MeleeSlots;

    public ArmorSlot ArmorSlots;
    public MainHandSlot MainHandSlots;
    public OffHandEquippedSlot OffHandEquippedSlots;

    public AudioSource OpenBagSound;
    public AudioClip TheOpenBagSound;
    public AudioClip TheCloseBagSound;

    public UnableallPanel UnablePanel;

    public InventoryDrag TheDrag;
    //服装类槽位
    public ClothUpperSlot TheClothUpperSlot;
    public ClothLowerSlot TheClothLowerSlot;
    public HelmetSlot TheHelmetSlot;
    public ShoesSlot TheShoesSlot;

    private void OnValidate()
    {
        for (int i = 0; i < ItemSlot.Length; i++)
        {
            ItemSlot[i].TheIndex = i;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        Background.gameObject.SetActive(true);
        BagIsOpened = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Bag"))
        {
            OpenBag();
        }
    }

    private void OpenBag()
    {
        if (!Background.activeSelf)
        {
            OpenBagSound.clip = TheOpenBagSound;
            OpenBagSound.Play();
            Background.SetActive(true);
            BagIsOpened = true;
        }
        else if (Background.activeSelf)
        {
            Background.SetActive(false);
            BagIsOpened = false;

            TheDrag.ClearTheDragPanel();
        }

    }
}
