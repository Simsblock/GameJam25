using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ClearHighlight : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    private GameObject SPCSlot;

    private void Start()
    {

    }

    

    public void OnPointerClick(PointerEventData eventData)
    {
        ClearAssigned();
    }
    public void ClearAssigned()
    {
        SPCSlot.GetComponent<DropHandler>().ClearAssigned();
        gameObject.SetActive(false);
    }
}
