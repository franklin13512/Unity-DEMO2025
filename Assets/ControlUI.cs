using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControlUI : MonoBehaviour
{
    public Animator TheAnimator;

    public Button TheButton;

    public bool Opened;

    // Start is called before the first frame update
    void Start()
    {
        Opened = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ControlTheUI()
    {
        if (Opened)
        {
            TheAnimator.SetBool("Show", false);
            TheAnimator.SetBool("Hide", true);

            Opened = false;
        }
        else if (!Opened)
        {
            TheAnimator.SetBool("Hide", false);
            TheAnimator.SetBool("Show", true);

            Opened = true;
        }

    }
}
