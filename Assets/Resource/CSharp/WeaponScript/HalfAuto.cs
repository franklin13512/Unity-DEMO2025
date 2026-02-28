using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

//public class HalfAuto : MonoBehaviour
//{
//    public MainHandSlot TheMainHandSlot;
//    public Item TheItem;
//    public GetJson GetThisItem;

//    public AudioSource GunSound;
//    public AudioSource LightSound;
//    public AudioClip ShootSound;
//    public AudioClip OutOfAmmo;
//    public AudioClip TurnOnLightSound;

//    public MagazineSlot TheMagazine;

//    [Serializable]
//    public class OldHalfAuto : CreateNewJson.LongRangeWeapon
//    {

//    }

//    public OldHalfAuto ThisItem;

//    public bool IsEquipped;

//    public GameObject Bullet;
//    public GameObject Shell;
//    public Rigidbody2D BulletRb;
//    public Rigidbody2D ShellRb;

//    public GameObject ShootPos;
//    public GameObject ChamberPos;
//    public Light2D FireLight;

//    public float BulletSpeed;
//    public float ChamberSpeed;
//    public float BackwardForce;

//    public WatchMouse Watch;
//    public bool IsForward;

//    public Light2D FlashLight;
//    public bool LightTurned;

//    public bool FirePermision;

//    public CameraMove TheCamera;

//    public float BackWardForce;
//    public GameObject TheGun;
//    public GameObject RightHand;
//    public GameObject LeftHand;
//    public Vector2 vect;

//    public PrefabList ThePrefabs;

//    public MatchMainHandGun TheMatch;

//    //private void Awake()
//    //{
//    //    TheMatch = GameObject.Find("ImportantResource/TheMatch").gameObject.GetComponent<MatchMainHandGun>();
//    //    GetThisItem = TheMatch.GetThisItem;
//    //    TheMainHandSlot = TheMatch.TheMainHandSlot;
//    //    TheMagazine = TheMatch.TheMagazine;
//    //    Watch = TheMatch.Watch;
//    //    TheCamera = TheMatch.Camera;
//    //    ThePrefabs = TheMatch.ThePrefabs;

//    //    ThisItem = new OldHalfAuto();

//    //    for (int i = 0; i < GetThisItem.LongRangeWeapon.AllLongRangeWeapons.Length; i++)
//    //    {
//    //        if (GetThisItem.LongRangeWeapon.AllLongRangeWeapons[i].UniqueCode == "_HalfAutoRifle")
//    //        {
//    //            ThisItem.UniqueCode = GetThisItem.LongRangeWeapon.AllLongRangeWeapons[i].UniqueCode;
//    //            //ThisItem.TheSprite = gameObject.GetComponent<SpriteRenderer>().sprite;
//    //            ThisItem.Damage = GetThisItem.LongRangeWeapon.AllLongRangeWeapons[i].Damage;
//    //            ThisItem.AmmoTypeId = GetThisItem.LongRangeWeapon.AllLongRangeWeapons[i].AmmoTypeId;
//    //            ThisItem.MaxMagazine = GetThisItem.LongRangeWeapon.AllLongRangeWeapons[i].MaxMagazine;
//    //            ThisItem.Type = GetThisItem.LongRangeWeapon.AllLongRangeWeapons[i].Type;
//    //            ThisItem.ShootSpeed = GetThisItem.LongRangeWeapon.AllLongRangeWeapons[i].ShootSpeed;

//    //            break;
//    //        }
//    //    }
//    //    TheItem.AllType = CreateNewJson.TheType.LongRangeWeapon;
//    //    //TheItem.TheSprite = ThisItem.TheSprite;

//    //    TheItem.IsMainHand = true;
//    //    TheItem.TheMagazine = TheMagazine;

//    //    for (int i = 0; i < ThePrefabs.AllMagazine.Length; i++)
//    //    {
//    //        Item TheItem1 = ThePrefabs.AllMagazine[i].gameObject.GetComponent<Item>();
//    //        if (TheItem1.UniqueCode == "_HalfAutoAmmo")
//    //        {
//    //            ThisItem.AmmoTypeId = TheItem1.Id;
//    //            ThisItem.MaxMagazine = TheItem1.RemainAmmo;
//    //        }
//    //    }

//    //}
//    // Start is called before the first frame update
//    void Start()
//    {
//        GunSound = gameObject.AddComponent<AudioSource>();
//        TheItem = gameObject.GetComponent<Item>();

//        FirePermision = true;

//        IsForward = Watch.IsTurned;


//        if (gameObject.transform.parent != null)
//        {
//            if (gameObject.transform.parent.parent != null)
//            {
//                RightHand = gameObject.transform.parent.parent.gameObject;
//                BackWardControler TheBackwardController = RightHand.GetComponent<BackWardControler>();
//                if (TheBackwardController != null)
//                {
//                    LeftHand = TheBackwardController.LeftHand;
//                }
//            }
//        }
//    }

//    void Update()
//    {
//        TheMagazine = TheItem.TheMagazine;
//        //ThisItem.MaxMagazine = TheMagazine.RemainAmmo;

//        IsForward = Watch.IsTurned;

//        IsEquipped = TheItem.IsEquiped;

//        if (IsEquipped && Input.GetMouseButton(1) && Input.GetMouseButtonDown(0) && FirePermision == true && ThisItem.MaxMagazine > 0)
//        {
//            StartCoroutine(WaitForNextShoot());
//            TheMagazine.RemainAmmo--;
//            ChangeSliderValue();

//        }
//        else if (IsEquipped && Input.GetMouseButton(1) && Input.GetMouseButtonDown(0) && FirePermision == true && ThisItem.MaxMagazine <= 0)
//        {
//            StartCoroutine(WaitForNextSound());
//        }

//        if (IsEquipped && LightTurned == false && Input.GetButtonDown("FlashLight"))
//        {
//            LightTurned = true;
//            FlashLight.gameObject.SetActive(true);
//            LightSound.clip = TurnOnLightSound;
//            LightSound.Play();
//        }
//        else if (IsEquipped && LightTurned == true && Input.GetButtonDown("FlashLight"))
//        {
//            LightTurned = false;
//            FlashLight.gameObject.SetActive(false);
//            LightSound.clip = TurnOnLightSound;
//            LightSound.Play();
//        }
//    }

//    public void Shoot()
//    {
//        FirePermision = false;

//        Vector2 ShootPosition = ShootPos.transform.position;
//        Vector2 ChamberPosition = ChamberPos.transform.position;

//        float ShootY = ShootPosition.y;

//        GameObject CBullet = Instantiate(Bullet, ShootPosition, transform.rotation);
//        GameObject CShell = Instantiate(Shell, ChamberPosition, transform.rotation);

//        BulletRb = CBullet.gameObject.GetComponent<Rigidbody2D>();
//        ShellRb = CShell.gameObject.GetComponent<Rigidbody2D>();

//        if (IsForward == false)
//        {
//            BulletRb.velocity = CBullet.transform.right * -BulletSpeed;
//            ShellRb.velocity = CShell.transform.up * ChamberSpeed;

//            Vector3 BackwardDirection = TheGun.transform.right;
//            Vector2 BackwardPos = RightHand.transform.position + BackwardDirection * BackWardForce;
//            Vector2 NewPosRight = Vector2.SmoothDamp(RightHand.transform.position, BackwardPos, ref vect, 0.2f);
//            Vector2 NewPosLeft = Vector2.SmoothDamp(LeftHand.transform.position, BackwardPos, ref vect, 0.2f);
//            RightHand.transform.position = NewPosRight;
//            LeftHand.transform.position = NewPosLeft;
//        }
//        else if (IsForward == true)
//        {
//            BulletRb.velocity = CBullet.transform.right * BulletSpeed;
//            ShellRb.velocity = CShell.transform.up * ChamberSpeed;

//            Vector3 BackwardDirection = -TheGun.transform.right;
//            Vector2 BackwardPos = RightHand.transform.position + BackwardDirection * BackWardForce;
//            Vector2 NewPosRight = Vector2.SmoothDamp(RightHand.transform.position, BackwardPos, ref vect, 0.2f);
//            Vector2 NewPosLeft = Vector2.SmoothDamp(LeftHand.transform.position, BackwardPos, ref vect, 0.2f);
//            RightHand.transform.position = NewPosRight;
//            LeftHand.transform.position = NewPosLeft;

//        }

//        TheCamera.ShackTime = 0.06f;
//        TheCamera.Shacking = 0.1f;
//        TheCamera.ShakingFunc();

//        Destroy(CBullet, 10);
//        Destroy(CShell, 5);
//    }

//    IEnumerator TurnOnLight()
//    {
//        yield return new WaitForSeconds(1 / 10);
//        FireLight.gameObject.SetActive(false);
//    }

//    IEnumerator WaitForNextShoot()
//    {
//        FireLight.gameObject.SetActive(true);

//        GunSound.clip = ShootSound;
//        GunSound.Play();

//        Shoot();
//        StartCoroutine(TurnOnLight());
//        yield return new WaitForSeconds(0.95f);
//        FirePermision = true;

//    }

//    IEnumerator WaitForNextSound()
//    {
//        FirePermision = false;

//        GunSound.clip = OutOfAmmo;
//        GunSound.Play();

//        yield return new WaitForSeconds(0.7f);
//        FirePermision = true;
//    }

//    public void ChangeSliderValue()
//    {
//        TheMagazine.AmmoSlider.value = (float)TheMagazine.RemainAmmo / TheMagazine.MaxAmmo;
//    }
//}
