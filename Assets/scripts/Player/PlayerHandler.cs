using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerHandler : MonoBehaviour
{
    private List<string> specialCards;
    private Dictionary<string,int> playerCards;
    public int curSum; //Total Hand Value
    [SerializeField]
    private GameObject GameHandler;
    [SerializeField]
    private GameObject CardHand;
    private DisplaySpecial DisplaySpecial;
    [SerializeField]
    private GameObject CardPrefab, CardParent;
    private float cardSpace = 0.65f;
    private Vector3 leftCardPos;
    private Vector3 rightCardPos;

    void Start()
    {
        if(specialCards == null) specialCards = new List<string>();
        if(playerCards == null) playerCards = new Dictionary<string,int>();
        DisplaySpecial = CardHand.GetComponent<DisplaySpecial>();
    }

    public void PullMulti(int count)
    {
        while (count > 0)
        {
            PullCard();
            count--;
        }
    }

    public void PullCard()
    {
        var card = Deck.PullCard();
        playerCards.Add(card.Key, card.Value );
        DisplayPlayerCards();
        curSum += card.Value;
        //checks for aces n shit
        if (playerCards.Keys.Any(k => k.Contains("A"))&&curSum>21)
        {
            foreach (var item in playerCards.Where(p => p.Key.Contains("A")))
            {
                if (curSum > 21)
                {
                    playerCards[item.Key] = 1;
                    curSum -= 10;
                }
            }
        }

    }

    public void RemoveCard(string cardKey)
    {
        curSum -= playerCards[cardKey];
        playerCards.Remove(cardKey);
        //checks for aces too :)
        if (curSum < 12&& playerCards.Keys.Any(k => k.Contains("A")))
        {
            foreach (var item in playerCards.Where(p => p.Key.Contains("A")))
            {
                if (curSum <12)
                {
                    playerCards[item.Key] = 11;
                    curSum += 10;
                }
            }
        }
    }

    public void ClearPlayerCards()
    {
        playerCards = new Dictionary<string, int>();
    }
    public void ClearSpecialCards()
    {
        specialCards = new List<string>();
    }

    public void AddSpecialCard(string name)
    {
        specialCards.Add(name);
    }

    public void DisplaySpecialCards()
    {
        Debug.Log("triggered");
        //tmp
        AddSpecialCard("Test1");
        AddSpecialCard("Test2");

        DisplaySpecial.Draw(specialCards);
    }
    public void DisplayPlayerCards()
    {
        GameObject card = Instantiate(CardPrefab);
        card.transform.SetParent(CardParent.transform);
        Vector3 cardPos = new Vector3(0f, 0f, 0f);
        int childrenAmt = CardParent.transform.childCount;
        Debug.Log(childrenAmt);
        // Check if first
        if (childrenAmt == 1)
        {
            cardPos = new Vector3(-0.65f, 0f, 0f);
            leftCardPos = cardPos;
            rightCardPos = cardPos;
        }
        else if(childrenAmt % 2 == 0)
        {
            cardPos = rightCardPos + new Vector3(cardSpace*2,0f,0f);
            rightCardPos = cardPos;
        }
        else if (childrenAmt % 2 != 0)
        {
            cardPos = leftCardPos + new Vector3(-cardSpace*2, 0f, 0f);
            leftCardPos = cardPos;
        }
        card.transform.localPosition = cardPos;
        // Create the SpriteRenderer component for the card
        SpriteRenderer spriteRenderer = card.AddComponent<SpriteRenderer>();
        //spriteRenderer.sprite = 
    }

}
