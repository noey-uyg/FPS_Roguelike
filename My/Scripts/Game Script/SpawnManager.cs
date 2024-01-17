using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SpawnManager : MonoBehaviour
{

    [SerializeField]
    private Transform[] spawnPoint;

    //[SerializeField]
    //private GameObject[] enemys;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(EnemySpawn());
    }


    IEnumerator EnemySpawn()
    {
        GameObject enemy = PoolManager.instance.ActivateObj(Random.Range(0, 7));
        enemy.transform.position = spawnPoint[Random.Range(0, spawnPoint.Length)].position;

        yield return new WaitForSeconds(2.5f);

        StartCoroutine(EnemySpawn());
    }
}
