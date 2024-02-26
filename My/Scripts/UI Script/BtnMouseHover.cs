using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BtnMouseHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Button myBtn;
    public Sprite hoverSprite;
    public Sprite originalSprite;

    // Start is called before the first frame update
    void Start()
    {
        myBtn = GetComponent<Button>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        myBtn.image.sprite = hoverSprite;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        myBtn.image.sprite = originalSprite;
    }
}
