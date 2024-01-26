using System.Collections;
using System.Collections.Generic;
using System.Threading;
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
    public Image myImage;

    [SerializeField]
    private UserInfoUI userInfoUI;

    [SerializeField]
    private RectTransform descRect;
    private Text[] texts;
    private Text shopNameText;
    private Text shopDescText;
    private bool isMouseOverSlot = false;

    public int count;

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

        if (count <= 0)
        {
            myButton.interactable = false;
            SetAlpha(0.5f);
        }
        else
        {
            myButton.interactable = true;
            SetAlpha(1f);
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
                count--;

                break;
            case ShopType.Scroll:
                userInfoUI.AcquireScroll(gameObject.GetComponent<Scroll>().scrollData);
                gameObject.GetComponent<Scroll>().scrollData.haveScroll = true;
                gameObject.GetComponent<Scroll>().DeleteHaveScroll();
                count--;

                break;
            case ShopType.Awake:
                gameObject.GetComponent<RandomAwakening>().AwakeUpgrade();
                count--;

                break;
            case ShopType.Upgrade:
                GameManager.Instance.extraDamage += 0.1f;
                count--;

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
                nameText = "����";
                descText = "������ ������ �ϳ� ��� �˴ϴ�.";
                break;
            case ShopType.Upgrade:
                nameText = "���� ��ȭ";
                descText = "������ �������� 10% �����մϴ�.";
                break;
        }

        shopNameText.text = nameText;
        shopDescText.text = descText;

        if (isMouseOverSlot || myButton.interactable)
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

    public void SetAlpha(float alphaValue)
    {
        Color newColor = myImage.color;
        newColor.a = alphaValue;
        myImage.color = newColor;
    }
}
