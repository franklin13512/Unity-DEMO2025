using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//public class HandsawCs : MonoBehaviour
//{
//    public GetJson TheGetJsom;
//    public Item TheItem;

//    public void OnValidate()
//    {
//        TheItem.UniqueCode = "_Handsaw";
//        TheItem.AllType = CreateNewJson.TheType.MeleeWeapon;
//        TheItem.IsMainHand = true;
//    }

//    public MeleeAttacking TheMeleeAttacking;

//    [Serializable]
//    public class TheCrowbar : CreateNewJson.MeleeWeapon
//    {
//    }

//    public TheCrowbar ThisItem;


//    private void Awake()
//    {
//        TheGetJsom = GameObject.Find("ImportantResource/TheJson").gameObject.GetComponent<GetJson>();

//        if (TheGetJsom != null)
//        {
//            TheItem = gameObject.GetComponent<Item>();
//            ThisItem = new TheCrowbar();

//            for (int i = 0; i < TheGetJsom.MeleeWeapon.AllMeleeWeapons.Length; i++)
//            {
//                if (TheGetJsom.MeleeWeapon.AllMeleeWeapons[i].UniqueCode == "_Handsaw")
//                {

//                    ThisItem.UniqueCode = TheGetJsom.MeleeWeapon.AllMeleeWeapons[i].UniqueCode;
//                    ThisItem.TheSprite = gameObject.GetComponent<SpriteRenderer>().sprite;
//                    ThisItem.Damage = TheGetJsom.MeleeWeapon.AllMeleeWeapons[i].Damage;
//                    ThisItem.Type = TheGetJsom.MeleeWeapon.AllMeleeWeapons[i].Type;

//                    break;
//                }
//            }


//            TheItem.AllType = CreateNewJson.TheType.MeleeWeapon;
//            TheItem.TheSprite = ThisItem.TheSprite;

//            TheItem.IsMainHand = true;

//            TheMeleeAttacking.MeleeDamage = ThisItem.Damage;
//        }
//    }
//    // Start is called before the first frame update
//    void Start()
//    {


//    }



//    // Update is called once per frame
//    void Update()
//    {
//        if (TheMeleeAttacking.TheCollision != null)
//        {
//            TakeDamage TheTakeDamage = TheMeleeAttacking.TheCollision.gameObject.GetComponent<TakeDamage>();
//            if (TheTakeDamage != null)
//            {
//                TheTakeDamage.CutSeverity += 1.5f;
//                StatusManager TheStatuseManager = TheTakeDamage.TheStatuseManager;
//                if (TheStatuseManager != null)
//                {
//                    float TheChance = UnityEngine.Random.Range(0, 3);
//                    if (TheChance <= 1)
//                    {
//                        TheStatuseManager.StartBleeding = true;
//                        TheStatuseManager.BleedingSeverity++;
//                        TheStatuseManager.BleedingTime += 20;
//                        Vector2 NewPos = new Vector2(TheMeleeAttacking.TheCollision.transform.position.x + 0.25f, TheMeleeAttacking.TheCollision.transform.position.y);
//                        GameObject TheBlood = Instantiate(TheStatuseManager.TheBloodProject, NewPos, Quaternion.identity);
//                        BloodPart TheBloodPart = TheBlood.gameObject.GetComponent<BloodPart>();
//                        if (TheBloodPart != null)
//                        {
//                            TheBloodPart.MaintainTime = TheStatuseManager.BleedingTime;
//                        }
//                        TheBlood.transform.SetParent(TheMeleeAttacking.TheCollision.transform);

//                        Destroy(TheBlood, TheBloodPart.MaintainTime + 5f);

//                    }
//                }
//            }

//            TheMeleeAttacking.TheCollision = null;
//        }
//    }
//}
