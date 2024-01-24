using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class SpawnManager : MonoBehaviour
{

    [SerializeField]
    private Transform[] spawnPoint;

    private float spawnTime = 0f;
    private float spawnmaxTime = 2.5f;

    private void Update()
    {
        if (!GameManager.Instance.gameIsStart) return;

        spawnTime += Time.deltaTime;

        if(spawnTime >= spawnmaxTime)
        {
            SpawnEnemy();
        }

        if (GameManager.Instance.enemyKilledNum >= GameManager.Instance.maxEnemyKilledNum)
        {
            if (GameManager.Instance.eliteSpawnCount > 0)
            {
                StartCoroutine(EliteSpawnCoroutine());
                GameManager.Instance.eliteSpawnCount--;
            }
        }
    }

    private void SpawnEnemy()
    {
        GameObject enemy = PoolManager.instance.ActivateObj(Random.Range(0, 7));
        enemy.transform.position = spawnPoint[Random.Range(0, spawnPoint.Length)].position;

        spawnTime = 0f;
    }
    
    IEnumerator EliteSpawnCoroutine()
    {
        GameObject elite = PoolManager.instance.ActivateObj(Random.Range(0, 7));
        elite.transform.position = spawnPoint[Random.Range(0,spawnPoint.Length)].position;

        yield return new WaitForSeconds(3f);
    }
}
