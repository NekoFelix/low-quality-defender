using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusLucky : MonoBehaviour
{
    [SerializeField] private int _lucky = 6;

    public int GetLucky()
    {
        return _lucky;
    }

    public void Hit()
    {
        Destroy(gameObject);
    }
}
