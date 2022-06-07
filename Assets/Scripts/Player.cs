using System;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [Header("Player Move")]
    [SerializeField] private bool isMovePlayerByAxis = true;
    [SerializeField] private float offset;
    [SerializeField] float speedPlayer = 50f;

    [Header("Player Shoot")]
    [SerializeField] AudioClip _shootSFX;
    [SerializeField] [Range(0, 1)] private float _volumeShootSFX = 0.5f;
    [SerializeField] private float _firePerSecond;
    [SerializeField] private float _bulletSpeed;
    [SerializeField] GameObject _bulletPrefab;

    [Header("Player Health")]
    [SerializeField] Image[] _healthImages;
    [SerializeField] Sprite _fullHeart;
    [SerializeField] Sprite _emptyHeart;
    [SerializeField] private int _heartSlots = 5;
    [SerializeField] private int _health = 4;
    
    [Header("Bonus Drop")]
    [SerializeField] private float _bonusTime = 10f;
    [SerializeField] private int _lucky = 1;

    [Header("Explosion")]
    [SerializeField] AudioClip _explosionSFX;
    [SerializeField] [Range(0,1)] private float _volumeExplosionSFX = 0.75f;
    [SerializeField] GameObject _explosion;

    private float _screenHeightInUnits;
    private float _screenWidthInUnits;
    private float _minX;
    private float _maxX;
    private float _minY;
    private float _maxY;

    private float _timeToShoot;
    private float _timeEndBonus;

    private void Start()
    {
        HealthUpdater();
        SetUpMoveBoundaries();
    }

    private void SetUpMoveBoundaries()
    {
        Camera cam = Camera.main;
        _minX = cam.ViewportToWorldPoint(new Vector3(0, 0, 0)).x + offset;
        _maxX = cam.ViewportToWorldPoint(new Vector3(1, 0, 0)).x - offset;
        _minY = cam.ViewportToWorldPoint(new Vector3(0, 0, 0)).y + offset;
        _maxY = cam.ViewportToWorldPoint(new Vector3(0, 1, 0)).y - offset;
        _screenHeightInUnits = cam.orthographicSize * 2;
        _screenWidthInUnits = cam.orthographicSize;
    }

    private void Update()
    {
        EndTimeOfLuck(_bonusTime);
        Shoot();
        if (isMovePlayerByAxis)
        {
            MovePlayerByAxis();
        }
        else
        {
            MovePlayerByMousePos();
        }
    }

    private void EndTimeOfLuck(float _bonusTime)
    {
        _timeEndBonus += Time.deltaTime;
        if (_timeEndBonus >= _bonusTime)
        {
            _lucky = 1;
            _timeEndBonus = 0;
        }
    }

    public int GetLucky()
    {
        return _lucky;
    }

    private void HealthUpdater()
    {
        if (_health > _heartSlots)
        {
            _health = _heartSlots;
        }

        for (int i = 0; i < _healthImages.Length; i++)
        {
            if (i < _heartSlots)
            {
                _healthImages[i].enabled = true;
            }
            else
            {
                _healthImages[i].enabled = false;
            }

            if (i < _health)
            {
                _healthImages[i].sprite = _fullHeart;
            }
            else
            {
                _healthImages[i].sprite = _emptyHeart;
            }
        }
    }

    private void Shoot()
    {
        _timeToShoot += Time.deltaTime;
        if (Input.GetButton("Fire1") && (_timeToShoot >= 1f / _firePerSecond))
        {
            AudioSource.PlayClipAtPoint(_shootSFX, transform.position, _volumeShootSFX);
            GameObject bullet = Instantiate(_bulletPrefab, transform.position, Quaternion.identity) as GameObject;
            bullet.GetComponent<Rigidbody2D>().velocity = new Vector2(0, _bulletSpeed);
            _timeToShoot = 0f;
        }
    }

    private void MovePlayerByAxis()
    {
        var playerPosition = new Vector2();

        var deltaX = Input.GetAxis("Mouse X") * Time.deltaTime * speedPlayer;
        var deltaY = Input.GetAxis("Mouse Y") * Time.deltaTime * speedPlayer;

        playerPosition.x = Mathf.Clamp(transform.position.x + deltaX, _minX, _maxX);
        playerPosition.y = Mathf.Clamp(transform.position.y + deltaY, _minY, _maxY);

        transform.position = playerPosition;
    }

    private void MovePlayerByMousePos()
    {
        Vector2 playerPosition = new Vector2(transform.position.x, transform.position.y);
        playerPosition.x = Mathf.Clamp(GetXPos(), _minX, _maxX);
        playerPosition.y = Mathf.Clamp(GetYPos(), _minY, _maxY);
        transform.position = playerPosition;
    }

    private float GetYPos()
    {
        return Input.mousePosition.y / Screen.height * _screenHeightInUnits;
    }

    private float GetXPos()
    {
        return Input.mousePosition.x / Screen.width * _screenWidthInUnits;
    }
   

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GetDamage(collision);
        GetBonusHeart(collision);
        GetBonusLucky(collision);
    }

    private void GetBonusLucky(Collider2D collision)
    {
        BonusLucky bonusLucky = collision.gameObject.GetComponent<BonusLucky>();
        if (!bonusLucky) { return; }
        _lucky += bonusLucky.GetLucky();
        bonusLucky.Hit();
    }

    private void GetBonusHeart(Collider2D collision)
    {
        BonusHeart bonusHeart = collision.gameObject.GetComponent<BonusHeart>();
        if (!bonusHeart) { return; }
        _health += bonusHeart.GetHealth();
        bonusHeart.Hit();
        HealthUpdater();
    }

    private void GetDamage(Collider2D collision)
    {
        DamageDealer damageDealer = collision.gameObject.GetComponent<DamageDealer>();
        if (!damageDealer) { return; }
        _health -= damageDealer.GetDamage();
        damageDealer.Hit();
        HealthUpdater();
        if (_health <= 0)
        {
            FindObjectOfType<SceneLoadManager>().LoadGameOverScene();
            AudioSource.PlayClipAtPoint(_explosionSFX, transform.position, _volumeExplosionSFX);
            Instantiate(_explosion, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
