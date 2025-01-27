using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerButtonHandler : MonoBehaviour
{
    [SerializeField]
    private GameObject player,Dealer;
    public void Pull()
    {
        player.GetComponent<PlayerHandler>().PullMulti(1);
    }

    public void Stand()
    {
        Dealer.GetComponent<Dealer>().PullRest();
    }
}
