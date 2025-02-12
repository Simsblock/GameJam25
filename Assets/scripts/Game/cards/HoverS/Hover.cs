using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Hover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject descriptionText; // Assign the text object in the inspector
    [HideInInspector] public Canvas canvas; // Reference to the canvas for positioning
    private RectTransform descriptionRect;
    private bool hovering, flipped;
    private float x, y;
    [SerializeField]
    private GameObject Image;
    [SerializeField]
    private TMP_Text PriceText;
    private SpecialCardsList SPCList;
    private EffectDto Effect;

    public void OnPointerEnter(PointerEventData eventData)
    {
        descriptionText.SetActive(true);
        if (Effect != null && Effect.DynamicHover)
        {
            hovering = true;
        }
        //a static hover would just enable the descriptiontext component
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        hovering = false;
        descriptionText.SetActive(false);
    }


    void Start()
    {
        flipped = false;
        hovering = false;
        canvas = GameObject.Find("MainCanvas").GetComponent<Canvas>();
        Effect = GetComponent<EffectDto>();
        LoadData();
        if (descriptionText != null)
        {
            descriptionRect = descriptionText.GetComponent<RectTransform>();
            descriptionText.SetActive(false); // Ensure the description starts hidden
        }
        else
        {
            Debug.LogError("Description Text is not assigned!");
        }

        //check for things that don't need an effectsDto
        if (Effect == null)
        {
            Effect = new EffectDto();
            Effect.DynamicHover = false;
        }
    }

    private void LoadData()
    {
        if (Image != null)
        {
            SPCList = GameObject.Find("GameHandler").GetComponent<SpecialCardsList>();
            //image mapping missing
            //Image.GetComponent<Image>().sprite = SPCList.SpecialCardsUi[GetComponent<EffectDto>().name].GetComponent<Image>().sprite;
        }
        if (PriceText != null)
        {
            PriceText.text = GetComponent<EffectDto>().Price.ToString();
        }
    }

    private void DynamicHover()
    {
        if (canvas != null && descriptionRect != null)
        {
            Vector3 worldPos;
            RectTransformUtility.ScreenPointToWorldPointInRectangle(
                canvas.GetComponent<RectTransform>(), Input.mousePosition, canvas.worldCamera, out worldPos);
            x = worldPos.x * 1.01f;
            if (Input.mousePosition.x > Screen.width * 0.9025) x = Screen.width * 0.91f;

            if (Input.mousePosition.y < Screen.height * 0.8)
            {
                y = Screen.height * 0.05f;
                descriptionRect.transform.position = new Vector3(x, worldPos.y + y, worldPos.z);
                if (flipped)
                {
                    descriptionText.transform.GetChild(0).Rotate(new Vector3(-180, 0, 0));
                    descriptionText.transform.GetChild(0).GetChild(0).Rotate(new Vector3(180, 0, 0));
                    flipped = false;
                }
            }
            else
            {
                y = Screen.height * 0.17f;
                descriptionRect.transform.position = new Vector3(x, worldPos.y - y, worldPos.z);
                if (!flipped)
                {
                    descriptionText.transform.GetChild(0).Rotate(new Vector3(180, 0, 0));
                    descriptionText.transform.GetChild(0).GetChild(0).Rotate(new Vector3(-180, 0, 0));
                    flipped = true;
                }
            }
        }
    }

    void Update()
    {
        if (hovering)
        {
            DynamicHover();
        }
    }
}
