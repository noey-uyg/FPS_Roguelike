using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackManager : MonoBehaviour
{
    [SerializeField]
    private float attackMoveSpeed;

    // Update is called once per frame
    void Update()
    {
        AttackMove();
    }

    private void AttackMove()
    {
        // Transform�� ���� �����Ͽ� �̵�
        transform.Translate(Vector3.forward * attackMoveSpeed * Time.deltaTime);
    }
}
