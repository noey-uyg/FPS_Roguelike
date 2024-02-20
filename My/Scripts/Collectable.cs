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
                GameManager.Instance.puzzleHandDam = item.isEquip;
                GameManager.Instance.handExtraDamage = 0.5f;
                break;
            case 7:
                GameManager.Instance.puzzleGunDam = item.isEquip;
                GameManager.Instance.gunExtraDamage = 0.5f;
                break;
            case 8:
                GameManager.Instance.puzzleAxeDam = item.isEquip;
                GameManager.Instance.axeExtraDamage = 0.5f;
                break;
            case 9:
                GameManager.Instance.puzzleScrollDam = item.isEquip;
                break;
            case 10:
                GameManager.Instance.puzzleCriEnemyDam = item.isEquip;
                break;
            case 11:
                GameManager.Instance.puzzleResur = item.isEquip;
                GameManager.Instance.playerResur += 1;
                break;
            case 12:
                GameManager.Instance.puzzleResurDam = item.isEquip;
                break;
            case 13:
                GameManager.Instance.puzzleKillNearby = item.isEquip;
                break;
            case 14:
                GameManager.Instance.puzzleCriNearby = item.isEquip;
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
