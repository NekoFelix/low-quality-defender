using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreState : MonoBehaviour
{
    private int _score = 0;

    private void Awake()
    {
        SetUpSingleton();
    }

    private void SetUpSingleton()
    {
        if (FindObjectsOfType<ScoreState>().Length > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    public int GetScore()
    {
        return _score;
    }

    public void AddToScore(int _reward)
    {
        _score += _reward;
    }

    public void ResetScore()
    {
        Destroy(gameObject);
    }
}
