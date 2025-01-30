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
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (effects.Used)
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
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (effects.Used)
        {
            return;
        }
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        //if (effects.Used)
        //{
        //    return;
        //}
        transform.SetParent(postDragParent);
        image.raycastTarget = true;

    }

    public void OnPointerClick(PointerEventData eventData)
    {
        
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
            PlayBuySound();
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        effects = GetComponent<EffectDto>();
        Content = transform.parent;
        playerHandler = GameObject.Find("Player").GetComponent<PlayerHandler>();
        SpecialCards =GameObject.Find("GameHandler").GetComponent<SpecialCardsList>();
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
