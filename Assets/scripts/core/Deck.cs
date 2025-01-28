using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class Deck
{
    public static KeyValuePair<string, int> PullCard()
    {
        System.Random rand = new System.Random();
        KeyValuePair<string, int> pulled;
        do
        {
            pulled = DeckCards.ElementAt(rand.Next(DeckCards.Count));
        }
        while (PulledCards.Contains(pulled.Key));
        PulledCards.Add(pulled.Key);
        return pulled;
    }

    public static void Clear()
    {
        PulledCards.Clear();
    }

    public static List<string> PulledCards = new List<string>();

    public static readonly Dictionary<string, int> DeckCards = new Dictionary<string, int>
    {
        // Hearts
        ["H2"] = 2,
        ["H3"] = 3,
        ["H4"] = 4,
        ["H5"] = 5,
        ["H6"] = 6,
        ["H7"] = 7,
        ["H8"] = 8,
        ["H9"] = 9,
        ["H10"] = 10,
        ["H11"] = 10,
        ["H12"] = 10,
        ["H13"] = 10,
        ["H1"] = 11,

        // Diamonds
        ["D2"] = 2,
        ["D3"] = 3,
        ["D4"] = 4,
        ["D5"] = 5,
        ["D6"] = 6,
        ["D7"] = 7,
        ["D8"] = 8,
        ["D9"] = 9,
        ["D10"] = 10,
        ["D11"] = 10,
        ["D12"] = 10,
        ["D13"] = 10,
        ["D1"] = 11,

        // Clubs
        ["C2"] = 2,
        ["C3"] = 3,
        ["C4"] = 4,
        ["C5"] = 5,
        ["C6"] = 6,
        ["C7"] = 7,
        ["C8"] = 8,
        ["C9"] = 9,
        ["C10"] = 10,
        ["C11"] = 10,
        ["C12"] = 10,
        ["C13"] = 10,
        ["C1"] = 11,

        // Spades
        ["S2"] = 2,
        ["S3"] = 3,
        ["S4"] = 4,
        ["S5"] = 5,
        ["S6"] = 6,
        ["S7"] = 7,
        ["S8"] = 8,
        ["S9"] = 9,
        ["S10"] = 10,
        ["S11"] = 10,
        ["S12"] = 10,
        ["S13"] = 10,
        ["S1"] = 11
    };
}
