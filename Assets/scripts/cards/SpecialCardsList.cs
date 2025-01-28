using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialCardsList : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> SpecialCards;
    [SerializeField]
    private List<string> CardNames,CardDescription;
    public Dictionary<string, GameObject> SpecialCardsUi;
    public Dictionary<string, string> SpecialCardsDescription;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(CardNames);
        //maps the card names to prefabs
        for (int i = 0; i < CardNames.Count; i++)
        {
            if(CardNames[i] != null && SpecialCards[i] != null && CardDescription[i]!=null)
            {
                SpecialCardsUi.Add(CardNames[i],SpecialCards[i]);
                SpecialCardsDescription.Add(CardNames[i],CardDescription[i]);
            }
        }
    }
}


