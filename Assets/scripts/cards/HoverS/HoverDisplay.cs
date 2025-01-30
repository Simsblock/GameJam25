using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HoverDisplay : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject descriptionText; // Assign the text object in the inspector
    [HideInInspector] public Canvas canvas; // Reference to the canvas for positioning
    private RectTransform descriptionRect;
    private bool hovering,flipped;
    private float x;
    void Start()
    {
        flipped = false;
       canvas= canvas = GameObject.Find("MainCanvas").GetComponent<Canvas>();
        if (descriptionText != null)
        {
            descriptionRect = descriptionText.GetComponent<RectTransform>();
            descriptionText.SetActive(false); // Ensure the description starts hidden
        }
        else
        {
            Debug.LogError("Description Text is not assigned!");
        }
    }

    void Update()
    {
        if (hovering)
        {
            if (canvas != null)
            {
                Vector3 worldPos;
                RectTransformUtility.ScreenPointToWorldPointInRectangle(
                    canvas.GetComponent<RectTransform>(), Input.mousePosition, canvas.worldCamera, out worldPos);
                x = worldPos.x * 1.01f;
                if (Input.mousePosition.x > Screen.width * 0.9025) x = Screen.width * 0.91f;

                    if (Input.mousePosition.y < Screen.height*0.8)
                {
                    
                        descriptionRect.transform.position = new Vector3(x, worldPos.y *1.05f, worldPos.z);
                    if (flipped)
                    {
                        descriptionText.transform.GetChild(0).Rotate(new Vector3(-180, 0, 0));
                        descriptionText.transform.GetChild(0).GetChild(0).Rotate(new Vector3(180, 0, 0));
                        flipped = false;
                    }
                }
                else
                {
                    descriptionRect.transform.position = new Vector3(x, worldPos.y* 0.83f, worldPos.z);
                    if (!flipped)
                    {
                        descriptionText.transform.GetChild(0).Rotate(new Vector3(180, 0, 0));
                        descriptionText.transform.GetChild(0).GetChild(0).Rotate(new Vector3(-180, 0, 0));
                        flipped = true;
                    }
                }
            }
        }
    }

    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        //transform.SetAsLastSibling();
        if (descriptionText != null)
        {
            hovering = true;
            if (Input.mousePosition.y > Screen.height - 100 && !flipped)
            {
                descriptionText.transform.GetChild(0).Rotate(new Vector3(180, 0, 0));
                descriptionText.transform.GetChild(0).GetChild(0).Rotate(new Vector3(-180, 0, 0));
                flipped = true;
            }
            else if (flipped)
            {
                descriptionText.transform.GetChild(0).Rotate(new Vector3(-180, 0, 0));
                descriptionText.transform.GetChild(0).GetChild(0).Rotate(new Vector3(180, 0, 0));
                flipped = false;
            }
                // Show the description
                descriptionText.SetActive(true);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
       
            hovering = false;
            descriptionText.SetActive(false);
    }
}
