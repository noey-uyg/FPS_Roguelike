using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AxeController : CloseWeaponController
{
    //활성화 여부
    public static bool isActivate = false;

    Enemy enemy;

    private void Start()
    {
        WeaponManager.currentWeapon = currentCloseWeapon.GetComponent<Transform>();
        WeaponManager.currentWeaponAnim = currentCloseWeapon.anim;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isActivate || GameManager.Instance.isOpenTab)
            return;
        TryAttack();
    }

    protected override IEnumerator HitCoroutine()
    {
        while (isSwing)
        {
            EnemyAttackDam();

            yield return null;
        }
    }

    private void EnemyAttackDam()
    {
        if (CheckObject())
        {
            isSwing = false;

            enemy = hitInfo.collider.GetComponent<Enemy>();

            if (enemy != null && enemy.enemyCurrentHP > 0)
            {
                GameManager.Instance.puzzleCriExtraDam();
                int cri = Random.Range(0, 100);
                int criPer = (int)(currentCloseWeapon.criticalPer + (GameManager.Instance.extraCriticalPer * 100));
                float cridam = currentCloseWeapon.criticalDamage + (GameManager.Instance.extraCriticalDamage);
                float damage = currentCloseWeapon.damage + (currentCloseWeapon.damage * GameManager.Instance.extraDamage) + (currentCloseWeapon.damage * GameManager.Instance.axeExtraDamage);
                GameManager.Instance.playerCurDamage = damage;
                if (GameManager.Instance.axeFear)
                {
                    damage = damage + (damage * GameManager.Instance.axeFearDamage);
                }

                if (cri < criPer)
                {
                    CriticalNearbyEnemyAttack();
                    enemy.enemyCurrentHP -= (damage + (damage * cridam)) + ((damage + (damage * cridam)) * GameManager.Instance.extraFinalDamage) + GameManager.Instance.REBAddAttack(enemy, damage);
                }
                else
                {
                    enemy.enemyCurrentHP -= damage + (damage * GameManager.Instance.extraFinalDamage) + GameManager.Instance.REBAddAttack(enemy, damage);
                }

                if (GameManager.Instance.axeBleeding)
                {
                    StartCoroutine(BleedingDamCoroutine(damage));
                }
            }
        }
    }

    IEnumerator BleedingDamCoroutine(float damage)
    {
        float elapsedTime = 0f;
        float duration = 5f;

        while(elapsedTime < duration)
        {
            enemy.enemyCurrentHP -= (damage + (damage * GameManager.Instance.extraFinalDamage)) * GameManager.Instance.axeBleedingDamage;
            Debug.Log(elapsedTime);

            yield return new WaitForSeconds(1f);
            elapsedTime += 1;

        }
        
    }

    public override void CloseWeaponChange(CloseWeapon hand)
    {
        base.CloseWeaponChange(hand);
        isActivate = true;
    }

    //치명타 시 주변 적 공격(퍼즐)
    private void CriticalNearbyEnemyAttack()
    {
        if (GameManager.Instance.puzzleCriNearby)
        {
            Collider[] colliders = Physics.OverlapSphere(hitInfo.point, 10f, LayerMask.GetMask("Enemy"))
                .Where(collider => collider.gameObject != hitInfo.collider.gameObject)
                .ToArray();

            for (int i = 0; i < colliders.Length; i++)
            {
                Enemy nearbyEnemy = colliders[i].GetComponent<Enemy>();

                if (nearbyEnemy != null && nearbyEnemy.enemyCurrentHP > 0)
                {
                    nearbyEnemy.enemyCurrentHP -= (nearbyEnemy.enemyCurrentHP * 0.05f);
                }
            }
        }
    }
}
