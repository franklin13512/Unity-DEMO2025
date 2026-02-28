using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeapon : MonoBehaviour
{
    public GetJson TheGetJsom;
    public Item TheItem;
    public SpriteRenderer TheRenderer;
    public BoxCollider2D TheBox;

    private float StartTime = 0.2f;

    //[HideInInspector]
    public CreateNewJson.MeleeWeapon ThisItem;

    public MeleeAttacking TheMeleeAttacking;

    public MeleeAttackAnimation TheMeleeAnim;

    public AudioSource TheStrikeSound;


    public void OnValidate()
    {

        //TheMeleeAttacking = gameObject.GetComponent<MeleeAttacking>();
    }

    public void Start()
    {
        TheItem = gameObject.GetComponent<Item>();
        TheRenderer = gameObject.GetComponent<SpriteRenderer>();
        TheBox = gameObject.GetComponent<BoxCollider2D>();
        TheMeleeAttacking = gameObject.GetComponent<MeleeAttacking>();

        TheStrikeSound = gameObject.GetComponent<AudioSource>();

    }

    public void Fake_Awake(GetJson TheGetJson)
    {
        //TheGetJsom = GameObject.Find("ImportantResource/TheJson").gameObject.GetComponent<GetJson>();

        if (TheGetJson != null)
        {
            TheItem = gameObject.GetComponent<Item>();


            for (int i = 0; i < TheGetJson.MeleeWeapon.AllMeleeWeapons.Length; i++)
            {
                if (TheGetJson.MeleeWeapon.AllMeleeWeapons[i].UniqueCode == TheItem.UniqueCode)
                {
                    ThisItem = new CreateNewJson.MeleeWeapon(
                        TheGetJson.MeleeWeapon.AllMeleeWeapons[i].UniqueCode,
                        TheGetJson.MeleeWeapon.AllMeleeWeapons[i].Type,
                        TheGetJson.MeleeWeapon.AllMeleeWeapons[i].Damage,
                        TheGetJson.MeleeWeapon.AllMeleeWeapons[i].TimeForNextAttack,
                        TheGetJson.MeleeWeapon.AllMeleeWeapons[i].ChanceToBleed
                        );

                    //ThisItem.TheSprite = gameObject.GetComponent<SpriteRenderer>().sprite;

                    break;
                }
            }


            TheItem.TheBasicData.Type = CreateNewJson.TheType.MeleeWeapon;

            //TheMeleeAttacking.MeleeDamage = ThisItem.Damage;
        }
    }

    private void Update()
    {
        if (StartTime > 0)
        {
            StartTime -= Time.deltaTime;
        }else if (StartTime <= 0)
        {
            StartTime = 0;
        }
    }


    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (TheMeleeAnim && StartTime == 0)
        {

            if (TheMeleeAnim.AttackTime > 0f && TheMeleeAnim.AttackTime < 0.7f)
            {

                Rigidbody2D Rb = gameObject.GetComponent<Rigidbody2D>();
                //Debug.Log(Rb.velocity.magnitude);

                if (Rb && Rb.velocity.magnitude > 0.002 )
                {

                    //Debug.Log(Rb.velocity.magnitude);
                    TakeDamage TheTakeDamage = collision.gameObject.GetComponent<TakeDamage>();

                    float RandomPitch = UnityEngine.Random.Range(0.5f, 1f);
                    TheStrikeSound.pitch = RandomPitch;
                    TheStrikeSound.Play();

                    TheTakeDamage.TakingDamage(ThisItem.Damage);

                    StatusManager TheStatuseManager = TheTakeDamage.TheStatuseManager;


                    if (UnityEngine.Random.value < ThisItem.ChanceToBleed * 0.01f) //触发流血机制并生成血液
                    {

                        TheTakeDamage.TheStatuseManager.BleedingTime += 5f;
                        TheTakeDamage.TheStatuseManager.BleedingSeverity += 1;
                        TheTakeDamage.TheStatuseManager.StartBleeding = true;

                        foreach (ContactPoint2D contact in collision.contacts)
                        {
                            GameObject TheObj = Instantiate(TheMeleeAnim.ThePrefabList.BloodPartical.gameObject, contact.point, Quaternion.identity);

                            TheObj.transform.SetParent(collision.gameObject.transform);

                            TheObj.transform.localScale = new Vector2(1, 1);
                            TheObj.gameObject.SetActive(true);

                            BloodDirt TheBlood = TheObj.GetComponent<BloodDirt>();
                        }

                    }

                    StartTime = 0.2f;


                }
            }
        }
    }

}
