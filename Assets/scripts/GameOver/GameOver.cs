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
    private AudioManager audioManager;
    void Start()
    {
        Score_Text.text = $"Score: {PlayerPrefs.GetInt("Score")}";
        HighScore_Text.text = $"Highscore: {PlayerPrefs.GetInt("HighScore")}";
        GlobalData.ClearAll();
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }
    public void Quit()
    {
        audioManager.PlaySFX(audioManager.buttonClickSound);
        SceneManager.LoadScene("Home");
    }
    public void Restart()
    {
        audioManager.PlaySFX(audioManager.buttonClickSound);
        SceneManager.LoadScene("Main");
    }
}