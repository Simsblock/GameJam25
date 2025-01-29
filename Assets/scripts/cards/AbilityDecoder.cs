using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

public class AbilityDecoder : MonoBehaviour
{
    
    private PlayerHandler playerHandler;
    private Dealer Dealer;
    System.Random rand = new System.Random();
    // Start is called before the first frame update
    void Start()
    {
        playerHandler = GameObject.Find("Player").GetComponent<PlayerHandler>();
    }

   

    public void Use(string Effect)
    {
        //effect pattern: xxx:n:m;
        //xxx -> effect identifier
        //n -> number for use (draw n cards)
        //m -> target (P = player D = Dealer)
        //only xxx is mandatory
        //to concat multible effects chain with ; and no spaces (xxx;xxx:n;xxx:n:m)
        if (Effect != null)
        {
            string[] effects = Effect.Split(';');

            foreach (string effect in effects)
            {
                string[] details = effect.Split(":");
                switch (details[0])
                {
                    case "draw":
                        if (details[2] == "P") playerHandler.PullMulti(int.Parse(details[1]));
                        if (details[2] == "D") Dealer.PullMulti(int.Parse(details[1]));
                        break;
                    case "remove":
                        //maby
                        break;
                        //for diceroll n= n1-n2
                    case "diceRoll":
                        string[] numbers = details[1].Split("-");
                        playerHandler.curSum+=(rand.Next(int.Parse(numbers[0]), int.Parse(numbers[1])));
                        break;
                    //actual abilities laut notion
                    case "Seer":
                        Deck.NextCard = Deck.GetCard();
                        Seer();
                        break;
                    case "ThreeKings":
                        playerHandler.curSum -= 3;
                        break;
                    case "Switcheroo": //RANDOM FOR NOW
                        //Choose a card to switch Dealer
                        //Remove from Dealer adn addd to player
                        KeyValuePair<string, int> temp = Dealer.DealerHand.ElementAt(rand.Next(Dealer.DealerHand.Count));
                        Dealer.DealerHand.Remove(temp.Key);
                        playerHandler.playerCards.Add(temp.Key,temp.Value);
                        //Remove from Player and add to dealer
                        temp = playerHandler.playerCards.ElementAt(rand.Next(playerHandler.playerCards.Count));
                        playerHandler.playerCards.Remove(temp.Key);
                        Dealer.DealerHand.Add(temp.Key,temp.Value);
                        break;
                    case "Cashback":
                        //Gain 50% of your bet back on a bust. On a win, you only gain 75%.
                        GlobalData.BetLossRate = 50;
                        GlobalData.BetPayoutRate = 75;
                        break;
                    case "Parallel Universe":
                        //Clear
                        playerHandler.ClearBaseCards();
                        Dealer.ClearHand();
                        Deck.Clear();
                        //Reset Cards
                        Dealer.PullInit();
                        playerHandler.PullMulti(2);
                        break;
                    case "Ass":
                        //E1:1 for Ass
                        string key = "E1";
                        playerHandler.AddCard(key, 1); 
                        break;
                    case "Shortcut":
                        GlobalData.DealerWinCond = 17;
                        break;
                    case "Joker":
                        //Joker:1 | Joker:5 | Joker:10
                        int.TryParse(details[1], out int value);
                        playerHandler.curSum += value;
                        playerHandler.DisplayPlayerCards($"E{value}"); //Nicht ganz baba, weil es für 1 ein Ass displayed, evtl Joker Kartenset warrats
                        break;
                    case "The Twins":
                        //Pull 2 cards and look at them. Choose which one to add to your count.

                        break;
                    case "Destroy":
                        //destroy/anull first special card dealer uses
                        break;
                    case "Gambit":
                        GlobalData.bet *= 2;
                        break;
                }   
            }
        }
    }
    //NOT TESTED
    private IEnumerator Seer()
    {
        // Make GameObject
        GameObject card = new GameObject("Card");
        card.transform.position = new Vector3(0, 0, 0);

        // Get Card Data
        CardManager.DeckConverter(Deck.NextCard.Key, out string suit, out int rank);
        CardManager CM = FindObjectOfType<CardManager>();
        Sprite s = CM.GetCardSprite(suit, rank);

        // Add SpriteRenderer
        SpriteRenderer spriteRenderer = card.AddComponent<SpriteRenderer>();
        spriteRenderer.sprite = s;
        spriteRenderer.sortingOrder = 5;

        // Set Scale
        card.transform.localScale = new Vector3(6f, 6f, 6f);

        // Display Time
        yield return new WaitForSeconds(2f);

        // Destroy Object
        Destroy(card);
    }

}
