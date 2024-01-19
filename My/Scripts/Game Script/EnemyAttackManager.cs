using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackManager : MonoBehaviour
{
    [SerializeField]
    private float attackMoveSpeed;
    [SerializeField]
    private float attackDamage;

    private float destroyAttack;

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

    private void AttackMove()
    {
        // Transform�� ���� �����Ͽ� �̵�
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
        destroyAttack = 4f;
    }
}