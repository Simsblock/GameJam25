using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine;

public abstract class Participant : MonoBehaviour
{
    //General
    [SerializeField]
    protected GameObject GameHandler;
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
        DisplayCard(card.Key);
        AceCheck(true);
    }

    public void RemoveCard(string cardKey, int WinCondition)
    {
        HandCards.Remove(cardKey);
        if (TotalValue <= WinCondition-10 && HandCards.Keys.Any(k => k.Contains("A")))
        {
            AceCheck(false);
        }
    }

    public void PullCard()
    {
        KeyValuePair<string, int> card = Deck.PullCard();
        AddCard(card.Key,card.Value);
    }

    public void PullMultipleCards(int count)
    {
        for(;count > 0;count--)
        {
            PullCard();
        }
        AceCheck(true);
    }


    //Display Cards
    public virtual IEnumerator DisplayCard(string cardKey) //public for now
    {
        yield return null;
    }
}