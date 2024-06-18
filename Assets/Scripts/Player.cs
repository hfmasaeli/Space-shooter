using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
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


    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(0, 0, 0);
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        if (_spawnManager == null)
        {
            Debug.LogError("Spawn Manger is NULL!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();
        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)
        {
            FireLaser();
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
    }

    public void Damage()
    {
        if (_isShield)
        {
            _isShield = false;
            _shieldPrefab.SetActive(false);
            return;
        }
        _lives--;
        if (_lives < 1)
        {
            _spawnManager.OnPlayerDeath();
            Destroy(gameObject);
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
        StartCoroutine(SpeedPowerDown());
    }

    IEnumerator SpeedPowerDown()
    {
        yield return new WaitForSeconds(3f);
        _isSpeed = false;
        _boost.SetActive(false);
    }
}
