using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusNova : MonoBehaviour
{
    [SerializeField] private int _numOfNovaWaves;
    [SerializeField] GameObject _novaFireBallsPrefab;
    [SerializeField] AudioClip _bonusSFX;
    [SerializeField][Range(0, 1)] private float _volumeBonusSFX = 0.75f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var player = collision.gameObject.GetComponent<Player>();
        if (!player) { return; }
        AudioSource.PlayClipAtPoint(_bonusSFX, transform.position, _volumeBonusSFX);
        for (int i = 0; i <= 43; i++) // 43 чтобы круг был красивенький
        {
            GameObject _nova = Instantiate(_novaFireBallsPrefab, transform.position, Quaternion.identity) as GameObject;
            _nova.GetComponent<Rigidbody2D>().velocity = new Vector3(Mathf.Cos(i + Mathf.PI) - 0.5f, Mathf.Sin(i + Mathf.PI), 0);
            GameObject _nova2 = Instantiate(_novaFireBallsPrefab, transform.position, Quaternion.identity) as GameObject;
            _nova2.GetComponent<Rigidbody2D>().velocity = new Vector3(Mathf.Cos(i + Mathf.PI) + 0.5f, Mathf.Sin(i + Mathf.PI) - 0.5f, 0);
            GameObject _nova3 = Instantiate(_novaFireBallsPrefab, transform.position, Quaternion.identity) as GameObject;
            _nova3.GetComponent<Rigidbody2D>().velocity = new Vector3(Mathf.Cos(i + Mathf.PI) + 0.5f, Mathf.Sin(i + Mathf.PI) + 0.5f, 0);
        }        
        Destroy(gameObject);
    }
}
