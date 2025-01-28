using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HoverDisplay : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject descriptionText; // Assign the text object in the inspector
    public Canvas canvas; // Reference to the canvas for positioning
    private RectTransform descriptionRect;
    private bool hovering;
    void Start()
    {
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
            if (Input.mousePosition.y < Screen.height - 38)
            {
                Vector3 worldPos;
                RectTransformUtility.ScreenPointToWorldPointInRectangle(
                    canvas.GetComponent<RectTransform>(), Input.mousePosition, canvas.worldCamera, out worldPos);
                descriptionRect.transform.position = new Vector3(worldPos.x, worldPos.y + 2, worldPos.z);
            }
            else
            {
                Vector3 worldPos;
                RectTransformUtility.ScreenPointToWorldPointInRectangle(
                    canvas.GetComponent<RectTransform>(), Input.mousePosition, canvas.worldCamera, out worldPos);
                descriptionRect.transform.position = new Vector3(worldPos.x, worldPos.y - 35, worldPos.z);
            }
        }
    }

    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        if (descriptionText != null)
        {
            hovering = true;
            // Show the description
            descriptionText.SetActive(true);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (descriptionText != null)
        {
            hovering = false;
            descriptionText.SetActive(false); // Hide the description
        }
    }

}
