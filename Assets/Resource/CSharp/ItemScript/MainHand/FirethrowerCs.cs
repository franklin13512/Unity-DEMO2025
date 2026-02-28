using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.Rendering.Universal;

//public class FirethrowerCs : MonoBehaviour
//{

//    public MainHandSlot MainHandEquipped;
//    public Item TheItem;
//    public GetJson GetThisItem;

//    public AudioSource GunSound;
//    //public AudioSource GunSound2;
//    public AudioClip ShootSound;
//    public AudioClip OutOfAmmo;

//    public MagazineSlot TheMagazine;

//    [Serializable]
//    public class OldFireThrower : CreateNewJson.LongRangeWeapon
//    {

//    }

//    public OldFireThrower ThisItem;

//    public bool IsEquipped;

//    public GameObject Bullet;
//    public GameObject Bullet2;
//    public GameObject Bullet3;

//    public Rigidbody2D BulletRb;
//    public Rigidbody2D GunRb;

//    public GameObject ShootPos;
//    public Light2D FireLight;

//    public float BulletSpeed;
//    public float BackwardForce;

//    public WatchMouse Watch;
//    public bool IsForward;


//    public bool FirePermision;

//    public bool Played1;
//    public bool Played2;

//    public PickUp ThePickUp;

//    public CameraMove TheCamera;

//    public PrefabList ThePrefabs;

//    public MatchMainHandGun TheMatch;

//    //public Slotsss TheSlotsss;
//    private void Awake()
//    {
//        TheMatch = GameObject.Find("ImportantResource/TheMatch").gameObject.GetComponent<MatchMainHandGun>();
//        GetThisItem = TheMatch.GetThisItem;
//        MainHandEquipped = TheMatch.TheMainHandSlot;
//        TheMagazine = TheMatch.TheMagazine;
//        Watch = TheMatch.Watch;
//        TheCamera = TheMatch.Camera;
//        ThePrefabs = TheMatch.ThePrefabs;
//        ThePickUp = TheMatch.ThePickUp;

//        ThisItem = new OldFireThrower();

//        for (int i = 0; i < GetThisItem.LongRangeWeapon.AllLongRangeWeapons.Length; i++)
//        {
//            if (GetThisItem.LongRangeWeapon.AllLongRangeWeapons[i].UniqueCode == "_Firethrower")
//            {
//                ThisItem.UniqueCode = GetThisItem.LongRangeWeapon.AllLongRangeWeapons[i].UniqueCode;
//                ThisItem.TheSprite = gameObject.GetComponent<SpriteRenderer>().sprite;
//                ThisItem.Damage = GetThisItem.LongRangeWeapon.AllLongRangeWeapons[i].Damage;
//                ThisItem.AmmoTypeId = GetThisItem.LongRangeWeapon.AllLongRangeWeapons[i].AmmoTypeId;
//                ThisItem.MaxMagazine = GetThisItem.LongRangeWeapon.AllLongRangeWeapons[i].MaxMagazine;
//                ThisItem.Type = GetThisItem.LongRangeWeapon.AllLongRangeWeapons[i].Type;
//                ThisItem.ShootSpeed = GetThisItem.LongRangeWeapon.AllLongRangeWeapons[i].ShootSpeed;

//                break;
//            }
//        }
//        TheItem.AllType = CreateNewJson.TheType.LongRangeWeapon;
//        TheItem.TheSprite = ThisItem.TheSprite;

//        TheItem.IsMainHand = true;
//        TheItem.TheMagazine = TheMagazine;

//        for (int i = 0; i < ThePrefabs.AllMagazine.Length; i++)
//        {
//            Item TheItem1 = ThePrefabs.AllMagazine[i].gameObject.GetComponent<Item>();
//            if (TheItem1.UniqueCode == "_BigFireJar")
//            {
//                ThisItem.AmmoTypeId = TheItem1.Id;
//                ThisItem.MaxMagazine = TheItem1.RemainAmmo;
//            }
//        }

//    }
//    // Start is called before the first frame update
//    void Start()
//    {
//        Played1 = false;
//        Played2 = true;

//        GunSound = gameObject.AddComponent<AudioSource>();
//        TheItem = gameObject.GetComponent<Item>();

//        FirePermision = true;

//        IsForward = Watch.IsTurned;
//        GunRb = gameObject.GetComponent<Rigidbody2D>();

//    }

//    void Update()
//    {
//        TheMagazine = TheItem.TheMagazine;
//        ThisItem.MaxMagazine = TheMagazine.RemainAmmo;

//        IsForward = Watch.IsTurned;

//        IsEquipped = TheItem.IsEquiped;

//        if (IsEquipped && Input.GetMouseButton(1) && Input.GetMouseButton(0) && FirePermision == true && ThisItem.MaxMagazine > 0)
//        {
//            StartCoroutine(WaitForNextShoot());
//            TheMagazine.RemainAmmo--;
//            ChangeSliderValue();

//            if (!GunSound.isPlaying)
//            {
//                GunSound.clip = ShootSound;
//                GunSound.Play();
//            }

//        }
//        else if (IsEquipped && Input.GetMouseButton(1) && Input.GetMouseButtonDown(0) && FirePermision == true && ThisItem.MaxMagazine <= 0)
//        {
//            StartCoroutine(WaitForNextSound());
//        }

//    }

//    public void Shoot()
//    {
//        FirePermision = false;

//        Vector2 ShootPosition = ShootPos.transform.position;

//        float ShootY = ShootPosition.y;

//        GameObject[] AllBullet = new GameObject[] { Bullet};

//        if (IsForward == false)
//        {
//            for (int i = 0; i< AllBullet.Length; i++)
//            {
//                GameObject CBullet = Instantiate(AllBullet[i], ShootPosition, transform.rotation);

//                BulletRb = CBullet.gameObject.GetComponent<Rigidbody2D>();

//                BulletRb.velocity = CBullet.transform.right * -BulletSpeed;
//                //PistolRb.velocity = new Vector2(-BackwardForce, 0);
//                GunRb.velocity = transform.up * BackwardForce;

//                Destroy(CBullet, 10);
//            }
//        }
//        else if (IsForward == true)
//        {
//            for (int i = 0; i < AllBullet.Length; i++)
//            {
//                GameObject CBullet = Instantiate(AllBullet[i], ShootPosition, transform.rotation);

//                BulletRb = CBullet.gameObject.GetComponent<Rigidbody2D>();

//                BulletRb.velocity = CBullet.transform.right * BulletSpeed;
//                //PistolRb.velocity = new Vector2(-BackwardForce, 0);
//                GunRb.velocity = transform.up * BackwardForce;

//                Destroy(CBullet, 10);
//            }

//        }

//        TheCamera.ShackTime = 0.01f;
//        TheCamera.Shacking = 0.04f;
//        TheCamera.ShakingFunc();
//    }

//    IEnumerator TurnOnLight()
//    {
//        yield return new WaitForSeconds(1 / 10);
//        FireLight.gameObject.SetActive(false);
//    }

//    IEnumerator WaitForNextShoot()
//    {
//        FireLight.gameObject.SetActive(true);

//        Shoot();
//        StartCoroutine(TurnOnLight());
//        yield return new WaitForSeconds(0);
//        //if (Played1 == false)
//        //{
//        //    GunSound.clip = ShootSound;
//        //    GunSound.Play();

//        //    Played1 = true;
//        //    Played2 = false;
//        //}
//        //else if (Played2 == false)
//        //{
//        //    //GunSound2.clip = ShootSound;
//        //    //GunSound2.Play();

//        //    Played1 = false;
//        //    Played2 = true;
//        //}
//        //GunSound2.Play();
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
