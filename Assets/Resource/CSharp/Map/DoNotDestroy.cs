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

    public void LoadSceneAsync(string sceneName)
    {
        StartCoroutine(LoadAsync(sceneName));
    }

    IEnumerator LoadAsync(string sceneName)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        while (!asyncLoad.isDone)
        {
            ObjNotToDestroy[1].gameObject.GetComponent<Player>().BodyBox.attachedRigidbody.bodyType = RigidbodyType2D.Static;
            foreach (Rigidbody2D TheRb in ObjNotToDestroy[1].gameObject.GetComponent<Player>().TheLimbRbs)
            {
                if (TheRb.gameObject.GetComponent<HingeJoint2D>().enabled == true)
                {
                    TheRb.gameObject.GetComponent<HingeJoint2D>().enabled = false;
                }

            }
            // 可以在这里更新加载进度条
            yield return null;
        }



        foreach (Rigidbody2D TheRb in ObjNotToDestroy[1].gameObject.GetComponent<Player>().TheLimbRbs)
        {
            if (TheRb.gameObject.GetComponent<HingeJoint2D>().enabled == false)
            {
                TheRb.gameObject.GetComponent<HingeJoint2D>().enabled = true;
            }

        }

        yield return new WaitForSeconds(8);

        ObjNotToDestroy[1].gameObject.GetComponent<Player>().BodyBox.attachedRigidbody.bodyType = RigidbodyType2D.Dynamic;

        ObjNotToDestroy[1].gameObject.GetComponent<Player>().BodyBox.attachedRigidbody.velocity = Vector2.zero;

    }
}
