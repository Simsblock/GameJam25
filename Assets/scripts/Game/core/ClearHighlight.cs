using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ClearHighlight : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    private GameObject SPCSlot,Clearer;

    private void Start()
    {

    }

    

    public void OnPointerClick(PointerEventData eventData)
    {
        ClearAssigned();
    }
    public void ClearAssigned()
    {
        if (SPCSlot!=null)
        {
            SPCSlot.GetComponent<DropHandler>().ClearAssigned();
            
        }
        if (Clearer != null)
        {
            Clearer.SetActive(false);
        }
        
    }
}
