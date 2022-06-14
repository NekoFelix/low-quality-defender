using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusDouble : MonoBehaviour
{
    [SerializeField] AudioClip _bonusSFX;
    [SerializeField][Range(0, 1)] private float _volumeBonusSFX = 0.75f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var player = collision.gameObject.GetComponent<Player>();
        if (!player) { return; }
        player.SetBulletsOffset(new Vector3(-0.35f, 0, 0), new Vector3(0.35f, 0, 0));
        player.SetDoubleShotActive(true);
        AudioSource.PlayClipAtPoint(_bonusSFX, transform.position, _volumeBonusSFX);
        Destroy(gameObject);
    }
}
