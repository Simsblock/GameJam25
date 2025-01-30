using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class BetBtn : MonoBehaviour
{
    int bet;
    [SerializeField]
    int amount;
    [SerializeField]
    private TMP_Text bet_text;

    public void Raise()
    {
        if (bet + amount < 99900 && bet+amount <= PlayerPrefs.GetInt("Money"))
        {
            bet = int.Parse(bet_text.text);
            bet += amount;
            bet_text.text = bet.ToString();
        }
    }
    public void Lower()
    {
        if (bet - amount >= amount)
        {
            bet = int.Parse(bet_text.text);
            bet -= amount;
            bet_text.text = bet.ToString();
        }
    }
}