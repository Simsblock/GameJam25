using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SoulBuy : MonoBehaviour,IPointerClickHandler
{
    [SerializeField]
    private int price;
    [SerializeField]
    private TMP_Text priceTag;
    private Player playerHandler;
    private AudioManager audio;
    

    public void OnPointerClick(PointerEventData eventData)
    {
            if (PlayerPrefs.GetInt("Money") < price)
            {
                //maby animation oda so
                return;
            }
            PlayBuySound();
        //end game in victorry
    }

    // Start is called before the first frame update
    void Start()
    {
        playerHandler = GameObject.Find("Player").GetComponent<Player>();
        audio = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
        priceTag.text=price.ToString();
    }

    private void PlayBuySound()
    {
        System.Random r = new System.Random();
        switch (r.Next(3))
        {
            case 0:
                audio.PlaySFX(audio.BuyingMagicCard);
                break;
            case 1:
                audio.PlaySFX(audio.BuyingMagicCard2);
                break;
            case 2:
                audio.PlaySFX(audio.BuyingMagicCard3);
                break;
        }
    }
}
