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
    private int dealerName;
    private AudioManager audio;

    // Start is called before the first frame update
    void Start()
    {
        DropHandler = SPCSlotL.GetComponent<DropHandler>();
        SpecialCardsList = GameHandler.GetComponent<SpecialCardsList>();
        SpriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        ChangeDealer();
        audio = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
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
        ValueModifier = 0;
        OpenCard = new KeyValuePair<string, int>();
        foreach (Transform card in DealerCardParent.transform)
        {
            GameObject.Destroy(card.gameObject);
        }
        yield return null;
    }

    public Dictionary<string, int> DealerHand = new Dictionary<string, int>();
    public KeyValuePair<string, int> OpenCard { get; set; }
    public int ValueModifier;
    public int TotalValue => DealerHand.Values.Sum()+ValueModifier;


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

    public void AddCard(string key, int value)
    {
        KeyValuePair<string, int> card = new KeyValuePair<string, int>(key, value);
        DealerHand.Add(card.Key, card.Value);
        DisplayDealerCards(card.Key);

        List<string> keysToModify = new List<string>();
        foreach (string nkey in DealerHand.Keys)
        {
            if (key.Contains("A") && DealerHand.Values.Sum() > 21)
            {
                keysToModify.Add(key);
            }
        }
        foreach (string nkey in keysToModify)
        {
            DealerHand[key] = 1;
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
        dealerName = index;
        Abilities = DealerAbilities[index % 3].ToList();
    }

    private string[][] DealerAbilities = new string[][]
    {
        new string[] { "Double-Sided Blade", "ThreeKings", "TheTwins"},
        new string[] { "Player+1", "Switcheroo" }, //Restart is pain so no
        new string[] { "Ass", "Joker" } //Destroy makes no sense
    };
    private List<string> Abilities;
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
                Filter = new string[] { "Ass" }; //"Switcheroo"
            }

            // Player under 21 abilities
            /*
            else if (playerHandler != null && playerHandler.curSum <= 21)
            {
                Filter = new string[] { "Blade", "Switcheroo" };
            }*/

            // Under 17 abilities
            else if (TotalValue <= 17)
            {
                Filter = new string[] { "ThreeKings", "TheTwins", "Joker", "Ass" }; //"Switcheroo"
            }
            else Filter = new string[] { "Ass" };
            StartCoroutine(PickAndRemoveAbility(Filter));
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
        if (SpecialCardsList.SpecialCardsUi.ContainsKey(selectedAbility))
        {
            DropHandler.TriggerSPCEffect(SpecialCardsList.DealerSpecialCardsUi[selectedAbility]);
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

        switch (dealerName%3)
        {
            case 0:
                audio.PlaySFX(audio.DragonLaugh);
                break;
            case 1:
                audio.PlaySFX(audio.DevilLaugh);
                break;
            case 2:
                audio.PlaySFX(audio.DinoLaugh);
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
