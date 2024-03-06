using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;

public class ItemPickUp : MonoBehaviour
{
    public Item item;

    [SerializeField]
    private Transform player;
    private float moveSpeed = 50f;
    private float pickupRadius = 5f;
    private Rigidbody rb;

    private void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (item.itemType == Item.ItemType.Scroll) return;
        // �÷��̾���� �Ÿ� ���
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // �÷��̾���� �Ÿ��� ���� ���� �̳��� ���
        if (distanceToPlayer <= pickupRadius)
        {
            // �������� �÷��̾� ������ �̵�
            MoveTowardsPlayer();
        }
    }

    void MoveTowardsPlayer()
    {
        // �÷��̾� ������ �̵� ���� ���
        Vector3 moveDirection = (player.position - transform.position).normalized;

        // Rigidbody�� ���� �̵�
        rb.MovePosition(transform.position + moveDirection * moveSpeed * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            SoundManager.instance.PlaySE("Item_PickUp");
            switch (item.itemType)
            {
                case Item.ItemType.EXP:
                    GameManager.Instance.GetEXP();
                    gameObject.SetActive(false);
                    break;
                case Item.ItemType.HP:
                    GameManager.Instance.GetHP();
                    gameObject.SetActive(false);
                    break;
                case Item.ItemType.Crystal:
                    GameManager.Instance.GetCrystal();
                    gameObject.SetActive(false);
                    break;
                case Item.ItemType.Gold:
                    GameManager.Instance.GetGold();
                    gameObject.SetActive(false);
                    break;
                case Item.ItemType.Bullet:
                    GameManager.Instance.GetBullet();
                    gameObject.SetActive(false);
                    break;
            }
        }
    }

}
