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
        destroyAttack = maxDestroyAttack;
    }
    // Update is called once per frame
    void Update()
    {
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
            GameManager.Instance.playerCurHP -= attackDamage;
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
