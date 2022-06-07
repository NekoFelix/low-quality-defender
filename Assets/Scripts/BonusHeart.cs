using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusHeart : MonoBehaviour
{
    [SerializeField] private int _health = 1;

    public int GetHealth()
    {
        return _health;
    }

    public void Hit()
    {
        Destroy(gameObject);
    }
}
