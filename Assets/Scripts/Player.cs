using System;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [Header("Player Move")]
    [SerializeField] private bool _isMovePlayerByTouch = true;
    [SerializeField] private bool _isMovePlayerByAxis = true;
    [SerializeField] private float _offset;
    [SerializeField] private float _speedPlayer = 50f;

    [Header("Player Shoot")]
    [SerializeField] AudioClip _shootSFX;
    [SerializeField][Range(0, 1)] private float _volumeShootSFX = 0.5f;
    [SerializeField] private float _firePerSecond;
    [SerializeField] private float _bulletSpeed;
    [SerializeField] GameObject _bulletPrefab;
    [SerializeField] GameObject _secondBulletPrefab;

    [Header("Player Health")]
    [SerializeField] Image[] _healthImages;
    [SerializeField] Sprite _fullHeart;
    [SerializeField] Sprite _emptyHeart;
    [SerializeField] private int _heartSlots = 5;
    [SerializeField] private int _health;
    HealthState _healthState;


    [Header("Player Skins")]
    [SerializeField] Sprite _commonPlayerSkin;
    [SerializeField] Sprite _shieldedPlayerSkin;

    [Header("Explosion")]
    [SerializeField] AudioClip _explosionSFX;
    [SerializeField] [Range(0,1)] private float _volumeExplosionSFX = 0.75f;
    [SerializeField] GameObject _explosion;

    private bool _isShieldActive;

    private Touch touch;
    private float _screenHeightInUnits;
    private float _screenWidthInUnits;
    private float _minX;
    private float _maxX;
    private float _minY;
    private float _maxY;

    private float _nextLevelLoadingDelayTime;
    private float _nextLevelLoadTimer = 3.5f;
    private float _bonusTimer = 0;
    private float _bonusTime;
    private float _timeToShoot;
    private bool _activateBoundaries = true;
    private bool _takeoverControl = true;
    private bool _isDoubleShoot = false;
    private Vector3 _firstBulletOffset;
    private Vector3 _secondBulletOffset;

    private void Start()
    {
        _healthState = FindObjectOfType<HealthState>();
        _health = _healthState.GetHealth();
        HealthUpdater();
        SetUpMoveBoundaries();
    }

    private void Update()
    {
        DragPlayerToNextLevel(_nextLevelLoadTimer);
        Shoot();
        ShootByTouch();
        BonusTimer(_bonusTime);
        if (_isMovePlayerByAxis)
        {
            _isMovePlayerByTouch = false;
            MovePlayerByAxis();
        }
        else if (_isMovePlayerByTouch)
        {
            _isMovePlayerByAxis = false;
            MovePlayerByTouch();
        }
        else
        {
            MovePlayerByMousePos();
        }
    }

    private void DragPlayerToNextLevel(float value)
    {
        _nextLevelLoadingDelayTime += Time.deltaTime;
        if(!FindObjectOfType<Enemy>() && _nextLevelLoadingDelayTime >= value)
        {
            _activateBoundaries = false;
            _takeoverControl = false;
            _nextLevelLoadingDelayTime = 0;
        }
    }

    private void BonusTimer(float value)
    {
        _bonusTimer += Time.deltaTime;
        if (_bonusTimer >= value)
        {
            SetShieldActive(false);
            _bonusTimer = 0;
        }
    }

    // *** Getters ***

    public float GetBulletSpeed()
    {
        return _bulletSpeed;
    }

    public int GetHealth()
    {
        return _health;
    }

    // *** End getters ***

    // *** Setters *** 

    public void SetBonusTime(float value)
    {
        _bonusTime = value;
    }

    public void SetHealth(int value)
    {
        _health = value;
        _healthState.SetHealth(_health);
        HealthUpdater();
    }

    public void SetBulletSpeed(float value)
    {
        _bulletSpeed = value;
    }

    public void SetDoubleShotActive(bool value)
    { 
        _isDoubleShoot = value;
    }

    public void SetShieldActive(bool value)
    {
        _isShieldActive = value;
        SetPlayerSkin();
    }

    private void SetPlayerSkin()
    {
        if (_isShieldActive)
        {
            FindObjectOfType<Player>().GetComponent<SpriteRenderer>().sprite = _shieldedPlayerSkin;
        }
        if (!_isShieldActive)
        {
            FindObjectOfType<Player>().GetComponent<SpriteRenderer>().sprite = _commonPlayerSkin;
        }
    }
    
    public void SetBulletsOffset(Vector3 value01, Vector3 value02)
    {
        _firstBulletOffset = value01;
        _secondBulletOffset = value02;
    }
    
    private void SetUpMoveBoundaries()
    {
        if (_activateBoundaries)
        {
            Camera cam = Camera.main;
            _minX = cam.ViewportToWorldPoint(new Vector3(0, 0, 0)).x + _offset;
            _maxX = cam.ViewportToWorldPoint(new Vector3(1, 0, 0)).x - _offset;
            _minY = cam.ViewportToWorldPoint(new Vector3(0, 0, 0)).y + _offset;
            _maxY = cam.ViewportToWorldPoint(new Vector3(0, 1, 0)).y - _offset;
            _screenHeightInUnits = cam.orthographicSize * 2;
            _screenWidthInUnits = cam.orthographicSize;
        }
    }

    private float GetYPos()
    {
        return Input.mousePosition.y / Screen.height * _screenHeightInUnits;
    }

    private float GetXPos()
    {
        return Input.mousePosition.x / Screen.width * _screenWidthInUnits;
    }

    private void GetDamage(Collider2D collision)
    {
        DamageDealer damageDealer = collision.gameObject.GetComponent<DamageDealer>();
        if (!damageDealer) { return; }
        if (!_isShieldActive)
        {
            SetHealth(this._health - damageDealer.GetDamage());
            damageDealer.Hit();
            if (_health <= 0)
            {
                _healthState.ResetHealth();
                FindObjectOfType<SceneLoadManager>().LoadGameOverScene();
                AudioSource.PlayClipAtPoint(_explosionSFX, transform.position, _volumeExplosionSFX);
                Instantiate(_explosion, transform.position, Quaternion.identity);
                Destroy(gameObject);
            }
        }
        else
        {
            damageDealer.Hit();
            return;
        }
    }

    // *** End setters ***

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

    private void ShootByTouch()
    {
        _timeToShoot += Time.deltaTime;
        if (Input.touchCount > 0)
        {
            touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Moved)
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
    }

    // *** Move Player ***

    private void MovePlayerByTouch()
    {
        if (_takeoverControl)
        {
            if (Input.touchCount > 0)
            {
                touch = Input.GetTouch(0);
                if (touch.phase == TouchPhase.Moved)
                {
                    var playerPosition = new Vector2( transform.position.x + touch.deltaPosition.x, 
                                                      transform.position.y + touch.deltaPosition.y);
                    transform.position = playerPosition;
                }
            }
        }
        else 
        {
            DragAnimation();
        }
    }

    private void MovePlayerByAxis()
    {
        var playerPosition = new Vector2();

        var deltaX = Input.GetAxis("Mouse X") * Time.deltaTime * _speedPlayer;
        var deltaY = Input.GetAxis("Mouse Y") * Time.deltaTime * _speedPlayer;

        playerPosition.x = Mathf.Clamp(transform.position.x + deltaX, _minX, _maxX);
        playerPosition.y = Mathf.Clamp(transform.position.y + deltaY, _minY, _maxY);

        transform.position = playerPosition;
    }

    private void MovePlayerByMousePos()
    {
        if (_takeoverControl)
        {
            Vector2 playerPosition = new Vector2(transform.position.x, transform.position.y);
            playerPosition.x = Mathf.Clamp(GetXPos(), _minX, _maxX);
            playerPosition.y = Mathf.Clamp(GetYPos(), _minY, _maxY);
            transform.position = playerPosition;
        }
        else 
        {
            DragAnimation(); 
        }
    }

    private void DragAnimation()
    {
        Vector3 triggerPos = FindObjectOfType<NextSceneTrigger>().transform.position;
        transform.position = Vector3.MoveTowards(transform.position, triggerPos, _speedPlayer / 10 * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GetDamage(collision);
    }
}
