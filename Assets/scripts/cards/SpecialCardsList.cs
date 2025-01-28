using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialCardsList : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> SpecialCards;
    [SerializeField]
    private List<string> CardNames;
    public Dictionary<string, GameObject> SpecialCardsUi;

    // Start is called before the first frame update
    void Start()
    {
        SpecialCardsUi = new Dictionary<string, GameObject>();
        Debug.Log(CardNames);
        //maps the card names to prefabs
        for (int i = 0; i < CardNames.Count; i++)
        {
            if(CardNames[i] != null && SpecialCards[i] != null )
            {
                SpecialCardsUi.Add(CardNames[i],SpecialCards[i]);
            }
        }
    }
}


