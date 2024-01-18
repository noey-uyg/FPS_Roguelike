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
        // Transform을 직접 조작하여 이동
        transform.Translate(Vector3.forward * attackMoveSpeed * Time.deltaTime);
    }
}
