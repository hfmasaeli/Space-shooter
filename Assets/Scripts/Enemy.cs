using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _speed = 4.0f;
    private Player _player;

    private Animator _anim;
    private AudioSource _audioSource;
    [SerializeField]
    private GameObject _laserPrefab;
    private float _canFire = -1;
    private float _fireRate = 3.0f;
    private bool _isExploded = false;



    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();

        _audioSource = GetComponent<AudioSource>();
        if (_player == null)
        {
            Debug.LogError("Player not Found!");
        }

        _anim = GetComponent<Animator>();
        if (_anim == null)
        {
            Debug.LogError("Enemy animator not found");
        }
        if (_audioSource == null)
        {
            Debug.LogError("AudioSource on enemy is NULL!");
        }

    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();
        if (Time.time > _canFire && _isExploded == false)
        {
            _fireRate = Random.Range(3.0f, 5.0f);
            _canFire = Time.time + _fireRate;

            GameObject enemyLaser = Instantiate(_laserPrefab, transform.position, Quaternion.identity);
            Laser[] lasers = enemyLaser.GetComponentsInChildren<Laser>();
            for (int i = 0; i < lasers.Length; i++)
            {
                lasers[i].EnemyLaserInstantiation();
            }
        }


    }

    private void CalculateMovement()
    {
        transform.Translate(_speed * Time.deltaTime * Vector3.down);
        if (transform.position.y < -5.3)
        {
            transform.position = new Vector3(Random.Range(-8.0f, 8.0f), 7.3f, 0);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Player player = other.transform.GetComponent<Player>();
            if (player != null)
            {
                player.Damage();
            }
            _anim.SetTrigger("OnEnemyDeath");
            _speed = 0;
            _isExploded = true;
            Destroy(this.gameObject, 2.5f);
            Destroy(GetComponent<Collider2D>());
            _audioSource.Play();
        }

        if (other.CompareTag("Laser"))
        {
            Destroy(other.gameObject);
            if (_player != null)
            {
                _player.ScoreCalculator(10);
            }
            _anim.SetTrigger("OnEnemyDeath");
            _speed = 0;
            _isExploded = true;
            Destroy(this.gameObject, 2.5f);
            Destroy(GetComponent<Collider2D>());
            _audioSource.Play();
        }
    }
}
