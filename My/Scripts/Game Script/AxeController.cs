using System.Collections;
using System.Collections.Generic;
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
        if (!isActivate)
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
                int cri = Random.Range(0, 100);
                int criPer = (int)(currentCloseWeapon.criticalPer + (GameManager.Instance.extraCriticalPer * 100));
                float cridam = currentCloseWeapon.criticalDamage + (GameManager.Instance.extraCriticalDamage);
                float damage = currentCloseWeapon.damage + (currentCloseWeapon.damage * GameManager.Instance.axeExtraDamage);

                if (GameManager.Instance.axeFear)
                {
                    damage = damage + (damage * GameManager.Instance.axeFearDamage);
                }

                if (cri < criPer)
                {
                    enemy.enemyCurrentHP -= (damage + (damage * cridam)) + ((damage + (damage * cridam)) * GameManager.Instance.extraFinalDamage);
                }
                else
                {
                    enemy.enemyCurrentHP -= damage + (damage * GameManager.Instance.extraFinalDamage);
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
}
