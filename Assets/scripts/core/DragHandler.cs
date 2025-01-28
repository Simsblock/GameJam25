using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragHandler : MonoBehaviour,IDragHandler,IBeginDragHandler,IEndDragHandler
{
    public Transform postDragParent;
    [SerializeField]
    private Image image;
    public void OnBeginDrag(PointerEventData eventData)
    {
        image.raycastTarget = false;
        postDragParent = transform.parent;
        transform.SetParent(transform.root);
        transform.SetAsLastSibling();
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.SetParent(postDragParent);
        image.raycastTarget = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
