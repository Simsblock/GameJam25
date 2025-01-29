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
    private bool isStandButtonCooldown = false;

    private void Start()
    {
        GameHandlerScript = GameHandler.GetComponent<GameHandler>();
    }

    public void Hit()
    {
        hitBtn.interactable = false;
        player.GetComponent<PlayerHandler>().PullCard();
        StartCoroutine(ReenableButtonAfterDelay(hitBtn, 0.1f));
    }

    public void Stand()
    {
        standBtn.interactable = false;
        GameHandlerScript.stand = 1;
        Dealer.GetComponent<Dealer>().PullRest();
        StartCoroutine(ReenableButtonAfterDelay(standBtn, 0.1f));
    }

    private IEnumerator ReenableButtonAfterDelay(Button btn, float delay)
    {
        yield return new WaitForSeconds(delay);
        btn.interactable = true;
    }
}
