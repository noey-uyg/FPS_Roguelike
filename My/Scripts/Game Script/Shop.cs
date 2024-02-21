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

    public int cost;
    public Text costText;

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

    GameObject shopObject;
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

    private void LateUpdate()
    {
        costText.text = cost.ToString();
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

    public void OnEnable()
    {
        cost = (cost * GameManager.Instance.playerLevel) / GameManager.Instance.wave;

        if (shopObject == null)
        {
            shopObject = GameObject.FindWithTag("Shop");
        }

        switch (shopType)
        {
            case ShopType.Heal:
                count = shopObject.GetComponent<ShopObject>().healCount;
                break;
            case ShopType.Upgrade:
                count = shopObject.GetComponent<ShopObject>().upgradeCount;
                break;
            case ShopType.Scroll:
                count = shopObject.GetComponent<ShopObject>().scrollCount;
                break;
            case ShopType.Awake:
                count = shopObject.GetComponent<ShopObject>().awakeCount;
                break;
        }
    }

    public void OnClick()
    {
        switch (shopType)
        {
            case ShopType.Heal:
                if(cost <= GameManager.Instance.playerGold)
                {
                    GameManager.Instance.GetHP();
                    count--;
                }
                break;
            case ShopType.Scroll:
                if (cost <= GameManager.Instance.playerGold)
                {
                    userInfoUI.AcquireScroll(gameObject.GetComponent<Scroll>().scrollData);
                    gameObject.GetComponent<Scroll>().scrollData.haveScroll = true;
                    gameObject.GetComponent<Scroll>().DeleteHaveScroll();
                    GameManager.Instance.scrollCount++;
                    GameManager.Instance.PuzzleScrollDam();
                    count--;
                }
                break;
            case ShopType.Awake:
                if (cost <= GameManager.Instance.playerGold)
                {
                    gameObject.GetComponent<RandomAwakening>().AwakeUpgrade();
                    count--;
                }
                break;
            case ShopType.Upgrade:
                if (cost <= GameManager.Instance.playerGold)
                {
                    GameManager.Instance.extraDamage += 0.1f;
                    count--;
                }
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

        if (!myButton.interactable)
        {
            descRect.localScale = Vector3.zero;
        }

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

    public void SetAlpha(float alphaValue)
    {
        Color newColor = myImage.color;
        newColor.a = alphaValue;
        myImage.color = newColor;
    }
}
