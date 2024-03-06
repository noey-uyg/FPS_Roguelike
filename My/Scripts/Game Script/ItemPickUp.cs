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
        // 플레이어와의 거리 계산
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // 플레이어와의 거리가 일정 범위 이내인 경우
        if (distanceToPlayer <= pickupRadius)
        {
            // 아이템을 플레이어 쪽으로 이동
            MoveTowardsPlayer();
        }
    }

    void MoveTowardsPlayer()
    {
        // 플레이어 쪽으로 이동 벡터 계산
        Vector3 moveDirection = (player.position - transform.position).normalized;

        // Rigidbody를 통해 이동
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
