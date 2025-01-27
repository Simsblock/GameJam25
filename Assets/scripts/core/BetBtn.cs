using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BetBtn : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        btn = GetComponent<Button>();
    }
    // Update is called once per frame
    void Update()
    {
       
    }

    Button btn;
    long bet;
    [SerializeField]
    private TMP_Text bet_text;

    public void OnPress()
    {
        bet = long.Parse(bet_text.text);
        if (btn.name.Contains("Raise"))
        {
            bet += 100;
        }else if (btn.name.Contains("Lower"))
        {
            bet -= 100;
        }
    }
}