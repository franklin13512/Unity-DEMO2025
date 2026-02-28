using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhetherPo : MonoBehaviour
{
    public bool IsPositive;
    public bool IsNegative;

    public bool IsPositiveTurn;
    public bool IsNegativeTurn;

    public bool AllUse;
    public bool AllUse_Walk;

    public RightWalking Right;
    public LeftWalking Left;

    // Start is called before the first frame update
    void Start()
    {
        IsPositive = true;
        IsNegative = false;

        IsPositiveTurn = true;
        IsNegativeTurn = false;

        AllUse = false;
        AllUse_Walk = false;
    }

    void Update()
    {
    }
}
