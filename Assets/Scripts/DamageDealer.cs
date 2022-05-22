using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageDealer : MonoBehaviour
{
    [SerializeField] private int _damage = 1;

    public int GetDamage()
    {
        return _damage;
    }

    public void Hit()
    {
        Destroy(gameObject);
    }
}
