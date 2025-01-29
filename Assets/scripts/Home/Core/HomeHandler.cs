using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HomeHandler : MonoBehaviour
{
    [SerializeField]
    public TMP_Text Highscore_Text;

    void Start()
    {
        Highscore_Text.text = $"Highscore\n{PlayerPrefs.GetInt("HighScore")}";
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