using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerHandler : MonoBehaviour
{
    private List<string> specialCards;
    public Dictionary<string,int> playerCards;
    public int curSum; //Total Hand Value
    [SerializeField]
    private GameObject GameHandler;
    [SerializeField]
    private GameObject CardHand;
    private DisplaySpecial DisplaySpecial;
    [SerializeField]
    private GameObject CardPrefab, CardParent;
    private CardManager CM;
    private float cardSpace = 0.65f;
    private Vector3 leftCardPos;
    private Vector3 rightCardPos;
    private bool DisplaySPC;

    void Start()
    {
        if(specialCards == null) specialCards = new List<string>();
        if(playerCards == null) playerCards = new Dictionary<string,int>();
        DisplaySpecial = CardHand.GetComponent<DisplaySpecial>();
        CM = CardParent.GetComponent<CardManager>();
        DisplaySPC = false;
    }

    public void PullMulti(int count)
    {
        if (playerCards.Count < 10)
        {
            while (count > 0)
            {
                PullCard();
                count--;
            }
        }
    }

    public void PullCard()
    {
        if (playerCards.Count < 10)
        {
            var card = Deck.PullCard();
            playerCards.Add(card.Key, card.Value);
            DisplayPlayerCards(card.Key);
            curSum += card.Value;
            //checks for aces n shit
            if (playerCards.Keys.Any(k => k.Contains("A")) && curSum > 21)
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
        if (!DisplaySPC)
        {
            DisplaySpecial.Display(specialCards);
            DisplaySPC = true;
        }
        else
        {
            DisplaySpecial.EndDisplay();
            DisplaySPC = false;
        }
        
    }
    public void DisplayPlayerCards(string cardKey)
    {
        GameObject card = Instantiate(CardPrefab);
        card.transform.SetParent(CardParent.transform);
        Vector3 cardPos = new Vector3(0f, 0f, 0f);
        int childrenAmt = CardParent.transform.childCount;
        //Debug.Log(childrenAmt);
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
        CardManager.DeckConverter(cardKey, out string suit, out int rank);
        Sprite s= CM.GetCardSprite(suit, rank);
        SpriteRenderer spriteRenderer = card.GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = s;
    }

    public void AddCard(string key, int value)
    {
        DisplayPlayerCards(key);
        playerCards.Add(key, value);
        curSum += value;
        if (playerCards.Keys.Any(k => k.Contains("A")) && curSum > 21)
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

    public void ClearBaseCards()
    {
        curSum = 0;
        playerCards = new Dictionary<string, int>();
        foreach (Transform card in CardParent.transform)
        {
            GameObject.Destroy(card.gameObject);
        }
    }

}
