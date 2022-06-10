using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusHeart : MonoBehaviour
{
    [SerializeField] private int _health = 1;
    [SerializeField] AudioClip _bonusSFX;
    [SerializeField][Range(0, 1)] private float _volumeBonusSFX = 0.75f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var player = collision.gameObject.GetComponent<Player>();
        if (!player) { return; }
        player.SetHealth(player.GetHealth() + _health);
        AudioSource.PlayClipAtPoint(_bonusSFX, transform.position, _volumeBonusSFX);
        Destroy(gameObject);
    }
}
