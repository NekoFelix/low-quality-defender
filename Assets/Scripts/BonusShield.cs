using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusShield : MonoBehaviour
{
    [SerializeField] AudioClip _bonusSFX;
    [SerializeField][Range(0, 1)] private float _volumeBonusSFX = 0.75f;
    [SerializeField] private float _bonusTime;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var player = collision.gameObject.GetComponent<Player>();
        if (!player) { return; }
        player.SetShieldActive(true);
        player.RestartBonusTime();
        player.SetBonusTime(_bonusTime);
        AudioSource.PlayClipAtPoint(_bonusSFX, transform.position, _volumeBonusSFX);
        Destroy(gameObject);
    }
}
