using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameHandler : MonoBehaviour
{
    [SerializeField]
    private GameObject Player,Dealer;
    private PlayerHandler playerHandler;
    private Dealer dealer;
    [SerializeField]
    private TMP_Text bet_text;

    // Start is called before the first frame updatet a
    void Start()
    {
        playerHandler = Player.GetComponent<PlayerHandler>();
        dealer = Dealer.GetComponent<Dealer>();
    }

    //SetBet 
    public void SetBet()
    {
        GlobalData.bet = long.Parse(bet_text.text);
    }

    public void EndGame()
    {
        if (dealer.TotalValue > 21)
        {
            GlobalData.money += GlobalData.bet; //win
        }
        else if (playerHandler.curSum > 21)
        {
            GlobalData.money -= GlobalData.bet; //loss
        }
        else if (playerHandler.curSum < dealer.TotalValue)
        {
            GlobalData.money -= GlobalData.bet; //loss
        }
        else if (playerHandler.curSum > dealer.TotalValue)
        {
            GlobalData.money += GlobalData.bet; //win
        }
        else if (playerHandler.curSum == dealer.TotalValue)
        {
            //draw
        }
        CheckGameOver();
    }

    public void CheckGameOver()
    {
        if (GlobalData.money <= 0)
        {
            SceneManager.LoadScene("GameOver"); //to be made UwU
        }
    }
    public void Stand()
    {
        dealer.PullRest();
        //special card from dealer
    }

    public void Setup()
    {
        //clear old Cards
        dealer.ClearHand();
        playerHandler.ClearPlayerCards();
        //set Bets
        SetBet();
        //Init new starting cards
        dealer.PullInit();
        playerHandler.PullMulti(2); //Init
    }

}
