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

    private void Start()
    {
        GameManager.OnWaveStart += SpawnShop;
    }

    private void Update()
    {
        if (GameManager.Instance.isOpenTab) return;

        if (GameManager.Instance.gameIsStart)
        {
            spawnTime += Time.deltaTime;

            if (GameManager.Instance.isElite)
            {
                if (GameManager.Instance.eliteSpawnCount > 0)
                {
                    SpawnElite();
                }
            }
            else
            {
                if (spawnTime >= spawnmaxTime)
                {
                    SpawnEnemy();
                }
            }
        }
    }


    //일반 몬스터 스폰
    private void SpawnEnemy()
    {
        if (GameManager.Instance.enemyKilledNum >= GameManager.Instance.maxEnemyKilledNum) return;

        GameObject enemy = PoolManager.instance.ActivateObj(Random.Range(0, 7));
        enemy.transform.position = spawnPoint[Random.Range(0, spawnPoint.Length)].position;

        spawnTime = 0f;
    }
    
    //엘리트몬스터 스폰
    private void SpawnElite()
    {
        GameObject elite = PoolManager.instance.ActivateObj(Random.Range(17, 19));
        elite.transform.position = spawnPoint[Random.Range(0,spawnPoint.Length)].position;

        GameManager.Instance.eliteSpawnCount--;
    }

    //상점 스폰
    private void SpawnShop()
    {
        GameObject shop = PoolManager.instance.ActivateObj(19);
        shop.transform.position = spawnPoint[Random.Range(0, spawnPoint.Length)].position;
    }
}
