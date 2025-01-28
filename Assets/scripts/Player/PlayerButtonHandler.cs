using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerButtonHandler : MonoBehaviour
{
    [SerializeField]
    private GameObject player,Dealer;
    [SerializeField]
    private GameObject GameHandler;
    private GameHandler GameHandlerScript;

    private void Start()
    {
        GameHandlerScript = GameHandler.GetComponent<GameHandler>();
    }
    public void Pull()
    {
        player.GetComponent<PlayerHandler>().PullMulti(1);
    }

    public void Stand()
    {
        GameHandlerScript.stand = 1;
        Dealer.GetComponent<Dealer>().PullRest();
    }
}
