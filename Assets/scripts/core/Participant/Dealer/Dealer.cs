using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Unity.Collections;
using UnityEngine;
using static UnityEditor.Progress;

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
    private List<string> Abilities;

    [SerializeField]
    public GameObject Player, SPCSlotL; //Slot is ugly af, aber es geht fast und das brauchma jetzt
    private DropHandler DropHandler;
    private SpecialCardsList SpecialCardsList;
    private int dealerName;
    //Dealer Abilities End

    //Display Cards
    [SerializeField]
    private GameObject DealerCardParent, CardPrefab;
    [SerializeField]
    private Sprite CardBack;
    private Vector3 leftCardPos;
    //Display Cards End

    // Start is called before the first frame update
    void Start()
    {
        WinCondition = GlobalData.DealerWinCond;
        //Load Components & Objects
        DropHandler = SPCSlotL.GetComponent<DropHandler>();
        SpecialCardsList = GameHandler.GetComponent<SpecialCardsList>();
        SpriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        Audio = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
        //Initialize First Dealer
        ChangeDealer();
    }

    //Edit HandCards
    public void PullInit()
    {
        //leftCardPos
        leftCardPos = new Vector3(0, 0, 0);
        //Pull open first Card
        KeyValuePair<string, int> card = Deck.PullCard();
        OpenCard = card;
        HandCards.Add(card.Key,card.Value);
        StartCoroutine(DisplayCard(card.Key));
        //Pull second hidden Card
        card = Deck.PullCard();
        HandCards.Add(card.Key, card.Value);
        StartCoroutine(DisplayCard(card.Key));
    }
    
    public IEnumerator PullRest()
    {
        StartCoroutine(BounceEffect(gameObject));
        yield return StartCoroutine(PullRestInternally());
    }
    public IEnumerator PullRestInternally()
    {
        TurnCardsOver();
        KeyValuePair<string, int> card;
        while (TotalValue < MaxVal)
        {
            {
                card = Deck.PullCard();
                if (!HandCards.ContainsKey(card.Key)) //Working Card
                {
                    yield return StartCoroutine(DisplayCardWithDelay(card));
                }
            }
        }
        AceCheck(true);
        if (TotalValue < MaxVal) yield return StartCoroutine(PullRestInternally());
    }

    public IEnumerator ClearHand()
    {
        //HandCards.Clear();
        HandCards = new Dictionary<string, int>();
        ValueModifier = 0;
        OpenCard = new KeyValuePair<string, int>();
        foreach (Transform card in DealerCardParent.transform)
        {
            GameObject.Destroy(card.gameObject);
        }
        yield return null;
    }
    //Edit HandCards END


    public void ChangeDealer()
    {
        System.Random rand = new System.Random();
        int index = rand.Next(Dealers.Length);
        SpriteRenderer.sprite = Dealers[index];
        index++;
        if (index < 3) Abilities = DealerAbilities[index-1].ToList();
        else Abilities = DealerAbilities[index/3-1].ToList();
        dealerName = index;
        Abilities = DealerAbilities[index % 3].ToList();
    }

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
            PlayerHandler playerHandler = Player.GetComponent<PlayerHandler>();
            // Over 21 abilities
            string[] Filter = new string[] { };
            if (TotalValue > 21)
            {
                Filter = new string[] { "ThreeKings" }; //"Switcheroo"
            }

            // Player High Number
            else if (playerHandler != null && playerHandler.curSum >= 17 && playerHandler.curSum <= 21)
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


    public override IEnumerator DisplayCard(string cardKey)
    {
        GameObject card = Instantiate(CardPrefab);
        card.transform.localScale = new Vector3(2.5f, 3f, 2.5f);
        card.name = cardKey;
        card.transform.SetParent(DealerCardParent.transform);
        Vector3 cardPos = new Vector3(1.3f, 0f, 0f);
        int childrenAmt = DealerCardParent.transform.childCount;
        // Check if first
        if (childrenAmt == 1)
        {
            cardPos = new Vector3(0f, 0f, 0f);
            leftCardPos = cardPos;
        } else leftCardPos += cardPos;
        if (childrenAmt == 1 || GameHandler.GetComponent<GameHandler>().stand >= 3)
        {
            CardManager.DeckConverter(cardKey, out string suit, out int rank);
            Sprite s = DealerCardParent.GetComponent<CardManager>().GetCardSprite(suit, rank);
            card.GetComponent<SpriteRenderer>().sprite = s;
        }
        else card.GetComponent<SpriteRenderer>().sprite = CardBack;
        card.transform.localPosition = leftCardPos;
        yield return null;
    }

    public void TurnCardsOver()
    {
        foreach (Transform child in DealerCardParent.transform)
        {
            string cardKey = child.name;
            CardManager.DeckConverter(cardKey, out string suit, out int rank);
            Sprite s = DealerCardParent.GetComponent<CardManager>().GetCardSprite(suit, rank);
            child.GetComponent<SpriteRenderer>().sprite = s;
        }
    }
    //Animation
    private IEnumerator DisplayCardWithDelay(KeyValuePair<string, int> card)
    {
        yield return new WaitForSeconds(0.6f);
        HandCards.Add(card.Key, card.Value);
        yield return StartCoroutine(DisplayCard(card.Key));
    }

    private IEnumerator BounceEffect(GameObject cardObject)
    {
        Vector3 originalPos = cardObject.transform.localPosition;
        Vector3 bouncePos = originalPos + new Vector3(0, 0.5f, 0);

        float duration = 0.1f;
        float elapsedTime = 0f;

        switch (dealerName%3)
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
