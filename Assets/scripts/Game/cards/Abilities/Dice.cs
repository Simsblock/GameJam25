using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dice : MonoBehaviour
{
    [SerializeField]
    public int SideCount;
    [SerializeField]
    [Tooltip("-1 or 1")]
    public int Multiplier;
    
    public int RollDie()
    {
        System.Random rand = new System.Random();
        return (rand.Next(SideCount)+1) * Multiplier;
    }
}
