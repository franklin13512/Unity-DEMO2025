using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhetherPositive : MonoBehaviour
{
    public bool IsPositive;
    public bool IsPositiveTurn;
    public bool AllUse;

    public RightRunning Right;
    public LeftRuning Left;

    // Start is called before the first frame update
    void Start()
    {
        IsPositive = true;
        IsPositiveTurn = true;
        AllUse = false;
    }

    void Update()
    {
    }

}
