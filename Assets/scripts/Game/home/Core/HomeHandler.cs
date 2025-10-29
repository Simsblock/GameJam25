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
        
        //loading initial JSON data
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
        GlobalData.SPCTextures = new Dictionary<string, Sprite>();
        StartCoroutine(AssetsLoader.LoadTexture("SPCimgs/SPCard_SpriteSheet.png", sprite =>
        {
            List<Sprite> LoadedSprites= AssetsLoader.SliceTexture(sprite, 14, 1);
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
        StartCoroutine(AssetsLoader.LoadTexture("SPCimgs/Dice_SpriteSheet.png", sprite =>
        {
            List<Sprite> loadedSprites= AssetsLoader.SliceTexture(sprite, 5, 5);
            //maping sprites to names :)
            GlobalData.SPCTextures.Add("d6_6_white",loadedSprites[20]);
            GlobalData.SPCTextures.Add("d6_5_white",loadedSprites[21]);
            GlobalData.SPCTextures.Add("d6_4_white",loadedSprites[22]);
            GlobalData.SPCTextures.Add("d6_3_white",loadedSprites[23]);
            GlobalData.SPCTextures.Add("d6_2_white",loadedSprites[24]);
            GlobalData.SPCTextures.Add("d6_1_white",loadedSprites[15]); //15
            GlobalData.SPCTextures.Add("d6_1_green",loadedSprites[16]);
            GlobalData.SPCTextures.Add("d6_2_green",loadedSprites[17]);
            GlobalData.SPCTextures.Add("d6_3_green",loadedSprites[18]);
            GlobalData.SPCTextures.Add("d6_4_green",loadedSprites[19]);
            GlobalData.SPCTextures.Add("d6_5_green",loadedSprites[10]); //10
            GlobalData.SPCTextures.Add("d6_6_green",loadedSprites[11]); //11
            GlobalData.SPCTextures.Add("d6_6_red",loadedSprites[12]); //12
            GlobalData.SPCTextures.Add("d6_5_red",loadedSprites[13]); //13
            GlobalData.SPCTextures.Add("d6_4_red",loadedSprites[14]); //14
            GlobalData.SPCTextures.Add("d6_3_red",loadedSprites[5]); //5
            GlobalData.SPCTextures.Add("d6_2_red",loadedSprites[6]); //6
            GlobalData.SPCTextures.Add("d6_1_red",loadedSprites[7]); //7
            GlobalData.SPCTextures.Add("d6_1_yellow",loadedSprites[8]); //8
            GlobalData.SPCTextures.Add("d6_2_yellow",loadedSprites[9]); //9
            GlobalData.SPCTextures.Add("d6_3_yellow",loadedSprites[0]); //0
            GlobalData.SPCTextures.Add("d6_4_yellow",loadedSprites[1]); //1
            GlobalData.SPCTextures.Add("d6_5_yellow",loadedSprites[2]); //2
            GlobalData.SPCTextures.Add("d6_6_yellow",loadedSprites[3]); //3
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