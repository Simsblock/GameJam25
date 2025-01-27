using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class BetBtn : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
    }
    // Update is called once per frame
    void Update()
    {
       
    }
    long bet;
    [SerializeField]
    int amount;
    [SerializeField]
    private TMP_Text bet_text;

    public void Raise()
    {
        if (bet + amount < 10000)
        {
            bet = long.Parse(bet_text.text);
            bet += amount;
            bet_text.text = bet.ToString();
        }
    }
    public void Lower()
    {
        if (bet - amount > amount)
        {
            bet = long.Parse(bet_text.text);
            bet -= amount;
            bet_text.text = bet.ToString();
        }
    }
}