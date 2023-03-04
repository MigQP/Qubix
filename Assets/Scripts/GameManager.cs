using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    int score;
    int level;
    int layersCleared;

    bool isGameOver;

    float fallSpeed;



    void Awake()
    {
        instance = this;    
    }

    void Start()
    {
        
        SetScore(score);
    }

    public void SetScore(int amount)
    {
        score += amount;
        CalculateLevel();
        UIHandler.instance.UpdateUi(score, level, layersCleared);
        // Update UI
    }

    public float ReadFallSpeed()
    {
        return fallSpeed;
    }

    public void LayersCleared(int amount)
    {
        if (amount == 1)
        {
            SetScore(500);
        }
        else if (amount == 2)
        {
            SetScore(1000);
        }
        else if (amount == 3)
        {
            SetScore(2000);
        }
        else if (amount == 4)
        {
            SetScore(4000);
        }

        layersCleared += amount;
        // Update UI
        UIHandler.instance.UpdateUi(score, level, layersCleared);
    }

    void CalculateLevel()
    {
        if (score <= 10000)
        {
            level = 1;
            fallSpeed = 3f;
        }
        else if (score > 10000 && score <= 20000)
        {
            level = 2;
            fallSpeed = 2.75f;
        }
        else if (score > 20000 && score <= 30000)
        {
            level = 3;
            fallSpeed = 2.5f;
        }
        else if (score > 30000 && score <= 40000)
        {
            level = 4;
            fallSpeed = 2.25f;
        }
        else if (score > 40000 && score <= 50000)
        {
            level = 5;
            fallSpeed = 2.0f;
        }
        else if (score > 50000 && score <= 60000)
        {
            level = 6;
            fallSpeed = 1.75f;
        }
        else if (score > 60000 && score <= 70000)
        {
            level = 7;
            fallSpeed = 1.5f;
        }
        else if (score > 70000 && score <= 80000)
        {
            level = 8;
            fallSpeed = 1.25f;
        }
        else if (score > 80000 && score <= 90000)
        {
            level = 9;
            fallSpeed = 1.0f;
        }
        else if (score  > 90000)
        {
            level = 10;
            fallSpeed = 0.8f;
        }
        // Update UI
    }

    public bool ReadIsGameOver()
    {
        return isGameOver;
    }

    public void SetIsGameOver()
    {
        isGameOver = true;
        UIHandler.instance.SetGameOverWindow();
        UIHandler.instance.UpdateGameOverUI(score);
    }
}
