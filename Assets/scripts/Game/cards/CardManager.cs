using System;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    public Sprite[] spadesSprites; // Array for spades
    public Sprite[] diamondsSprites; // Array for diamonds
    public Sprite[] heartsSprites; // Array for hearts
    public Sprite[] clubsSprites; // Array for clubs

    public static void DeckConverter(string key, out string suit, out int rank)
    {
        key = key.Split(':')[0];
        switch (key[0])
        {
            case 'H':
                suit = "diamonds";
                break;
            case 'D':
                suit = "diamonds";
                break;
            case 'C':
                suit = "spades";
                break;
            case 'S':
                suit = "spades";
                break;
            case 'E':
                suit = "diamonds";
                break;
            default:
                throw new ArgumentException("Wallah Billa, was hast du getan??");
        }
        string newKey = key.Substring(1);
        if (newKey == "A") rank = 1;
        else int.TryParse(newKey, out rank);
    }
    
    public Sprite GetCardSprite(string suit, int rank)
    {
        Sprite[] selectedSuit = null;

        switch (suit.ToLower())
        {
            case "spades":
                selectedSuit = spadesSprites;
                break;
            case "diamonds":
                selectedSuit = diamondsSprites;
                break;
            case "hearts":
                selectedSuit = heartsSprites;
                break;
            case "clubs":
                selectedSuit = clubsSprites;
                break;
            default:
                Debug.LogError("Invalid suit: " + suit);
                return null;
        }

        // Ensure the rank is within bounds
        if (rank < 1 || rank > selectedSuit.Length)
        {
            Debug.LogError("Invalid rank: " + rank);
            return null;
        }
        return selectedSuit[rank - 1]; // Return the sprite for the given rank
    }
}
