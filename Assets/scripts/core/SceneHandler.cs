using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneHandler : MonoBehaviour
{
    [SerializeField]
    private GameObject Player,Dealer;
    private PlayerHandler playerHandler;
    private Dealer dealer;

    // Start is called before the first frame update
    void Start()
    {
        playerHandler = Player.GetComponent<PlayerHandler>();
        dealer = Dealer.GetComponent<Dealer>();
    }

    public void EndGame()
    {
        if (dealer.TotalValue > 21)
        {
            //victory for player
        }
        else if (playerHandler.curSum > 21)
        {
            //loss for player
        }
        else if (playerHandler.curSum < dealer.TotalValue)
        {
            //loss for player
        }
        else if (playerHandler.curSum > dealer.TotalValue)
        {
            //victory for player
        }
        else if (playerHandler.curSum == dealer.TotalValue)
        {
            //draw
        }
    }

    public void Stand()
    {
        dealer.PullRest();
        //special card from dealer
    }
}
