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
            if (CheckObject())
            {
                isSwing = false;

                enemy = hitInfo.collider.GetComponent<Enemy>();

                if (enemy != null && enemy.enemyCurrentHP > 0)
                {
                    enemy.enemyCurrentHP -= currentCloseWeapon.damage;
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
