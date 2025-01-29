using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEngine.GraphicsBuffer;

public class GameOver : MonoBehaviour
{
    [SerializeField]
    public TMP_Text Score_Text, HighScore_Text;
    void Start()
    {
        Score_Text.text = $"Score: {GlobalData.Score}";
        HighScore_Text.text = $"Highscore: {PlayerPrefs.GetInt("HighScore")}";
    }
    public void Quit()
    {
        SceneManager.LoadScene("Home");
        GlobalData.ClearAll();
    }
    public void Restart()
    {
        SceneManager.LoadScene("Main");
        GlobalData.ClearAll();
    }
}