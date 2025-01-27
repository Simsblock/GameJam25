using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialCardsList : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> SpecialCards;
    public Dictionary<string, GameObject> SpecialCardsUi;
    public Dictionary<string, string> SpecialCardsDescription;

    // Start is called before the first frame update
    void Start()
    {
        //maps the card names to prefabs
        SpecialCardsUi = new Dictionary<string, GameObject>()
        {
            ["test"] = SpecialCards[0]
        };
        SpecialCardsDescription = new Dictionary<string, string>()
        {
            ["test"] = "idfk *shrugs*"
        };
    }
}


