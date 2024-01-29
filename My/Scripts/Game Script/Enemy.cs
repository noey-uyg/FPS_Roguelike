using NUnit.Framework.Constraints;
using System.Collections;
using System.Collections.Generic;
using Unity.Services.Analytics.Internal;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float maxHP;
    public float enemyCurrentHP = 0;

    [SerializeField]
    private Slider HPBar;

    [SerializeField]
    private GameObject targetPlayer;
    private NavMeshAgent agent;
    [SerializeField]
    private Animator animator;

    [SerializeField]
    private bool isReinforced = false;
    [SerializeField]
    private bool isElite = false;

    [Header("Attack")]
    [SerializeField]
    private float monsterRange;
    [SerializeField]
    private float monsterAdditionalRange;
    [SerializeField]
    private float attackDamage;
    [SerializeField]
    private GameObject attackPoint;
    [SerializeField]
    private int attackCount;
    [SerializeField]
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
        if (GameManager.Instance.isOpenTab)
        {
            agent.isStopped = true;
            return;
        }

        if(GameManager.Instance.enemyKilledNum >= GameManager.Instance.maxEnemyKilledNum)
        {
            DisableObject();
        }
        HPBar.value = enemyCurrentHP / maxHP;

        FollowTarget();
        EnemyDie();
    }

    private void FollowTarget()
    {
        if (!agent.isOnNavMesh) return;

        if(targetPlayer != null)
        {
            curAttackDelay += Time.deltaTime;

            agent.destination = targetPlayer.transform.position;

            float distance = Vector3.Distance(transform.position, targetPlayer.transform.position);
            bool isRange = distance <= monsterRange;

            if(distance > agent.stoppingDistance)
            {
                transform.LookAt(targetPlayer.transform.position);
            }

            //공격
            if (isRange)
            {
                if(curAttackDelay >= maxAttackDelay)
                {
                    agent.isStopped = true;
                    if (isElite)
                    {
                        if(distance < monsterAdditionalRange)
                        {
                            animator.SetTrigger("Attack2");
                        }
                        else
                        {
                            animator.SetTrigger("Attack1");
                        }    
                    }
                    else
                    {
                        StartCoroutine(EnemyAttackCoroutine());
                    }
                    curAttackDelay = 0f;
                }
            }
            else
            {
                agent.isStopped = false;
            }
        }
    }

    // 플레이어가 공격 범위에 들어왔는지 확인하는 함수
    private bool IsPlayerInAttackRange()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, targetPlayer.transform.position);
        return distanceToPlayer <= monsterRange;
    }

    //플레이어 데미지 입히기
    public void DamageToPlayer()
    {
        if(IsPlayerInAttackRange())
        {
            GameManager.Instance.playerCurHP -= attackDamage;
        }
    }

    private void IsReinforced()
    {
        if (isElite || GameManager.Instance.wave < 2) return;

        int ranNum = Random.Range(0, 100);

        if(GameManager.Instance.wave == 2)
        {
            if (ranNum < 5)
            {
                isReinforced = true;
            }
        }
        else if(GameManager.Instance.wave == 3)
        {
            if (ranNum < 20)
            {
                isReinforced = true;
            }
        }

    }

    private void SetObjectScale(Vector3 newScale)
    {
        gameObject.transform.localScale = newScale;
    }

    private void InitEnemyHP()
    {
        Vector3 newScale;
        IsReinforced();

        if (isReinforced)
        {
            newScale = new Vector3(5f, 5f, 5f);
            SetObjectScale(newScale);
            maxHP *= 5;
        }
        else if (isElite)
        {
            newScale = new Vector3(3f, 3f, 3f);
            SetObjectScale(newScale);
            maxHP *= 10;
        }
        else
        {
            newScale = new Vector3(3f, 3f, 3f);
            SetObjectScale(newScale);
        }

        enemyCurrentHP = (maxHP * GameManager.Instance.wave) + ((maxHP * GameManager.Instance.playerLevel) / 2);
    }

    //몬스터 웨이브 다 잡을 시 모두 비활성화
    private void DisableObject()
    {
        if (isElite) return;

        gameObject.SetActive(false);
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
        BulletDrop();

        GameManager.Instance.enemyKilledNum++;

        if (isElite)
        {
            GameManager.Instance.eliteEnemyKilledNum++;
        }
        
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
        GameObject eA = PoolManager.instance.ActivateObj(Random.Range(13, 15));
        eA.GetComponent<EnemyAttackManager>().InitBulletDamage(attackDamage);
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
        if(hpRandom < 50)
        {
            GameObject hp = PoolManager.instance.ActivateObj(9);
            hp.transform.position = gameObject.transform.position;
        }
    }

    void ScrollDrop()
    {
        int scrollRandom = Random.Range(0, 1000);
        if (scrollRandom < 5)
        {
            GameObject scroll = PoolManager.instance.ActivateObj(12);
            scroll.GetComponent<Scroll>().scrollData.haveScroll = true;
            scroll.GetComponent<Scroll>().DeleteHaveScroll();
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
        if (WeaponManager.currentWeaponType != "GUN") return;
        
        for(int i = 0; i < Random.Range(1, 3); i++)
        {
            GameObject bullet = PoolManager.instance.ActivateObj(20);
            bullet.transform.position = gameObject.transform.position;
        }
    }
}
