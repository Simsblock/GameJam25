using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
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
        if (SpecialCardsUi == null) Generate();
    }

    public void Generate()
    {
        SpecialCardsUi = new Dictionary<string, GameObject>();
        //maps the card names to prefabs
        for (int i = 0; i < CardNames.Count; i++)
        {
            if (CardNames[i] != null && SpecialCards[i] != null)
            {
                SpecialCardsUi.Add(CardNames[i], SpecialCards[i]);
            }
        }
    }
    public string GetName(GameObject gameObject)
    {
        string name = SpecialCardsUi.Where(s => gameObject.name.Contains(s.Value.name)).First().Key;
        return name;
    }
}


