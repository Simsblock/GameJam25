using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    // Start is called before the first frame update
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
        //tmp
        //AddSpecialCard("Test1");
        //AddSpecialCard("Test2");
        DisplaySpecial.Draw(specialCards);
    }
    public void DisplayPlayerCards()
    {

    }
}
