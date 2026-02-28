using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadNewLocation : MonoBehaviour
{
    public string SceneName;

    public DoNotDestroy TheDoNotDestroy;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }



    private void OnTriggerStay2D(Collider2D collision)
    {
        if (Input.GetButtonDown("Interact") && collision.gameObject.tag == "Player")
        {
            TheDoNotDestroy.LoadSceneAsync(SceneName);
        }
    }
}
