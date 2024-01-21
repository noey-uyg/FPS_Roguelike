using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseWeapon : MonoBehaviour
{
    public string closeWeaponName;

    //근접 무기 유형
    public bool isHand;
    public bool isAxe;

    public float range;
    public float damage;
    public float criticalPer;
    public float criticalDamage;
    public float attackDelay; // 공격 딜레이
    public float attackDelayA; // 공격 활성화 시점
    public float attackDelayB; // 공격 비활성화 시점

    public Animator anim;
    

}
