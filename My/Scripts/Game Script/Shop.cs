using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Shop : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public enum ShopType { Heal,Scroll,Awake,Upgrade}
    public ShopType shopType;

    string nameText;
    string descText;

    public Button myButton;

    [SerializeField]
    private UserInfoUI userInfoUI;

    [SerializeField]
    private RectTransform descRect;
    private Text[] texts;
    private Text shopNameText;
    private Text shopDescText;
    private bool isMouseOverSlot = false;


    private void Start()
    {
        myButton = GetComponent<Button>();
        texts = descRect.GetComponentsInChildren<Text>();
        shopNameText = texts[0];
        shopDescText = texts[1];
    }

    private void Update()
    {
        if (isMouseOverSlot)
        {
            UpdateDescPosition();
        }
    }

    // 스크롤에 마우스를 가져다 대면
    public void OnPointerEnter(PointerEventData eventData)
    {
        isMouseOverSlot = true;
        Debug.Log("마우스가" + shopType + "에 있습니다.");
        UpdateDescPosition();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isMouseOverSlot = false;
        Debug.Log("마우스가" + shopType + "를 벗어났습니다.");
        UpdateDescPosition();
    }

    public void OnClick()
    {
        switch (shopType)
        {
            case ShopType.Heal:
                GameManager.Instance.GetHP();
                myButton.interactable = false;
                break;
            case ShopType.Scroll:
                userInfoUI.AcquireScroll(gameObject.GetComponent<Scroll>().scrollData);
                gameObject.GetComponent<Scroll>().scrollData.haveScroll = true;
                gameObject.GetComponent<Scroll>().DeleteHaveScroll();
                myButton.interactable = false;
                break;
            case ShopType.Awake:
                AwakeningData data = gameObject.GetComponent<RandomAwakening>().data;
                gameObject.gameObject.GetComponent<Awakening>().data = data;
                gameObject.gameObject.GetComponent<Awakening>().OnClick();
                myButton.interactable = false;
                break;
            case ShopType.Upgrade:
                GameManager.Instance.extraDamage += 0.1f;
                myButton.interactable = false;
                break;
        }
    }

    private void UpdateDescPosition()
    {
        if (!myButton.interactable) return;
        
        switch (shopType)
        {
            case ShopType.Heal:
                nameText = "회복 아이템";
                descText = "최대 체력의 30% 회복";
                break;
            case ShopType.Scroll:
                nameText = "스크롤";
                descText = "랜덤한 스크롤을 하나 얻게 됩니다.";
                break;
            case ShopType.Awake:
                nameText = "각성 새로고침";
                descText = "랜덤한 각성을 하나 얻게 됩니다.";
                break;
            case ShopType.Upgrade:
                nameText = "무기 강화";
                descText = "무기의 데미지가 10% 증가합니다.";
                break;
        }

        shopNameText.text = nameText;
        shopDescText.text = descText;

        if (isMouseOverSlot)
        {
            Vector3 mousePosition = Input.mousePosition;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(descRect.parent as RectTransform, mousePosition, null, out Vector2 localMousePosition);

            descRect.localPosition = localMousePosition;
            descRect.localScale = Vector3.one;
        }
        else
        {
            descRect.localScale = Vector3.zero;
        }
    }
}
