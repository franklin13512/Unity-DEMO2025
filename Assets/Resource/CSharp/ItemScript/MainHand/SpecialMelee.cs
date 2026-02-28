using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class SpecialMelee : Weapon
{
    public GetJson GetThisItem;

    public AudioSource ShootSound;
    public AudioClip ShootSound1;

    public BoxCollider2D TheHitBox;//攻击判定碰撞盒
    public string UniqueCode;

    public WatchMouse Watch;
    public bool IsForward;

    public PickUp ThePickUp;

    public CameraMove TheCamera;

    public GameObject TheGun;
    public GameObject RightHand;
    public GameObject LeftHand;
    public Vector2 vect;

    public PrefabList ThePrefabs;

    public MatchMainHandGun TheMatch;

    public Item TheItem;

    public ParticleSystem TheSmoke;
    public float VolumeUpSpeed;

    public bool RestartUp;

    public CreateNewJson.LongRangeWeapon ThisItem;

    public ItemInventorySlot TheAdditionSlots;

    public bool Chaining;

    public BoxCollider2D HandBox;

    public void OnValidate()
    {
        TheItem = gameObject.GetComponent<Item>();
        if (TheItem != null)
        {
            TheItem.UniqueCode = UniqueCode;
        }

        TheHitBox = gameObject.GetComponent<BoxCollider2D>();
    }

    private void Awake()
    {
        TheGun = gameObject;
        TheAdditionSlots = gameObject.GetComponent<ItemInventorySlot>();
        //TheAdditionSlots.TheMagazineSlot = GameObject.Find("Canvas/Panel/Magazine00").gameObject.GetComponent<MagazineSlot>();

        TheMatch = GameObject.Find("ImportantResource/TheMatch").gameObject.GetComponent<MatchMainHandGun>();
        GetThisItem = TheMatch.GetThisItem;
        Watch = TheMatch.Watch;
        TheCamera = TheMatch.Camera;
        ThePrefabs = TheMatch.ThePrefabs;
        ThePickUp = TheMatch.ThePickUp;

        ThisItem = new CreateNewJson.LongRangeWeapon();

        for (int i = 0; i < GetThisItem.LongRangeWeapon.AllLongRangeWeapons.Length; i++)
        {
            if (GetThisItem.LongRangeWeapon.AllLongRangeWeapons[i].UniqueCode == UniqueCode)
            {
                ThisItem.UniqueCode = GetThisItem.LongRangeWeapon.AllLongRangeWeapons[i].UniqueCode;
                ThisItem.TheSprite_Path = Application.streamingAssetsPath + GetThisItem.LongRangeWeapon.AllLongRangeWeapons[i].TheSprite_Path;
                ThisItem.DamageMuti = GetThisItem.LongRangeWeapon.AllLongRangeWeapons[i].DamageMuti;
                ThisItem.AmmoCode = GetThisItem.LongRangeWeapon.AllLongRangeWeapons[i].AmmoCode;
                ThisItem.Type = GetThisItem.LongRangeWeapon.AllLongRangeWeapons[i].Type;

                break;
            }
        }
    }

    void Start()
    {


        IsForward = Watch.IsTurned;

        if (gameObject.transform.parent != null)
        {
            if (gameObject.transform.parent.parent != null)
            {
                RightHand = gameObject.transform.parent.parent.gameObject;
                BackWardControler TheBackwardController = RightHand.GetComponent<BackWardControler>();
                HandBox = gameObject.transform.parent.gameObject.GetComponent<BoxCollider2D>();
                if (TheBackwardController != null)
                {
                    LeftHand = TheBackwardController.LeftHand;
                }
            }
        }

    }

    void FixedUpdate()
    {
        if (gameObject.transform.parent != null && gameObject.transform.parent.gameObject.layer != TheHitBox.excludeLayers)
        {
            TheHitBox.excludeLayers = 1 << gameObject.transform.parent.gameObject.layer;
        }

        Shoot();
        IsForward = Watch.IsTurned;
    }

    public void Shoot()
    {



        if (TheItem.IsEquiped && Input.GetMouseButton(1) && Input.GetMouseButton(0) /*&&(TheAdditionSlots.TheMagazine != null && TheAdditionSlots.TheMagazine.RemainAmmo > 0)*/
                        && TheItem.TheInventoryManager.AllInventory_BaseItem[TheItem.CurrentIndex_ItemSlot].TheThingsInSlot.TheBaseItem.InventoryObjs.Length > 0
            && TheItem.TheInventoryManager.AllInventory_BaseItem[TheItem.CurrentIndex_ItemSlot].TheThingsInSlot.TheBaseItem.InventoryObjs[0].TheCount > 0
            && ((TheItem.TheInventoryManager.AllInventory_BaseItem[TheItem.CurrentIndex_ItemSlot].TheThingsInSlot.TheBaseItem.InventoryObjs[0].TheItemData as CreateNewJson.Ammo).RemainMagazine > 0 || TheItem.TheInventoryManager.AllInventory_BaseItem[TheItem.CurrentIndex_ItemSlot].TheThingsInSlot.TheBaseItem.InventoryObjs[0].TheItemData.Stackable == true))
        {

            if (RestartUp == false)
            {
                ShootSound.Play();
                ShootSound.volume = 0;
                RestartUp = true;
            }

            if (TheSmoke != null && !TheSmoke.isPlaying)
            {
                TheSmoke.Play();
            }

            if (ShootSound.volume < 1)
            {
                ShootSound.volume += VolumeUpSpeed * Time.deltaTime;
            }

            ChangeSliderValue();

            StartChain();
            //TheMagazine.RemainAmmo--;
            //ChangeSliderValue();

            //TheCamera.ShackTime = 0.01f;
            //TheCamera.Shacking = 0.03f;
            //TheCamera.ShakingFunc();
        }
        else if (TheItem.IsEquiped && (Input.GetMouseButtonUp(1) || Input.GetMouseButtonUp(0)) /*&& (TheAdditionSlots.TheMagazine == null || TheAdditionSlots.TheMagazine.RemainAmmo > 0)*/
            && TheItem.TheInventoryManager.AllInventory_BaseItem[TheItem.CurrentIndex_ItemSlot].TheThingsInSlot.TheBaseItem.InventoryObjs.Length > 0
            && TheItem.TheInventoryManager.AllInventory_BaseItem[TheItem.CurrentIndex_ItemSlot].TheThingsInSlot.TheBaseItem.InventoryObjs[0].TheCount > 0
            && ((TheItem.TheInventoryManager.AllInventory_BaseItem[TheItem.CurrentIndex_ItemSlot].TheThingsInSlot.TheBaseItem.InventoryObjs[0].TheItemData as CreateNewJson.Ammo).RemainMagazine > 0 || TheItem.TheInventoryManager.AllInventory_BaseItem[TheItem.CurrentIndex_ItemSlot].TheThingsInSlot.TheBaseItem.InventoryObjs[0].TheItemData.Stackable == true))
        {

            RestartUp = false;

            if (ShootSound.volume > 0)
            {
                ShootSound.volume -= VolumeUpSpeed * Time.deltaTime;
            }

            if (TheSmoke != null && TheSmoke.isPlaying)
            {
                TheSmoke.Stop();
            }

            TheHitBox.enabled = true;
            TheHitBox.isTrigger = false;
            gameObject.layer = LayerMask.NameToLayer("Weapon");

            Chaining = false;
        }
        else
        {
            RestartUp = false;

            if (ShootSound.volume > 0)
            {
                ShootSound.volume -= VolumeUpSpeed * Time.deltaTime;
            }

            if (TheSmoke != null && TheSmoke.isPlaying)
            {
                TheSmoke.Stop();
            }

            if (ShootSound.volume <= 0)
            {
                ShootSound.Stop();
                ShootSound.volume = 0;
            }
            TheHitBox.enabled = true;
            TheHitBox.isTrigger = false;
            gameObject.layer = LayerMask.NameToLayer("Weapon");

            Chaining = false;
        }

        if (HandBox)
        {
            TheGun.transform.position = HandBox.bounds.center;
        }

    }

    public void StartChain()
    {
        gameObject.layer = LayerMask.NameToLayer("MeleeWeapon");
        //TheHitBox.isTrigger = true;
        TheHitBox.enabled = false;
        TheHitBox.enabled = true;

        TheCamera.ShackTime = 0.01f;
        TheCamera.Shacking = 0.01f;
        TheCamera.ShakingFunc();

        Chaining = true;
    }


    public void ChangeSliderValue()
    {
        if (TheItem.TheInventoryManager.AllInventory_BaseItem[TheItem.CurrentIndex_ItemSlot].TheThingsInSlot.TheBaseItem.InventoryObjs[0].TheItemData.Stackable == false)
        {
            CreateNewJson.Ammo TheAmmoBase =
                TheItem.TheInventoryManager.AllInventory_BaseItem[TheItem.CurrentIndex_ItemSlot].TheThingsInSlot.TheBaseItem.InventoryObjs[0].TheItemData as CreateNewJson.Ammo;

            TheAmmoBase.RemainMagazine--;

            //CreateNewJson.Ammo TheAmmoBase2 = TheItem.TheBasicData.InventoryObjs[0].TheItemData as CreateNewJson.Ammo;

            //TheAmmoBase2.RemainMagazine--;

            TheAdditionSlots.TheMagazineSlot.AmmoSlider.value = (float)TheAmmoBase.RemainMagazine / TheAmmoBase.MaxMagazine;
        }
        else if (TheItem.TheInventoryManager.AllInventory_BaseItem[TheItem.CurrentIndex_ItemSlot].TheThingsInSlot.TheBaseItem.InventoryObjs[0].TheItemData.Stackable == true)
        {
            CreateNewJson.Ammo TheAmmoBase =
                TheItem.TheInventoryManager.AllInventory_BaseItem[TheItem.CurrentIndex_ItemSlot].TheThingsInSlot.TheBaseItem.InventoryObjs[0].TheItemData as CreateNewJson.Ammo;

            TheItem.TheInventoryManager.AllInventory_BaseItem[TheItem.CurrentIndex_ItemSlot].TheThingsInSlot.TheBaseItem.InventoryObjs[0].TheCount--;

            TheAdditionSlots.TheMagazineSlot.TheText.text = TheItem.TheInventoryManager.AllInventory_BaseItem[TheItem.CurrentIndex_ItemSlot].TheThingsInSlot.TheBaseItem.InventoryObjs[0].TheCount.ToString();

            if (TheItem.TheInventoryManager.AllInventory_BaseItem[TheItem.CurrentIndex_ItemSlot].TheThingsInSlot.TheBaseItem.InventoryObjs[0].TheCount <= 0)
            {
                if (TheAdditionSlots.TheEmptySprite != null)
                    TheAdditionSlots.TheMagazineSlot.ItemImage.sprite = TheAdditionSlots.TheEmptySprite;
                else
                    TheAdditionSlots.TheMagazineSlot.ItemImage.sprite = null;

                TheAdditionSlots.TheMagazineSlot.TheText.gameObject.SetActive(false);
                TheAdditionSlots.TheMagazineSlot.IsFull = false;
            }

        }
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if(Chaining == true)
        {
            TakeDamage TheTakeDamage = collision.gameObject.GetComponent<TakeDamage>();
            Structure TheStructure = collision.gameObject.GetComponent<Structure>();

            float RandomPitch = UnityEngine.Random.Range(0.5f, 1f);
            ShootSound.pitch = RandomPitch;
            ShootSound.Play();

            if (TheTakeDamage != null)
            {
                StatusManager TheStatuseManager = TheTakeDamage.TheStatuseManager;
                TheTakeDamage.TakingDamage(ThisItem.DamageMuti);

                if (UnityEngine.Random.value < 50 * 0.01f) //触发流血机制并生成血液
                {

                    TheTakeDamage.TheStatuseManager.BleedingTime += 5f;
                    TheTakeDamage.TheStatuseManager.BleedingSeverity += 1;
                    TheTakeDamage.TheStatuseManager.StartBleeding = true;

                    foreach (ContactPoint2D contact in collision.contacts)
                    {
                        GameObject TheObj = Instantiate(ThePrefabs.BloodPartical.gameObject, contact.point, Quaternion.identity);

                        TheObj.transform.SetParent(collision.gameObject.transform);

                        TheObj.transform.localScale = new Vector2(1, 1);
                        TheObj.gameObject.SetActive(true);

                        BloodDirt TheBlood = TheObj.GetComponent<BloodDirt>();
                    }

                }
            }

            if (TheStructure != null)
            {
                TheStructure.TakeDamage_ST(ThisItem.DamageMuti * 0.1f);
            }







        }


    }
}
