using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollierChecker : MonoBehaviour
{
    public bool OntheGround;

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision != null)
        {
            OntheGround = true;
        }
        else
        {
            OntheGround=false;
        }
    }
}
