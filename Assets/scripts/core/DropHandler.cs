using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DropHandler : MonoBehaviour, IDropHandler
{
    
    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log(transform.name);
        if (transform.childCount == 0)
        {
            GameObject Dropped = eventData.pointerDrag;
            if (Dropped != null)
            {
                Dropped.GetComponent<DragHandler>().postDragParent = transform;
            }
        }
    }
}
