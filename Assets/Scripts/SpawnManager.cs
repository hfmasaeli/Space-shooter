using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemyPrefab;
    [SerializeField]
    private GameObject _enemyContainer;
    [SerializeField]
    private GameObject[] _powerUps = new GameObject[3];


    private bool _stopSpawning = false;
    // Start is called before the first frame update
    void Start()
    {

    }
    public void StartSpawning()
    {
        StartCoroutine(SpawnEnemyRoutine());
        StartCoroutine(SpawnPowerupRoutine());
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator SpawnEnemyRoutine()

    {
        yield return new WaitForSeconds(1.3f);
        while (_stopSpawning == false)
        {
            GameObject newEnemy = Instantiate(_enemyPrefab, new Vector3(Random.Range(-8.0f, 8.0f), 7.3f, 0), Quaternion.identity);
            newEnemy.transform.parent = _enemyContainer.transform;
            yield return new WaitForSeconds(5.0f);

        }
    }
    IEnumerator SpawnPowerupRoutine()
    {
        yield return new WaitForSeconds(1.3f);
        while (_stopSpawning == false)
        {
            int rand = Random.Range(0, 3);
            GameObject powerup = Instantiate(_powerUps[rand], new Vector3(Random.Range(-8.0f, 8.0f), 7.3f, 0), Quaternion.identity);
            powerup.transform.parent = _enemyContainer.transform;
            yield return new WaitForSeconds(Random.Range(3, 8));

        }
    }

    public void OnPlayerDeath()
    {
        _stopSpawning = true;
    }
}
