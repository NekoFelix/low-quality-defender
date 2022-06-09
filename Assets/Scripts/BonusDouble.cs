using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusDouble : MonoBehaviour
{
    Vector3 _firstOffset = new Vector3(-0.3f, 0, 0);
    Vector3 _secondOffset = new Vector3(0.3f, 0, 0);
    private bool _activeDoubleShoot = true;

    public bool SetActiveBonusDouble()
    {
        return _activeDoubleShoot;
    }

    public Vector3 GetFirstOffset()
    {
        return _firstOffset;
    }
    
    public Vector3 GetSecondOffset()
    {
        return _secondOffset;
    }

    public void Hit()
    {
        Destroy(gameObject);
    }
}
