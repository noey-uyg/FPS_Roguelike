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
    }

    public void ClearSlot()
    {
        scroll = null;
    }

    // 스크롤에 마우스를 가져다 대면
    public void OnPointerEnter(PointerEventData eventData)
    {
        isMouseOverSlot = true;
        Debug.Log("마우스가" + gameObject.name + "에 있습니다.");
        UpdateDescPosition();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isMouseOverSlot = false;
        Debug.Log("마우스가" + gameObject.name + "를 벗어났습니다.");
        UpdateDescPosition();
    }

    private void UpdateDescPosition()
    {
        scrollNameText.text = scroll.scrollName;
        scrollDescText.text = string.Format(scroll.scrollDesc, scroll.baseDamage * 10);
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

