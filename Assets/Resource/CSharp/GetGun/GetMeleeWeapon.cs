
using UnityEngine;
using static CreateNewJson;

public class GetMeleeWeapon : MonoBehaviour
{
    public Rigidbody2D RightHandRig;

    public GameObject RightHandPos;

    private GameObject MeleeWeapon;

    public Item TheItem;

    public BoxCollider2D HandPos;

    public GameObject TheMeleeWeapon;

    public PrefabList ThePrefabs;
    public MainHandSlot MainHandEquipped;

    public WatchMouse Watch;
    public bool IsForward;

    public bool Equipeed;
    public bool Turned1;
    public bool Turned2;

    public GetJson TheJson;
    public Slotsss TheSlotsss;

    public HealthManager TheHealth;

    public string MasterTag;

    public MeleeAttackAnimation TheMeleeAnim;
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
        MasterTag = RightHandPos.gameObject.tag;

        if (TheHealth.Health <= 0)
        {
            DropGun();
        }

        if (Equipeed && !Turned1 && IsForward == false)
        {
            TheMeleeWeapon.transform.position = HandPos.bounds.center;
            Turned1 = true;
            Turned2 = false;
        }
        else if (Equipeed && !Turned2 && IsForward == true)
        {
            TheMeleeWeapon.transform.position = HandPos.bounds.center;
            Turned1 = true;
            Turned2 = false;
        }

    }

    public void GetGun(string UniqueCode)
    {
        ThePrefabs.MeleeWeaponsDic.TryGetValue(UniqueCode, out GameObject TheObj);
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

        TheMeleeWeapon = Instantiate(TempObj, HandPos.bounds.center, RightHandPos.transform.rotation);
        TheMeleeWeapon.gameObject.SetActive(true);
        ThePlayer.ThingInHand = TheMeleeWeapon;
        Destroy(TempObj);

        Rigidbody2D Rb2 = TheMeleeWeapon.GetComponent<Rigidbody2D>();
        TheMeleeWeapon.transform.position = HandPos.bounds.center;

        ///////////////////////////////////////////////////////////绑定在手上
        Item PistolC = TheMeleeWeapon.GetComponent<Item>();
        PistolC.IsEquiped = true;

        DistanceJoint2D Dx = TheMeleeWeapon.AddComponent<DistanceJoint2D>();
        //Fx.autoConfigureConnectedAnchor = false;
        Dx.connectedBody = RightHandRig;
        Dx.autoConfigureDistance = false;
        Dx.distance = 0;

        FixedJoint2D Fx = TheMeleeWeapon.AddComponent<FixedJoint2D>();
        Fx.connectedBody = RightHandRig;
        ///////////////////////////////////////////////////////////

        Item TheItem2 = TheMeleeWeapon.gameObject.GetComponent<Item>();
        TheItem2.IsEquiped = true;


        TheMeleeWeapon.transform.SetParent(RightHandPos.transform);

        RightHandPos.SetActive(true);
        Equipeed = true;

        MeleeWeapon TheME = TheMeleeWeapon.gameObject.GetComponent<MeleeWeapon>();
        if (TheME)
            TheME.TheMeleeAnim = this.TheMeleeAnim;

        TheMeleeWeapon.transform.localScale = new Vector2(-Mathf.Abs(TheMeleeWeapon.transform.localScale.x), TheMeleeWeapon.transform.localScale.y);
        BoxCollider2D TheBox = TheMeleeWeapon.gameObject.GetComponent<BoxCollider2D>();
        TheMeleeWeapon.gameObject.layer = 0;
        if (TheBox)
        {
            TheBox.excludeLayers = 1 << TheMeleeWeapon.gameObject.transform.parent.gameObject.layer;
        }
    }

    public void GetGun(GameObject TheObj)
    {
        //ThePrefabs.MeleeWeaponsDic.TryGetValue(UniqueCode, out GameObject TheObj);
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

        TheMeleeWeapon = Instantiate(TempObj, HandPos.bounds.center, RightHandPos.transform.rotation);
        TheMeleeWeapon.gameObject.SetActive(true);
        ThePlayer.ThingInHand = TheMeleeWeapon;
        Destroy(TempObj);

        Rigidbody2D Rb2 = TheMeleeWeapon.GetComponent<Rigidbody2D>();
        TheMeleeWeapon.transform.position = HandPos.bounds.center;

        ///////////////////////////////////////////////////////////绑定在手上
        Item PistolC = TheMeleeWeapon.GetComponent<Item>();
        PistolC.IsEquiped = true;

        DistanceJoint2D Dx = TheMeleeWeapon.AddComponent<DistanceJoint2D>();
        //Fx.autoConfigureConnectedAnchor = false;
        Dx.connectedBody = RightHandRig;
        Dx.autoConfigureDistance = false;
        Dx.distance = 0;

        FixedJoint2D Fx = TheMeleeWeapon.AddComponent<FixedJoint2D>();
        Fx.connectedBody = RightHandRig;
        ///////////////////////////////////////////////////////////

        Item TheItem2 = TheMeleeWeapon.gameObject.GetComponent<Item>();
        TheItem2.IsEquiped = true;


        TheMeleeWeapon.transform.SetParent(RightHandPos.transform);

        RightHandPos.SetActive(true);
        Equipeed = true;

        MeleeWeapon TheME = TheMeleeWeapon.gameObject.GetComponent<MeleeWeapon>();
        if (TheME)
            TheME.TheMeleeAnim = this.TheMeleeAnim;

        TheMeleeWeapon.transform.localScale = new Vector2(-Mathf.Abs(TheMeleeWeapon.transform.localScale.x), TheMeleeWeapon.transform.localScale.y);
        BoxCollider2D TheBox = TheMeleeWeapon.gameObject.GetComponent<BoxCollider2D>();
        TheMeleeWeapon.gameObject.layer = 0;
        if (TheBox)
        {
            TheBox.excludeLayers = 1 << TheMeleeWeapon.gameObject.transform.parent.gameObject.layer;
        }
    }

    public void DropGun()
    {
        ThePlayer.ThingInHand = null;

        Destroy(TheMeleeWeapon);
        RightHandPos.SetActive(false);

        Equipeed = false;
    }
}
