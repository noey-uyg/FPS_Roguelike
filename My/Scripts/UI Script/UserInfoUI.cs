using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserInfoUI : MonoBehaviour
{
    [SerializeField]
    private RectTransform rect;
    [SerializeField]
    private GameObject userInfoRect;
    [SerializeField]
    private GameObject scrollInfoRect;
    [SerializeField]
    private GameObject awakeInfoRect;
    [SerializeField]
    private GameObject nextBtn;
    [SerializeField]
    private GameObject prevBtn;
    
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
        if (GameManager.Instance.isOpenTab)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Hide();
            }
        }
    }

    void InfoTextUpdate()
    {
        levelText.text = string.Format("Lv." + GameManager.Instance.playerData.playerLevel);
        userInfoTexts[0].text = GameManager.Instance.playerData.playerMaxHP.ToString();
        userInfoTexts[1].text = GameManager.Instance.playerData.playerCurDamage.ToString();
        userInfoTexts[2].text = (GameManager.Instance.playerData.playerCriticalPer + (GameManager.Instance.extraCriticalPer * 100)).ToString();
        userInfoTexts[3].text = (GameManager.Instance.playerData.playerCriticalDam + GameManager.Instance.extraCriticalDamage).ToString();
        userInfoTexts[4].text = (GameManager.Instance.playerData.playerWalkSpeed + (GameManager.Instance.playerData.playerWalkSpeed * GameManager.Instance.extraSpeed)).ToString();
        userInfoTexts[5].text = (GameManager.Instance.playerData.playerRunSpeed + (GameManager.Instance.playerData.playerRunSpeed * GameManager.Instance.extraSpeed)).ToString();
        userInfoTexts[6].text = (GameManager.Instance.playerData.playerCrouchSpeed + (GameManager.Instance.playerData.playerCrouchSpeed * GameManager.Instance.extraSpeed)).ToString();
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

    public void NextPage()
    {
        SoundManager.instance.PlaySE("Button_Click");
        userInfoRect.SetActive(false);
        scrollInfoRect.SetActive(false);
        nextBtn.SetActive(false);

        awakeInfoRect.SetActive(true);
        prevBtn.SetActive(true);
    }

    public void PrevPage()
    {
        SoundManager.instance.PlaySE("Button_Click");
        userInfoRect.SetActive(true);
        scrollInfoRect.SetActive(true);
        nextBtn.SetActive(true);

        awakeInfoRect.SetActive(false);
        prevBtn.SetActive(false);
    }
}
