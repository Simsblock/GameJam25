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

    void Start()
    {
        if (GlobalData.FirstLoad()) GlobalData.ClearAll();
        Highscore_Text.text = $"Highscore\n{PlayerPrefs.GetInt("HighScore")}";
        LoaderBtn.interactable = GlobalData.LoadableCheck();
    }

    public void Load()
    {
        SceneManager.LoadScene("Main");
    }
    public void Play()
    {
        SceneManager.LoadScene("Main");
        GlobalData.ClearAll();
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void Credits()
    {
        SceneManager.LoadScene("Credits");
    }
}