using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class PrefabList : MonoBehaviour
{
    //远程武器字典
    public SerializedDictionary<string, GameObject> RangeWeaponDic = new SerializedDictionary<string, GameObject>();
    //弹药字典
    public SerializedDictionary<string, GameObject> AllMagazineDic = new SerializedDictionary<string, GameObject>();
    //近战武器字典
    public SerializedDictionary<string, GameObject> MeleeWeaponsDic = new SerializedDictionary<string, GameObject>();
    //消耗品字典
    public SerializedDictionary<string, GameObject> ConsumableDic = new SerializedDictionary<string, GameObject>();
    //装备字典
    public SerializedDictionary<string, GameObject> EquipmentDic = new SerializedDictionary<string, GameObject>();
    //其他物品字典
    public SerializedDictionary<string, GameObject> ItemDic = new SerializedDictionary<string, GameObject>();

    [Space]
    //远程武器
    public GameObject[] RangeWeaponPrefab;
    //弹药
    public GameObject[] AllMagazine;
    //近战武器
    public GameObject[] MeleeWeaponsPrefab;
    //消耗品
    public GameObject[] ConsumablePrefab;
    //装备
    public GameObject[] EquipmentPrefab;
    //其它物品
    public Item[] Items;

    [Space]
    public GameObject[] StatusIcon;

    [Space]
    //Canvas中已经存在的状态栏
    public List<GameObject> ExistedStatusIcon;

    [Space]
    public GameObject BloodPartical;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public GameObject FindItem(string Key)
    {
        RangeWeaponDic.TryGetValue(Key, out GameObject TheObj1);
        if (TheObj1 == null)
        {
            AllMagazineDic.TryGetValue(Key, out GameObject TheObj2);
            if (TheObj2 == null)
            {
                MeleeWeaponsDic.TryGetValue(Key, out GameObject TheObj3);
                if (TheObj3 == null)
                {
                    ConsumableDic.TryGetValue(Key, out GameObject TheObj4);
                    if (TheObj4 == null)
                    {
                        EquipmentDic.TryGetValue(Key, out GameObject TheObj5);
                        if (TheObj5 == null)
                        {
                            ItemDic.TryGetValue(Key, out GameObject TheObj6);
                            if (TheObj6 == null)
                            {
                                return null;
                            }
                            else
                            {
                                return TheObj6;
                            }
                        }
                        else
                        {
                            return TheObj5;
                        }

                    }
                    else
                    {
                        return TheObj4;
                    }
                }
                else
                {
                    return TheObj3;
                }
            }
            else
            {
                return TheObj2;
            }
        }
        else
        {
            return TheObj1;
        }
    }
}
