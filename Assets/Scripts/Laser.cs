using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEditor;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField]
    private float _speed = 8.0f;
    private bool _isEnemy = false;

    void Update()
    {
        if (_isEnemy == false)
        {
            MoveUp();
        }
        else
        {
            MoveDown();
        }

    }
    void MoveUp()
    {
        transform.Translate(_speed * Time.deltaTime * Vector3.up);
        if (transform.position.y > 7f)
        {
            if (transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }
            Destroy(gameObject);
        }
    }
    void MoveDown()
    {
        transform.Translate(_speed * Time.deltaTime * Vector3.down);
        if (transform.position.y < -7f)
        {
            if (transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }
            Destroy(gameObject);
        }
    }
    public void EnemyLaserInstantiation()
    {
        _isEnemy = true;
        gameObject.tag = "EnemyLaser";
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && _isEnemy == true)
        {
            Player player = other.GetComponent<Player>();
            player.Damage();
        }
    }
}
