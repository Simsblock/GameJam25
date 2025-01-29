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
    private GameObject GameHandler;
    private GameHandler GameHandlerScript;

    private bool isHitButtonCooldown = false;

    private void Start()
    {
        GameHandlerScript = GameHandler.GetComponent<GameHandler>();
    }

    private void Update()
    {
        if (isHitButtonCooldown || player.GetComponent<PlayerHandler>().curSum >= 21)
        {
            hitBtn.interactable = false;
        }else hitBtn.interactable = true;
    }

    public void Hit()
    {
        player.GetComponent<PlayerHandler>().PullCard();
        StartCoroutine(ReenableButtonAfterDelay(hitBtn, 0.1f));
        isHitButtonCooldown = true;
    }

    public void Stand()
    {
        GameHandlerScript.stand = 1;
        Dealer.GetComponent<Dealer>().PullRest();
    }

    public void Deal()
    {
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
