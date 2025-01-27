using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityDecoder : MonoBehaviour
{
    
    private PlayerHandler playerHandler;
    private Dealer Dealer;
    // Start is called before the first frame update
    void Start()
    {
        playerHandler = GameObject.Find("Player").GetComponent<PlayerHandler>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Use(string Effect)
    {
        //effect pattern: xxx:n:m;
        //xxx -> effect identifier
        //n -> number for use (draw n cards)
        //m -> target (P = player D = Dealer)
        //only xxx is mandatory
        //to concat multible effects chain with ; and no spaces (xxx;xxx:n;xxx:n:m)
        if (Effect != null)
        {
            string[] effects = Effect.Split(';');

            foreach (string effect in effects)
            {
                string[] details = effect.Split(":");
                switch (details[0])
                {
                    case "draw":
                        if (details[2] == "P") playerHandler.PullMulti(int.Parse(details[1]));
                        if (details[2] == "D") Dealer.PullMulti(int.Parse(details[1]));
                        break;
                    case "remove":
                        //maby
                        break;
                }
            }
        }
    }
}
