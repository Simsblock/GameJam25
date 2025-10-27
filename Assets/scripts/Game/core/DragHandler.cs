using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
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
    private Player playerHandler;
    private SpecialCardsList SpecialCards;
    [HideInInspector] public string Shopname;
    private AudioManager audio;
    private GameObject SlotL, SlotR,DiceSlot;
    private DropHandler SlotLHandler, SlotRHandler;
    private GameObject ClearDrop;
    private bool Assigned;
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

        //may be subject to change
        if (effects != null)
        {
            if (SlotRHandler != null)
            {
                SlotRHandler.ClearAssigned();
            }
            if (effects.isDice)
            {
                HighlightOn(false);
            }
            else
            {
                HighlightOn(true);
            }
            Assigned = true;
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
        
        if (effects != null)
        {
            if (SlotRHandler != null)
            {
                SlotRHandler.ClearAssigned();
            }
            if (effects.isDice)
            {
                HighlightOff(false);
            }
            else
            {
                HighlightOff(true);
            }
            Assigned = false;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (Content.name.Equals("SpecialContent")&&!effects.isDice)
        {
            
            if (SlotL == null || SlotR == null || ClearDrop == null)
            {
                FindStuff();
               
            }
            if (!Assigned)
            {
                if (SlotRHandler != null)
                {
                   
                    SlotRHandler.ClearAssigned();
                    SlotRHandler.ToAssign = gameObject;
                    SlotRHandler.EnabClear();
                    HighlightOn(true);
                    Assigned = true;
                }
                else Debug.Log("SlotRHandler is null");
                if (SlotLHandler != null)
                {
                    
                    SlotLHandler.ToAssign = gameObject;
                }else Debug.Log("SlotLHandler is null");
                
            }
            else
            {
                if (SlotRHandler != null)
                {
                    
                    SlotRHandler.ClearAssigned();
                    SlotRHandler.DisableClear();
                    SlotRHandler.ToAssign = null;
                    //Assigned = false;
                }else Debug.Log("SlotRHandler is null");
                if (SlotLHandler != null)
                {
                    
                    SlotLHandler.ToAssign = null;
                }else Debug.Log("SlotLHandler is null");
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
            Debug.Log("card added");
            PlayerPrefs.SetInt("Money", PlayerPrefs.GetInt("Money")-effects.Price);
            StartCoroutine(BetShit());
            PlayBuySound();
            Destroy(gameObject);
        }
        
    }

    private void HighlightOn(bool card)
    {
            if (SlotL == null || SlotR == null || ClearDrop == null)
            {
                FindStuff();
            }        
            if (card&&SlotL != null && SlotR != null)
            {
            Highlight.FieldHighlightOn(SlotL);
            Highlight.FieldHighlightOn(SlotR);

            }
            else if(!card&&DiceSlot != null) 
            {
            Highlight.FieldHighlightOn(DiceSlot);
            }
            Highlight.SelectHighlightOn(gameObject);
    }
    private void HighlightOff(bool card)
    {
        if (SlotL == null || SlotR == null || ClearDrop == null)
        {
            FindStuff();
        }
            if (card&& SlotL != null && SlotR != null)
            {
            Highlight.FieldHighlightOff(SlotL);
            Highlight.FieldHighlightOff(SlotR);
            }
            if (!card && DiceSlot != null)
            {
            Highlight.FieldHighlightOff(DiceSlot);
            }
            Highlight.SelectHighlightOff(gameObject);
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
        playerHandler = GameObject.Find("Player").GetComponent<Player>();
        SpecialCards =GameObject.Find("GameHandler").GetComponent<SpecialCardsList>();
        audio = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
        SlotRHandler = GameObject.Find("GameHandler").GetComponent<GameHandler>().GetSPCRHandler();
        Assigned = false;
        FindStuff();
        
    }
    private void FindStuff() {
        SlotL = GameObject.Find("SPCSlotL");
        SlotR = GameObject.Find("SPCSlotR");
        DiceSlot = GameObject.Find("DiceDrop");
        ClearDrop = GameObject.Find("Clearer");
        if(SlotL!=null) SlotLHandler = SlotL.GetComponent<DropHandler>();
        if(SlotR!=null&&SlotRHandler==null) SlotRHandler = SlotR.GetComponent<DropHandler>();
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
        if(SlotLHandler != null) SlotLHandler.ToAssign = null;
        if (SlotRHandler != null) SlotRHandler.ToAssign = null;
        HighlightOff(true);
        Assigned = false;

    }
    public void RemoveFromPlayer()
    {
        playerHandler.RemoveSpecialCard(SpecialCards.GetName(gameObject));
    }
}
