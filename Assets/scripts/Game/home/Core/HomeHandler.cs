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
        StartCoroutine(AssetsLoader.ReadJsonTo<List<PlayerSpEffectDto>>("PlayerSPEffects.json", effect =>
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
        StartCoroutine(AssetsLoader.LoadTexture("SPCimgs/SPCard_SpriteSheet.png", sprite =>
        {
            List<Sprite> LoadedSprites= AssetsLoader.SliceTexture(sprite, 14, 1);
            GlobalData.SPCTextures = new Dictionary<string, Sprite>();
            //maping sprites to names :)
            GlobalData.SPCTextures.Add("Piggibank",LoadedSprites[0]);
            GlobalData.SPCTextures.Add("Dealer+1",LoadedSprites[1]);
            GlobalData.SPCTextures.Add("Seer",LoadedSprites[2]);
            GlobalData.SPCTextures.Add("DoubleEdgedSword",LoadedSprites[3]);
            GlobalData.SPCTextures.Add("ThreeKings",LoadedSprites[4]);
            GlobalData.SPCTextures.Add("Switch",LoadedSprites[5]);
            GlobalData.SPCTextures.Add("Cashback",LoadedSprites[6]);
            GlobalData.SPCTextures.Add("Restart",LoadedSprites[7]);
            GlobalData.SPCTextures.Add("Ass",LoadedSprites[8]);
            GlobalData.SPCTextures.Add("Joker",LoadedSprites[9]);
            GlobalData.SPCTextures.Add("Destroy",LoadedSprites[10]);
            GlobalData.SPCTextures.Add("TheTwins",LoadedSprites[11]);
            GlobalData.SPCTextures.Add("Shortcut",LoadedSprites[12]);
            GlobalData.SPCTextures.Add("Gambit",LoadedSprites[13]);
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