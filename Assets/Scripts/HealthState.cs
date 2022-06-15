using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthState : MonoBehaviour
{
    [SerializeField] private int _health = 2;

    private void Awake()
    {
        SetUpSingleton();
    }

    private void SetUpSingleton()
    {
        if (FindObjectsOfType<HealthState>().Length > 1)
        {
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    public int GetHealth()
    {
        return _health;
    }

    public void SetHealth(int value)
    {
        _health = value;
    }

    public void ResetHealth()
    {
        Destroy(gameObject);
    }
}
