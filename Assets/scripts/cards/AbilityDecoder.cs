using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

public class AbilityDecoder : MonoBehaviour
{
    
    internal PlayerHandler playerHandler;
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
                    case "Restart":
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
                        GlobalData.DuplicateAmt++;
                        string key = $"EA:{GlobalData.DuplicateAmt}";
                        playerHandler.AddCard(key, 11); 
                        break;
                    case "Shortcut":
                        GlobalData.DealerWinCond = 17;
                        break;
                    case "Joker":
                        //Joker
                        Joker();
                        break;
                    case "TheTwins":
                        //Pull 2 cards and look at them. Choose which one to add to your count.
                        Twins();
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

    private void Joker()
    {
        StartCoroutine(SpawnCards(new Vector3(-4, 0, 0), 3, 2, new string[] { "EA","E5","E13" }, false));
    }
    private void Seer()
    {
        StartCoroutine(SpawnCards(new Vector3(0, 0, 0), 1, 0, new string[] { Deck.NextCard.Key }, true));
    }

    private void Twins()
    {
        StartCoroutine(SpawnCards(new Vector3(-2, 0, 0), 1, 1, new string[] { Deck.GetCard().Key, Deck.GetCard().Key }, false));
    }


    private IEnumerator SpawnCards(Vector3 pos, int cardAmount, int ClickedUsage, string[] keys, bool timer)
    {
        // Create first card
        List<GameObject> cards = new List<GameObject>();
        GameObject firstCard = CreateCard(pos, ClickedUsage, keys[0]);
        cards.Add(firstCard);
        for (int i = 1; i < cardAmount; i++)
        {
            pos += new Vector3(4, 0, 0);
            GameObject cardX = CreateCard(pos, ClickedUsage, keys[i]);
            cards.Add(cardX);
        }

        //wait
        if(!timer) yield return new WaitUntil(() => cardClicked);
        else yield return new WaitForSeconds(2f);

        // Destroy cards
        foreach (GameObject card in cards)
        {
            Destroy(card);
        }
    }

    private GameObject CreateCard(Vector3 position, int clickedUsage, string key)
    {
        GameObject card = new GameObject("Card");
        card.transform.position = position;

        // Get card data
        CardManager.DeckConverter(key, out string suit, out int rank);
        CardManager CM = FindObjectOfType<CardManager>();
        Sprite s = CM.GetCardSprite(suit, rank);

        // Add SpriteRenderer
        SpriteRenderer spriteRenderer = card.AddComponent<SpriteRenderer>();
        spriteRenderer.sprite = s;
        spriteRenderer.sortingOrder = 5;

        // Set Scale
        card.transform.localScale = new Vector3(6f, 6f, 6f);

        // Add Collider for Click Detection
        BoxCollider2D collider = card.AddComponent<BoxCollider2D>();
        collider.isTrigger = true;
        // Add Click Handler
        card.AddComponent<CardClickHandler>();
        CardClickHandler cch = card.GetComponent<CardClickHandler>();
        cch.Decoder = this;
        cch.Key = key;
        cch.ClickedUsage = clickedUsage;

        return card;
    }
    private bool cardClicked;
    public void OnCardClicked()
    {
        cardClicked = true;
    }

}

public class CardClickHandler : MonoBehaviour
{
    public AbilityDecoder Decoder { get; set; }
    public string Key { get; set; }
    public int ClickedUsage { get; set; } //NextCard | AddCard | ...
    private void OnMouseDown()
    {
        Decoder.OnCardClicked();
        Debug.Log("Card Clicked: " + gameObject.name);
        switch (ClickedUsage)
        {
            case 0:
                break;
            case 1:
                AddCardFromDeck();
                break;
            case 2:
                AddExtra();
                break;
                // Add any interaction logic here (e.g., flip card, highlight, etc.)
        }
    }
    private void AddCardFromDeck()
    {
        Deck.AddPulledCard(Key);
        Decoder.playerHandler.AddCard(Key, Deck.DeckCards[Key]);
    }
    private void AddExtra()
    {
        CardManager.DeckConverter(Key, out string suit, out int rank);
        if (rank == 1) rank += 10;
        Decoder.playerHandler.AddCard(Key, rank);
    }

}

