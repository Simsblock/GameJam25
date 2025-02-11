using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Unity.Collections;
using UnityEngine;

public class Dealer : Participant
{
    //Dealer AI
    [Tooltip("Max Value where Dealer pulls another Card")]
    [SerializeField] private int MaxVal; //17 standart
    //Dealer AI

    //Dealer Display
    [SerializeField]
    public Sprite[] Dealers;
    private SpriteRenderer SpriteRenderer;
    //Dealer Display End

    //Open Dealer Card
    public KeyValuePair<string, int> OpenCard { get; private set; } //The first card the Dealer pulls, is displayed to Player

    //Dealer Abilities
    private Player Player;
    private List<string> Abilities;

    private DropHandler DropHandler;
    private SpecialCardsList SpecialCardsList;
    private int dealerName;
    //Dealer Abilities End

    //Display Cards
    private Vector3 leftCardPos;
    //Display Cards End

    // Start is called before the first frame update
    void Awake()
    {
        //Load Components & Objects
        ParentAwake();
        Player = GameObject.Find("Player").GetComponent<Player>();
        DropHandler = GameObject.Find("SPCSlotL").GetComponent<DropHandler>(); //Could be betta
        SpecialCardsList = GameHandler.GetComponent<SpecialCardsList>();
        SpriteRenderer = gameObject.GetComponent<SpriteRenderer>();
    }
    
    void Start()
    {
        WinCondition = GlobalData.DealerWinCond;
        //Initialize First Dealer
        ChangeDealer();
    }

    //Edit HandCards
    public void PullInit()
    {
        //leftCardPos
        leftCardPos = new Vector3(0, 0, 0);
        //Pull open first Card
        KeyValuePair<string, int> card = PullCard();
        OpenCard = card;
        StartCoroutine(DisplayCard(card.Key)); //eventuell DisplayCard gleich in Pullcard einbauen?
        //Pull second hidden Card
        card = PullCard();
        StartCoroutine(DisplayCard(card.Key));
    }

    public IEnumerator PullRest()
    {
        StartCoroutine(BounceEffect(gameObject));
        TurnCardsOver();
        KeyValuePair<string, int> card;
        while (TotalValue < MaxVal)
        {
            card = Deck.PullCard();
            if (!HandCards.ContainsKey(card.Key)) //Kinda Useless Check init? cause Deck allready does the Deck-Check, what do u think Max
            {
                AddCard(card);
                yield return StartCoroutine(DisplayCardWithDelay(card.Key));
            }
        }
        if (TotalValue < MaxVal) yield return StartCoroutine(PullRest());
    }

    public override IEnumerator ClearHand()
    {
        StartCoroutine(base.ClearHand());
        OpenCard = new KeyValuePair<string, int>();
        yield return null;
    }

    //ChangeDealer
    public void ChangeDealer()
    {
        System.Random rand = new System.Random();
        int index = rand.Next(Dealers.Length);
        SpriteRenderer.sprite = Dealers[index];
        index++;
        if (index < 3) Abilities = DealerAbilities[index - 1].ToList();
        else Abilities = DealerAbilities[index / 3 - 1].ToList();
        dealerName = index;
        Abilities = DealerAbilities[index % 3].ToList();
    }

    //Abilities
    private string[][] DealerAbilities = new string[][]
    {
        new string[] { "Double-Sided Blade", "ThreeKings", "TheTwins"},
        new string[] { "Player+1", "Switcheroo" }, //Restart is pain so no
        new string[] { "Ass", "Joker" } //Destroy makes no sense
    };
    public IEnumerator UseAbilities()
    {
        for (int i = 0; i < 2; i++)
        {
            System.Random rand = new System.Random();
            // Over 21 abilities
            string[] Filter = new string[] { };
            if (TotalValue > 21)
            {
                Filter = new string[] { "ThreeKings" }; //"Switcheroo"
            }

            // Player High Number
            else if (Player != null && Player.TotalValue >= 17 && Player.TotalValue <= 21)
            {
                Debug.Log("whyy");
                Filter = new string[] { "Player+1" }; //"Switcheroo"
            }

            // Under 17 abilities
            else if (TotalValue <= 17)
            {
                Filter = new string[] { "TheTwins", "Joker", "Ass" }; //"Switcheroo"
            }
            else if (TotalValue == 20)
            {
                Filter = new string[] { "Joker", "Ass" };
            }
            else Filter = new string[] { };
            yield return StartCoroutine(PickAndRemoveAbility(Filter));
        }
        yield return null;
    }

    private IEnumerator PickAndRemoveAbility(string[] abilityPool)
    {
        // Ensure abilityPool is not null or empty
        if (abilityPool == null || abilityPool.Length == 0)
        {
            Debug.LogWarning("Ability pool is null or empty.");
            yield break; // Exit coroutine early
        }

        // Ensure Abilities is not null or empty
        if (Abilities == null || Abilities.Count == 0)
        {
            Debug.LogWarning("Abilities list is null or empty.");
            yield break; // Exit coroutine early
        }

        // Filter abilities that exist in both `abilityPool` and `Abilities`
        var validAbilities = abilityPool.Where(a => Abilities.Contains(a)).ToList();
        // Check if there are no valid abilities to use
        if (validAbilities.Count == 0)
        {
            Debug.LogWarning("No valid abilities found to use.");
            yield break; // Exit coroutine early
        }
        // Pick a random ability
        string selectedAbility = validAbilities[UnityEngine.Random.Range(0, validAbilities.Count)];
        // Call Ability

        if (SpecialCardsList.DealerSpecialCardsUi.ContainsKey(selectedAbility))
        {
            DropHandler.TriggerSPCEffect(SpecialCardsList.DealerSpecialCardsUi[selectedAbility]);
            yield return new WaitForSeconds(2f);
            Debug.Log($"Used ability: {selectedAbility}");
        }
        else
        {
            Debug.LogError($"Key '{selectedAbility}' not found in SpecialCardsUi!");
            yield break;
        }
        // Remove from Abilities (convert array to list first if needed)
        List<string> abilitiesList = Abilities.ToList();
        if (abilitiesList.Remove(selectedAbility))
        {
            Abilities = abilitiesList; // Update back if Abilities is an array
            Debug.Log($"Successfully removed ability: {selectedAbility}");
        }
        else Debug.LogError($"Failed to remove ability: {selectedAbility}");
        yield return null;
    }

    //Display Card
    protected override bool ShouldRevealCard(int childrenAmt)
    {
        return childrenAmt == 1 || GameHandler.stand >= 3;
    }

    protected override Vector3 GetCardPosition(int childrenAmt)
    {
        Vector3 cardPos = new Vector3(1.3f, 0f, 0f);
        if (childrenAmt == 1)
        {
            cardPos = Vector3.zero;
            leftCardPos = cardPos;
        }
        else
        {
            leftCardPos += cardPos;
        }
        return leftCardPos;
    }

    public void TurnCardsOver()
    {
        foreach (Transform child in CardParent.transform)
        {
            string cardKey = child.name;
            CardManager.DeckConverter(cardKey, out string suit, out int rank);
            Sprite s = CardParent.GetComponent<CardManager>().GetCardSprite(suit, rank);
            child.GetComponent<SpriteRenderer>().sprite = s;
        }
    }

    //Animation
    private IEnumerator DisplayCardWithDelay(string cardKey)
    {
        yield return new WaitForSeconds(0.6f);
        yield return StartCoroutine(DisplayCard(cardKey));
    }

    private IEnumerator BounceEffect(GameObject cardObject)
    {
        Vector3 originalPos = cardObject.transform.localPosition;
        Vector3 bouncePos = originalPos + new Vector3(0, 0.5f, 0);

        float duration = 0.1f;
        float elapsedTime = 0f;

        switch (dealerName % 3)
        {
            case 0:
                Audio.PlaySFX(Audio.DragonLaugh);
                break;
            case 1:
                Audio.PlaySFX(Audio.DevilLaugh);
                break;
            case 2:
                Audio.PlaySFX(Audio.DinoLaugh);
                break;
        }
        // Move up
        while (elapsedTime < duration)
        {
            cardObject.transform.localPosition = Vector3.Lerp(originalPos, bouncePos, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Move down
        elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            cardObject.transform.localPosition = Vector3.Lerp(bouncePos, originalPos, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        cardObject.transform.localPosition = originalPos; // Ensure exact original position
    }
}