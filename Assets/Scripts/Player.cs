using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public bool player1 = false;
    public bool player2 = false;
    [SerializeField]
    private float _speed = 3.5f;
    [SerializeField]
    private float _speedBoost = 4f;
    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private float _fireRate = 0.15f;
    private float _canFire = -1f;
    private Vector3 _offset = new Vector3(0, 0.8f, 0);
    [SerializeField]
    private int _lives = 3;
    private SpawnManager _spawnManager;
    private bool _isTripleShot = false;
    private bool _isShield = false;
    private bool _isSpeed = false;
    [SerializeField]
    private GameObject _tripleShotPrefab;

    [SerializeField]
    private GameObject _shieldPrefab;
    [SerializeField]
    private GameObject _boost;
    [SerializeField]
    private GameObject[] _damagedEngines;
    [SerializeField]
    private int _score;
    private UIManager _uiManager;
    private GameManager _gameManager;
    private AudioSource _audioSource;
    [SerializeField]
    private AudioClip _laserSound;
    [SerializeField]
    private AudioClip _boostSound;
    [SerializeField]
    private GameObject _explosionPrefab;






    // Start is called before the first frame update
    void Start()
    {
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();
        _audioSource = GetComponent<AudioSource>();


        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        if (_spawnManager == null)
        {
            Debug.LogError("Spawn Manger is NULL!");
        }
        if (_uiManager == null)
        {
            Debug.Log("UIManger not Found");

        }
        if (_gameManager == null)
        {
            Debug.LogError("Game Manager is NULL!");
        }
        if (_audioSource == null)
        {
            Debug.LogError("AudioSource1 on the player is NULL!");
        }

        if (_gameManager.isCoopMode == false)
        {
            transform.position = new Vector3(0, 0, 0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (player1)
        {
            CalculateMovement();
            if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)
            {
                FireLaser();
            }
        }
        if (player2)
        {
            CalculateMovement2();
            if (Input.GetKeyDown(KeyCode.Keypad0) && Time.time > _canFire)
            {
                FireLaser();
            }
        }

    }

    void CalculateMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);

        if (_isSpeed)
        {
            transform.Translate(_speed * _speedBoost * Time.deltaTime * direction);
        }
        else
        {
            transform.Translate(_speed * Time.deltaTime * direction);
        }

        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -3.8f, 0), 0);

        if (transform.position.x > 11.3f)
        {
            transform.position = new Vector3(-11, transform.position.y, 0);
        }
        else if (transform.position.x < -11.3f)
        {
            transform.position = new Vector3(11, transform.position.y, 0);
        }

    }
    void CalculateMovement2()
    {


        if (_isSpeed)
        {
            if (Input.GetKey(KeyCode.Keypad8))
            {
                transform.Translate(_speed * _speedBoost * Time.deltaTime * Vector3.up);
            }
            if (Input.GetKey(KeyCode.Keypad6))
            {
                transform.Translate(_speed * _speedBoost * Time.deltaTime * Vector3.right);
            }
            if (Input.GetKey(KeyCode.Keypad5))
            {
                transform.Translate(_speed * _speedBoost * Time.deltaTime * Vector3.down);
            }
            if (Input.GetKey(KeyCode.Keypad4))
            {
                transform.Translate(_speed * _speedBoost * Time.deltaTime * Vector3.left);
            }
        }
        else
        {
            if (Input.GetKey(KeyCode.Keypad8))
            {
                transform.Translate(_speed * Time.deltaTime * Vector3.up);
            }
            if (Input.GetKey(KeyCode.Keypad6))
            {
                transform.Translate(_speed * Time.deltaTime * Vector3.right);
            }
            if (Input.GetKey(KeyCode.Keypad2))
            {
                transform.Translate(_speed * Time.deltaTime * Vector3.down);
            }
            if (Input.GetKey(KeyCode.Keypad4))
            {
                transform.Translate(_speed * Time.deltaTime * Vector3.left);
            }
        }

        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -3.8f, 0), 0);

        if (transform.position.x > 11.3f)
        {
            transform.position = new Vector3(-11, transform.position.y, 0);
        }
        else if (transform.position.x < -11.3f)
        {
            transform.position = new Vector3(11, transform.position.y, 0);
        }

    }

    void FireLaser()
    {
        _canFire = Time.time + _fireRate;
        if (_isTripleShot)
        {
            Instantiate(_tripleShotPrefab, transform.position, Quaternion.identity);
        }
        else
        {
            Instantiate(_laserPrefab, transform.position + _offset, Quaternion.identity);
        }

        _audioSource.PlayOneShot(_laserSound);
    }
    //     void FireLaser2()
    // {
    //     _canFire = Time.time + _fireRate;
    //     if (_isTripleShot)
    //     {
    //         Instantiate(_tripleShotPrefab, transform.position, Quaternion.identity);
    //     }
    //     else
    //     {
    //         Instantiate(_laserPrefab, transform.position + _offset, Quaternion.identity);
    //     }

    //     _audioSource.PlayOneShot(_laserSound);
    // }

    public void Damage()
    {
        if (_isShield)
        {
            _isShield = false;
            _shieldPrefab.SetActive(false);
            return;
        }
        _lives--;
        if (_lives == 2)
        {
            _damagedEngines[0].SetActive(true);
        }
        if (_lives == 1)
        {
            _damagedEngines[1].SetActive(true);
        }
        _uiManager.UpdateLives(_lives);
        if (_lives < 1)
        {

            _spawnManager.OnPlayerDeath();
            Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
            Destroy(gameObject);
            _uiManager.IsGameOver();
            _gameManager.GameOver();

        }
    }
    public void TripleShotActive()
    {
        _isTripleShot = true;
        StartCoroutine(TripleShotPowerDown());

    }


    IEnumerator TripleShotPowerDown()
    {
        yield return new WaitForSeconds(5.0f);
        _isTripleShot = false;
    }

    public void ShieldActive()
    {
        _isShield = true;
        _shieldPrefab.SetActive(true);
        StartCoroutine(ShieldPowerDown());
    }

    IEnumerator ShieldPowerDown()
    {
        yield return new WaitForSeconds(7f);
        _isShield = false;
        _shieldPrefab.SetActive(false);


    }
    public void SpeedActive()
    {
        _isSpeed = true;
        _boost.SetActive(true);
        _audioSource.PlayOneShot(_boostSound);
        StartCoroutine(SpeedPowerDown());
    }

    IEnumerator SpeedPowerDown()
    {
        yield return new WaitForSeconds(3f);
        _isSpeed = false;
        _boost.SetActive(false);
    }

    public void ScoreCalculator(int points)
    {
        _score += points;
        _uiManager.UpdateScore(_score);
    }
}
