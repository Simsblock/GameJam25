using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HoverSoul : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject descriptionText; 
    void Start()
    {
            descriptionText.SetActive(false);   
    }


    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        //transform.SetAsLastSibling();
        if (eventData.pointerCurrentRaycast.gameObject.name.Equals("Soul"))
        {
            descriptionText.SetActive(true);
        }
        else
        {
            descriptionText.SetActive(false);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        descriptionText.SetActive(false);
    }
}
