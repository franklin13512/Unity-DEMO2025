using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    public Transform TheSpawnPoint;

    public bool TheFirstPlace;

    //用于设定新场景物体生成位置
    // Start is called before the first frame update
    void Start()
    {
        GameObject ThePlayer = GameObject.Find("Player02_Female").gameObject; GameObject TheSpawnObj = new GameObject(); 

        TheSpawnObj.transform.position = ThePlayer.gameObject.GetComponent<Player>().BodyBox.bounds.center; 
        ThePlayer.transform.SetParent(TheSpawnObj.transform);

        DontDestroyOnLoad(TheSpawnObj.transform);

        ThePlayer.transform.parent.position = TheSpawnPoint.position;
        ThePlayer.transform.SetParent(null);
        Destroy(TheSpawnObj);

        DontDestroyOnLoad(ThePlayer);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
