using System;
using System.Collections;
using UnityEngine;

//public class LaserCannonCs : MonoBehaviour
//{
//    public MainHandSlot MainHandEquipped;
//    public Item TheItem;
//    public GetJson GetThisItem;

//    public void OnValidate()
//    {
//        TheItem.UniqueCode = "_LaserCannon";
//        TheItem.AllType = CreateNewJson.TheType.LongRangeWeapon;
//        TheItem.IsMainHand = true;
//    }

//    public AudioSource GunSound;
//    public AudioClip ShootSound;
//    public AudioClip OutOfAmmo;

//    public MagazineSlot TheMagazine;

//    [Serializable]
//    public class OldLaserCannon : CreateNewJson.LongRangeWeapon
//    {

//    }

//    public OldLaserCannon ThisItem;

//    public bool IsEquipped;

//    public Rigidbody2D PistolRb;

//    public GameObject ShootPos;
//    public GameObject Pistol;

//    public float BackwardForce;

//    public WatchMouse Watch;
//    public bool IsForward;

//    public bool FirePermision;

//    public CameraMove TheCamera;

//    public float BackWardForce;
//    public GameObject TheGun;
//    public GameObject RightHand;
//    public GameObject LeftHand;
//    public Vector2 vect;

//    public MatchMainHandGun TheMatch;

//    public PrefabList ThePrefabs;

//    public LineRenderer TheLaserLine;

//    public GameObject EndFireLight;
//    //记录蓄力时间
//    public float FireTimerStart;
//    public float FireTimerDuration;
//    public bool IsTiming;
//    public bool StartTiming;

//    public AudioSource RechargeSound;
//    public AudioSource ExplosionSound;
//    public AudioSource RingingSource;
//    public float ExplosionRadius;
//    public float Maganification;
//    public float MaxDamage;

//    private void Awake()
//    {
//        TheMatch = GameObject.Find("ImportantResource/TheMatch").gameObject.GetComponent<MatchMainHandGun>();
//        GetThisItem = TheMatch.GetThisItem;
//        MainHandEquipped = TheMatch.TheMainHandSlot;
//        TheMagazine = TheMatch.TheMagazine;
//        Watch = TheMatch.Watch;
//        TheCamera = TheMatch.Camera;
//        ThePrefabs = TheMatch.ThePrefabs;

//        ThisItem = new OldLaserCannon();

//        for (int i = 0; i < GetThisItem.LongRangeWeapon.AllLongRangeWeapons.Length; i++)
//        {
//            if (GetThisItem.LongRangeWeapon.AllLongRangeWeapons[i].UniqueCode == "_LaserCannon")
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
//             if (TheItem1.UniqueCode == "_LaserCannonAmmo")
//            {
//                ThisItem.AmmoTypeId = TheItem1.Id;
//                ThisItem.MaxMagazine = TheItem1.RemainAmmo;
//            }
//        }

//    }
//    // Start is called before the first frame update
//    void Start()
//    {
//        GunSound = gameObject.AddComponent<AudioSource>();
//        TheItem = gameObject.GetComponent<Item>();

//        FirePermision = true;

//        IsForward = Watch.IsTurned;
//        PistolRb = gameObject.GetComponent<Rigidbody2D>();

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
//        ThisItem.MaxMagazine = TheMagazine.RemainAmmo;

//        IsForward = Watch.IsTurned;

//        IsEquipped = TheItem.IsEquiped;

//        if (IsEquipped && Input.GetMouseButton(1) && Input.GetMouseButton(0) && FirePermision == true && ThisItem.MaxMagazine > 0)
//        {
//            if (StartTiming == false)
//            {
//                RechargeSound.Play();
//                FireTimerStart = Time.time;
//                StartTiming = true;
//                IsTiming = true;
//            }

//            if (IsTiming == true && (Time.time - FireTimerStart >= FireTimerDuration))
//            {
//                StartCoroutine(WaitForNextShoot());
//                TheMagazine.RemainAmmo--;
//                ChangeSliderValue();

//                StartTiming = false;
//                IsTiming = false;
//                RechargeSound.Stop();
//            }

//        }
//        else if (IsEquipped && Input.GetMouseButton(1) && Input.GetMouseButtonDown(0) && FirePermision == true && ThisItem.MaxMagazine <= 0)
//        {
//            StartCoroutine(WaitForNextSound());
//        }else if (Input.GetMouseButtonUp(0) || Input.GetMouseButtonUp(1))
//        {
//            IsTiming = false;
//            StartTiming = false;
//            //FireTimerStart = 0;
//            RechargeSound.Stop();
//        }

//    }

//    public void Shoot()
//    {
//        FirePermision = false;

//        Vector3 ShootPosition = ShootPos.transform.position;

//        float ShootY = ShootPosition.y;


//        if (IsForward == false)
//        {
//            RaycastHit2D TheHit = Physics2D.Raycast(ShootPosition, -ShootPos.transform.right);
//            if (TheHit)
//            {
//                TheLaserLine.SetPosition(0, ShootPosition);
//                TheLaserLine.SetPosition(1, TheHit.point);
//                //PistolRb.velocity = new Vector2( 0 , BackwardForce);
//                PistolRb.velocity = Pistol.transform.up * BackwardForce;

//                GameObject TheFireLight = Instantiate(EndFireLight, TheHit.point, transform.rotation);
//                ExplosionSound.Play();
//                TheCamera.ShackTime = 0.3f;
//                TheCamera.Shacking = 0.5f;
//                TheCamera.ShakingFunc();

//                ExplosionFunc(TheFireLight);

//                Destroy(TheFireLight, 1f);

//                TakeDamage TheTakeDamage = TheHit.collider.GetComponent<TakeDamage>();
//                if (TheTakeDamage != null)
//                {
//                    TheTakeDamage.TakingDamage(ThisItem.Damage);
//                }
//            }
//            else
//            {
//                TheLaserLine.SetPosition(0, ShootPosition);
//                TheLaserLine.SetPosition(1, ShootPosition - ShootPos.transform.right * 100);
//            }

//            Vector3 BackwardDirection = TheGun.transform.right;
//            Vector2 BackwardPos = RightHand.transform.position + BackwardDirection * BackWardForce;
//            Vector2 NewPosRight = Vector2.SmoothDamp(RightHand.transform.position, BackwardPos, ref vect, 0.2f);
//            Vector2 NewPosLeft = Vector2.SmoothDamp(LeftHand.transform.position, BackwardPos, ref vect, 0.2f);
//            RightHand.transform.position = NewPosRight;
//            LeftHand.transform.position = NewPosLeft;

//        }
//        else if (IsForward == true)
//        {
//            RaycastHit2D TheHit = Physics2D.Raycast(ShootPosition, ShootPos.transform.right);
//            if (TheHit)
//            {
//                TheLaserLine.SetPosition(0, ShootPosition);
//                TheLaserLine.SetPosition(1, TheHit.point);
//                //PistolRb.velocity = new Vector2(0, BackwardForce);
//                PistolRb.velocity = Pistol.transform.up * BackwardForce;

//                GameObject TheFireLight = Instantiate(EndFireLight, TheHit.point, transform.rotation);
//                ExplosionSound.Play();
//                TheCamera.ShackTime = 0.3f;
//                TheCamera.Shacking = 0.5f;
//                TheCamera.ShakingFunc();

//                ExplosionFunc(TheFireLight);

//                Destroy(TheFireLight, 1f);

//                TakeDamage TheTakeDamage = TheHit.collider.GetComponent<TakeDamage>();
//                if (TheTakeDamage != null)
//                {
//                    TheTakeDamage.TakingDamage(ThisItem.Damage);
//                }
//            }
//            else
//            {
//                TheLaserLine.SetPosition(0, ShootPosition);
//                TheLaserLine.SetPosition(1, ShootPosition + ShootPos.transform.right * 100);
//            }

//            Vector3 BackwardDirection = -TheGun.transform.right;
//            Vector2 BackwardPos = RightHand.transform.position + BackwardDirection * BackWardForce;
//            Vector2 NewPosRight = Vector2.SmoothDamp(RightHand.transform.position, BackwardPos, ref vect, 0.2f);
//            Vector2 NewPosLeft = Vector2.SmoothDamp(LeftHand.transform.position, BackwardPos, ref vect, 0.2f);
//            RightHand.transform.position = NewPosRight;
//            LeftHand.transform.position = NewPosLeft;

//        }


//        TheCamera.ShackTime = 0.1f;
//        TheCamera.Shacking = 0.4f;
//        TheCamera.ShakingFunc();

//    }

//    IEnumerator TurnOnLight()
//    {
//        yield return new WaitForSeconds(0.1f);
//        TheLaserLine.gameObject.SetActive(false);
//    }

//    IEnumerator WaitForNextShoot()
//    {
//        TheLaserLine.gameObject.SetActive(true);

//        GunSound.clip = ShootSound;
//        GunSound.Play();

//        Shoot();
//        StartCoroutine(TurnOnLight());
//        yield return new WaitForSeconds(1f);
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

//    public void ExplosionFunc(GameObject TheBoomProject)
//    {
//        Collider2D[] TheCollider = Physics2D.OverlapCircleAll(TheBoomProject.transform.position, ExplosionRadius);

//        for (int i = 0; i < TheCollider.Length; i++)
//        {
//            Rigidbody2D TheRb = TheCollider[i].gameObject.GetComponent<Rigidbody2D>();
//            TakeDamage TheDamage = TheCollider[i].GetComponent<TakeDamage>();

//            if (TheRb != null)
//            {
//                UnityEngine.Vector2 TheDirection = TheCollider[i].transform.position - TheBoomProject.transform.position;
//                TheRb.AddForce(TheDirection.normalized * Maganification, ForceMode2D.Impulse);
//            }

//            if (TheDamage != null)
//            {
//                float TheDistance = UnityEngine.Vector2.Distance(TheBoomProject.transform.position, TheCollider[i].transform.position);

//                if (TheDamage.TheHealthManager.Exploded == false)
//                {
//                    if (TheCollider[i].gameObject.tag == "Player")
//                    {
//                        RingingSource.Play();
//                    }
//                    TheDamage.TheHealthManager.Exploded = true;
//                    TheDamage.TakingDamage((1 - (TheDistance / ExplosionRadius)) * MaxDamage);

//                    TheDamage.TheStatuseManager.Collapse();
//                    //导致内脏损伤
//                    TheDamage.TheStatuseManager.IsInternalInjures = true;
//                    TheDamage.TheStatuseManager.InternalInjures_TimeToRecover = 120f;
//                    TheDamage.TheStatuseManager.InternalInjures_TimerStart = false;
//                    TheDamage.TheStatuseManager.InternalInjures_Severity += 6 * (1 - (TheDistance / ExplosionRadius));
//                }

//                //导致烧伤(会使多个肢体同时造成烧伤)
//                TheDamage.BurnSeverity += 1.5f * (1 - (TheDistance / ExplosionRadius));

//            }
//        }
//    }
//}
