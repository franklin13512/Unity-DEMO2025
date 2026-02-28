using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;

public class Magazine : MonoBehaviour
{
    public float Damage;//基础伤害
    public float Damage_ST;//爆炸最低伤害(我也不太记得了)

    public float ChanceToBleed = 10;

    public float ExplosionRadius;//爆炸半径
    public float Maganification;//爆炸力度

    public GetJson GetThisItem;
    public Item TheItem;

    public CreateNewJson.Ammo ThisItem;

    public bool IsInstalled;

    public MatchMagazine TheMatchMagazine;

    public int RemainAmmo;
    public int MaxAmmo;

    public bool BackForce;
    public bool IsExplosion;

    public Weapon.BulletType TheBulletType;

    public bool DontAwake;

    private void Awake()
    {
        if (DontAwake == false)
        {
            StartCoroutine(Wait());
        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }
    private void Update()
    {

    }

    IEnumerator Wait()
    {
        yield return null;

        TheMatchMagazine = GameObject.Find("ImportantResource/TheMatch").gameObject.GetComponent<MatchMagazine>();
        GetThisItem = TheMatchMagazine.GetThisItem;

        TheItem = gameObject.GetComponent<Item>();
        ThisItem = new CreateNewJson.Ammo();

        //for (int i = 0; i < GetThisItem.Ammo.AllAmmo.Length; i++)
        //{
        //    if (GetThisItem.Ammo.AllAmmo[i].UniqueCode == TheItem.UniqueCode)
        //    {
        //        ThisItem.UniqueCode = GetThisItem.Ammo.AllAmmo[i].UniqueCode;
        //        ThisItem.TheSprite_Path = Application.streamingAssetsPath + GetThisItem.Ammo.AllAmmo[i].TheSprite_Path;
        //        ThisItem.MaxMagazine = GetThisItem.Ammo.AllAmmo[i].MaxMagazine;
        //        ThisItem.Type = GetThisItem.Ammo.AllAmmo[i].Type;
        //        ThisItem.RemainMagazine = GetThisItem.Ammo.AllAmmo[i].MaxMagazine;

        //        break;
        //    }
        //}

        TheItem.TheBasicData.Type = CreateNewJson.TheType.Ammo;

        byte[] bytes = File.ReadAllBytes(ThisItem.TheSprite_Path);

        Texture2D texture = new Texture2D(2, 2);
        texture.LoadImage(bytes); // 加载图片数据

        Sprite sprite = Sprite.Create(
            texture,
            new Rect(0, 0, texture.width, texture.height),
            new Vector2(0.5f, 0.5f) // 设置锚点为中心
        );

        TheItem.TheSprite = sprite;

        if(TheItem.Stackable == false)
        {
            MaxAmmo = ThisItem.MaxMagazine;
            RemainAmmo = ThisItem.RemainMagazine;
        }
        else
        {
            MaxAmmo = 0;
            RemainAmmo = 0;
        }

    }
}
