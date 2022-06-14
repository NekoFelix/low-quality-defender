using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadNextSceneTrigger : MonoBehaviour
{
    [SerializeField] SceneLoadManager sceneLoadManadger;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player player = collision.gameObject.GetComponent<Player>();
        if (!player) { return; }

        sceneLoadManadger.LoadGameOverScene();

    }
}
