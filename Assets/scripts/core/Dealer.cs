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
    private Vector3 leftCardPos;

    // Start is called before the first frame update
    void Start()
    {
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
        //Pull second hidden Card
        card = Deck.PullCard();
        DealerHand.Add(card.Key, card.Value);
    }
    public void PullRest()
    {
        KeyValuePair<string, int> card;
        while (TotalValue < MaxVal)
        {
            {
                card = Deck.PullCard();
                if (!DealerHand.ContainsKey(card.Key))
                {
                    DealerHand.Add(card.Key, card.Value);
                }
            }
        }
        foreach (string key in DealerHand.Keys)
        {
            if (key.Contains("A") && DealerHand.Values.Sum() > 21)
            {
                DealerHand[key] = 1;
            }
        }
        if (TotalValue < MaxVal) PullRest();
    }

    public void ClearHand()
    {
        DealerHand.Clear();
        OpenCard = new KeyValuePair<string, int>();
    }

    public Dictionary<string, int> DealerHand = new Dictionary<string, int>();
    public KeyValuePair<string, int> OpenCard { get; set; }
    public int TotalValue => DealerHand.Values.Sum();


    public void PullMulti(int count)
    {
        while (count > 0)
        {
            KeyValuePair<string, int> card = Deck.PullCard();
            DealerHand.Add(card.Key, card.Value);
            count--;
        }
        

        foreach (string key in DealerHand.Keys)
        {
            if (key.Contains("A") && DealerHand.Values.Sum() > 21)
            {
                DealerHand[key] = 1;
            }
        }
        
    }

    public void UseAbilities()
    {

    }



    [SerializeField]
    public Sprite[] Dealers;
    private SpriteRenderer SpriteRenderer;

    public void ChangeDealer() //ONLY SPRITE ATM
    {
        System.Random rand = new System.Random();
        int index = rand.Next(Dealers.Length);
        SpriteRenderer.sprite = Dealers[index];
    }


    public void DisplayPlayerCards(string cardKey)
    {
        GameObject card = Instantiate(CardPrefab);
        card.transform.SetParent(DealerCardParent.transform);
        Vector3 cardPos = new Vector3(0f, 0f, 0f);
        int childrenAmt = DealerCardParent.transform.childCount;
        // Check if first
        if (childrenAmt == 1)
        {
            cardPos = new Vector3(-0.65f, 0f, 0f);
            leftCardPos = cardPos;
        }
        card.transform.localPosition = cardPos;
        CardManager.DeckConverter(cardKey, out string suit, out int rank);
        //Sprite s = CM.GetCardSprite(suit, rank);
        SpriteRenderer spriteRenderer = card.GetComponent<SpriteRenderer>();
        //spriteRenderer.sprite = s;
    }
    

}
