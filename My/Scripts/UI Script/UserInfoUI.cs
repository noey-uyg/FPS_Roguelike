using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserInfoUI : MonoBehaviour
{
    [SerializeField]
    private Text levelText;
    [SerializeField]
    private Text[] texts;

    [SerializeField]
    private RectTransform rect;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            InfoTextUpdate();
            Show();
        }
        else if(Input.GetKeyUp(KeyCode.Tab))
        {
            Hide();
        }
    }

    void InfoTextUpdate()
    {
        levelText.text = string.Format("Lv." + GameManager.Instance.playerLevel);
        texts[0].text = GameManager.Instance.playerMaxHP.ToString();
        texts[1].text = GameManager.Instance.playerDamage.ToString();
        texts[2].text = GameManager.Instance.playerCriticalPer.ToString();
        texts[3].text = GameManager.Instance.playerCriticalDam.ToString();
        texts[4].text = GameManager.Instance.playerWalkSpeed.ToString();
        texts[5].text = GameManager.Instance.playerRunSpeed.ToString();
        texts[6].text = GameManager.Instance.playerCrouchSpeed.ToString();

    }

    void Show()
    {
        rect.localScale = Vector3.one;
    }

    void Hide()
    {
        rect.localScale = Vector3.zero;
    }
}
