using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialCardsList : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> SpecialCards;
    public Dictionary<string, GameObject> SpecialCardsUi;

    // Start is called before the first frame update
    void Start()
    {
        //maps the card names to prefabs
        SpecialCardsUi = new Dictionary<string, GameObject>()
        {
            ["test"] = SpecialCards[0]
        };
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
