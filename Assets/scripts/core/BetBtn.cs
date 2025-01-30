using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class BetBtn : MonoBehaviour
{
    [SerializeField]
    int amount;
    [SerializeField]
    private TMP_Text Bet_text;

    public void Raise()
    {
        if ((PlayerPrefs.GetInt("Bet")) + amount < 99900 && (PlayerPrefs.GetInt("Bet"))+amount <= PlayerPrefs.GetInt("Money"))
        {
            PlayerPrefs.SetInt("Bet", int.Parse(Bet_text.text));
            PlayerPrefs.SetInt("Bet", PlayerPrefs.GetInt("Bet") + amount);
            Bet_text.text = (PlayerPrefs.GetInt("Bet")).ToString();
        }
    }
    public void Lower()
    {
        if ((PlayerPrefs.GetInt("Bet")) - amount >= amount)
        {
            PlayerPrefs.SetInt("Bet", int.Parse(Bet_text.text));
            PlayerPrefs.SetInt("Bet", PlayerPrefs.GetInt("Bet")-amount);
            Bet_text.text = (PlayerPrefs.GetInt("Bet")).ToString();
        }
    }
}