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

    void Start()
    {
        screen2.SetActive(false);
        if (GlobalData.FirstLoad()) GlobalData.ClearAll();
        Highscore_Text.text = $"Highscore\n{PlayerPrefs.GetInt("HighScore")}";
        LoaderBtn.interactable = GlobalData.LoadableCheck();
        Debug.Log(GlobalData.LoadableCheck());
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
    public void Tutorial()
    {
        screen2.SetActive(true);
        screen1.SetActive(false);
    }
    public void CloseTut()
    {
        screen2.SetActive(false);
        screen1.SetActive(true);
    }
}