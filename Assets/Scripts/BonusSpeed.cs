using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusSpeed : MonoBehaviour
{
    [SerializeField] private float _speed;

    public float GetSpeed()
    {
        return _speed;
    }

    public void Hit()
    {
        Destroy(gameObject);
    }
}
