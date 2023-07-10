using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DesignPatterns;

public class PointOfInterest : MonoBehaviour
{
    public int lookProbability, stealProbability, breakProbability, buyProbability;
    private int totalProbability
    {
        get
        {
            return lookProbability + stealProbability + breakProbability + buyProbability;
        }
    }
    public void AddActionToQueue(EventQueue queue, bool isThief)
    {

    }

}
