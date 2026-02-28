using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using UnityEditor;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Container : Structure
{

    public int Size;//容器大小

    public GameObject BackgroundPanel;
    public GameObject BackgroundPanelScene;
    public GameObject ItemSlotPrefab;

    public GameObject Canvas;

    public float TheRadius;//超出范围ui关闭

    public LayerMask PlayerLayer;

    public InventoryManager TheInventoryManager;

    public InventoryDrag TheDrag;

    // Start is called before the first frame update
    void Start()
    {
        TheInventoryManager = gameObject.GetComponent<InventoryManager>();
        TheDrag = GameObject.Find("ImportantResource/InventoryDrag").GetComponent<InventoryDrag>();

        IsOpen = false;

        if (TheInventoryManager.AllInventory_BaseItem.Count <= 0)
        {
            TheInventoryManager.AllInventory_BaseItem = Enumerable.Repeat<InventoryManager.TheSlot>(null, Size).ToList();
        }
        else
        {
            Size = TheInventoryManager.AllInventory_BaseItem.Count;

            //for(int i = 0; i < TheInventoryManager.AllInevntoryObj_Container.Count; i++)
            //{
            //    TheInventoryManager.DicOfContainer.Add(i, new InventoryManager.ThingsInSlot(TheInventoryManager.AllInevntoryObj_Container[i].Count, TheInventoryManager.AllInevntoryObj_Container[i].TheObj));
            //}
        }
    }

    public override void Interact()
    {
        //BackgroundPanel.SetActive(true);
        GameObject BackGround = Instantiate(BackgroundPanel, Canvas.transform);

        BackgroundPanelScene = BackGround;

        for (int i = 0; i < Size; i++)
        {
            GameObject TheItemSlot = Instantiate(ItemSlotPrefab);

            TheItemSlot.transform.SetParent(BackGround.transform);

            ItemSlot TheItemSlotScp = TheItemSlot.GetComponent<ItemSlot>();
            TheItemSlotScp.TheIndex = i;
            TheItemSlotScp.TheInventoryManager = gameObject.GetComponent<InventoryManager>();
            TheItemSlotScp.TheDrag = TheDrag;

            if (TheInventoryManager.AllInventory_BaseItem[i].TheThingsInSlot.Count > 0)
            {
                //if (TheInventoryManager.AllInventory_BaseItem[i].TheObj)
                //{
                //    TheItemSlotScp.IsFull = true;

                //    if (TheInventoryManager.AllInventory_BaseItem[i].TheObj.tag != "Cloth")
                //    {
                //        Item TheItem = TheInventoryManager.AllInventory_BaseItem[i].TheObj.GetComponent<Item>();
                //        TheItemSlotScp.ItemImage.sprite = TheItem.TheSprite;
                //    }
                //    else
                //    {
                //        ClothItem TheItem = TheInventoryManager.AllInventory_BaseItem[i].TheObj.GetComponent<ClothItem>();
                //        TheItemSlotScp.ItemImage.sprite = TheItem.PreviewSprite;
                //    }

                //}
            }
        }

        IsOpen = true;
    }

    // Update is called once per frame
    void Update()
    {
        Collider2D[] TheCollier = Physics2D.OverlapCircleAll(gameObject.transform.position, TheRadius, PlayerLayer);

        bool playerFound = false;

        foreach (Collider2D col in TheCollier)
        {
            if (col.CompareTag("Player"))
            {
                playerFound = true;
                break;
            }
        }

        if ((!playerFound && IsOpen)|| Input.GetKeyDown(KeyCode.Escape))
        {
            Destroy(BackgroundPanelScene);
            IsOpen = false;

            foreach (Transform child in BackgroundPanel.transform)
            {
                Destroy(child.gameObject);
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = UnityEngine.Color.red;
        Gizmos.DrawWireSphere(gameObject.transform.position, TheRadius);
    }
}
