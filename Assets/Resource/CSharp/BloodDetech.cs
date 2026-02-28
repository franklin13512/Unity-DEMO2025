using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodDetech : MonoBehaviour
{
    public SpriteMask TheSpriteMask;
    public int RemainBlood;

    // Start is called before the first frame update
    void Start()
    {
        RemainBlood = 0;
        TheSpriteMask = gameObject.GetComponent<SpriteMask>();
        TheSpriteMask.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (RemainBlood == 0)
        {
            TheSpriteMask.enabled = false;
        }
        else if (RemainBlood > 0)
        {
            TheSpriteMask.enabled = true;
        }
    }
}
