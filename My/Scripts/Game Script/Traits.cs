using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Traits : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public TraitsData traitsdata;

    [SerializeField]
    private RectTransform descRect;
    private Text[] texts;
    private Text traitsNameText;
    private Text traitsDescText;
    private bool isMouseOverSlot = false;

    private Button btn;
    private Image[] traitsImg;
    private Image[] descTraitsImg;

    private void Awake()
    {
        btn = GetComponent<Button>();
        traitsImg = GetComponentsInChildren<Image>();
        descTraitsImg = descRect.GetComponentsInChildren<Image>();

        traitsImg[1].sprite = traitsdata.traitsSprite;
    }

    private void Start()
    {
        texts = descRect.GetComponentsInChildren<Text>();
        traitsNameText = texts[0];
        traitsDescText = texts[1];
    }

    private void Update()
    {
        if (isMouseOverSlot)
        {
            UpdateDescPosition();
        }
    }

    private void OnEnable()
    {
        SkillUnlock();
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
        traitsNameText.text = traitsdata.traitsName;
        traitsDescText.text = traitsdata.traitsDesc;
        descTraitsImg[2].sprite = traitsdata.traitsSprite;

        if (isMouseOverSlot)
        {
            Vector3 mousePosition = Input.mousePosition;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(descRect.parent as RectTransform, mousePosition, null, out Vector2 localMousePosition);

            if (mousePosition.x > Screen.width / 2)
            {
                localMousePosition.x -= descRect.rect.width;
            }

            if(mousePosition.y < Screen.height / 2)
            {
                localMousePosition.y += descRect.rect.height;
            }

            descRect.localPosition = localMousePosition;
        }
        descRect.localScale = isMouseOverSlot ? Vector3.one : Vector3.zero;
    }

    void SkillUnlock()
    {
        if(traitsdata.prevTraits.Length == 0)
        {
            traitsdata.isUnlocked = true;
        }
        else if (AllPrevTraitsUnlock())
        {
            traitsdata.isUnlocked = true;
        }
        else
        {
            traitsdata.isUnlocked = false;
        }

        btn.interactable = traitsdata.isUnlocked;
    }

    bool AllPrevTraitsUnlock()
    {
        foreach (var prevTrait in traitsdata.prevTraits)
        {
            if(!prevTrait.isUnlocked || prevTrait.level < prevTrait.damage.Length - 1)
            {
                return false;
            }
        }

        return true;
    }

    public void TraitsUpgrade()
    {
        if (traitsdata.level >= traitsdata.damage.Length - 1) return;

        if(traitsdata.cost <= GameManager.Instance.playerCrystal)
        {
            GameManager.Instance.playerCrystal -= traitsdata.cost;
            traitsdata.level++;
            Debug.Log("��ȭ�Ϸ�");
        }
        else
        {
            Debug.Log("��ȭ����");
        }

        SkillUnlock();
    }
}
