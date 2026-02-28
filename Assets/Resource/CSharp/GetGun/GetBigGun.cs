
using UnityEngine;
using UnityEngine.UIElements;

public class GetBigGun : MonoBehaviour
{
    public Rigidbody2D RightHandRig;
    public Rigidbody2D RightHandRig2;

    public RelativeJoint2D LeftFixedPoint;
    public Rigidbody2D TheRightHandRig;

    public GameObject RightHandPos;
    public GameObject RightHandPos2;

    private GameObject BigGun;

    public Item TheItem;

    public BoxCollider2D HandPos;
    public BoxCollider2D HandPos2;

    public PrefabList ThePrefabs;
    public MainHandSlot MainHandEquipped;

    public WatchMouse Watch;
    public bool IsForward;

    public bool Equipeed;
    public bool Turned1;
    public bool Turned2;

    public GetJson TheJson;
    public Slotsss TheSlotsss;

    public MagazineSlot TheMagazine;

    public HealthManager TheHealth;
    public Player ThePlayer;
    public GameObject TItem;

    public bool HasExcuted;

    // Start is called before the first frame update
    void Start()
    {
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

        if (Equipeed && !Turned1 && IsForward == false && MainHandEquipped.Types == CreateNewJson.TheType.LongRangeWeapon)
        {
            BigGun.transform.position = HandPos.bounds.center;
            Turned1 = true;
            Turned2 = false;
        }
        else if (Equipeed && !Turned2 && IsForward == true && MainHandEquipped.Types == CreateNewJson.TheType.LongRangeWeapon)
        {
            BigGun.transform.position = HandPos.bounds.center;
            Turned1 = true;
            Turned2 = false;
        }

    }

    public void GetGun(string ObjKey)
    {
        ThePrefabs.RangeWeaponDic.TryGetValue(ObjKey,out GameObject TheObj);
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

        BigGun = Instantiate(TempObj, HandPos.bounds.center, RightHandPos.transform.rotation);
        ThePlayer.ThingInHand = BigGun;
        Destroy(TempObj);

        Rigidbody2D Rb2 = BigGun.GetComponent<Rigidbody2D>();
        BigGun.transform.position = HandPos.bounds.center;

        TheItem = BigGun.GetComponent<Item>();
        if (TheItem.IsMainHand == true)
        {
            Item AssaultRifleC = BigGun.GetComponent<Item>();
            AssaultRifleC.IsEquiped = true;

            DistanceJoint2D Dx = BigGun.AddComponent<DistanceJoint2D>();
            Dx.connectedBody = RightHandRig;
            Dx.autoConfigureDistance = false;
            Dx.distance = 0;

            FixedJoint2D Fx = BigGun.AddComponent<FixedJoint2D>();
            Fx.connectedBody = RightHandRig;

            LeftFixedPoint.enabled = true;
            LeftFixedPoint.connectedBody = TheRightHandRig;
            LeftFixedPoint.maxForce = 10;
            LeftFixedPoint.maxTorque = 10;
        }

        Item TheItem2 = BigGun.gameObject.GetComponent<Item>();
        TheItem2.IsEquiped = true;


        BigGun.transform.SetParent(RightHandPos.transform);

        RightHandPos.SetActive(true);
        Equipeed = true;

        BigGun.transform.localScale = new Vector2(-Mathf.Abs(BigGun.transform.localScale.x), BigGun.transform.localScale.y);
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

            //Debug.Log(1);
        }
        else
        {

            TempObj.gameObject.transform.localScale = new Vector3(-TempObj.transform.localScale.x, TempObj.transform.localScale.y, TempObj.transform.localScale.z);
            //Debug.Log(2);
        }


        //Debug.Log(TempObj.transform.localScale.x + "     " + TempObj.transform.localScale.y + "     " + TempObj.transform.localScale.z);

        BigGun = Instantiate(TempObj, HandPos.bounds.center, RightHandPos.transform.rotation);
        ItemInventorySlot TheAdd2 = BigGun.gameObject.GetComponent<ItemInventorySlot>();

        if (TheAdd1 && TheAdd2 && TheAdd1.TheMagazine)
        {
            TheAdd2.TheMagazine = TheAdd1.TheMagazine;
        }

        ThePlayer.ThingInHand = BigGun;

        //Debug.Log("存在：" +  ThePlayer.ThingInHand.name);
        Destroy(TempObj);

        ItemInventorySlot TheAdditionSlot1 = BigGun.gameObject.GetComponent<ItemInventorySlot>();
        ItemInventorySlot TheAdditionSlot2 = TheObj.gameObject.GetComponent<ItemInventorySlot>();
        if (TheAdditionSlot1 && TheAdditionSlot2)
            TheAdditionSlot1.OriginOne = TheAdditionSlot2;
        //丢弃后重置

        BigGun.gameObject.SetActive(true);

        Rigidbody2D Rb2 = BigGun.GetComponent<Rigidbody2D>();
        BigGun.transform.position = HandPos.bounds.center;

        TheItem = BigGun.GetComponent<Item>();
        if (TheItem.IsMainHand == true)
        {
            Item AssaultRifleC = BigGun.GetComponent<Item>();
            AssaultRifleC.IsEquiped = true;

            DistanceJoint2D Dx = BigGun.AddComponent<DistanceJoint2D>();
            Dx.connectedBody = RightHandRig;
            Dx.autoConfigureDistance = false;
            Dx.distance = 0;

            FixedJoint2D Fx = BigGun.AddComponent<FixedJoint2D>();
            Fx.connectedBody = RightHandRig;

            LeftFixedPoint.enabled = true;
            LeftFixedPoint.connectedBody = TheRightHandRig;
            LeftFixedPoint.maxForce = 10;
            LeftFixedPoint.maxTorque = 10;
        }

        Item TheItem2 = BigGun.gameObject.GetComponent<Item>();
        TheItem2.IsEquiped = true;


        BigGun.transform.SetParent(RightHandPos.transform);

        RightHandPos.SetActive(true);
        Equipeed = true;

        BigGun.transform.localScale = new Vector2(-Mathf.Abs(BigGun.transform.localScale.x), BigGun.transform.localScale.y);

        //Debug.Log("存在：" + ThePlayer.ThingInHand.name);
    }

    public void GetItem(string ObjKey)
    {
        if(string.IsNullOrEmpty(ObjKey))
        { return; }

        //Debug.Log(ObjKey);

        GameObject TheObj = ThePrefabs.AllMagazineDic[ObjKey];
        GameObject TempObj = Instantiate(TheObj);

        if (Watch.IsTurned == true)
        {
            TempObj.gameObject.transform.localScale = new Vector3(TempObj.transform.localScale.x, TempObj.transform.localScale.y, TempObj.transform.localScale.z);
        }
        else
        {
            TempObj.gameObject.transform.localScale = new Vector3(-TempObj.transform.localScale.x, TempObj.transform.localScale.y, TempObj.transform.localScale.z);
        }

        TItem = Instantiate(TempObj, HandPos2.bounds.center, RightHandPos2.transform.rotation);
        ThePlayer.ThingInHand = TItem;
        Destroy(TempObj);

        Rigidbody2D Rb2 = TItem.GetComponent<Rigidbody2D>();
        TItem.transform.position = HandPos2.bounds.center;

        TheItem = TItem.GetComponent<Item>();

         Item AssaultRifleC = TItem.GetComponent<Item>();
         AssaultRifleC.IsEquiped = true;

         DistanceJoint2D Dx = TItem.AddComponent<DistanceJoint2D>();
         Dx.connectedBody = RightHandRig2;
         Dx.autoConfigureDistance = false;
         Dx.distance = 0;

         FixedJoint2D Fx = TItem.AddComponent<FixedJoint2D>();
         Fx.connectedBody = RightHandRig2;

        Item TheItem2 = TItem.gameObject.GetComponent<Item>();
        TheItem2.IsEquiped = true;


        TItem.transform.SetParent(RightHandPos2.transform);

        RightHandPos2.SetActive(true);
        Equipeed = false;
    }

    public void DropGun()
    {
        ThePlayer.ThingInHand = null;

        Destroy(BigGun);
        RightHandPos.SetActive(false);

        LeftFixedPoint.enabled = false;

        Equipeed = false;
    }

    public void DropItem()
    {
        ThePlayer.ThingInHand = null;

        Destroy(TItem);
        RightHandPos2.SetActive(false);

        Equipeed = false;
    }
}
