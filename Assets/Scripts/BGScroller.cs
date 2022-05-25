using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGScroller : MonoBehaviour
{
    [SerializeField] private float _speedBGScroll = 0.05f;
    Material _material;
    Vector2 _offset;

    private void Start()
    {
        _material = GetComponent<Renderer>().material;
        _offset = new Vector2(0f, _speedBGScroll); 
    }

    private void Update()
    {
        _material.mainTextureOffset += _offset * Time.deltaTime;
    }
}
