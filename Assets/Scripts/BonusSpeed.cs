using System;
using UnityEngine;

public class BonusSpeed : MonoBehaviour
{
    [SerializeField] private float _speed;

    [SerializeField] AudioClip _bonusSFX;
    [SerializeField][Range(0, 1)] private float _volumeBonusSFX = 0.75f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var player = collision.gameObject.GetComponent<Player>();
        if (!player) { return; }
        player.SetBulletSpeed(player.GetBulletSpeed() * _speed);
        AudioSource.PlayClipAtPoint(_bonusSFX, transform.position, _volumeBonusSFX);
        Destroy(gameObject);
    }
}
