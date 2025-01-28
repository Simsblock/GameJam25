using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityDecoder : MonoBehaviour
{
    
    private PlayerHandler playerHandler;
    private Dealer Dealer;
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
                        System.Random rand = new System.Random();
                        playerHandler.curSum+=(rand.Next(int.Parse(numbers[0]), int.Parse(numbers[1])));
                        break;
                    //actual abilities laut notion
                    case "seer":
                        //see the next card
                        break;
                    case "ThreeKings":
                        playerHandler.curSum -= 3;
                        break;
                    case "Switcheroo":
                        //Choose a card to switch Dealer
                        break;
                    case "Cashback":
                        //Gain 50% of your bet back on a bust. On a win, you only gain 75%.
                        break;
                    case "Parallel Universe":
                        playerHandler.ClearBaseCards();
                        Dealer.ClearHand();
                        Deck.Clear();
                        //Restart Roudn Missing
                        break;
                    case "add":
                        //E1:1 for Ass | E1:1, E5:5, E10:10 for Joker
                        string key = details[1];
                        int.TryParse(details[2], out int value);
                        playerHandler.AddCard(key, value); 
                        break;
                    case "Shortcut":
                        //21 win cond to 17 for Dealer
                        break;
                    case "Joker":
                        //playerHandler.AddCard(); //1,5,10
                        break;
                    case "The Twins":
                        //Pull 2 cards and look at them. Choose which one to add to your count.
                        break;
                    case "Destroy":
                        //destroy/anull first special card dealer uses
                        break;
                    case "Gambti":
                        GlobalData.bet *= 2;
                        break;
                }
            }
        }
    }
}
