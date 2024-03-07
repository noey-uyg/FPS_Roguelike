using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackManager : MonoBehaviour
{
    [SerializeField]
    private float attackMoveSpeed;
    [SerializeField]
    private float attackDamage;
    [SerializeField]
    private float maxDestroyAttack;
    private float destroyAttack;

    private void OnEnable()
    {
        SoundManager.instance.PlaySE("Enemy_Attack");
        destroyAttack = maxDestroyAttack;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.isOpenTab || GameManager.Instance.isOpenPause) return;

        destroyAttack -= Time.deltaTime;

        AttackMove();
        if(destroyAttack <= 0)
        {
            DestroyBullet();
        }
    }

    public void InitBulletDamage(float damage)
    {
        attackDamage = damage;
    }

    private void AttackMove()
    {
        // Transform을 직접 조작하여 이동
        transform.Translate(Vector3.forward * attackMoveSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            SoundManager.instance.PlaySE("Player_Hit");
            float addDam = GameManager.Instance.isBloodCurse ? attackDamage * 0.5f : 0;
            attackDamage += addDam;
            GameManager.Instance.playerData.playerCurHP -= (attackDamage - (attackDamage * GameManager.Instance.playerTraitsData.traitsReduceDam));
            gameObject.SetActive(false);
        }
        else if (other.CompareTag("Ground"))
        {
            gameObject.SetActive(false);
        }
    }

    private void DestroyBullet()
    {
        gameObject.SetActive(false);
        destroyAttack = maxDestroyAttack;
    }
}
