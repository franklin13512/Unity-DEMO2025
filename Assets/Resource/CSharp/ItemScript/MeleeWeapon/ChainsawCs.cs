using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using static CreateNewJson;

//public class ChainsawCs : MonoBehaviour
//{
//    public MainHandSlot MainHandEquipped;
//    public Item TheItem;
//    public GetJson GetThisItem;

//    public void OnValidate()
//    {
//        TheItem.UniqueCode = "_Chainsaw";
//        TheItem.AllType = CreateNewJson.TheType.LongRangeWeapon;
//        TheItem.IsMainHand = true;
//    }

//    public AudioSource SawSound;
//    public AudioSource TheStrikeSound;
//    public AudioClip ChainsawSound;

//    public BoxCollider2D TheChainBox;

//    public MagazineSlot TheMagazine;

//    public GameObject BloodPartical;

//    [Serializable]
//    public class OldAssault : CreateNewJson.LongRangeWeapon
//    {

//    }

//    public OldAssault ThisItem;

//    public bool IsEquipped;

//    public WatchMouse Watch;
//    public bool IsForward;

//    public bool LightTurned;

//    public PickUp ThePickUp;

//    public CameraMove TheCamera;

//    public PrefabList ThePrefabs;

//    public MatchMainHandGun TheMatch;

//    public float VolumeUpSpeed;

//    public bool RestartUp;
//    public ParticleSystem TheSmoke;

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

//        ThisItem = new OldAssault();

//        for (int i = 0; i < GetThisItem.LongRangeWeapon.AllLongRangeWeapons.Length; i++)
//        {
//            if (GetThisItem.LongRangeWeapon.AllLongRangeWeapons[i].UniqueCode == "_Chainsaw")
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
//            Item TheNewItem = ThePrefabs.AllMagazine[i].gameObject.GetComponent<Item>();
//            if (TheNewItem.UniqueCode == "_BigFireJar")
//            {
//                ThisItem.AmmoTypeId = TheNewItem.Id;
//                ThisItem.MaxMagazine = TheNewItem.RemainAmmo;
//            }
//        }

//    }
//    // Start is called before the first frame update
//    void Start()
//    {

//        TheItem = gameObject.GetComponent<Item>();

//        IsForward = Watch.IsTurned;

//        TheSmoke.Stop();

//    }

//    void FixedUpdate()
//    {
//        TheMagazine = TheItem.TheMagazine;
//        ThisItem.MaxMagazine = TheMagazine.RemainAmmo;

//        IsForward = Watch.IsTurned;

//        IsEquipped = TheItem.IsEquiped;

//        if (IsEquipped && Input.GetMouseButton(1) && Input.GetMouseButton(0) && ThisItem.MaxMagazine > 0)
//        {

//            if (RestartUp == false)
//            {
//                SawSound.Play();
//                SawSound.volume = 0;
//                RestartUp = true; 
//            }

//            if (!TheSmoke.isPlaying)
//            {
//                TheSmoke.Play();
//            }

//            if (SawSound.volume < 1)
//            {
//                SawSound.volume += VolumeUpSpeed * Time.deltaTime;
//            }

//            StartChain();
//            TheMagazine.RemainAmmo--;
//            ChangeSliderValue();

//        }else if (IsEquipped && (Input.GetMouseButtonUp(1) || Input.GetMouseButtonUp(0)) && ThisItem.MaxMagazine > 0)
//        {
//            RestartUp = false;

//            if (SawSound.volume > 0)
//            {
//                SawSound.volume -= VolumeUpSpeed * Time.deltaTime;
//            }

//            TheChainBox.enabled = true;
//            TheChainBox.isTrigger = false;
//            gameObject.layer = LayerMask.NameToLayer("Weapon");
//        }
//        else
//        {
//            RestartUp = false;

//            if (SawSound.volume > 0)
//            {
//                SawSound.volume -= VolumeUpSpeed * Time.deltaTime;
//            }

//            if (SawSound.volume <= 0)
//            {
//                SawSound.Stop();
//                SawSound.volume = 0;
//            }
//            TheChainBox.enabled = true;
//            TheChainBox.isTrigger = false;
//            gameObject.layer = LayerMask.NameToLayer("Weapon");
//        }

//    }

//    public void StartChain()
//    {
//        gameObject.layer = LayerMask.NameToLayer("MeleeWeapon");
//        TheChainBox.isTrigger = true;
//        TheChainBox.enabled = false;
//        TheChainBox.enabled = true;

//        TheCamera.ShackTime = 0.01f;
//        TheCamera.Shacking = 0.01f;
//        TheCamera.ShakingFunc();
//    }


//    public void ChangeSliderValue()
//    {
//        TheMagazine.AmmoSlider.value = (float)TheMagazine.RemainAmmo / TheMagazine.MaxAmmo;
//    }

//    public void OnTriggerEnter2D(Collider2D collision)
//    {
//        if (gameObject.transform.parent != null && collision.gameObject.tag != gameObject.transform.parent.tag)
//        {
//            TakeDamage TheTakeDamage = collision.gameObject.GetComponent<TakeDamage>();
//            if (TheTakeDamage != null)
//            {
//                BloodFunc(collision.gameObject, TheTakeDamage);
//                if (TheStrikeSound.isPlaying == false)
//                {
//                    float RandomPitch = UnityEngine.Random.Range(0.5f, 1f);
//                    TheStrikeSound.pitch = RandomPitch;
//                    TheStrikeSound.Play();
//                }
//                TheTakeDamage.TakingDamage(ThisItem.Damage);
//                TheTakeDamage.CutSeverity += 0.1f;
//                TheTakeDamage.TheStatuseManager.BleedingSeverity += 0.1f;
//            }
//        }
//    }

//    public void BloodFunc(GameObject collision, TakeDamage TheTakeDamage)
//    {
//        Vector2 NewPos = collision.transform.position;

//        GameObject TheBlood = Instantiate(BloodPartical, NewPos, collision.transform.rotation);
//        TheBlood.transform.SetParent(gameObject.transform);

//        BloodPart BRb2 = TheBlood.gameObject.GetComponent<BloodPart>();
//        BRb2.Magnification = new Vector2(0, 2);

//        Destroy(TheBlood, 0.2f);
//        /////////////////////////////////////////////////////////////////////

//        //Ëæ»úÉú³ÉÑªÒºÍ¼Æ¬
//        float RandomBlood = UnityEngine.Random.Range(0, TheTakeDamage.TheDirtManager.Bloods.Length + 50);

//        if (0 < RandomBlood && RandomBlood <= TheTakeDamage.TheDirtManager.Bloods.Length - 1)
//        {
//            int RandomBlood2 = UnityEngine.Random.Range(0, TheTakeDamage.TheDirtManager.Bloods.Length - 1);
//            Instantiate(TheTakeDamage.TheDirtManager.Bloods[RandomBlood2], collision.transform.position, collision.transform.rotation);
//        }
//    }
//}
