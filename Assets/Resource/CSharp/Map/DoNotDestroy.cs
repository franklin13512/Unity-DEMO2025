using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DoNotDestroy : MonoBehaviour
{
    public GameObject[] ObjNotToDestroy;

    // Start is called before the first frame update
    void Awake()
    {
        foreach (GameObject TheObj in ObjNotToDestroy)
        {
            DontDestroyOnLoad(TheObj);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
