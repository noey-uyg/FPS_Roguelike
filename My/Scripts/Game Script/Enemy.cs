using NUnit.Framework.Constraints;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Services.Analytics.Internal;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public float maxHP;
    public float enemyCurrentHP = 0;

    [SerializeField]
    private Slider HPBar;

    [SerializeField]
    private GameObject targetPlayer;
    private NavMeshAgent agent;
    [SerializeField]
    private Animator animator;

    public bool isReinforced = false;
    public bool isElite = false;
    public bool isBoss = false;

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
    [SerializeField]
    private PuzzleItemData[] puzzleData;

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
        if (GameManager.Instance.isOpenTab || GameManager.Instance.isOpenPause)
        {
            agent.isStopped = true;
            return;
        }

        if(GameManager.Instance.isEliteWave || GameManager.Instance.mainScene)
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
                SmoothLookAt(targetPlayer.transform.position);
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
                    else if (isBoss)
                    {
                        StartCoroutine(BossThink());
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

    private void SmoothLookAt(Vector3 targetPosition)
    {
        Vector3 direction = (targetPosition - transform.position).normalized;
        Quaternion toRotation = Quaternion.LookRotation(direction);

        transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, agent.speed * Time.deltaTime);
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
            float addDam = GameManager.Instance.isBloodCurse ? attackDamage * 0.5f : 0;
            attackDamage += addDam;
            GameManager.Instance.playerData.playerCurHP -= (attackDamage - (attackDamage * GameManager.Instance.playerTraitsData.traitsReduceDam));
        }
    }

    private void IsReinforced()
    {
        if (isElite || isBoss || GameManager.Instance.wave < 2) return;

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
            maxHP *= 1;
        }
        else if (isBoss)
        {
            maxHP *= 100;
        }
        else
        {
            newScale = new Vector3(3f, 3f, 3f);
            SetObjectScale(newScale);
        }

        maxHP = (maxHP * GameManager.Instance.wave) + ((maxHP * GameManager.Instance.playerData.playerLevel) / 2);
        maxHP *= GameManager.Instance.difficultyLevel;
        attackDamage *= GameManager.Instance.difficultyLevel;
        enemyCurrentHP = maxHP;
    }

    //몬스터 웨이브 다 잡을 시 모두 비활성화
    private void DisableObject()
    {
        if (isElite || isBoss) return;

        gameObject.SetActive(false);
    }

    private void EnemyDie()
    {
        if (enemyCurrentHP <= 0)
        { 
            StartCoroutine(EnemyDieCoroutine());
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

        GameManager.Instance.DestroyerScroll();

        if (isBoss)
        {
            GameManager.Instance.isClear = true;
            GameManager.Instance.gameIsStart = false;
            GameManager.Instance.ClearUIShow();
            PuzzleDrop();
        }
        if (isElite)
        {
            GameManager.Instance.EliteKiler();
            GameManager.Instance.eliteEnemyKilledNum++;
        }
        else
        {
            GameManager.Instance.enemyKilledNum++;
        }

        GameManager.Instance.allEnemyKill++;
        CriticalNearbyEnemyAttack();
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
        int scrollRandom = Random.Range(0, 5);
        if(isReinforced || isElite)
        {
            if (scrollRandom < 15)
            {
                ScrollObjectDrop();
            }
        }
        else if(scrollRandom < 5)
        {
            ScrollObjectDrop();
        }
    }

    void PuzzleDrop()
    {
        bool allHave = true;

        foreach (var puzzle in puzzleData)
        {
            if (!puzzle.isHave)
            {
                allHave = false;
                break;
            }
        }

        if (allHave) return;

        while (true)
        {
            int ranNum = Random.Range(0, puzzleData.Length);

            if (!puzzleData[ranNum].isHave)
            {
                puzzleData[ranNum].isHave = true;
                break;
            }
        }
    }

    void ScrollObjectDrop()
    {
        GameObject scroll = PoolManager.instance.ActivateObj(12);
        scroll.GetComponent<Scroll>().scrollData.dropScroll = true;
        scroll.GetComponent<Scroll>().DeleteHaveScroll();
        scroll.transform.position = gameObject.transform.position;
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

    //보스
    IEnumerator BossThink()
    {
        float randPattern = Random.Range(0, 5);

        switch (randPattern)
        {
            case 0:
            case 1:
            case 2:
                StartCoroutine(Bombard());
                break;
            case 3:
            case 4:
                StartCoroutine(IndisciAttack());
                break;
        }

        yield return new WaitForSeconds(maxAttackDelay*2);
    }

    IEnumerator Bombard()
    {
        animator.SetTrigger("Bombard");

        yield return new WaitForSeconds(0.1f);

        StartCoroutine(BombardIndicator());
    }

    IEnumerator BombardIndicator()
    {
        GameObject indicator = PoolManager.instance.ActivateObj(23);
        Vector3 targetPosition = targetPlayer.transform.position;
        targetPosition.y -= 1f;
        indicator.transform.position = targetPosition;

        indicator.GetComponent<DrawCircle>().DrawCircleAtPosition(targetPosition);
        yield return new WaitForSeconds(3f);

        StartCoroutine(BombAttack(indicator.transform.position));
    }

    IEnumerator BombAttack(Vector3 position)
    {
        GameObject Bomb = PoolManager.instance.ActivateObj(Random.Range(24, 26));
        Bomb.transform.position = position;

        float explosionRadius = 3.5f;

        Collider[] colliders = Physics.OverlapSphere(position, explosionRadius);

        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Player"))
            {
                GameManager.Instance.playerData.playerCurHP -= (10 - (10 * GameManager.Instance.playerTraitsData.traitsReduceDam));
            }
        }

        yield return new WaitForSeconds(0.5f);

        Bomb.SetActive(false);
    }

    IEnumerator IndisciAttack()
    {
        animator.SetTrigger("IndisciAttack");

        yield return new WaitForSeconds(0.5f);

        for (int i=0;i<attackCount; i++)
        {
            BossAttack();

            yield return new WaitForSeconds(0.3f);
        }

        yield return new WaitForSeconds(maxAttackDelay);
    }

    Vector3 CalculateAttackPosition()
    {
        // 플레이어 주변 위치 계산 로직
        Vector3 playerPosition = targetPlayer.transform.position;
        Vector3 attackPosition = playerPosition + Random.insideUnitSphere * (monsterRange/4);

        float desiredY = 7f;
        attackPosition.y = Mathf.Max(attackPosition.y, desiredY);

        return attackPosition;
    }

    void BossAttack()
    {
        Vector3 attackPosition = CalculateAttackPosition();

        GameObject attackPt = PoolManager.instance.ActivateObj(22);
        GameObject eA = PoolManager.instance.ActivateObj(Random.Range(13,15));
        attackPt.transform.position = attackPosition;

        Vector3 directionToPlayer = (targetPlayer.transform.position - attackPt.transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(directionToPlayer);
        attackPt.transform.rotation = lookRotation;

        eA.GetComponent<EnemyAttackManager>().InitBulletDamage(attackDamage);
        eA.transform.position = attackPt.transform.position;
        eA.transform.rotation = Quaternion.LookRotation(directionToPlayer, Vector3.up);

        StartCoroutine(DisableAfterDelay(attackPt));
    }

    IEnumerator DisableAfterDelay(GameObject obj)
    {
        yield return new WaitForSeconds(0.3f);
        obj.SetActive(false);
    }

    //처치시 주위 적에게 데미지(퍼즐)
    private void CriticalNearbyEnemyAttack()
    {
        if (GameManager.Instance.puzzleKillNearby)
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, 10f, LayerMask.GetMask("Enemy"))
                    .Where(collider => collider.gameObject != gameObject)
                    .ToArray();

            for (int i = 0; i < colliders.Length; i++)
            {
                Enemy nearbyEnemy = colliders[i].GetComponent<Enemy>();

                if (nearbyEnemy != null && nearbyEnemy.enemyCurrentHP > 0)
                {
                    nearbyEnemy.enemyCurrentHP -= (nearbyEnemy.enemyCurrentHP * 0.15f);
                }
            }
        }
    }
}
