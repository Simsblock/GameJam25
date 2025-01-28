using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEngine.GraphicsBuffer;

public class GameHandler : MonoBehaviour
{
    [SerializeField]
    private GameObject Player,Dealer,ShopKeep;
    [SerializeField]
    private GameObject GameUI, ShopUI;
    private PlayerHandler playerHandler;
    private Dealer dealer;
    [SerializeField]
    private TMP_Text bet_text;
    private int OffCamerPos=16;

    // Start is called before the first frame updatet a
    void Start()
    {
        playerHandler = Player.GetComponent<PlayerHandler>();
        dealer = Dealer.GetComponent<Dealer>();
        GameUI.SetActive(false);

        //LoadShop();
    }

    private void Update()
    {
        
    }
    
    //Start Round
    public void StartRound() 
    {
        Debug.Log("start");
        Console.WriteLine("staart");
        //clear old Cards
        dealer.ClearHand();
        playerHandler.ClearPlayerCards();
        SetBet();
        LoadShop();
        //wait ig n such
        dealer.PullInit();
        playerHandler.PullMulti(2); //Init
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
        //animations n stuff
        LoadShop();
    }

    public void CheckGameOver()
    {
        if (GlobalData.money <= 0)
        {
            SceneManager.LoadScene("GameOver"); //to be made UwU
        }
    }
    public void LoadShop()
    {
        Debug.Log("load");
        Vector3 target=new Vector3(0,0,0);
        if (Dealer.transform.position.x == OffCamerPos - OffCamerPos) target = Dealer.transform.position + new Vector3(OffCamerPos, 0, 0);
        else if (Dealer.transform.position.x == OffCamerPos) target = Dealer.transform.position - new Vector3(OffCamerPos, 0, 0);
        //Un/load Game UI
        GameUI.SetActive(!GameUI.active);
        //Move Dealer and ShopKeep
        StartCoroutine(MoveObjectOffCamera(Dealer,target));
        StartCoroutine(MoveObjectOffCamera(ShopKeep,target-new Vector3(OffCamerPos,0,0)));
        //Un/Load Shop UI
        ShopUI.SetActive(!GameUI.active);
    }

    private IEnumerator MoveObjectOffCamera(GameObject obj, Vector3 target)
    {
        while (Vector3.Distance(obj.transform.position, target) > 0.01f)
        {
            obj.transform.position = Vector3.MoveTowards(obj.transform.position, target, 5f * Time.deltaTime);
            yield return null; // Wait for the next frame
        }

        // Snap to the exact target position (to handle floating-point imprecision)
        obj.transform.position = target;
    }

}
