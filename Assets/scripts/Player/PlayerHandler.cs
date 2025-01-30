using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHandler : MonoBehaviour
{
    private List<string> specialCards;
    public Dictionary<string,int> playerCards;
    public int curSum; //Total Hand Value
    [SerializeField]
    private GameObject GameHandler;
    [SerializeField]
    private GameObject CardHand;
    private DisplaySpecial DisplaySpecial;
    [SerializeField]
    private GameObject CardPrefab, CardParent;
    private CardManager CM;
    private float cardSpace = 0.65f;
    private Vector3 leftCardPos;
    private Vector3 rightCardPos;
    private bool DisplaySPC;
    [SerializeField]
    private Button AbilityBtn;
    private AudioManager audio;

    void Start()
    {
        if(specialCards == null) specialCards = new List<string>();
        if(playerCards == null) playerCards = new Dictionary<string,int>();
        DisplaySpecial = CardHand.GetComponent<DisplaySpecial>();
        CM = CardParent.GetComponent<CardManager>();
        //SPC
        DisplaySPC = false;
        specialCards = GlobalData.LoadSPC();
        audio = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
        AddSpecialCard("Joker");
        AddSpecialCard("Ass");
        AddSpecialCard("DiceDefault");
    }

    public void PullMulti(int count)
    {
        if (playerCards.Count < 10)
        {
            while (count > 0)
            {
                PullCard();
                count--;
            }
            audio.PlaySFX(audio.placeCard);
        }
    }

    public void PullCard()
    {
        if (playerCards.Count < 10)
        {
            var card = Deck.PullCard();
            playerCards.Add(card.Key, card.Value);
            DisplayPlayerCards(card.Key);
            curSum += card.Value;
            //checks for aces n shit
            if (playerCards.Keys.Any(k => k.Contains("A")) && curSum > 21)
            {
                List<string> keysToModify = new List<string>();
                foreach (var item in playerCards.Where(p => p.Key.Contains("A")))
                {
                    if (curSum > 21 && !(playerCards[item.Key]==1))
                    {
                        keysToModify.Add(item.Key);
                        curSum -= 10;
                    }
                }
                foreach (string key in keysToModify)
                {
                    playerCards[key] = 1;
                }
            }
            RandomDrawSound();
        }
    }

    public void RemoveCard(string cardKey)
    {
        curSum -= playerCards[cardKey];
        playerCards.Remove(cardKey);
        //checks for aces too :)
        if (curSum < 12&& playerCards.Keys.Any(k => k.Contains("A")))
        {
            List<string> keysToModify = new List<string>();
            foreach (var item in playerCards.Where(p => p.Key.Contains("A")))
            {
                if (curSum > 21 && !(playerCards[item.Key] == 11))
                {
                    keysToModify.Add(item.Key);
                    curSum += 10;
                }
            }
            foreach (string key in keysToModify)
            {
                playerCards[key] = 11;
            }
        }
    }

    public void ClearSpecialCards()
    {
        specialCards = new List<string>();
        PlayerPrefs.SetString("SPCs", "");
    }

    public void AddSpecialCard(string name)
    {
        specialCards.Add(name);
        GlobalData.SaveSPC(specialCards);
    }
    public void RemoveSpecialCard(string name)
    {
        if (name.Contains("Dice"))
        {
            return;
        }
        specialCards.Remove(name);
        GlobalData.SaveSPC(specialCards);
    }

    public void UpdateDisplay()
    {
        if(DisplaySPC) DisplaySpecial.Display(specialCards);
    }

    public void DisplaySpecialCards()
    {
        if (!DisplaySPC)
        {
            AbilityBtn.transform.localPosition = new Vector3(175, 180, 1);
            DisplaySpecial.Display(specialCards);
            DisplaySPC = true;
        }
        else
        {
            AbilityBtn.transform.localPosition = new Vector3(365, 180, 1);
            DisplaySpecial.EndDisplay();
            DisplaySPC = false;
        }
        
    }
    public void DisplayPlayerCards(string cardKey)
    {
        GameObject card = Instantiate(CardPrefab);
        card.transform.SetParent(CardParent.transform);
        Vector3 cardPos = new Vector3(0f, 0f, 0f);
        int childrenAmt = CardParent.transform.childCount;
        //Debug.Log(childrenAmt);
        // Check if first
        if (childrenAmt == 1)
        {
            cardPos = new Vector3(-0.65f, 0f, 0f);
            leftCardPos = cardPos;
            rightCardPos = cardPos;
        }
        else if(childrenAmt % 2 == 0)
        {
            cardPos = rightCardPos + new Vector3(cardSpace*2,0f,0f);
            rightCardPos = cardPos;
        }
        else if (childrenAmt % 2 != 0)
        {
            cardPos = leftCardPos + new Vector3(-cardSpace*2, 0f, 0f);
            leftCardPos = cardPos;
        }
        card.transform.localPosition = cardPos;
        CardManager.DeckConverter(cardKey, out string suit, out int rank);
        Sprite s= CM.GetCardSprite(suit, rank);
        SpriteRenderer spriteRenderer = card.GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = s;
    }

    public void AddCard(string key, int value)
    {
        DisplayPlayerCards(key);
        playerCards.Add(key, value);
        curSum += value;
        if (playerCards.Keys.Any(k => k.Contains("A")) && curSum > 21)
        {
            List<string> keysToModify = new List<string>();
            foreach (var item in playerCards.Where(p => p.Key.Contains("A")))
            {
                if (curSum > 21 && !(playerCards[item.Key] == 1))
                {
                    keysToModify.Add(item.Key);
                    curSum -= 10;
                }
            }
            foreach (string nkey in keysToModify)
            {
                playerCards[nkey] = 1;
            }
        }
    }

    public IEnumerator ClearBaseCards()
    {
        curSum = 0;
        playerCards = new Dictionary<string, int>();
        foreach (Transform card in CardParent.transform)
        {
            GameObject.Destroy(card.gameObject);
        }
        yield return null;
    }

    private void RandomDrawSound()
    {
        System.Random r= new System.Random();
        switch (r.Next(5))
        {
            case 0:
                audio.PlaySFX(audio.drawCard);
                break;
            case 1:
                audio.PlaySFX(audio.drawCard1);
                break;
            case 2:
                audio.PlaySFX(audio.drawCard2);
                break;
            case 3:
                audio.PlaySFX(audio.drawCard3);
                break;
            case 4:
                audio.PlaySFX(audio.drawCard4);
                break;
        }

    }
}
