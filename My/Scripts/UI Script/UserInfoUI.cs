using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserInfoUI : MonoBehaviour
{
    [SerializeField]
    private RectTransform rect;

    [Header("UserInfo")]
    [SerializeField]
    private Text levelText;
    [SerializeField]
    private Text[] userInfoTexts;

    [Header("ScrollInfo")]
    [SerializeField]
    private GameObject slotParents;
    [SerializeField]
    private Slot[] slots;

    private void Start()
    {
        slots = slotParents.GetComponentsInChildren<Slot>();
        InitSlot();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (!GameManager.Instance.isOpenTab)
            {
                InfoTextUpdate();
                Show();
            }
            else
            {
                Hide();
            }

        }
    }

    void InfoTextUpdate()
    {
        levelText.text = string.Format("Lv." + GameManager.Instance.playerLevel);
        userInfoTexts[0].text = GameManager.Instance.playerMaxHP.ToString();
        userInfoTexts[1].text = GameManager.Instance.playerCurDamage.ToString();
        userInfoTexts[2].text = GameManager.Instance.playerCriticalPer.ToString();
        userInfoTexts[3].text = GameManager.Instance.playerCriticalDam.ToString();
        userInfoTexts[4].text = GameManager.Instance.playerWalkSpeed.ToString();
        userInfoTexts[5].text = GameManager.Instance.playerRunSpeed.ToString();
        userInfoTexts[6].text = GameManager.Instance.playerCrouchSpeed.ToString();

    }

    void Show()
    {
        GameManager.Instance.isOpenTab = true;
        rect.localScale = Vector3.one;
    }

    void Hide()
    {
        GameManager.Instance.isOpenTab = false;
        rect.localScale = Vector3.zero;
    }

    private void InitSlot()
    {
        for(int i = 0; i < slots.Length; i++)
        {
            slots[i].gameObject.SetActive(false);
        }
    }
    public void AcquireScroll(ScrollData addScroll)
    {
        for(int i = 0; i < slots.Length; i++)
        {
            if(slots[i].scroll == null)
            {
                slots[i].AddScroll(addScroll);
                slots[i].gameObject.SetActive(true);
                return;
            }
        }
    }
}
