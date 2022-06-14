using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Enemy Shoot")]
    [SerializeField] AudioClip _shootSFX;
    [SerializeField][Range(0, 1)] private float _volumeShootSFX = 0.2f;
    [SerializeField][Range(1, 10)] private float _shotFrequency;
    [SerializeField][Range(1, 20)] private float _bulletSpeed;
    [SerializeField] GameObject _bulletPrefab;
    [SerializeField] GameObject _secondBulletPrefab;
    [SerializeField] Vector3 _bulletOffset;
    [SerializeField] Vector3 _secondBulletOffset;
    [SerializeField] private bool _isDoubleShot;

    [Header("Enemy Health")]
    [SerializeField] private int health = 10;
    [SerializeField] private int _reward = 10;

    [Header("Explosion")]
    [SerializeField] AudioClip _explosionSFX;
    [SerializeField][Range(0, 1)] private float _volumeExplosionSFX = 0.3f;
    [SerializeField] GameObject _explosion;

    [Header("Drop")]
    [SerializeField] GameObject _bonusHeartPrefab;
    [SerializeField] GameObject _bonusDoublePrefab;
    [SerializeField] GameObject _bonusSpeedPrefab;
    [SerializeField] GameObject _bonusShieldPrefab;

    WaveConfig _waveConfig;
    ScoreState _scoreState;

    private List<Transform> _wayPoints;
    int _indexWayPointsList = 0;
    private float _timeToShoot = 2f;

    private void Start()
    {
        _scoreState = FindObjectOfType<ScoreState>();
        _wayPoints = _waveConfig.GetWayPoints();
        transform.position = _wayPoints[_indexWayPointsList].transform.position;
    }

    public void SetWaveConfig(WaveConfig waveConfig)
    {
        this._waveConfig = waveConfig;
    }

    private void Update()
    {
        Shoot();
        if (_indexWayPointsList <= _wayPoints.Count - 1)
        {
            var _targetPos = _wayPoints[_indexWayPointsList].transform.position;
            transform.position = Vector2.MoveTowards(transform.position, _targetPos, _waveConfig.GetEnemySpeed() * Time.deltaTime);
            if (transform.position == _targetPos)
            {
                _indexWayPointsList++;
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Shoot()
    {
        _timeToShoot -= Time.deltaTime;
        if (_timeToShoot <= 0f)
        {
            AudioSource.PlayClipAtPoint(_shootSFX, transform.position, _volumeShootSFX);
            if (_isDoubleShot)
            {
                AudioSource.PlayClipAtPoint(_shootSFX, transform.position, _volumeShootSFX);
                GameObject secondBullet = Instantiate(_bulletPrefab, transform.position + _secondBulletOffset, Quaternion.identity) as GameObject;
                secondBullet.GetComponent<Rigidbody2D>().velocity = new Vector2(0, _bulletSpeed * -1);
            }
            GameObject bullet = Instantiate(_bulletPrefab, transform.position + _bulletOffset, Quaternion.identity) as GameObject;
            bullet.GetComponent<Rigidbody2D>().velocity = new Vector2(0, _bulletSpeed * -1);
            _timeToShoot = UnityEngine.Random.Range(_shotFrequency * 0.1f, _shotFrequency);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GetDamage(collision);
        GetPlayerCollision(collision);
    }

    private void GetPlayerCollision(Collider2D collision)
    {
        Player player = collision.gameObject.GetComponent<Player>();
        if (!player) { return; }
        health = 0;
        if (health <= 0)
        {
            _scoreState.AddToScore(_reward);
            AudioSource.PlayClipAtPoint(_explosionSFX, transform.position, _volumeExplosionSFX);
            Instantiate(_explosion, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }

    private void GetDamage(Collider2D collision)
    {
        DamageDealer damageDealer = collision.gameObject.GetComponent<DamageDealer>();
        if (!damageDealer) { return; }
        health -= damageDealer.GetDamage();
        damageDealer.Hit();
        if (health <= 0)
        {
            if (FindObjectOfType<Player>())
            {
                DropBonus(FindObjectOfType<Player>().GetHealth());
            }
            _scoreState.AddToScore(_reward);
            AudioSource.PlayClipAtPoint(_explosionSFX, transform.position, _volumeExplosionSFX);
            Instantiate(_explosion, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }


    private void DropBonus(int health)
    {
        if (health <= 1)
        {
            Instantiate(_bonusHeartPrefab, transform.position, Quaternion.identity);
        }
        else
        {
            int dropChance = UnityEngine.Random.Range(0,4);
            if (dropChance == 0)
            {
                return;
            }
            if (dropChance == 1)
            {
                Instantiate(_bonusSpeedPrefab, transform.position, Quaternion.identity);
            }
            if (dropChance == 2)
            {
                Instantiate(_bonusShieldPrefab, transform.position, Quaternion.identity);
            }
            if (dropChance == 3)
            {
                Instantiate(_bonusDoublePrefab, transform.position, Quaternion.identity);
            }
        }
    }
}
