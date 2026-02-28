using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GetSmallGun : MonoBehaviour
{
    public Rigidbody2D RightHandRig;
    public Rigidbody2D RightHandRig2;

    public RelativeJoint2D LeftFixedPoint;
    public Rigidbody2D TheRightHandRig;

    public GameObject RightHandPos;
    public GameObject RightHandPos2;

    private GameObject SmallGun;

    public Item TheItem;

    public BoxCollider2D HandPos;

    public PrefabList ThePrefabs;
    public OffHandEquippedSlot OffHandEquipped;

    public WatchMouse Watch;
    public bool IsForward;

    public bool Equipeed;
    public bool Turned1;
    public bool Turned2;

    public GetJson TheJson;
    public Slotsss TheSlotsss;

    public HealthManager TheHealth;

    public bool HasExcuted;

    public Player ThePlayer;

    // Start is called before the first frame update
    void Start()
    {
        ThePlayer = HandPos.transform.parent.transform.parent.GetComponent<Player>();
        IsForward = Watch.IsTurned;
        RightHandPos.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (TheHealth.Health <= 0)
        {
            DropGun();
        }

        if (Equipeed && !Turned1 && IsForward == false)
        {
            SmallGun.transform.position = HandPos.bounds.center;
            Turned1 = true;
            Turned2 = false;
        }else if (Equipeed && !Turned2 && IsForward == true)
        {
            SmallGun.transform.position = HandPos.bounds.center;
            Turned1 = true;
            Turned2 = false;
        }

    }

    public void GetGun(string ObjKey)
    {

        ThePrefabs.RangeWeaponDic.TryGetValue(ObjKey, out GameObject TheObj);
        if (TheObj == null)
        {
            return;
        }
        GameObject TempObj = Instantiate(TheObj);

        if (Watch.IsTurned == true)
        {
            TempObj.gameObject.transform.localScale = new Vector3(TempObj.transform.localScale.x, TempObj.transform.localScale.y, TempObj.transform.localScale.z);
        }
        else
        {
            TempObj.gameObject.transform.localScale = new Vector3(-TempObj.transform.localScale.x, TempObj.transform.localScale.y, TempObj.transform.localScale.z);
        }

        SmallGun = Instantiate(TempObj, HandPos.bounds.center, RightHandPos.transform.rotation);
        ThePlayer.ThingInHand = SmallGun;
        Destroy(TempObj);

        Rigidbody2D Rb2 = SmallGun.GetComponent<Rigidbody2D>();
        SmallGun.transform.position = HandPos.bounds.center;

        TheItem = SmallGun.GetComponent<Item>();
        if (TheItem.IsMainHand == false)
        {
            Item PistolC = SmallGun.GetComponent<Item>();
            PistolC.IsEquiped = true;

            DistanceJoint2D Dx = SmallGun.AddComponent<DistanceJoint2D>();
            //Fx.autoConfigureConnectedAnchor = false;
            Dx.connectedBody = RightHandRig;
            Dx.autoConfigureDistance = false;
            Dx.distance = 0;

            FixedJoint2D Fx = SmallGun.AddComponent<FixedJoint2D>();
            Fx.connectedBody = RightHandRig;


            LeftFixedPoint.enabled = true;
            LeftFixedPoint.autoConfigureOffset = false;
            LeftFixedPoint.angularOffset = 0;
            LeftFixedPoint.connectedBody = TheRightHandRig;
            LeftFixedPoint.maxForce = 10;
            LeftFixedPoint.maxTorque = 10;
            //FixedJoint2D Fx2 = RightHandPos2.gameObject.AddComponent<FixedJoint2D>();
            //Fx2.connectedBody = Rb2;
        }

        Item TheItem2 = SmallGun.gameObject.GetComponent<Item>();
        TheItem2.IsEquiped = true;


        SmallGun.transform.SetParent(RightHandPos.transform);

        RightHandPos.SetActive(true);
        Equipeed = true;


        SmallGun.transform.localScale = new Vector2(-Mathf.Abs(SmallGun.transform.localScale.x), SmallGun.transform.localScale.y);
    }

    public void GetGun(GameObject TheObj)
    {
        ItemInventorySlot TheAdd1 = TheObj.GetComponent<ItemInventorySlot>();

        if (TheObj == null)
        {
            return;
        }
        GameObject TempObj = Instantiate(TheObj);

        if (Watch.IsTurned == true)
        {
            TempObj.gameObject.transform.localScale = new Vector3(TempObj.transform.localScale.x, TempObj.transform.localScale.y, TempObj.transform.localScale.z);
        }
        else
        {
            TempObj.gameObject.transform.localScale = new Vector3(-TempObj.transform.localScale.x, TempObj.transform.localScale.y, TempObj.transform.localScale.z);
        }

        SmallGun = Instantiate(TempObj, HandPos.bounds.center, RightHandPos.transform.rotation);

        ItemInventorySlot TheAdd2 = SmallGun.gameObject.GetComponent<ItemInventorySlot>();

        if (TheAdd1 && TheAdd2 && TheAdd1.TheMagazine)
        {
            TheAdd2.TheMagazine = TheAdd1.TheMagazine;
        }

        ThePlayer.ThingInHand = SmallGun;
        Destroy(TempObj);

        ItemInventorySlot TheAdditionSlot1 = SmallGun.gameObject.GetComponent<ItemInventorySlot>();
        ItemInventorySlot TheAdditionSlot2 = TheObj.gameObject.GetComponent<ItemInventorySlot>();
        if (TheAdditionSlot1 && TheAdditionSlot2)
            TheAdditionSlot1.OriginOne = TheAdditionSlot2;

        SmallGun.gameObject.SetActive(true);

        Rigidbody2D Rb2 = SmallGun.GetComponent<Rigidbody2D>();
        SmallGun.transform.position = HandPos.bounds.center;

        TheItem = SmallGun.GetComponent<Item>();
        if (TheItem.IsMainHand == false)
        {
            Item PistolC = SmallGun.GetComponent<Item>();
            PistolC.IsEquiped = true;

            DistanceJoint2D Dx = SmallGun.AddComponent<DistanceJoint2D>();
            //Fx.autoConfigureConnectedAnchor = false;
            Dx.connectedBody = RightHandRig;
            Dx.autoConfigureDistance = false;
            Dx.distance = 0;

            FixedJoint2D Fx = SmallGun.AddComponent<FixedJoint2D>();
            Fx.connectedBody = RightHandRig;


            LeftFixedPoint.enabled = true;
            LeftFixedPoint.autoConfigureOffset = false;
            LeftFixedPoint.angularOffset = 0;
            LeftFixedPoint.connectedBody = TheRightHandRig;
            LeftFixedPoint.maxForce = 10;
            LeftFixedPoint.maxTorque = 10;
            //FixedJoint2D Fx2 = RightHandPos2.gameObject.AddComponent<FixedJoint2D>();
            //Fx2.connectedBody = Rb2;
        }

        Item TheItem2 = SmallGun.gameObject.GetComponent<Item>();
        TheItem2.IsEquiped = true;


        SmallGun.transform.SetParent(RightHandPos.transform);

        RightHandPos.SetActive(true);
        Equipeed = true;

        SmallGun.transform.localScale = new Vector2(-Mathf.Abs(SmallGun.transform.localScale.x), SmallGun.transform.localScale.y);
    }

    public void DropGun()
    {

        ThePlayer.ThingInHand = null;
        LeftFixedPoint.enabled = false;

        Destroy(SmallGun);
        RightHandPos.SetActive(false);

        Equipeed = false;
    }
}
