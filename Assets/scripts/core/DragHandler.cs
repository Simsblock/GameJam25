using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragHandler : MonoBehaviour,IDragHandler,IBeginDragHandler,IEndDragHandler,IPointerClickHandler
{
    [HideInInspector]public Transform postDragParent;
    [SerializeField]
    private Image image;
    private Transform Content;
    private EffectDto effects;
    private PlayerHandler playerHandler;
    private SpecialCardsList SpecialCards;
    [HideInInspector] public string Shopname;
    private AudioManager audio;
    private GameObject SlotL, SlotR,DiceSlot;
    private Image SlotLImg, SlotRImg,DiceSlotImg;
    private DropHandler SlotLHandler, SlotRHandler;
    private GameObject ClearDrop;
    private SpecialCardsList SPCList;
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (effects.Used || Content.name.Equals("Shop"))
        {
            return;
        }
        image.raycastTarget = false;
        
        //canvasGroup.interactable = false;
        if (postDragParent != null&&postDragParent.name.Contains("SPCSlot"))
        {
            postDragParent = Content;
        }
        else
        {
            postDragParent = transform.parent;
        }
        transform.SetParent(transform.root);
        transform.SetAsLastSibling();
        if (effects.isDice)
        {
            HighlightOn(false);
        }
        else
        {
            HighlightOn(true);
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (effects.Used || Content.name.Equals("Shop"))
        {
            return;
        }
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (Content.name.Equals("Shop"))
        {
            return;
        }
        transform.SetParent(postDragParent);
        image.raycastTarget = true;
        if (effects.isDice)
        {
            HighlightOff(false);
        }
        else
        {
            HighlightOff(true);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (Content.name.Equals("SpecialContent")&&!effects.isDice)
        {
            WhipeToAssign();
            if (SlotL == null || SlotR == null || ClearDrop == null)
            {
                FindStuff();
            }
            HighlightOn(true);
            if (SlotLHandler != null && SlotRHandler != null)
            {
                SlotLHandler.ToAssign = gameObject;
                SlotRHandler.ToAssign = gameObject;
            }
            if (ClearDrop != null)
            {
                ClearDrop.SetActive(true);
            }
        }
        if (Content.name.Equals("Shop"))
        {
            if (PlayerPrefs.GetInt("Money")-100 < effects.Price)
            {
                //maby animation oda so
                return;
            }
            playerHandler.AddSpecialCard(effects.name);
            playerHandler.UpdateDisplay();
            PlayerPrefs.SetInt("Money", PlayerPrefs.GetInt("Money")-effects.Price);
            StartCoroutine(BetShit());
            PlayBuySound();
            Destroy(gameObject);
        }
        
    }

    private void HighlightOn(bool cards)
    {
        if (SlotL == null || SlotR == null || ClearDrop == null)
        {
            FindStuff();
        }        
        if (SlotLImg != null && SlotLImg != null)
        {
            Color c;
            if (cards)
            {
                c = SlotLImg.color;
                c.a = 0.5f;
                SlotLImg.color = c;
                c = SlotRImg.color;
                c.a = 0.5f;
                SlotRImg.color = c;
            }
            else
            {

            }
            c = transform.GetComponent<Image>().color;
            c *= 0.8f;
            transform.GetComponent<Image>().color = c;
        }
    }
    private void HighlightOff(bool dice)
    {
        if (SlotL == null || SlotR == null || ClearDrop == null)
        {
            FindStuff();
        }
        if (SlotLImg != null && SlotLImg != null)
        {
            Color c;
            if (dice)
            {
                c = SlotLImg.color;
                c.a = 0f;
                SlotLImg.color = c;
                c = SlotRImg.color;
                c.a = 0f;
                SlotRImg.color = c;
            }
            
            c = transform.GetComponent<Image>().color;
            c *= 1.2f;
            transform.GetComponent<Image>().color = c;
        }
    }

    private IEnumerator BetShit()
    {
        GameHandler gh = GameObject.Find("GameHandler").GetComponent<GameHandler>();
        if (int.Parse(gh.bet_text.text) > PlayerPrefs.GetInt("Money"))
        {
            PlayerPrefs.SetInt("Bet", PlayerPrefs.GetInt("Money"));
            gh.bet_text.text = $"{PlayerPrefs.GetInt("Bet")}";
        }
        yield return null;
    }

    // Start is called before the first frame update
    void Start()
    {
        effects = GetComponent<EffectDto>();
        Content = transform.parent;
        playerHandler = GameObject.Find("Player").GetComponent<PlayerHandler>();
        SpecialCards =GameObject.Find("GameHandler").GetComponent<SpecialCardsList>();
        audio = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
        FindStuff();
        if (ClearDrop != null)
        {
            ClearDrop.SetActive(false);
        }
    }
    private void FindStuff() {
        SlotL = GameObject.Find("SPCSlotL");
        SlotR = GameObject.Find("SPCSlotR");
        ClearDrop = GameObject.Find("Clearer");
        if (SlotL != null && SlotR != null)
        {
            SlotLImg = SlotL.GetComponent<Image>();
            SlotRImg = SlotR.GetComponent<Image>();
            SlotLHandler = SlotL.GetComponent<DropHandler>();
            SlotRHandler = SlotR.GetComponent<DropHandler>();
        }
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
    public void ResetPos()
    {
        if (GetComponent<EffectDto>().permanent)
        {
            transform.SetParent(Content);
        }
    }
    public void WhipeToAssign()
    {
        if(SlotL== null || SlotR == null)
        {
            FindStuff();
        }
        if (SlotLHandler != null && SlotRHandler != null)
        {
            SlotLHandler.ToAssign = null;
            SlotRHandler.ToAssign = null;
            HighlightOff(true);
        }
        
    }
    public void RemoveFromPlayer()
    {
        playerHandler.RemoveSpecialCard(SpecialCards.GetName(gameObject));
    }
}
