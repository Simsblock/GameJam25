using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
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
    private Player playerHandler;
    private Dealer dealer;
    [SerializeField]
    public TMP_Text bet_text, money, score, dealerScore;
    private int OffCamerPos=16;
    [SerializeField]
    private GameObject win, loose, draw;
    private bool isShop = true;
    [SerializeField]
    private GameObject SPCSlotL, SPCSlotR,DiceDrop,Shopkeep,DiceShop;
    private DisplaySpecial displaySpecial,displaydice;
    private SpecialCardsList specialCards;
    //Timer
    [SerializeField]
    public Image Timebar;
    private float MaxTime = 20f;
    private float RemainingTime;
    private bool TimeIsRunning = false;
    //Pause
    [SerializeField]
    public GameObject PauseUI;
    //audioHandler
    private AudioManager audio;
    [SerializeField]
    private GameObject ParticleSystem;
    //spirets for card generation

    public SerializableDictionary<string, Sprite> SPCSpriteMapper;

    private void Awake()
    {
        audio = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    // Start is called before the first frame updatet a
    void Start()
    {
        win.SetActive(false);
        loose.SetActive(false);
        draw.SetActive(false);
        playerHandler = Player.GetComponent<Player>();
        dealer = Dealer.GetComponent<Dealer>();
        GameUI.SetActive(false);
        PauseUI.SetActive(false);
        displaySpecial = ShopKeep.GetComponent<DisplaySpecial>();
        displaydice=DiceShop.GetComponent<DisplaySpecial>();
        specialCards = GetComponent<SpecialCardsList>();
        displaydice.DisplayShop();
        displaySpecial.DisplayShop();
        //LoadShop();
        
    }



    public int stand = 0;
    private void Update()
    {
        //Score and Money Display
        money.text = $"{PlayerPrefs.GetInt("Money")}";
        score.text = $"{playerHandler.TotalValue}";

        //Pause
        if (Input.GetKeyDown(KeyCode.Escape) && ShopUI.activeInHierarchy && !PauseUI.activeInHierarchy)
        {
            PauseUI.SetActive(true);
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && PauseUI.activeInHierarchy) Unpause();
        //Stand
        if (stand == 1)
        {
            UseSPC();
            //Animationshit
            StartCoroutine(EndGame());
        }
        else if (stand == 0) dealerScore.text = $"{dealer.OpenCard.Value}";
        else if (stand == 3) dealerScore.text = $"{dealer.TotalValue}";
        else { }
        //Timer
        if (TimeIsRunning && RemainingTime > 0)
        {
            RemainingTime -= Time.deltaTime;
            Timebar.fillAmount = RemainingTime / MaxTime;
        }
        else if (RemainingTime <= 0 && TimeIsRunning)
        {
            
            UseSPC();
            clearSPC();
            StartCoroutine(EndGame());
        }
    }

    //Start Round
    public IEnumerator StartRound() 
    {
        //SetScore
        if (PlayerPrefs.GetInt("Money") > PlayerPrefs.GetInt("Score")) PlayerPrefs.SetInt("Score",PlayerPrefs.GetInt("Money"));
        if (PlayerPrefs.GetInt("HighScore") < PlayerPrefs.GetInt("Score")) PlayerPrefs.SetInt("HighScore", PlayerPrefs.GetInt("Score"));
        //clear old Cards
        Deck.Clear();
        SetBet();
        yield return StartCoroutine(LoadShop());
        audio.ChangeBGMusic(audio.casinoBackround);
        audio.PlaySFX(audio.shuffleCards);
        //wait ig n such
        dealer.PullInit();
        playerHandler.PullMultipleCards(2); //Init
        //Timer
        RemainingTime = MaxTime;
        TimeIsRunning = true;
        audio.StartTimerAudio();
    }

    //SetBet 
    public void SetBet()
    {
        PlayerPrefs.SetInt("Bet", int.Parse(bet_text.text));
    }
    private void SetBetText()
    {
        bet_text.text= PlayerPrefs.GetInt("Bet").ToString();
    }
    public IEnumerator EndGame()
    {
        TimeIsRunning=false;
        audio.StopTimerSFX();
        clearSPC();
        stand = 3;
        //Dealer Reveal
        yield return StartCoroutine(dealer.PullRest());
        yield return StartCoroutine(dealer.UseAbilities());
        stand = 4;
        yield return new WaitForSeconds(1f);

        dealerScore.text = $"{dealer.TotalValue}";
        //Win or Loose?
        if (dealer.TotalValue > GlobalData.DealerWinCond && playerHandler.TotalValue <= GlobalData.PlayerWinCond)
        {
            PlayerPrefs.SetInt("Money", PlayerPrefs.GetInt("Money") + PlayerPrefs.GetInt("Bet") * GlobalData.BetPayoutRate / 100); //win
            //win.SetActive(true);
           StartCoroutine( ParticelSystemStart());
            audio.PlaySFX(audio.winSound);
        }
        else if (playerHandler.TotalValue > GlobalData.PlayerWinCond)
        {
            PlayerPrefs.SetInt("Money", PlayerPrefs.GetInt("Money") - PlayerPrefs.GetInt("Bet") * GlobalData.BetLossRate / 100); //loss
            //loose.SetActive(true);
            audio.PlaySFX(audio.loseMoney);
        }
        else if (playerHandler.TotalValue < dealer.TotalValue)
        {
            PlayerPrefs.SetInt("Money", PlayerPrefs.GetInt("Money") - PlayerPrefs.GetInt("Bet") * GlobalData.BetLossRate / 100); //loss
            //loose.SetActive(true);
            audio.PlaySFX(audio.loseMoney);
        }
        else if (playerHandler.TotalValue > dealer.TotalValue)
        {
            PlayerPrefs.SetInt("Money", PlayerPrefs.GetInt("Money") + PlayerPrefs.GetInt("Bet") * GlobalData.BetPayoutRate / 100); //win
            //win.SetActive(true);
            StartCoroutine(ParticelSystemStart());
            audio.PlaySFX(audio.winSound);
        }
        else if (playerHandler.TotalValue == dealer.TotalValue)
        {
            //draw.SetActive(true);
        }
        else
        {
            //draw.SetActive(true);
        }
        if (int.Parse(bet_text.text) > PlayerPrefs.GetInt("Money"))
        {
            PlayerPrefs.SetInt("Bet", PlayerPrefs.GetInt("Money"));
            Debug.Log(int.Parse(bet_text.text));
            SetBetText();
        }
        CheckGameOver();
        //animations n stuff
        //yield return new WaitUntil(() => Input.anyKeyDown);
        yield return new WaitForSeconds(0.7f);
        //Clear UI
        HideUI();
        StartCoroutine(playerHandler.ClearHand());

        StartCoroutine(dealer.ClearHand());
        // Reset win/lose/draw images
        //win.SetActive(false);
        //loose.SetActive(false);
        //draw.SetActive(false);
        // Load the shop
        stand = 0;
        audio.ChangeBGMusic(audio.shopBackround);
        yield return StartCoroutine(LoadShop());
        //Change Dealer
        dealer.ChangeDealer();
        GlobalData.ResetAbilityValues();
    }

    private IEnumerator ParticelSystemStart()
    {
        ParticleSystem.SetActive(true);
        yield return new WaitForSeconds(2);
        ParticleSystem.SetActive(false);
    }
    public void CheckGameOver()
    {
        if (PlayerPrefs.GetInt("Money") <= 0)
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
        audio.PlaySFX(audio.changeEnemy);
        StartCoroutine(MoveObjectOffCamera(Dealer,target));
        audio.PlaySFX(audio.rollingWheels);
        yield return StartCoroutine(MoveObjectOffCamera(ShopKeep,target-new Vector3(OffCamerPos,0,0)));
        //Un/Load Shop UI
        GameUI.SetActive(isShop);
        ShopUI.SetActive(!isShop);
        isShop=!isShop;
        displaySpecial.DisplayShop();
        displaydice.DisplayShop();
    }

    private void HideUI()
    {
        GameUI.SetActive(false);
        ShopUI.SetActive(false);
    }

    private IEnumerator MoveObjectOffCamera(GameObject obj, Vector3 target)
    {
        if (obj == null)
        {
            Debug.LogError("MoveObjectOffCamera: obj is null! Exiting coroutine.");
            yield break;
        }

        float speed = 5f;
        float maxDuration = 5f; // Safety timeout to prevent infinite loops
        float timer = 0f;

        while (obj != null && Vector3.Distance(obj.transform.position, target) > 0.01f && timer < maxDuration)
        {
            obj.transform.position = Vector3.MoveTowards(obj.transform.position, target, speed * Time.deltaTime);
            timer += Time.deltaTime;
            yield return null; // Wait for the next frame
        }

        if (obj != null)
        {
            // Snap to the exact target position (to handle floating-point imprecision)
            obj.transform.position = target;
            Debug.Log($"MoveObjectOffCamera: {obj.name} reached target.");
        }
        else
        {
            Debug.LogWarning("MoveObjectOffCamera: obj was destroyed before reaching target.");
        }

        if (timer >= maxDuration)
        {
            Debug.LogError($"MoveObjectOffCamera: Movement timed out after {maxDuration} seconds!");
        }
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
        
        //DiceDrop
        if (DiceDrop.transform.childCount == 1)
        {
            
            if (!DiceDrop.transform.GetChild(0).GetComponent<EffectDto>().preStand)
            {
                DiceDrop.GetComponent<DropHandler>().TriggerSPCEffect();
            }
        }
    }

    private void clearSPC()
    {
        //SPCL
        if (SPCSlotL.transform.childCount == 1)
        {
            if (!SPCSlotL.transform.GetChild(0).GetComponent<EffectDto>().permanent)
            {
                playerHandler.RemoveSpecialCard(specialCards.GetName(SPCSlotL.transform.GetChild(0).gameObject));
                Destroy(SPCSlotL.transform.GetChild(0).gameObject);
            }
        }



        //SPCR
        if (SPCSlotR.transform.childCount == 1)
        {
            if (!SPCSlotR.transform.GetChild(0).GetComponent<EffectDto>().permanent)
            {
                playerHandler.RemoveSpecialCard(specialCards.GetName(SPCSlotR.transform.GetChild(0).gameObject));
                Destroy(SPCSlotR.transform.GetChild(0).gameObject);
            }
                
        }

        //DiceDrop
        if (DiceDrop.transform.childCount == 1)
        {
            Debug.Log("1");
            if (!DiceDrop.transform.GetChild(0).GetComponent<EffectDto>().permanent)
            {
                playerHandler.RemoveSpecialCard(specialCards.GetName(DiceDrop.transform.GetChild(0).gameObject));
                Destroy(DiceDrop.transform.GetChild(0).gameObject);
            }
            else
            {
                Debug.Log(2);
                DiceDrop.transform.GetChild(0).GetComponent<DragHandler>().ResetPos();
            }

        }
    }

    //Pause
    public void Unpause()
    {
        audio.PlaySFX(audio.buttonClickSound);
        PauseUI.SetActive(false);
    }
    public void QuitToHome()
    {
        audio.PlaySFX(audio.buttonClickSound);
        SceneManager.LoadScene("Home");
    }
    //getSPCSlotR
    public DropHandler GetSPCRHandler()
    {
        return SPCSlotR.GetComponent<DropHandler>();
    }
}
