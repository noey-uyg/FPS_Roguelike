using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Collectable : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Vector3 posToGo;
    public PuzzleItemData item;

    [SerializeField]
    private RectTransform descRect;
    private Text[] texts;
    private Text puzzleNameText;
    private Text puzzleDescText;
    private Text UsableText;
    private Image myimage;

    private bool isMouseOverSlot = false;

    private void Start()
    {
        item.isEquip = false;

        myimage = GetComponent<Image>();
        myimage.sprite = item.puzzleIcon;
        texts = descRect.GetComponentsInChildren<Text>();

        puzzleNameText = texts[0];
        puzzleDescText = texts[1];
        UsableText = texts[2];
    }

    private void Update()
    {
        if (isMouseOverSlot)
        {
            UpdateDescPosition();
        }
        EquipPuzzle();
    }

    public void ClickBtn()
    {
        if (item.isEquip || !item.isHave) return;
        item.isEquip = TetrisSlot.instanceSlot.addInFirstSpace(item); //add to the bag matrix.
    }

    public void EquipPuzzle()
    {
        switch (item.puzzleID)
        {
            case 0:
                GameManager.Instance.puzzleDam = item.isEquip;
                break;
            case 1:
                GameManager.Instance.puzzleHP = item.isEquip;
                break;
            case 2:
                GameManager.Instance.puzzleSpeed = item.isEquip;
                break;
            case 3:
                GameManager.Instance.puzzleLevelDam = item.isEquip;
                break;
            case 4:
                GameManager.Instance.puzzleLevelHP = item.isEquip;
                break;
            case 5:
                GameManager.Instance.puzzleLevelSpeed = item.isEquip;
                break;
            case 6:
                break;
            case 7:
                break;
            case 8:
                break;
            case 9:
                break;
            case 10:
                break;
            case 11:
                break;
            case 12:
                break;
            case 13:
                break;
            case 14:
                break;
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
        puzzleNameText.text = item.puzzleName;
        puzzleDescText.text = item.puzzleDesc;
        UsableText.text = item.isEquip ? "Âø¿ëÁß" : "";

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
