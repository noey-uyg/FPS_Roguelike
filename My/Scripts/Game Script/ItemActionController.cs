using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.UI;

public class ItemActionController : MonoBehaviour
{
    [SerializeField]
    private float range;

    private bool pickupActivated = false;

    private RaycastHit hitInfo;

    [SerializeField]
    private LayerMask layerMask;

    [SerializeField]
    private Text actionText;

    [SerializeField]
    private RectTransform scrollRect;
    [SerializeField]
    private Text scrollNameText;
    [SerializeField]
    private Text scrollDescText;
    [SerializeField]
    private UserInfoUI userInfoUI;

    // Update is called once per frame
    void Update()
    {
        CheckItem();
        TryAction();
    }

    private void TryAction()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            CheckItem();
            CanPickUp();
        }
    }

    private void CanPickUp()
    {
        if(pickupActivated)
        {
            if(hitInfo.transform != null)
            {
                if (hitInfo.transform.tag == "Scroll")
                {
                    Debug.Log("ȹ��");
                    userInfoUI.AcquireScroll(hitInfo.transform.GetComponent<Scroll>().scrollData);
                    hitInfo.transform.gameObject.SetActive(false);
                    InfoDisAppear();
                }
                if (hitInfo.transform.tag == "Shop")
                {
                    return;
                }
            }
        }
    }

    //������ üũ
    private void CheckItem()
    {
        if (Physics.Raycast(transform.position, transform.forward, out hitInfo, range, layerMask))
        {
            if (hitInfo.transform.tag == "Scroll")
            {
                Scroll scroll = hitInfo.transform.GetComponent<Scroll>();
                scrollNameText.text = scroll.scrollData.scrollName;
                scrollDescText.text = string.Format(scroll.scrollData.scrollDesc, scroll.scrollData.baseDamage * 10);
                ItemInfoAppear();
            }
            if (hitInfo.transform.tag == "Shop")
            {
                Scroll scroll = hitInfo.transform.GetComponent<Scroll>();
                ShopAppear();
            }
        }
        else
        {
            InfoDisAppear();
        }
    }

    //��ũ�� �ؽ�Ʈ,UI Ȱ��ȭ
    private void ItemInfoAppear()
    {
        pickupActivated = true;
        scrollRect.localScale = Vector3.one;
        actionText.gameObject.SetActive(true);
        actionText.text = "ȹ���ϱ� <color=red> (F) </color>";
    }
    
    //���� �ؽ�Ʈ,UI Ȱ��ȭ
    private void ShopAppear()
    {
        pickupActivated = true;
        scrollRect.localScale = Vector3.one;
        actionText.gameObject.SetActive(true);
        actionText.text = "���� �̿��ϱ� <color=red> (F) </color>";
    }

    //�ؽ�Ʈ,UI ��Ȱ��ȭ
    private void InfoDisAppear()
    {
        pickupActivated = false;
        scrollRect.localScale = Vector3.zero;
        actionText.gameObject.SetActive(false);
    }

}
