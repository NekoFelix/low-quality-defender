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
    [SerializeField] private bool _isDoubleShoot = false;
    [SerializeField] private Vector3 _firstBulletOffset;
    [SerializeField] private Vector3 _secondBulletOffset;
    [SerializeField] GameObject _bulletPrefab;
    [SerializeField] GameObject _secondBulletPrefab;

    [Header("Player Health")]
    [SerializeField] Image[] _healthImages;
    [SerializeField] Sprite _fullHeart;
    [SerializeField] Sprite _emptyHeart;
    [SerializeField] private int _heartSlots = 5;
    [SerializeField] private int _health = 4;

    [Header("Bonus Drop")]
    [SerializeField] AudioClip _getBonusSFX;
    [SerializeField][Range(0, 1)] private float _volumeGetBonusSFX = 0.75f;
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
    private float _startBulletSpeed;
    private float _startFirePerSecond;
    private int _startLucky;

    private void Start()
    {
        _startBulletSpeed = _bulletSpeed;
        _startFirePerSecond = _firePerSecond;
        _startLucky = _lucky;
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
        EndTimeOfBonus(_bonusTime);
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
    //=======================ÏÎËÍÀß ÕÓÉÍß ïåðåäåëàòü=====
    private void EndTimeOfBonus(float _bonusTime)       //
    {
        _timeEndBonus += Time.deltaTime;                //
        if (_timeEndBonus >= _bonusTime)
        {
            _firstBulletOffset = new Vector3(0,0,0);    //
            _secondBulletOffset = new Vector3(0,0,0);
            _isDoubleShoot = false;                     //
            _firePerSecond = _startFirePerSecond;
            _bulletSpeed = _startBulletSpeed;
            _lucky = _startLucky;
            _timeEndBonus = 0;
        }
    }
    //====================================================
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
            if (_isDoubleShoot)
            {
                AudioSource.PlayClipAtPoint(_shootSFX, transform.position, _volumeShootSFX);
                GameObject secondBullet = Instantiate(_secondBulletPrefab, transform.position + _secondBulletOffset, Quaternion.identity) as GameObject;
                secondBullet.GetComponent<Rigidbody2D>().velocity = new Vector2(0, _bulletSpeed);
            }
            AudioSource.PlayClipAtPoint(_shootSFX, transform.position, _volumeShootSFX);
            GameObject bullet = Instantiate(_bulletPrefab, transform.position + _firstBulletOffset, Quaternion.identity) as GameObject;
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
        GetBonusSpeed(collision);
        GetBonusDouble(collision);
    }

    private void GetBonusDouble(Collider2D collision)
    {
        BonusDouble bonusDouble = collision.gameObject.GetComponent<BonusDouble>();
        if (!bonusDouble) { return; }
        _isDoubleShoot = bonusDouble.SetActiveBonusDouble();
        _firstBulletOffset = bonusDouble.GetFirstOffset();
        _secondBulletOffset = bonusDouble.GetSecondOffset();
        AudioSource.PlayClipAtPoint(_getBonusSFX, transform.position, _volumeGetBonusSFX);
        bonusDouble.Hit();
    }

    private void GetBonusSpeed(Collider2D collision)
    {
        BonusSpeed bonusSpeed = collision.gameObject.GetComponent<BonusSpeed>();
        if (!bonusSpeed) { return; }
        _bulletSpeed *= bonusSpeed.GetSpeed();
        AudioSource.PlayClipAtPoint(_getBonusSFX, transform.position, _volumeGetBonusSFX);
        bonusSpeed.Hit();
    }

    private void GetBonusLucky(Collider2D collision)
    {
        BonusLucky bonusLucky = collision.gameObject.GetComponent<BonusLucky>();
        if (!bonusLucky) { return; }
        _lucky += bonusLucky.GetLucky();
        AudioSource.PlayClipAtPoint(_getBonusSFX, transform.position, _volumeGetBonusSFX);
        bonusLucky.Hit();
    }

    private void GetBonusHeart(Collider2D collision)
    {
        BonusHeart bonusHeart = collision.gameObject.GetComponent<BonusHeart>();
        if (!bonusHeart) { return; }
        _health += bonusHeart.GetHealth();
        bonusHeart.Hit();
        AudioSource.PlayClipAtPoint(_getBonusSFX, transform.position, _volumeGetBonusSFX);
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
