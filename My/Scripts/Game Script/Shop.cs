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

    // ��ũ�ѿ� ���콺�� ������ ���
    public void OnPointerEnter(PointerEventData eventData)
    {
        isMouseOverSlot = true;
        Debug.Log("���콺��" + shopType + "�� �ֽ��ϴ�.");
        UpdateDescPosition();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isMouseOverSlot = false;
        Debug.Log("���콺��" + shopType + "�� ������ϴ�.");
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
                nameText = "ȸ�� ������";
                descText = "�ִ� ü���� 30% ȸ��";
                break;
            case ShopType.Scroll:
                nameText = "��ũ��";
                descText = "������ ��ũ���� �ϳ� ��� �˴ϴ�.";
                break;
            case ShopType.Awake:
                nameText = "���� ���ΰ�ħ";
                descText = "������ ������ �ϳ� ��� �˴ϴ�.";
                break;
            case ShopType.Upgrade:
                nameText = "���� ��ȭ";
                descText = "������ �������� 10% �����մϴ�.";
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
