using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine;

public abstract class Participant : MonoBehaviour
{
    //General
    protected GameHandler GameHandler;
    protected AudioManager Audio;
    protected int WinCondition;

    //HandCards
    protected Dictionary<string, int> HandCards = new Dictionary<string, int>();
    public IReadOnlyDictionary<string, int> GetHandCards()
    {
        return new ReadOnlyDictionary<string, int>(HandCards);
    }

    //Score
    public int ValueModifier { get; set; }
    public int TotalValue => HandCards.Values.Sum() + ValueModifier;

    // CardDisplay
    [SerializeField]
    protected GameObject CardParent, CardPrefab;
    [SerializeField]
    protected Sprite CardBack;

    protected void ParentAwake()
    {
        GameHandler = GameObject.Find("GameHandler").GetComponent<GameHandler>();
        Audio = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    //Methods
    protected bool AceCheck(bool addCard) //returnValue = if something changed
    {
        bool Changed = false;
        int AceCheckValue = 1;
        int TempFixValue = 10;
        int BiggerValue = TotalValue;
        int SmallerValue = WinCondition;
        if (!addCard)
        {
            AceCheckValue = 11;
            TempFixValue = -TempFixValue;
            SmallerValue = TotalValue;
            BiggerValue = WinCondition;
        }

        List<string> keysToModify = new List<string>();
        foreach (string key in HandCards.Keys)
        {
            if (key.Contains("A") && BiggerValue > SmallerValue && !(HandCards[key] == AceCheckValue))
            {
                Changed = true;
                ValueModifier -= TempFixValue;
                keysToModify.Add(key);
            }
        }
        foreach (string key in keysToModify)
        {
            if (addCard) ValueModifier += TempFixValue;
            HandCards[key] = AceCheckValue;
        }
        return Changed;
    }

    //Edit HandCards
    public void AddCard(string key, int value)
    {
        AddCard(new KeyValuePair<string, int>(key, value));
    }
    public void AddCard(KeyValuePair<string, int> card)
    {
        HandCards.Add(card.Key, card.Value);
        StartCoroutine(DisplayCard(card.Key));
        AceCheck(true);
    }

    public void RemoveCard(string cardKey)
    {
        HandCards.Remove(cardKey);
        if (TotalValue <= WinCondition-10 && HandCards.Keys.Any(k => k.Contains("A")))
        {
            AceCheck(false);
        }
    }

    public KeyValuePair<string, int> PullCard()
    {
        KeyValuePair<string, int> card = Deck.PullCard();
        RandomDrawSound();
        AddCard(card);
        return card;
    }

    public void PullMultipleCards(int count)
    {
        for(;count > 0;count--)
        {
            PullCard();
        }
    }
    public virtual IEnumerator ClearHand()
    {
        HandCards = new Dictionary<string, int>();
        ValueModifier = 0;
        foreach (Transform card in CardParent.transform)
        {
            GameObject.Destroy(card.gameObject);
        }
        yield return null;
    }

    //Display Cards
    protected virtual bool ShouldRevealCard(int childrenAmt)
    {
        return true; // Default behavior
    }

    protected abstract Vector3 GetCardPosition(int childrenAmt);

    public virtual IEnumerator DisplayCard(string cardKey)
    {
        GameObject card = CreateCard(cardKey);
        int childrenAmt = CardParent.transform.childCount;

        card.transform.localPosition = GetCardPosition(childrenAmt);

        if (ShouldRevealCard(childrenAmt))
        {
            CardManager.DeckConverter(cardKey, out string suit, out int rank);
            card.GetComponent<SpriteRenderer>().sprite = CardParent.GetComponent<CardManager>().GetCardSprite(suit, rank);
        }
        else
        {
            card.GetComponent<SpriteRenderer>().sprite = CardBack;
        }
        yield return null;
    }

    protected GameObject CreateCard(string cardKey)
    {
        Debug.Log(cardKey);
        GameObject card = Instantiate(CardPrefab);
        card.name = cardKey;
        card.transform.SetParent(CardParent.transform);
        Debug.Log(CardParent.transform.lossyScale);
        return card;
    }

    //Audio
    protected void RandomDrawSound()
    {
        System.Random r = new System.Random();
        switch (r.Next(5))
        {
            case 0:
                Audio.PlaySFX(Audio.drawCard);
                break;
            case 1:
                Audio.PlaySFX(Audio.drawCard1);
                break;
            case 2:
                Audio.PlaySFX(Audio.drawCard2);
                break;
            case 3:
                Audio.PlaySFX(Audio.drawCard3);
                break;
            case 4:
                Audio.PlaySFX(Audio.drawCard4);
                break;
        }

    }
}