using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ItemInventorySlot : MonoBehaviour
{
    public PrefabList ThePrefabList;

    public ItemSlot TheMagazineSlot;
    public bool MagazineFull;
    public string UniqueCode;
    public int MaxAmmo;
    public int RemainAmmo;

    public ItemSlot[] AccessorySlot;//类需要新创建
    public ItemSlot[] InventorySlot;//类需要新创建

    public UnableallPanel TheUPanel;

    public string[] ProperMagazine;//适配的弹药

    public int MaxStack;//最高stack限制
    public bool StackableForMagazine;

    public Weapon.BulletType TheBulletType;
    public float BulletForce;

    public AudioSource SoundManager;
    public AudioClip ExplosionSound;
    public AudioClip RingingSource;
    public GameObject ExplosionPrefab;

    public ItemInventorySlot OriginOne;

    public Magazine TheMagazine;

    public Sprite TheEmptySprite;


    //子弹效果，由弹药决定

    //单纯伤害,建议用于炮台
    public void ShootEffect(GameObject HitObj, GameObject ShootPos, float Damage, float Damage_ST, float ChangeToBleed, RaycastHit2D TheRayHit)
    {
        TakeDamage TheTakeDamage = HitObj.GetComponent<TakeDamage>();
        Structure TheStructure = HitObj.GetComponent<Structure>();
        if (TheTakeDamage)
        {

            TheTakeDamage.TakingDamage(Damage);//扣血

            if (Random.value < ChangeToBleed * 0.01f) //触发流血机制并生成血液
            {


                TheTakeDamage.TheStatuseManager.BleedingTime += 5f;
                TheTakeDamage.TheStatuseManager.BleedingSeverity += 1;
                TheTakeDamage.TheStatuseManager.StartBleeding = true;

                GameObject TheObj = Instantiate(ThePrefabList.BloodPartical.gameObject, TheRayHit.point, Quaternion.identity);

                TheObj.transform.SetParent(HitObj.transform);

                TheObj.transform.localScale = new Vector2(1, 1);
                TheObj.gameObject.SetActive(true);
                //ParticleSystem ThePart = TheObj.GetComponent<ParticleSystem>();
                //ThePart.collision.


                BloodDirt TheBlood = TheObj.GetComponent<BloodDirt>();
                if (TheBlood != null)
                {

                    TheBlood.TheStatus = TheTakeDamage.TheStatuseManager;
                }

            }
        }

        if (TheStructure != null)
        {
            TheStructure.TakeDamage_ST(Damage_ST);
        }

    }

    //后坐力+伤害
    public void ShootEffect(bool IsForward, GameObject HitObj, GameObject ShootPos, float Damage, float Damage_ST, float ChangeToBleed, RaycastHit2D TheRayHit)
    {

        TakeDamage TheTakeDamage = HitObj.GetComponent<TakeDamage>();
        Structure TheStructure = HitObj.GetComponent<Structure>();
        if (TheTakeDamage)
        {
            TheTakeDamage.TakingDamage(Damage);//扣血

            if (Random.value < ChangeToBleed * 0.01f) //触发流血机制并生成血液
            {

                TheTakeDamage.TheStatuseManager.BleedingTime += 5f;
                TheTakeDamage.TheStatuseManager.BleedingSeverity += 1;
                TheTakeDamage.TheStatuseManager.StartBleeding = true;

                GameObject TheObj = Instantiate(ThePrefabList.BloodPartical.gameObject, TheRayHit.point, Quaternion.identity);

                TheObj.transform.SetParent(HitObj.transform);

                TheObj.transform.localScale = new Vector2(1, 1);
                TheObj.gameObject.SetActive(true);
                //ParticleSystem ThePart = TheObj.GetComponent<ParticleSystem>();
                //ThePart.collision.

                BloodDirt TheBlood = TheObj.GetComponent<BloodDirt>();  
                if (TheBlood != null)
                {

                    TheBlood.TheStatus = TheTakeDamage.TheStatuseManager;
                }

            }
        }

        Rigidbody2D TheRb = HitObj.GetComponent<Rigidbody2D>();//击退
        if (TheBulletType == Weapon.BulletType.BackForce)
        {
            if (TheRb)
            {
                Vector2 TheV2 = new Vector2(ShootPos.transform.position.x, ShootPos.transform.position.y);

                if (IsForward == false)
                    TheRb.AddForce(-TheV2 * BulletForce);
                else
                    TheRb.AddForce(TheV2 * BulletForce);
            }

        }

        if (TheStructure != null)
        {
            TheStructure.TakeDamage_ST (Damage_ST);
        }

    }

    //子弹效果爆炸重载
    public void ShootEffect(bool IsForward, Vector2 HitObj, GameObject ShootPos, float ExplosionRadius, float Maganification, float MaxDamage, float Damage_ST)
    {
        if (TheBulletType == Weapon.BulletType.Explosion)
        {
            GameObject TheBoomProject = Instantiate(ExplosionPrefab, HitObj, transform.rotation);

            SoundManager.clip = ExplosionSound;
            SoundManager.Play();

            Collider2D[] TheCollider = Physics2D.OverlapCircleAll(TheBoomProject.transform.position, ExplosionRadius);

            for (int i = 0; i < TheCollider.Length; i++)
            {
                Rigidbody2D TheRb = TheCollider[i].gameObject.GetComponent<Rigidbody2D>();
                TakeDamage TheDamage = TheCollider[i].GetComponent<TakeDamage>();
                Structure TheStructure = TheCollider[i].gameObject.GetComponent<Structure>();

                if (TheRb != null)
                {
                    UnityEngine.Vector2 TheDirection = TheCollider[i].transform.position - TheBoomProject.transform.position;
                    TheRb.AddForce(TheDirection.normalized * Maganification, ForceMode2D.Impulse);
                }

                float TheDistance = UnityEngine.Vector2.Distance(TheBoomProject.transform.position, TheCollider[i].transform.position);

                if (TheDamage != null)
                {

                    if (TheDamage.TheHealthManager.Exploded == false)
                    {
                        if (TheCollider[i].gameObject.tag == "Player")
                        {
                            SoundManager.clip = RingingSource;
                            SoundManager.Play();
                        }
                        TheDamage.TheHealthManager.Exploded = true;
                        TheDamage.TakingDamage((1 - (TheDistance / ExplosionRadius)) * MaxDamage);

                        TheDamage.TheStatuseManager.Collapse();
                        //导致内脏损伤
                        TheDamage.TheStatuseManager.IsInternalInjures = true;
                        TheDamage.TheStatuseManager.InternalInjures_TimeToRecover = 120f;
                        TheDamage.TheStatuseManager.InternalInjures_TimerStart = false;
                        TheDamage.TheStatuseManager.InternalInjures_Severity += 6 * (1 - (TheDistance / ExplosionRadius));
                    }

                    //导致烧伤(会使多个肢体同时造成烧伤)
                    TheDamage.BurnSeverity += 1.5f * (1 - (TheDistance / ExplosionRadius));

                }

                if (TheStructure != null)
                {

                    TheStructure.TakeDamage_ST((1 - (TheDistance / ExplosionRadius)) * Damage_ST);
                }

            }
            Destroy(TheBoomProject, 1f);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        ThePrefabList = GameObject.Find("ImportantResource/PrefabManager").gameObject.GetComponent<PrefabList>();
    }

    // Update is called once per frame
    void Update()
    {
        if (TheMagazine != null)
        {
            RemainAmmo = TheMagazine.RemainAmmo;
            MaxAmmo = TheMagazine.MaxAmmo;
        }
    }
}
