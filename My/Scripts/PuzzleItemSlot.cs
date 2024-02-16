using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PuzzleItemSlot : MonoBehaviour
{
    public Vector2 size = new Vector2(60f, 60f); //slot cell size 
    public PuzzleItemData item;

    public Vector2 startPosition;
    public Vector2 oldPosition;
    public Image icon;

    TetrisSlot slots;

    void Start()
    {
        Rescaling();
    }

    void Rescaling()
    {
        GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, item.puzzleSize.y * size.y);
        GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, item.puzzleSize.x * size.x);

        foreach (RectTransform child in transform)
        {
            child.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, item.puzzleSize.y * child.rect.height);
            child.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, item.puzzleSize.x * child.rect.width);

            foreach (RectTransform iconChild in child)
            {
                iconChild.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, item.puzzleSize.y * iconChild.rect.height);
                iconChild.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, item.puzzleSize.x * iconChild.rect.width);
                iconChild.localPosition = new Vector2(child.localPosition.x + child.rect.width / 2, child.localPosition.y + child.rect.height / 2 * -1f);
            }

        }
    }
    
}
