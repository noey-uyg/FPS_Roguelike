using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HandController : CloseWeaponController
{
    //활성화 여부
    public static bool isActivate = false;

    Enemy enemy;

    private int handStack = 0;
    private float handStackDamage = 0;

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
            if (CheckObject())
            {
                isSwing = false;

                if (GameManager.Instance.handIsStack)
                {
                    if (handStack < 8) handStack++;
                }

                handStackDamage = GameManager.Instance.handStackDamage * handStack; 

                enemy = hitInfo.collider.GetComponent<Enemy>();

                if (enemy != null && enemy.enemyCurrentHP > 0)
                {
                    int cri = Random.Range(0, 100);
                    int criPer = (int)(currentCloseWeapon.criticalPer + (GameManager.Instance.extraCriticalPer * 100));
                    float cridam = currentCloseWeapon.criticalDamage + (GameManager.Instance.extraCriticalDamage);
                    float damage = currentCloseWeapon.damage + (currentCloseWeapon.damage * GameManager.Instance.extraDamage) + (currentCloseWeapon.damage * handStackDamage) + (currentCloseWeapon.damage * GameManager.Instance.handExtraDamage);

                    if (cri < criPer)
                    {
                        enemy.enemyCurrentHP -= (damage + (damage * cridam)) + ((damage + (damage * cridam)) * GameManager.Instance.extraFinalDamage);
                    }
                    else
                    {
                        enemy.enemyCurrentHP -= damage + (damage * GameManager.Instance.extraFinalDamage);
                    }

                    if (GameManager.Instance.handWave)
                    {
                        GameObject hitEfffect = PoolManager.instance.ActivateObj(16);
                        hitEfffect.transform.position = hitInfo.point;
                        hitEfffect.transform.rotation = hitInfo.transform.rotation;
                        HandWaveAttack(damage);
                    }
                }
            }
            yield return null;
        }
    }

    //파동공격
    private void HandWaveAttack(float damage)
    {
        Collider[] colliders = Physics.OverlapSphere(hitInfo.point, 20f, LayerMask.GetMask("Enemy"))
                .Where(collider => collider.gameObject != hitInfo.collider.gameObject)
                .ToArray();

        for (int i = 0; i < colliders.Length; i++)
        {
            Enemy nearbyEnemy = colliders[i].GetComponent<Enemy>();

            if (TargetInRange(hitInfo.transform, nearbyEnemy.transform))
            {
                if (nearbyEnemy != null && nearbyEnemy.enemyCurrentHP > 0)
                {
                    nearbyEnemy.enemyCurrentHP -= (damage + (damage * GameManager.Instance.handWaveDamage)) + (damage + (damage * GameManager.Instance.handWaveDamage)) * GameManager.Instance.extraFinalDamage;
                }
            }
        }
    }

    //피격당한 적 뒤의 몬스터 탐지
    private bool TargetInRange(Transform caster, Transform target)
    {
        float angleRange = 120f;

        Vector3 toTarget = target.position - caster.position;
        Vector3 back = -caster.forward;

        float angle = Vector3.Angle(back, toTarget);

        return angle <= angleRange * 0.5f;
    }

    public override void CloseWeaponChange(CloseWeapon hand)
    {
        base.CloseWeaponChange(hand);
        isActivate = true;
    }
}
