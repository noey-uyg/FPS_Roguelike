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
    [SerializeField]
    private float spawnmaxTime = 2.5f;

    private void Start()
    {
        GameManager.OnWaveStart += SpawnShop;
        GameManager.OnBossWave += SpawnBoss;
        GameManager.OnEliteWave += SpawnElite;
    }

    private void Update()
    {
        if (GameManager.Instance.isOpenTab) return;

        if (GameManager.Instance.gameIsStart)
        {
            spawnTime += Time.deltaTime;

            if (!GameManager.Instance.isEliteWave)
            {
                if (spawnTime >= SpawnTime())
                {

                    SpawnEnemy();
                }
            }
            
        }
    }

    //���̺� �� ���� �ð�
    private float SpawnTime()
    {
        return spawnmaxTime/ GameManager.Instance.wave;
    }

    //�Ϲ� ���� ����
    private void SpawnEnemy()
    {
        if (GameManager.Instance.enemyKilledNum >= GameManager.Instance.maxEnemyKilledNum || GameManager.Instance.wave == 4) return;

        GameObject enemy = PoolManager.instance.ActivateObj(Random.Range(0, 7));
        enemy.transform.position = GetValidSpawnPoint();

        spawnTime = 0f;
    }
    
    //����Ʈ���� ����
    private void SpawnElite()
    {
        for(int i=0;i<GameManager.Instance.eliteSpawnCount; i++)
        {
            GameObject elite = PoolManager.instance.ActivateObj(Random.Range(17, 19));
            elite.transform.position = GetValidSpawnPoint();
        }
    }

    //�������� ����
    private void SpawnBoss()
    {
        GameObject boss = PoolManager.instance.ActivateObj(21);
        boss.transform.position = spawnPoint[spawnPoint.Length-1].position;
    }
    //���� ����
    private void SpawnShop()
    {
        GameObject shop = PoolManager.instance.ActivateObj(19);
        shop.transform.position = GetValidSpawnPoint();
    }

    private Vector3 GetValidSpawnPoint()
    {
        // �������� ���õ� ���� ����Ʈ
        Transform selectedSpawnPoint = spawnPoint[Random.Range(0, spawnPoint.Length)];
        Vector3 spawnPosition = selectedSpawnPoint.position;

        // ��ħ üũ �� ����
        while (CheckOverlap(spawnPosition))
        {
            selectedSpawnPoint = spawnPoint[Random.Range(0, spawnPoint.Length)];
            spawnPosition = selectedSpawnPoint.position;
        }

        return spawnPosition;
    }

    private bool CheckOverlap(Vector3 position)
    {
        Collider[] colliders = Physics.OverlapSphere(position, 1f); // ���� �������� 1�� ����

        // ��ġ�� ������Ʈ�� �ִ��� üũ
        if(colliders.Length > 0)
        {
            return true;
        }

        return false; // ��ġ�� ������Ʈ�� ������ false ��ȯ
    }
}
