using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MatchMainHandGun : MonoBehaviour
{
    public MainHandSlot TheMainHandSlot;
    public GetJson GetThisItem;
    public MagazineSlot TheMagazine;
    public WatchMouse Watch;
    public PickUp ThePickUp;
    public CameraMove Camera;
    public PrefabList ThePrefabs;

    private void Awake()
    {
        //TheMainHandSlot = GameObject.Find("Canvas/BagBackground/EquipmentZone/EquippedZone/MainWeapon00").gameObject.GetComponent<MainHandSlot>();
        //GetThisItem = GameObject.Find("ImportantResource/TheJson").gameObject.GetComponent<GetJson>();
        //TheMagazine = GameObject.Find("Canvas/BagBackground/EquipmentZone/MagazineZone/Magazine00").gameObject.GetComponent <MagazineSlot>();
        //Watch = GameObject.Find("ImportantResource/WatchMouse").gameObject.GetComponent<WatchMouse>();
        //ThePickUp = GameObject.Find("ImportantResource/PickUp").gameObject.GetComponent<PickUp>();
        //Camera = GameObject.Find("ImportantResource/CameraMove").gameObject.GetComponent<CameraMove>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
