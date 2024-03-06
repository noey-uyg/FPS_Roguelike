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
    private Text traitsLevel;
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
        traitsLevel = texts[2];
    }

    private void Update()
    {
        if (isMouseOverSlot)
        {
            UpdateDescPosition();
        }
        SkillUnlock();
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
        traitsDescText.text = string.Format(traitsdata.traitsDesc, traitsdata.damage[Mathf.Min(traitsdata.damage.Length-1, traitsdata.level+1)]*100);
        traitsLevel.text = traitsdata.level.ToString() + " / " + (traitsdata.damage.Length - 1).ToString();
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

    //특성 개방
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
    
    //특성 개방 체크
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

    //특성 강화
    public void TraitsUpgrade()
    {
        if (traitsdata.level >= traitsdata.damage.Length - 1) return;

        if(traitsdata.cost <= GameManager.Instance.playerData.playerCrystal)
        {
            GameManager.Instance.playerData.playerCrystal -= traitsdata.cost;
            TraitsUpgradeApply();
            traitsdata.level++;
            Debug.Log("강화완료");

            GameManager.Instance.SavePlayerTraitsDataToJson();
        }
        else
        {
            Debug.Log("강화실패");
        }

        SkillUnlock();
    }

    public void TraitsUpgradeApply()
    {
        switch (traitsdata.traitsID)
        {
            case 0:
                GameManager.Instance.playerTraitsData.traitsCoin = (int)(traitsdata.damage[traitsdata.level]*100);
                break;
            case 1:
                GameManager.Instance.playerTraitsData.traitsShopUseAwake = true;
                break;
            case 2:
                GameManager.Instance.playerTraitsData.traitsDoubleCoinPer = traitsdata.damage[traitsdata.level]*100;
                break;
            case 3:
                GameManager.Instance.playerTraitsData.traitsDiscountShop = traitsdata.damage[traitsdata.level];
                break;
            case 4:
                GameManager.Instance.playerTraitsData.traitsScrollPer = true;
                break;
            case 5:
                GameManager.Instance.playerTraitsData.traitsExtraDam = traitsdata.damage[traitsdata.level];
                break;
            case 6:
                GameManager.Instance.playerTraitsData.traitsCriPer = traitsdata.damage[traitsdata.level];
                break;
            case 7:
                GameManager.Instance.playerTraitsData.traitsGunDam = traitsdata.damage[traitsdata.level];
                break;
            case 8:
                GameManager.Instance.playerTraitsData.traitsAxeSpeed = traitsdata.damage[traitsdata.level];
                break;
            case 9:
                GameManager.Instance.playerTraitsData.traitsHandRange = traitsdata.damage[traitsdata.level];
                break;
            case 10:
                GameManager.Instance.playerTraitsData.traitsAddDam = traitsdata.damage[traitsdata.level];
                break;
            case 11:
                GameManager.Instance.playerTraitsData.traitsMaxHP = (int)(traitsdata.damage[traitsdata.level] * 100);
                break;
            case 12:
                GameManager.Instance.playerTraitsData.traitsResur += 1;
                break;
            case 13:
                GameManager.Instance.playerTraitsData.traitsReduceDam = traitsdata.damage[traitsdata.level];
                break;
            case 14:
                GameManager.Instance.playerTraitsData.traitsShopRefrsh = true;
                break;
        }
        
    }
}
