using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandController : CloseWeaponController
{
    //활성화 여부
    public static bool isActivate = false;

    Enemy enemy;

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
            if (CheckObject())
            {
                isSwing = false;

                enemy = hitInfo.collider.GetComponent<Enemy>();

                if (enemy != null && enemy.enemyCurrentHP > 0)
                {
                    int cri = Random.Range(0, 100);
                    int criPer = (int)(currentCloseWeapon.criticalPer + (GameManager.Instance.extraCriticalPer * 100));
                    float cridam = currentCloseWeapon.criticalDamage + (GameManager.Instance.extraCriticalDamage);
                    float damage = currentCloseWeapon.damage + (currentCloseWeapon.damage * GameManager.Instance.gunExtraDamage);

                    if (cri < criPer)
                    {
                        enemy.enemyCurrentHP -= (damage + (damage * cridam)) + ((damage + (damage * cridam)) * GameManager.Instance.extraFinalDamage);
                    }
                    else
                    {
                        enemy.enemyCurrentHP -= damage + (damage * GameManager.Instance.extraFinalDamage);
                    }
                }
            }
            yield return null;
        }
    }

    public override void CloseWeaponChange(CloseWeapon hand)
    {
        base.CloseWeaponChange(hand);
        isActivate = true;
    }
}
