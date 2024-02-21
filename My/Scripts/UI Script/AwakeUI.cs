using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AwakeUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    private RectTransform rect;

    public AwakeningData data;

    private Text textName;
    private Text textType;
    private Text textDesc;
    private Text textLevel;

    private bool isMouseOverSlot = false;

    private void Start()
    {
        Text[] text = rect.GetComponentsInChildren<Text>();

        textName = text[0];
        textType = text[1];
        textDesc = text[2];
        textLevel = text[3];
    }

    private void Update()
    {
        if (isMouseOverSlot)
        {
            UpdateDescPosition();
        }
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
        textName.text = data.awakeningName.ToString();
        textType.text = string.Format(data.awakeningType.ToString());
        textDesc.text = string.Format(data.awakeningDesc, data.damage[Mathf.Min(data.damage.Length, data.level)] * 100);
        textLevel.text = string.Format("Lv." + data.level).ToString();

        if (isMouseOverSlot)
        {
            Vector3 mousePosition = Input.mousePosition;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(rect.parent as RectTransform, mousePosition, null, out Vector2 localMousePosition);

            if (mousePosition.x > Screen.width / 2)
            {
                localMousePosition.x -= rect.rect.width;
            }

            if (mousePosition.y < Screen.height / 2)
            {
                localMousePosition.y += rect.rect.height;
            }

            rect.localPosition = localMousePosition;
        }
        rect.localScale = isMouseOverSlot ? Vector3.one : Vector3.zero;
    }
}
