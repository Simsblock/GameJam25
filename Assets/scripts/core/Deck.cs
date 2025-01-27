using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Deck : MonoBehaviour
{
    // Start is called before the first frame update
    void Start(){}
    // Update is called once per frame
    void Update(){}

    public KeyValuePair<string, int> PullCard()
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

    public void Clear()
    {
        PulledCards.Clear();
    }

    public List<string> PulledCards = new List<string>();

    public readonly Dictionary<string, int> DeckCards = new Dictionary<string, int>
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
        ["HJ"] = 10,
        ["HQ"] = 10,
        ["HK"] = 10,
        ["HA"] = 11,

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
        ["DJ"] = 10,
        ["DQ"] = 10,
        ["DK"] = 10,
        ["DA"] = 11,

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
        ["CJ"] = 10,
        ["CQ"] = 10,
        ["CK"] = 10,
        ["CA"] = 11,

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
        ["SJ"] = 10,
        ["SQ"] = 10,
        ["SK"] = 10,
        ["SA"] = 11
    };
}
