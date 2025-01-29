using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
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
    private TMP_Text bet_text, money, score, dealerScore;
    private int OffCamerPos=16;
    [SerializeField]
    private GameObject win, loose, draw;
    private bool isShop = true;
    [SerializeField]
    private GameObject SPCSlotL, SPCSlotR,Shopkeep,DiceShop;
    private DisplaySpecial displaySpecial,displaydice;
    private SpecialCardsList specialCards;
    //Timer
    [SerializeField]
    public Image Timebar;
    private float MaxTime = 20f;
    private float RemainingTime;
    private bool TimeIsRunning = false;

    // Start is called before the first frame updatet a
    void Start()
    {
        win.SetActive(false);
        loose.SetActive(false);
        draw.SetActive(false);
        playerHandler = Player.GetComponent<PlayerHandler>();
        dealer = Dealer.GetComponent<Dealer>();
        GameUI.SetActive(false);
        displaySpecial = ShopKeep.GetComponent<DisplaySpecial>();
        displaydice=DiceShop.GetComponent<DisplaySpecial>();
        displaydice.DisplayShop();
        displaySpecial.DisplayShop();
        specialCards = GetComponent<SpecialCardsList>();
        //LoadShop();
    }

    public int stand = 0;
    private void Update()
    {
        //Score and Money Display
        money.text = $"{GlobalData.money}";
        score.text = $"{playerHandler.curSum}";
        //Stand
        if (stand == 1)
        {
            dealerScore.text = $"Dealer Score: {dealer.TotalValue}";
            UseSPC();
            //Animationshit
            EndGame();
        }
        else if (stand == 0) dealerScore.text = $"Dealer Score: {dealer.OpenCard.Value}";
        else { }
        //Timer
        if (TimeIsRunning && RemainingTime > 0)
        {
            RemainingTime -= Time.deltaTime;
            Timebar.fillAmount = RemainingTime / MaxTime;
        }
        else if (RemainingTime <= 0 && TimeIsRunning)
        {
            EndGame();
        }

    }

    //Start Round
    public IEnumerator StartRound() 
    {
        //clear old Cards
        SetBet();
        yield return StartCoroutine(LoadShop());
        //wait ig n such
        dealer.PullInit();
        playerHandler.PullMulti(2); //Init
        //Timer
        RemainingTime = MaxTime;
        TimeIsRunning = true;
    }

    //SetBet 
    public void SetBet()
    {
        GlobalData.bet = long.Parse(bet_text.text);
    }
    private void SetBetText()
    {
        bet_text.text= GlobalData.bet.ToString();
    }
    public void EndGame()
    {
        TimeIsRunning=false;
        clearSPC();
        HideUI();
        stand = 3;
        dealerScore.text = $"Dealer Score: {dealer.TotalValue}";
        if(RemainingTime <= 0)
        {
            GlobalData.money -= GlobalData.bet * GlobalData.BetLossRate / 100; //loss
            loose.SetActive(true);
        }
        else if (dealer.TotalValue > GlobalData.DealerWinCond && playerHandler.curSum <= GlobalData.PlayerWinCond)
        {
            GlobalData.money += GlobalData.bet*GlobalData.BetPayoutRate/100; //win
            win.SetActive(true);
        }
        else if (playerHandler.curSum > GlobalData.PlayerWinCond)
        {
            GlobalData.money -= GlobalData.bet * GlobalData.BetLossRate / 100; //loss
            loose.SetActive(true);
        }
        else if (playerHandler.curSum < dealer.TotalValue)
        {
            GlobalData.money -= GlobalData.bet*GlobalData.BetLossRate / 100; //loss
            loose.SetActive(true);
        }
        else if (playerHandler.curSum > dealer.TotalValue)
        {
            GlobalData.money += GlobalData.bet * GlobalData.BetPayoutRate / 100; //win
            win.SetActive(true);
        }
        else if (playerHandler.curSum == dealer.TotalValue)
        {
            draw.SetActive(true);
        }
        else
        {
            draw.SetActive(true);
        }
        if (GlobalData.bet > GlobalData.money)
        {
            GlobalData.bet = GlobalData.money;
            SetBetText();
        }
        CheckGameOver();
        //animations n stuff
        StartCoroutine(EndGameSequence());
    }

    private IEnumerator EndGameSequence()
    {
        // Wait for animations or other delays
        yield return new WaitUntil(() => Input.anyKeyDown);
        StartCoroutine(playerHandler.ClearBaseCards());

        dealer.ClearHand();
        // Reset win/lose/draw images
        win.SetActive(false);
        loose.SetActive(false);
        draw.SetActive(false);
        // Load the shop
        stand = 0;
        yield return StartCoroutine(LoadShop());
        //Change Dealer
        dealer.ChangeDealer();
        GlobalData.ResetAbilityValues();
    }

    public void CheckGameOver()
    {
        if (GlobalData.money <= 0)
        {

            SceneManager.LoadScene("GameOver"); //to be made UwU
        }
    }
    public IEnumerator LoadShop()
    {
        HideUI();
        Vector3 target=new Vector3(0,0,0);
        if (Dealer.transform.position.x == OffCamerPos - OffCamerPos) target = Dealer.transform.position + new Vector3(OffCamerPos, 0, 0);
        else if (Dealer.transform.position.x == OffCamerPos) target = Dealer.transform.position - new Vector3(OffCamerPos, 0, 0);
        //Move Dealer and ShopKeep
        StartCoroutine(MoveObjectOffCamera(Dealer,target));
        yield return StartCoroutine(MoveObjectOffCamera(ShopKeep,target-new Vector3(OffCamerPos,0,0)));
        //Un/Load Shop UI
        GameUI.SetActive(isShop);
        ShopUI.SetActive(!isShop);
        isShop=!isShop;
        displaySpecial.DisplayShop();
    }

    private void HideUI()
    {
        GameUI.SetActive(false);
        ShopUI.SetActive(false);
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
        Debug.Log("MoveObjectOffCamera: Object reached target.");
    }

    private void UseSPC()
    {
        //SPCL
        if(SPCSlotL.transform.childCount == 1)
        {
            if (!SPCSlotL.transform.GetChild(0).GetComponent<EffectDto>().preStand)
            {
                SPCSlotL.GetComponent<DropHandler>().TriggerSPCEffect();
            }
        }



        //SPCR
        if (SPCSlotR.transform.childCount == 1)
        {
            if (!SPCSlotR.transform.GetChild(0).GetComponent<EffectDto>().preStand)
            {
                SPCSlotR.GetComponent<DropHandler>().TriggerSPCEffect();
            }
        }
    }

    private void clearSPC()
    {
        //SPCL
        if (SPCSlotL.transform.childCount == 1)
        {
            playerHandler.RemoveSpecialCard(specialCards.GetName(SPCSlotL.transform.GetChild(0).gameObject));
            Destroy(SPCSlotL.transform.GetChild(0).gameObject);
        }



        //SPCR
        if (SPCSlotR.transform.childCount == 1)
        {
            playerHandler.RemoveSpecialCard(specialCards.GetName(SPCSlotR.transform.GetChild(0).gameObject));
            Destroy(SPCSlotR.transform.GetChild(0).gameObject);
        }
    }
}
