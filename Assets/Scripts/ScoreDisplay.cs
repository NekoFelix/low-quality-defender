using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class ScoreDisplay : MonoBehaviour
{
    [SerializeField]TextMeshProUGUI _scoreText;
    ScoreState _scoreState;

    private void Start()
    {
        _scoreState = FindObjectOfType<ScoreState>();
    }

    private void Update()
    {
        _scoreText.text = _scoreState.GetScore().ToString();
    }
}
