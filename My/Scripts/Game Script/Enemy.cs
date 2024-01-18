using NUnit.Framework.Constraints;
using System.Collections;
using System.Collections.Generic;
using Unity.Services.Analytics.Internal;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    private float maxHP = 100;
    public float enemyCurrentHP = 0;

    [SerializeField]
    private Slider HPBar;

    private NavMeshAgent agent;

    [SerializeField]
    private GameObject targetPlayer;

    [Header("Attack")]
    [SerializeField]
    private GameObject attackPoint;
    [SerializeField]
    private int attackCount;
    private float maxAttackDelay = 5f;
    private float curAttackDelay = 0f;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();        
    }

    private void OnEnable()
    {
        InitEnemyHP();
        targetPlayer = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        HPBar.value = enemyCurrentHP / maxHP;
        FollowTarget();
        EnemyDie();
    }

    private void FollowTarget()
    {
        if(targetPlayer != null)
        {
            agent.destination = targetPlayer.transform.position;
            transform.LookAt(targetPlayer.transform.position);

            bool isRange = Vector3.Distance(transform.position, targetPlayer.transform.position) <= agent.stoppingDistance;
            curAttackDelay += Time.deltaTime;

            //АјАн
            if (isRange)
            {
                if(curAttackDelay >= maxAttackDelay)
                {
                    StartCoroutine(EnemyAttackCoroutine());
                    curAttackDelay = 0f;
                }
            }
        }

    }

    private void InitEnemyHP()
    {
        enemyCurrentHP = maxHP;
    }

    private void EnemyDie()
    {
        if (enemyCurrentHP <= 0)
        {
            StartCoroutine(EnemyDieCoroutine());
            return;
        }
    }

    IEnumerator EnemyDieCoroutine()
    {
        agent.speed = 0;
        
        yield return new WaitForSeconds(0.4f);

        ExpDrop();
        HPDrop();
        ScrollDrop();
        GoldDrop();
        CrystalDrop();

        gameObject.SetActive(false);
    }

    IEnumerator EnemyAttackCoroutine()
    {
        for (int i = 0; i < attackCount; i++)
        {
            EnemyAttack();

            yield return new WaitForSeconds(1f);
        }
        
    }

    private void EnemyAttack()
    {
        Vector3 aim = (targetPlayer.transform.position - attackPoint.transform.position).normalized;
        GameObject eA = PoolManager.instance.ActivateObj(Random.Range(25, 27));
        eA.transform.position = attackPoint.transform.position;
        eA.transform.rotation = Quaternion.LookRotation(aim, Vector3.up);
    }

    void ExpDrop()
    {
        GameObject exp = PoolManager.instance.ActivateObj(8);
        exp.transform.position = gameObject.transform.position;
    }

    void HPDrop()
    {
        int hpRandom = Random.Range(0, 100);
        if(hpRandom < 1000)
        {
            GameObject hp = PoolManager.instance.ActivateObj(9);
            hp.transform.position = gameObject.transform.position;
        }
    }

    void ScrollDrop()
    {
        int hpRandom = Random.Range(0, 1000);
        if (hpRandom < 1000)
        {
            GameObject scroll = PoolManager.instance.ActivateObj(Random.Range(12,25));
            scroll.transform.position = gameObject.transform.position;
        }
    }

    void GoldDrop()
    {
        for(int i = 0; i < Random.Range(0, 5); i++)
        {
            GameObject gold = PoolManager.instance.ActivateObj(10);
            gold.transform.position = gameObject.transform.position;
        }
    }

    void CrystalDrop()
    {
        for(int i = 0; i < Random.Range(0, 3); i++)
        {
            GameObject crystal = PoolManager.instance.ActivateObj(11);
            crystal.transform.position = gameObject.transform.position;
        }
    }

    void BulletDrop()
    {

    }
}
