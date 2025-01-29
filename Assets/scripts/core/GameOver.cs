using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEngine.GraphicsBuffer;

public class GameOver : MonoBehaviour
{
    public void Restart()
    {
        SceneManager.LoadScene("Main");
        GlobalData.ClearAll();
    }
}