using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Slot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public ScrollData scroll;

    [SerializeField]
    private RectTransform descRect;
    private Text[] texts;
    private Text scrollNameText;
    private Text scrollDescText;
    private bool isMouseOverSlot = false;

    private void Start()
    {
        texts = descRect.GetComponentsInChildren<Text>();
        scrollNameText = texts[0];
        scrollDescText = texts[1];
    }

    private void Update()
    {
        if (isMouseOverSlot)
        {
            UpdateDescPosition();
        }
    }

    public void AddScroll(ScrollData addScroll)
    {
        scroll = addScroll;
        scroll.haveScroll = true;
        ApplyScroll(scroll);
    }

    public void ApplyScroll(ScrollData scroll)
    {
        switch (scroll.scrollID)
        {
            case 0:
                GameManager.Instance.scrollNoCriDam = true;
                break;
            case 1:
                GameManager.Instance.isScrollMaxHpDam = true;
                break;
            case 2:
                GameManager.Instance.isReduceDamHPHeal = true;
                break;
            case 3:
                GameManager.Instance.isSurpriseAttack = true;
                break;
            case 4:
                GameManager.Instance.isAdrenaline = true;
                break;
            case 5:
                GameManager.Instance.playerData.playerResur += 1;
                break;
            case 6:
                GameManager.Instance.isJudge = true;
                break;
            case 7:
                GameManager.Instance.isBloodCurse = true;
                GameManager.Instance.playerData.playerMaxHP += GameManager.Instance.playerData.playerMaxHP * 0.5f;
                GameManager.Instance.playerData.playerCurHP = GameManager.Instance.playerData.playerMaxHP;
                break;
            case 8:
                GameManager.Instance.isEliteKiller = true;
                break;
            case 9:
                GameManager.Instance.isGrabber = true;
                GameManager.Instance.grabberCount = 3;
                break;
            case 10:
                GameManager.Instance.isLifeCurse = true;
                GameManager.Instance.playerData.playerMaxHP -= GameManager.Instance.playerData.playerMaxHP * 0.5f;
                GameManager.Instance.extraDamage += 0.5f;
                break;
            case 11:
                GameManager.Instance.isDestroyer = true;
                break;
            case 12:
                break;
        }
    }

    public void ClearSlot()
    {
        scroll = null;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isMouseOverSlot = true;
        UpdateDescPosition();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isMouseOverSlot = false;
        UpdateDescPosition();
    }

    private void UpdateDescPosition()
    {
        scrollNameText.text = scroll.scrollName;
        scrollDescText.text = string.Format(scroll.scrollDesc, scroll.baseDamage * 100);
        if (isMouseOverSlot)
        {
            Vector3 mousePosition = Input.mousePosition;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(descRect.parent as RectTransform, mousePosition, null, out Vector2 localMousePosition);

            if (mousePosition.x > Screen.width / 2)
            {
                localMousePosition.x -= descRect.rect.width;
            }

            if (mousePosition.y < Screen.height / 2)
            {
                localMousePosition.y += descRect.rect.height;
            }

            descRect.localPosition = localMousePosition;
        }
        descRect.localScale = isMouseOverSlot ? Vector3.one : Vector3.zero;
    }
}

