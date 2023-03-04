using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; 

public class UIHandler : MonoBehaviour
{
    public static UIHandler instance;

    public TMP_Text scoreNumber;
    public TMP_Text levelText;
    public TMP_Text layersText;

    public TMP_Text scoreNumber_gameOver;
    public TMP_Text highScoreNumber_gameOver;

    public GameObject gameOverWindow;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        gameOverWindow.SetActive(false);
    }

    public void UpdateUi(int score, int level, int layers)
    {
        scoreNumber.text = score.ToString("D9");
        levelText.text = level.ToString("D2");
        layersText.text = layers.ToString("D9");
    }

    public void UpdateGameOverUI(int score)
    {
        scoreNumber_gameOver.text = score.ToString("D9");
    }

    public void SetGameOverWindow()
    {
        gameOverWindow.SetActive(true);
    }
}
