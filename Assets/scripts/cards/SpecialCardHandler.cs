using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialCardHandler : MonoBehaviour
{
    [SerializeField]
    public string Effect;
    private PlayerHandler playerHandler;
    private Dealer Dealer;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Use()
    {
        if (Effect != null)
        {
            string[] effects = Effect.Split(';');

        }
    }
}
