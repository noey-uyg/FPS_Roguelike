using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PuzzleItemSlot : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerDownHandler
{
    public Vector2 size = new Vector2(60f, 60f); //slot cell size 
    public PuzzleItemData item;

    public Vector2 startPosition;
    public Vector2 oldPosition;
    public Image icon;

    private RectTransform rectTransform;
    TetrisSlot slots;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        slots = TetrisSlot.instanceSlot;
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

    public void OnPointerDown(PointerEventData eventData)
    {
        DestroyPuzzle();

    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        oldPosition = transform.GetComponent<RectTransform>().anchoredPosition;

        GetComponent<CanvasGroup>().blocksRaycasts = false; // disable registering hit on item
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.position = eventData.position;

        //allow the intersection between old pos and new pos.
        for (int i = 0; i < item.puzzleSize.y; i++)
        {
            for (int j = 0; j < item.puzzleSize.x; j++)
            {
                slots.grid[(int)startPosition.x + j, (int)startPosition.y + i] = 0;

            }
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("끝");
        if (EventSystem.current.IsPointerOverGameObject())
        {
            Vector2 finalPos = GetComponent<RectTransform>().anchoredPosition; //position that the item was dropped on canvas

            Vector2 finalSlot;
            finalSlot.x = Mathf.Floor(finalPos.x / size.x); //which x slot it is
            finalSlot.y = Mathf.Floor(-finalPos.y / size.y); //which y slot it is

            if (((int)(finalSlot.x) + (int)(item.puzzleSize.x) - 1) < slots.maxGridX && ((int)(finalSlot.y) + (int)(item.puzzleSize.y) - 1) < slots.maxGridY && ((int)(finalSlot.x)) >= 0 && (int)finalSlot.y >= 0) // test if item is inside slot area
            {
                List<Vector2> newPosItem = new List<Vector2>(); //new item position in bag
                bool fit = false;

                for (int sizeY = 0; sizeY < item.puzzleSize.y; sizeY++)
                {
                    for (int sizeX = 0; sizeX < item.puzzleSize.x; sizeX++)
                    {
                        if (slots.grid[(int)finalSlot.x + sizeX, (int)finalSlot.y + sizeY] != 1)
                        {
                            Vector2 pos;
                            pos.x = (int)finalSlot.x + sizeX;
                            pos.y = (int)finalSlot.y + sizeY;
                            newPosItem.Add(pos);
                            fit = true;
                        }
                        else
                        {
                            fit = false;

                            this.transform.GetComponent<RectTransform>().anchoredPosition = oldPosition; //back to old pos
                            sizeX = (int)item.puzzleSize.x;
                            sizeY = (int)item.puzzleSize.y;
                            newPosItem.Clear();

                        }

                    }

                }
                if (fit)
                { //delete old item position in bag
                    for (int i = 0; i < item.puzzleSize.y; i++) //through item Y
                    {
                        for (int j = 0; j < item.puzzleSize.x; j++) //through item X
                        {
                            slots.grid[(int)startPosition.x + j, (int)startPosition.y + i] = 0; //clean old pos

                        }
                    }

                    for (int i = 0; i < newPosItem.Count; i++)
                    {
                        slots.grid[(int)newPosItem[i].x, (int)newPosItem[i].y] = 1; // add new pos
                    }

                    this.startPosition = newPosItem[0]; // set new start position
                    transform.GetComponent<RectTransform>().anchoredPosition = new Vector2(newPosItem[0].x * size.x, -newPosItem[0].y * size.y);
                }
                else //item voltou pra mesma posição da bag e marca com 1
                {
                    for (int i = 0; i < item.puzzleSize.y; i++) //through item Y
                    {
                        for (int j = 0; j < item.puzzleSize.x; j++) //through item X
                        {
                            slots.grid[(int)startPosition.x + j, (int)startPosition.y + i] = 1; //back to position 1;

                        }
                    }
                }
            }
            else
            { // out of index, back to the old pos
                this.transform.GetComponent<RectTransform>().anchoredPosition = oldPosition;
            }
        }

        GetComponent<CanvasGroup>().blocksRaycasts = true; //register hit on item again
    }

    public void DestroyPuzzle()
    {
        if(Input.GetMouseButtonDown(1))
        {
            for (int i = 0; i < item.puzzleSize.y; i++)
            {
                for (int j = 0; j < item.puzzleSize.x; j++)
                {
                    slots.grid[(int)startPosition.x + j, (int)startPosition.y + i] = 0;                                                              
                }
            }
            item.isEquip = false;
            Destroy(this.gameObject);
        }
    }
}
