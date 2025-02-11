using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

public class AbilityDecoder : MonoBehaviour
{
    [SerializeField]
    internal GameHandler GameHandler;
    internal Player playerHandler;
    internal Dealer Dealer;
    System.Random rand = new System.Random();
    [SerializeField]
    internal Sprite[] SPCSprites;
    [SerializeField]
    private GameObject BetUi;
    //+1
    private bool plusOne=false;
    private int drawAmt=0;
    private AudioManager audioManager;
    // Start is called before the first frame update
    void Start()
    {
        playerHandler = GameObject.Find("Player").GetComponent<Player>();
        Dealer = GameObject.Find("Dealer").GetComponent<Dealer>();
        GameHandler = GameObject.Find("GameHandler").GetComponent<GameHandler>();
        audioManager = GameObject.Find("AudioHandler").GetComponent<AudioManager>();
    }

    void Update()
    {
        if (GameHandler.stand == 4 && plusOne)
        {
            Dealer.PullMultipleCards(drawAmt);
            plusOne = false;
            drawAmt = 0;
            StartCoroutine(DisplaySPC("SPC1"));
        }
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
                        if (details[2] == "P")
                        {
                            StartCoroutine(DisplaySPC("SPC1"));
                            playerHandler.PullMultipleCards(int.Parse(details[1]));
                        }
                        if (details[2] == "D")
                        {
                            drawAmt = int.Parse(details[1]);
                            plusOne = true;
                        }
                        break;
                    case "remove":
                        //maby
                        break;
                        //for diceroll n= n1-n2
                    case "diceRoll":
                        Debug.Log(details[1]);
                        string[] numbers = details[1].Split(".");
                        playerHandler.ValueModifier+=(rand.Next(int.Parse(numbers[0]), int.Parse(numbers[1])+1));
                        break;
                    //actual abilities laut notion
                    case "Seer":
                        Deck.NextCard = Deck.GetCard();
                        Seer();
                        break;
                    case "ThreeKings":
                        StartCoroutine(ThreeKings(details[1]));
                        break;
                    case "Switcheroo": //PD
                        //Not working
                        break;
                    case "Cashback":
                        //Gain 50% of your bet back on a bust. On a win, you only gain 75%.
                        GlobalData.BetLossRate = 50;
                        GlobalData.BetPayoutRate = 75;
                        break;
                    case "Restart":
                        //Clear
                        StartCoroutine(Restart());
                        break;
                    case "Ass":
                        //E1:1 for Ass
                        StartCoroutine(Ass(details[1]));
                        break;
                    case "Shortcut":
                        GlobalData.DealerWinCond = 17;
                        GlobalData.PlayerWinCond = 17;
                        break;
                    case "Joker":
                        //Joker
                        if (details[1] == "P") JokerP();
                        else if (details[1] == "D")
                        {
                            StartCoroutine(JokerD());
                        }
                        break;
                    case "TheTwins":
                        //Pull 2 cards and look at them. Choose which one to add to your count.
                        if (details[1] == "P") TwinsP();
                        else if (details[1] == "D")
                        {
                            StartCoroutine(TwinsD());
                        }
                        break;
                    case "Destroy":
                        //destroy/anull first special card dealer uses
                        break;
                    case "Gambit":
                        PlayerPrefs.SetInt("Bet",PlayerPrefs.GetInt("Bet")*2);
                        break;
                    case "Piggibank":
                        BetUi.SetActive(true);
                        //BetUi.transform.GetChild(4).gameObject.SetActive(true);
                        break;
                }   
            }
        }
    }
    //NOT TESTED

    private IEnumerator Switcheroo(string mode)
    {
        if (mode == "P")
        {
            StartCoroutine(SpawnCards(new Vector3(-2 * Dealer.GetHandCards().Count, 0, -6), Dealer.GetHandCards().Count, 3, Dealer.GetHandCards().Keys.ToArray(), true));
            StartCoroutine(SpawnCards(new Vector3(-2 * Dealer.GetHandCards().Count, 0, -6), playerHandler.GetHandCards().Count, 4, playerHandler.GetHandCards().Keys.ToArray(), true));
        }
        else if (mode == "D")
        {
            yield return StartCoroutine(DisplaySPC("SPC5"));
            if (Dealer.TotalValue > 21)
            {
                //FUCK YOU
                //KeyValuePair<string,int> pc = playerHandler.playerCards.Where(c => Dealer.TotalValue - c.Value <= 21).FirstOrDefault();
            }
        }
    }


    private IEnumerator Ass(string mode)
    {
        GlobalData.DuplicateAmt++;
        string key = $"EA:{GlobalData.DuplicateAmt}";
        yield return StartCoroutine(DisplaySPC("SPC8"));
        if (mode == "P") playerHandler.AddCard($"EA:{GlobalData.DuplicateAmt}", 11);
        else if (mode == "D") Dealer.AddCard($"EA:{GlobalData.DuplicateAmt}", 11);
    }
    private IEnumerator ThreeKings(string mode)
    {
        yield return StartCoroutine(DisplaySPC("SPC4"));
        if (mode == "P") playerHandler.ValueModifier -= 3;
        else if (mode == "D") Dealer.ValueModifier -= 3;
    }
    private IEnumerator DisplaySPC(string SPC)
    {
        audioManager.PlaySFX(audioManager.PlayingMagicCard);
        yield return StartCoroutine(SpawnCards(new Vector3(0, 0, -6), 1, 0, new string[] { SPC }, true));
    }

    private void JokerP()
    {
        Debug.Log("P");
        StartCoroutine(SpawnCards(new Vector3(-4, 0, -6), 3, 2, new string[] { "EA","E5","E13" }, false));
    }
    private IEnumerator JokerD()
    {
        Debug.Log("D");
        yield return StartCoroutine(DisplaySPC("SPC9"));
        GlobalData.DuplicateAmt++;
        if (Dealer.TotalValue + 11 <= 21) Dealer.AddCard($"EA:{GlobalData.DuplicateAmt}",1); //Ace
        else if (Dealer.TotalValue + 10 <= 21) Dealer.AddCard($"E10:{GlobalData.DuplicateAmt}", 10); //10
        else if (Dealer.TotalValue + 10 <= 21) Dealer.AddCard($"E5:{GlobalData.DuplicateAmt}", 5); //5
        else Dealer.AddCard($"EA:{GlobalData.DuplicateAmt}", 1);//Ace
    }
    private void Seer()
    {
        StartCoroutine(SpawnCards(new Vector3(0, 0, -6), 1, 0, new string[] { Deck.NextCard.Key }, true));
    }

    private void TwinsP()
    {
        Debug.Log("P");
        StartCoroutine(SpawnCards(new Vector3(-2, 0, -6), 2, 1, new string[] { Deck.GetCard().Key, Deck.GetCard().Key }, false));
    }
    private IEnumerator TwinsD()
    {
        Debug.Log("D");
        yield return StartCoroutine(DisplaySPC("SPC11"));
        KeyValuePair<string, int> card1 = Deck.GetCard();
        KeyValuePair<string, int> card2 = Deck.GetCard();
        bool notSuicide1 = card1.Value + Dealer.TotalValue <= 21;
        bool notSuicide2 = card2.Value + Dealer.TotalValue <= 21;
        if (notSuicide1 && notSuicide2)
        {
            if (card1.Value > card2.Value) Deck.NextCard = card1;
            else Deck.NextCard = card2;
        }
        else if (notSuicide1) Deck.NextCard = card1;
        else if (notSuicide2) Deck.NextCard = card2;
        else
        {
            if (card1.Value < card2.Value) Deck.NextCard = card1;
            else Deck.NextCard = card2;
        }
        Dealer.PullCard();
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
        cardClicked = false;
    }

    private GameObject CreateCard(Vector3 position, int clickedUsage, string key)
    {
        GameObject card = new GameObject("Card");
        card.transform.position = position;

        // Get card data
        CardManager CM = FindObjectOfType<CardManager>();
        Sprite s;
        if (key.Contains("SPC"))
        {
            s = SPCSprites.Where(s => s.name.Contains(key.Substring(3))).FirstOrDefault();
        }
        else
        {
            CardManager.DeckConverter(key, out string suit, out int rank);
            s = CM.GetCardSprite(suit, rank);
        }

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

    private IEnumerator Restart()
    {
        StartCoroutine(playerHandler.ClearHand());
        yield return StartCoroutine(Dealer.ClearHand());
        Deck.Clear();
        //Reset Cards
        Dealer.PullInit();
        playerHandler.PullMultipleCards(2);
    }
}

public class CardClickHandler : MonoBehaviour
{
    public AbilityDecoder Decoder { get; set; }
    public string Key { get; set; }
    public int ClickedUsage { get; set; } //NextCard | AddCard | ...

    internal string SwitchKey1, SwitchKey2;

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
            case 3:
                Switcheroo1();
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
        int value;
        string AlteredKey = Key;
        if (Key.Contains('E'))
        {
            value = Deck.DeckCards[Key.Replace('E', 'S')];
        }
        else value = Deck.DeckCards[Key];
        Decoder.playerHandler.AddCard(Key, value);
    }

    private void Switcheroo1()
    {
        SwitchKey1 = Key;
    }
    private void Switcheroo2()
    {
        SwitchKey2 = Key;
        //Switch Action
        Decoder.playerHandler.RemoveCard(SwitchKey2);
        Decoder.playerHandler.AddCard(SwitchKey2, Deck.DeckCards[SwitchKey2]);
        //Decoder.Dealer.
        Decoder.Dealer.AddCard(SwitchKey2, Deck.DeckCards[SwitchKey2]);
    }

}

