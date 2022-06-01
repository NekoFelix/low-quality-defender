using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverLoader : MonoBehaviour
{
    [SerializeField] Player _player;
    [SerializeField] SceneLoadManager _sceneLoadManager;

    private void Start()
    {
        _player = FindObjectOfType<Player>();
        _sceneLoadManager = FindObjectOfType<SceneLoadManager>();
    }
    private void Update()
    {
        if (!_player)
        {
            StartCoroutine(DelayLoadGameOver(2.5f));
        }
    }
    private IEnumerator DelayLoadGameOver(float sec)
    {
        yield return new WaitForSeconds(sec);
        _sceneLoadManager.LoadGameOverScene();
    }
}
