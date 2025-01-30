using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SoulBuy : MonoBehaviour,IPointerClickHandler
{
    private PlayerHandler playerHandler;
    private AudioManager audio;
    

    public void OnPointerClick(PointerEventData eventData)
    {
            if (PlayerPrefs.GetInt("Money") < 20000)
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
        playerHandler = GameObject.Find("Player").GetComponent<PlayerHandler>();
        audio = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
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
