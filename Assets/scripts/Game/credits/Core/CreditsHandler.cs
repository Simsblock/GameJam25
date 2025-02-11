using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditsHandler : MonoBehaviour
{
    [SerializeField]
    public GameObject SpawnL, SpawnR;
    [SerializeField]
    public Sprite[] Sprites;
    [SerializeField]
    public GameObject SpinningCardsPrefab;
    private AudioManager audioManager;
    System.Random Random;

    private float cooldownTime = 0.3f; 
    private float cooldownTimer = 0f;

    void Start()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
        audioManager.ChangeBGMusic(audioManager.shopBackround);
        Random = new System.Random();
    }

    private void Update()
    {
        cooldownTimer -= Time.deltaTime;

        if (cooldownTimer <= 0f)
        {
            NewSpinner(false);  
            NewSpinner(true);   
            cooldownTimer = cooldownTime;
        }
    }

    public void Quit()
    {
        audioManager.PlaySFX(audioManager.buttonClickSound);
        SceneManager.LoadScene("Home");
    }

    public void NewSpinner(bool right)
    {
        GameObject spinning = Instantiate(SpinningCardsPrefab);

        if (right)
        {
            spinning.transform.SetParent(SpawnR.transform);
        }
        else
        {
            spinning.transform.SetParent(SpawnL.transform);
        }
        spinning.transform.localPosition = Vector3.zero;

        spinning.GetComponent<SpriteRenderer>().sprite = Sprites[Random.Next(Sprites.Length)];
    }
}
