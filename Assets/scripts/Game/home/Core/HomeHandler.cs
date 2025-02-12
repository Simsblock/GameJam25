using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HomeHandler : MonoBehaviour
{
    [SerializeField]
    public TMP_Text Highscore_Text;
    [SerializeField]
    public Button LoaderBtn;
    [SerializeField]
    private GameObject screen1, screen2;
    private AudioManager audioManager;

    void Start()
    {
        screen2.SetActive(false);
        if (GlobalData.FirstLoad()) GlobalData.ClearAll();
        Highscore_Text.text = $"Highscore\n{PlayerPrefs.GetInt("HighScore")}";
        LoaderBtn.interactable = GlobalData.LoadableCheck();
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
        audioManager.ChangeBGMusic(audioManager.shopBackround);
        StartCoroutine(JsonReader.ReadJsonTo<List<PlayerSPEffectDto>>("PlayerSPEffects.json", effect =>
        {
            if (effect != null)
            {
                GlobalData.Effects = effect;
            }
            else
            {
                Debug.LogError("Effects not read");
            }
        }));
    }

    public void Load()
    {
        audioManager.PlaySFX(audioManager.buttonClickSound);
        SceneManager.LoadScene("Main");
    }
    public void Play()
    {
        audioManager.PlaySFX(audioManager.buttonClickSound);
        SceneManager.LoadScene("Main");
        GlobalData.ClearAll();
    }

    public void Quit()
    {
        audioManager.PlaySFX(audioManager.buttonClickSound);
        Application.Quit();
    }

    public void Credits()
    {
        audioManager.PlaySFX(audioManager.buttonClickSound);
        SceneManager.LoadScene("Credits");
    }
    public void Tutorial()
    {
        audioManager.PlaySFX(audioManager.buttonClickSound);
        screen2.SetActive(true);
        screen1.SetActive(false);
    }
    public void CloseTut()
    {
        audioManager.PlaySFX(audioManager.buttonClickSound);
        screen2.SetActive(false);
        screen1.SetActive(true);
    }
}