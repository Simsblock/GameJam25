using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using UnityEngine;

public class Dealer : MonoBehaviour
{
    [SerializeField]
    private GameObject GameHandler;
    [Tooltip("to determin the playstyle of the Dealer")]
    [SerializeField] private int MaxVal;
    //Display Cards
    [SerializeField]
    public GameObject DealerCardParent, CardPrefab;
    [SerializeField]
    public Sprite CardBack;
    private Vector3 leftCardPos;
    //for Abilities
    [SerializeField]
    public GameObject Player, SPCSlotL; //Slot is ugly af, aber es geht fast und das brauchma jetzt
    private DropHandler DropHandler;
    private SpecialCardsList SpecialCardsList;

    // Start is called before the first frame update
    void Start()
    {
        DropHandler = SPCSlotL.GetComponent<DropHandler>();
        SpecialCardsList = GameHandler.GetComponent<SpecialCardsList>();
        SpriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        ChangeDealer();
    }

    public void PullInit()
    {
        Debug.Log("INIT");
        //leftCardPos
        leftCardPos = new Vector3(0, 0, 0);
        //Pull open first Card
        KeyValuePair<string, int> card = Deck.PullCard();
        OpenCard = card;
        DealerHand.Add(card.Key,card.Value);
        StartCoroutine(DisplayDealerCards(card.Key));
        //Pull second hidden Card
        card = Deck.PullCard();
        DealerHand.Add(card.Key, card.Value);
        StartCoroutine(DisplayDealerCards(card.Key));
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
        List<string> keysToModify = new List<string>();
        while (TotalValue < MaxVal)
        {
            {
                card = Deck.PullCard();
                if (!DealerHand.ContainsKey(card.Key)) //Working Card
                {
                    yield return StartCoroutine(DisplayDealerCardsWithDelay(card));
                }
            }
        }
        foreach (string key in DealerHand.Keys)
        {
            if (key.Contains("A") && DealerHand.Values.Sum() > 21)
            {
                keysToModify.Add(key);
            }
        }
        foreach (string key in keysToModify)
        {
            DealerHand[key] = 1;
        }
        if (TotalValue < MaxVal) yield return StartCoroutine(PullRestInternally());
    }

    public IEnumerator ClearHand()
    {
        DealerHand.Clear();
        OpenCard = new KeyValuePair<string, int>();
        foreach (Transform card in DealerCardParent.transform)
        {
            GameObject.Destroy(card.gameObject);
        }
        yield return null;
    }

    public Dictionary<string, int> DealerHand = new Dictionary<string, int>();
    public KeyValuePair<string, int> OpenCard { get; set; }
    public int TotalValue => DealerHand.Values.Sum();


    public void PullMulti(int count) //wtf wer hat das so dumm benannt
    {
        while (count > 0)
        {
            KeyValuePair<string, int> card = Deck.PullCard();
            DealerHand.Add(card.Key, card.Value);
            DisplayDealerCards(card.Key);
            count--;
        }

        List<string> keysToModify = new List<string>();
        foreach (string key in DealerHand.Keys)
        {
            if (key.Contains("A") && DealerHand.Values.Sum() > 21)
            {
                keysToModify.Add(key);
            }
        }
        foreach (string key in keysToModify)
        {
            DealerHand[key] = 1;
        }

    }

    private string[][] DealerAbilities = new string[][]
    {
        new string[] { "Double-Sided Blade", "ThreeKings", "TheTwins"},
        new string[] { "Player+1", "Switcheroo" }, //Restart is pain so no
        new string[] { "Ass", "Joker" } //Destroy makes no sense
    };
    private List<string> Abilities;
    public void UseAbilities()
    {
        System.Random rand = new System.Random();

        // Over 21 abilities
        string[] Over21Abilities = new string[] { "ThreeKings", "Switcheroo" };
        if (TotalValue > 21)
        {
            PickAndRemoveAbility(Over21Abilities);
        }

        // Player High Number
        string[] PlayerHigh = new string[] { "Player+1", "Switcheroo" };
        PlayerHandler playerHandler = Player.GetComponent<PlayerHandler>();
        if (playerHandler != null && playerHandler.curSum >= 17 && playerHandler.curSum <= 21)
        {
            PickAndRemoveAbility(PlayerHigh);
        }

        // Player under 21 abilities
        string[] PlayerOver21Abilities = new string[] { "Double-sided Blade", "Switcheroo" };
        if (playerHandler != null && playerHandler.curSum <= 21)
        {
            PickAndRemoveAbility(PlayerOver21Abilities);
        }

        // Under 17 abilities
        string[] Under17Abilities = new string[] { "ThreeKings", "Switcheroo" };
        if (TotalValue <= 17)
        {
            PickAndRemoveAbility(Under17Abilities);
        }
    }

    private void PickAndRemoveAbility(string[] abilityPool)
    {
        // Filter abilities that exist in both `abilityPool` and `Abilities`
        var validAbilities = abilityPool.Where(ability => Abilities.Contains(ability)).ToList();

        if (validAbilities.Count > 0)
        {
            System.Random rand = new System.Random();
            string selectedAbility = validAbilities[rand.Next(validAbilities.Count)];
            //Call Ability
            DropHandler.TriggerSPCEffect(SpecialCardsList.SpecialCardsUi[selectedAbility]);
            // Remove from Abilities
            Abilities.Remove(selectedAbility);
            Console.WriteLine($"Used ability: {selectedAbility}");
        }
    }


    [SerializeField]
    public Sprite[] Dealers;
    private SpriteRenderer SpriteRenderer;
    public void ChangeDealer() //ONLY SPRITE ATM
    {
        System.Random rand = new System.Random();
        int index = rand.Next(Dealers.Length);
        SpriteRenderer.sprite = Dealers[index];
        index++;
        if (index < 3) Abilities = DealerAbilities[index-1].ToList();
        else Abilities = DealerAbilities[index/3-1].ToList();
    }

    public IEnumerator DisplayDealerCards(string cardKey)
    {
        GameObject card = Instantiate(CardPrefab);
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
        if (childrenAmt == 1 || GameHandler.GetComponent<GameHandler>().stand == 3)
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
    private IEnumerator DisplayDealerCardsWithDelay(KeyValuePair<string, int> card)
    {
        yield return new WaitForSeconds(0.6f);
        DealerHand.Add(card.Key, card.Value);
        yield return StartCoroutine(DisplayDealerCards(card.Key));
    }

    private IEnumerator BounceEffect(GameObject cardObject)
    {
        Vector3 originalPos = cardObject.transform.localPosition;
        Vector3 bouncePos = originalPos + new Vector3(0, 0.5f, 0);

        float duration = 0.1f;
        float elapsedTime = 0f;

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
