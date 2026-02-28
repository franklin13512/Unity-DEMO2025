using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class ChargeWeapon : Weapon
{
    public GetJson GetThisItem;

    public AudioSource GunSound;
    public AudioSource LightSound;
    public AudioClip ShootSound;
    public AudioClip OutOfAmmo;
    public AudioClip TurnOnLightSound;

    public string UniqueCode;

    public GameObject Shell;
    public Rigidbody2D ShellRb;
    public GameObject Bullet;
    public Rigidbody2D BulletRb;

    public GameObject[] ShootPos;
    public GameObject ChamberPos;
    public Light2D FireLight;

    public float ChamberSpeed;

    public WatchMouse Watch;
    public bool IsForward;

    public Light2D FlashLight;
    public bool LightTurned;

    public bool FirePermision;

    public PickUp ThePickUp;

    public CameraMove TheCamera;

    public float BackWardForce;
    public GameObject TheGun;
    public GameObject RightHand;
    public GameObject LeftHand;
    public Vector2 vect;

    public PrefabList ThePrefabs;

    public MatchMainHandGun TheMatch;

    public Item TheItem;

    public CreateNewJson.LongRangeWeapon ThisItem;

    public LineRenderer TheLaserLine;
    public GameObject EndFireLight;
    public GameObject BulletEffect;

    public ItemInventorySlot TheAdditionSlots;

    public float ShootSpeed;
    public float BulletSpeed;

    //记录蓄力时间
    public float FireTimerStart;
    public float FireTimerDuration;
    public bool IsTiming;
    public bool StartTiming;

    public AudioSource RechargeSound;

    public void OnValidate()
    {
        TheItem = gameObject.GetComponent<Item>();
        if (TheItem != null)
        {
            TheItem.UniqueCode = UniqueCode;
        }
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
                ThisItem.ShootSpeed = GetThisItem.LongRangeWeapon.AllLongRangeWeapons[i].ShootSpeed;

                break;
            }
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        GunSound = gameObject.AddComponent<AudioSource>();

        FirePermision = true;

        IsForward = Watch.IsTurned;

        if (gameObject.transform.parent != null)
        {
            if (gameObject.transform.parent.parent != null)
            {
                RightHand = gameObject.transform.parent.parent.gameObject;
                BackWardControler TheBackwardController = RightHand.GetComponent<BackWardControler>();
                if (TheBackwardController != null)
                {
                    LeftHand = TheBackwardController.LeftHand;
                }
            }
        }
    }

    void FixedUpdate()
    {
        IsForward = Watch.IsTurned;

        if (TheItem.IsEquiped && Input.GetMouseButton(1) && Input.GetMouseButton(0) && FirePermision == true /*&& TheAdditionSlots.TheMagazine != null && TheAdditionSlots.TheMagazine.RemainAmmo > 0*/
             && TheItem.TheInventoryManager.AllInventory_BaseItem[TheItem.CurrentIndex_ItemSlot].TheThingsInSlot.TheBaseItem.InventoryObjs.Length > 0
            && TheItem.TheInventoryManager.AllInventory_BaseItem[TheItem.CurrentIndex_ItemSlot].TheThingsInSlot.TheBaseItem.InventoryObjs[0].TheCount > 0
            && ((TheItem.TheInventoryManager.AllInventory_BaseItem[TheItem.CurrentIndex_ItemSlot].TheThingsInSlot.TheBaseItem.InventoryObjs[0].TheItemData as CreateNewJson.Ammo).RemainMagazine > 0 || TheItem.TheInventoryManager.AllInventory_BaseItem[TheItem.CurrentIndex_ItemSlot].TheThingsInSlot.TheBaseItem.InventoryObjs[0].TheItemData.Stackable == true))
        {
            if (StartTiming == false)
            {
                RechargeSound.Play();
                FireTimerStart = Time.time;
                StartTiming = true;
                IsTiming = true;
            }

            if (IsTiming == true && (Time.time - FireTimerStart >= FireTimerDuration))
            {
                StartCoroutine(WaitForNextShoot());
                ChangeSliderValue();

                StartTiming = false;
                IsTiming = false;
                RechargeSound.Stop();
            }

        }
        else if (TheItem.IsEquiped && Input.GetMouseButton(1) && Input.GetMouseButtonDown(0) && FirePermision == true )
        {
            StartCoroutine(WaitForNextSound());
        }
        else if (Input.GetMouseButtonUp(0) || Input.GetMouseButtonUp(1))
        {
            IsTiming = false;
            StartTiming = false;
            //FireTimerStart = 0;
            RechargeSound.Stop();
        }

    }

    public void Shoot()
    {
        ChangeSliderValue();

        FirePermision = false;

        for (int i = 0; i < ShootPos.Length; i++)
        {
            Vector3 ShootPosition = ShootPos[i].transform.position;

            if (IsForward == false)
            {
                RaycastHit2D TheHit = Physics2D.Raycast(ShootPosition, -ShootPos[i].transform.right);
                if (TheHit)
                {

                    TheLaserLine.SetPosition(0, ShootPosition);
                    TheLaserLine.SetPosition(1, TheHit.point);

                    TheCamera.ShackTime = 0.3f;
                    TheCamera.Shacking = 0.5f;
                    TheCamera.ShakingFunc();

                    //调用子弹效果
                    TheAdditionSlots.TheBulletType = BulletType.Explosion;
                    TheAdditionSlots.ShootEffect(IsForward, TheHit.collider.gameObject, ShootPos[i],
                        (TheItem.TheInventoryManager.AllInventory_BaseItem[TheItem.CurrentIndex_ItemSlot].TheThingsInSlot.TheBaseItem.InventoryObjs[0].TheItemData as CreateNewJson.Ammo).Damage,
                        (TheItem.TheInventoryManager.AllInventory_BaseItem[TheItem.CurrentIndex_ItemSlot].TheThingsInSlot.TheBaseItem.InventoryObjs[0].TheItemData as CreateNewJson.Ammo).Damage * 0.1F,
                        0.1f,
                        TheHit);

                }
                else
                {
                    TheLaserLine.SetPosition(0, ShootPosition);
                    TheLaserLine.SetPosition(1, ShootPosition - ShootPos[i].transform.right * 100);
                }

                GameObject TheBulletEffect = Instantiate(BulletEffect, ShootPos[i].transform.position, ShootPos[i].transform.rotation);
                TheBulletEffect.gameObject.transform.localScale = new Vector3(-TheBulletEffect.gameObject.transform.localScale.x, TheBulletEffect.gameObject.transform.localScale.y, TheBulletEffect.gameObject.transform.localScale.z);

                TheBulletEffect.gameObject.AddComponent<DestoryDirt2>();
                DestoryDirt2 TheDestroy1 = TheBulletEffect.gameObject.GetComponent<DestoryDirt2>();
                if (!IsLaserWeapon)
                    TheDestroy1.DestroyTime = 0.4f;
                else
                    TheDestroy1.DestroyTime = 0.02f;

                Vector3 BackwardDirection = TheGun.transform.right;
                Vector2 BackwardPos = RightHand.transform.position + BackwardDirection * BackWardForce;
                Vector2 NewPosRight = Vector2.SmoothDamp(RightHand.transform.position, BackwardPos, ref vect, 0.2f);
                Vector2 NewPosLeft = Vector2.SmoothDamp(LeftHand.transform.position, BackwardPos, ref vect, 0.2f);
                RightHand.transform.position = NewPosRight;
                LeftHand.transform.position = NewPosLeft;

            }
            else if (IsForward == true)
            {
                RaycastHit2D TheHit = Physics2D.Raycast(ShootPosition, ShootPos[i].transform.right);
                if (TheHit)
                {
                    Debug.Log(TheHit.collider.gameObject.name);

                    TheLaserLine.SetPosition(0, ShootPosition);
                    TheLaserLine.SetPosition(1, TheHit.point);

                    //GameObject TheFireLight = Instantiate(EndFireLight, TheHit.point, transform.rotation);
                    TheCamera.ShackTime = 0.3f;
                    TheCamera.Shacking = 0.5f;
                    TheCamera.ShakingFunc();

                    //调用子弹效果
                    TheAdditionSlots.TheBulletType = BulletType.Explosion;
                    TheAdditionSlots.ShootEffect(IsForward, TheHit.collider.gameObject, ShootPos[i],
                        (TheItem.TheInventoryManager.AllInventory_BaseItem[TheItem.CurrentIndex_ItemSlot].TheThingsInSlot.TheBaseItem.InventoryObjs[0].TheItemData as CreateNewJson.Ammo).Damage,
                        (TheItem.TheInventoryManager.AllInventory_BaseItem[TheItem.CurrentIndex_ItemSlot].TheThingsInSlot.TheBaseItem.InventoryObjs[0].TheItemData as CreateNewJson.Ammo).Damage * 0.1F,
                        0.1f,
                        TheHit);

                }
                else
                {
                    TheLaserLine.SetPosition(0, ShootPosition);
                    TheLaserLine.SetPosition(1, ShootPosition + ShootPos[i].transform.right * 100);
                }

                GameObject TheBulletEffect = Instantiate(BulletEffect, ShootPos[i].transform.position, ShootPos[i].transform.rotation);

                TheBulletEffect.gameObject.AddComponent<DestoryDirt2>();
                DestoryDirt2 TheDestroy1 = TheBulletEffect.gameObject.GetComponent<DestoryDirt2>();
                if (!IsLaserWeapon)
                    TheDestroy1.DestroyTime = 0.4f;
                else
                    TheDestroy1.DestroyTime = 0.02f;

                Vector3 BackwardDirection = -TheGun.transform.right;
                Vector2 BackwardPos = RightHand.transform.position + BackwardDirection * BackWardForce;
                Vector2 NewPosRight = Vector2.SmoothDamp(RightHand.transform.position, BackwardPos, ref vect, 0.2f);
                Vector2 NewPosLeft = Vector2.SmoothDamp(LeftHand.transform.position, BackwardPos, ref vect, 0.2f);
                RightHand.transform.position = NewPosRight;
                LeftHand.transform.position = NewPosLeft;

            }
        }


        TheCamera.ShackTime = 0.1f;
        TheCamera.Shacking = 0.4f;
        TheCamera.ShakingFunc();

    }

    IEnumerator TurnOnLight()
    {
        yield return new WaitForSeconds(0.1f);
        TheLaserLine.gameObject.SetActive(false);
    }

    IEnumerator WaitForNextShoot()
    {

        TheLaserLine.gameObject.SetActive(true);

        GunSound.clip = ShootSound;
        GunSound.Play();

        Shoot();
        StartCoroutine(TurnOnLight());
        yield return new WaitForSeconds(1f);
        FirePermision = true;
    }

    IEnumerator WaitForNextSound()
    {
        FirePermision = false;

        GunSound.clip = OutOfAmmo;
        GunSound.Play();

        yield return new WaitForSeconds(0.7f);
        FirePermision = true;
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
}
