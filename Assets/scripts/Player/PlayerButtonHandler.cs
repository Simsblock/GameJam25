using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerButtonHandler : MonoBehaviour
{
    [SerializeField]
    private GameObject player, Dealer;
    [SerializeField]
    public Button hitBtn, standBtn;
    [SerializeField]
    private GameObject GameHandler,BetUi;
    private GameHandler GameHandlerScript;
    private AudioManager audioManager;

    private bool isHitButtonCooldown = false;
    private bool isStandButtonCooldown = false;

    private void Start()
    {
        GameHandlerScript = GameHandler.GetComponent<GameHandler>();
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    private void Update()
    {
        if (isHitButtonCooldown || player.GetComponent<PlayerHandler>().curSum >= 21)
        {
            hitBtn.interactable = false;
        }else hitBtn.interactable = true;

        if (GameHandlerScript.stand==3|| GameHandlerScript.stand == 4)
        {
            standBtn.interactable = false;
        }
        else standBtn.interactable = true;
    }

    public void Hit()
    {
        Debug.Log("Hit");
        player.GetComponent<PlayerHandler>().PullCard();
        StartCoroutine(ReenableButtonAfterDelay(hitBtn, 0.1f));
        isHitButtonCooldown = true;
        audioManager.PlaySFX(audioManager.buttonClickSound);
        Debug.Log("End of Hit");
    }

    public void Stand()
    {
        GameHandlerScript.stand = 1;
        Dealer.GetComponent<Dealer>().PullRest();
        audioManager.PlaySFX(audioManager.buttonClickSound);
    }

    public void Deal()
    {
        audioManager.PlaySFX(audioManager.buttonClickSound);
        StartCoroutine(GameHandlerScript.StartRound());
    }

    private IEnumerator ReenableButtonAfterDelay(Button btn, float delay)
    {
        yield return new WaitForSeconds(delay);
        if (btn == hitBtn)
        {
            isHitButtonCooldown = false;
        }
    }
}
