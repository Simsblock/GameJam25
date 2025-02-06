using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Player : Participant
{
    private List<string> specialCards;
    [SerializeField]
    private GameObject SPCSlotR;
    [SerializeField]
    private GameObject CardHand;
    private DisplaySpecial DisplaySpecial;
    private CardManager CM;
    private float cardSpace = 0.65f;
    private Vector3 leftCardPos;
    private Vector3 rightCardPos;
    private bool DisplaySPC;
    [SerializeField]
    private Button AbilityBtn;

    void Awake()
    {
        DisplaySpecial = CardHand.GetComponent<DisplaySpecial>();
        CM = CardParent.GetComponent<CardManager>();
    }

    void Start()
    {
        WinCondition = GlobalData.PlayerWinCond;

        if(specialCards == null) specialCards = new List<string>();
        if(HandCards == null) HandCards = new Dictionary<string,int>();
        //SPC
        DisplaySPC = false;
        specialCards = GlobalData.LoadSPC();
        AddSpecialCard("DiceDefault");
    }

    //Display
    protected override Vector3 GetCardPosition(int childrenAmt)
    {
        Vector3 cardPos = Vector3.zero;
        if (childrenAmt == 1)
        {
            cardPos = new Vector3(-0.65f, 0f, 0f);
            leftCardPos = cardPos;
            rightCardPos = cardPos;
        }
        else if (childrenAmt % 2 == 0)
        {
            cardPos = rightCardPos + new Vector3(cardSpace * 2, 0f, 0f);
            rightCardPos = cardPos;
        }
        else
        {
            cardPos = leftCardPos + new Vector3(-cardSpace * 2, 0f, 0f);
            leftCardPos = cardPos;
        }
        return cardPos;
    }

    //Special Cards aka Max Territory
    public void ClearSpecialCards()
    {
        specialCards = new List<string>();
        PlayerPrefs.SetString("SPCs", "");
    }

    public void AddSpecialCard(string name)
    {
        specialCards.Add(name);
        GlobalData.SaveSPC(specialCards);
    }
    public void RemoveSpecialCard(string name)
    {
        if (name.Contains("Default"))
        {
            return;
        }
        specialCards.Remove(name);
        GlobalData.SaveSPC(specialCards);
    }

    public void UpdateDisplay()
    {
        if(DisplaySPC) DisplaySpecial.Display(specialCards);
    }

    public void DisplaySpecialCards()
    {
        SPCSlotR.GetComponent<DropHandler>().ClearAssigned();
        if (!DisplaySPC)
        {
            AbilityBtn.transform.localPosition = new Vector3(175, 180, 1);
            DisplaySpecial.Display(specialCards);
            DisplaySPC = true;
        }
        else
        {
            AbilityBtn.transform.localPosition = new Vector3(365, 180, 1);
            DisplaySpecial.EndDisplay();
            DisplaySPC = false;
        }
        
    }
    //END Special Cards
}
