using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

public class Awakening : MonoBehaviour
{
    public AwakeningData data;

    Text textName;
    Text textType;
    Text textDesc;
    Text textLevel;

    private void Start()
    {
        Text[] texts = GetComponentsInChildren<Text>();
        data.level = 0;
        textName = texts[0];
        textType = texts[1];
        textDesc = texts[2];
        textLevel = texts[3];

        textName.text = data.awakeningName.ToString();
    }

    private void LateUpdate()
    {
        textType.text = string.Format(data.awakeningType.ToString());
        textDesc.text = string.Format(data.awakeningDesc, data.damage[data.level] * 100);
        textLevel.text = string.Format("Lv." + data.level).ToString();
    }

    public void OnClick()
    {
        switch (data.awakeningID)
        {
            case 0:
                if (data.level == 0) GameManager.Instance.axeBleeding = true;
                GameManager.Instance.axeBleedingDamage = data.damage[data.level];
                data.level++;
                break;
            case 1:
                if (data.level == 0) GameManager.Instance.axeFear = true;
                GameManager.Instance.axeFearDamage = data.damage[data.level];
                data.level++;
                break;
            case 2:
                GameManager.Instance.axeExtraDamage = data.damage[data.level];
                data.level++;
                break;
            case 3:
                if (data.level == 0) GameManager.Instance.gunLightning = true;
                GameManager.Instance.gunLightningCount = data.damage[data.level];
                data.level++;
                break;
            case 4:
                if (data.level == 0) GameManager.Instance.gunIsSpeed = true;
                GameManager.Instance.gunSpeed = data.damage[data.level];
                data.level++;
                break;
            case 5:
                GameManager.Instance.gunExtraDamage = data.damage[data.level];
                data.level++;
                break;
            case 6:
                if (data.level == 0) GameManager.Instance.handWave = true;
                GameManager.Instance.handWaveDamage = data.damage[data.level];
                data.level++;
                break;
            case 7:
                GameManager.Instance.handStackDamage = data.damage[data.level];
                data.level++;
                break;
            case 8:
                if (data.level == 0) GameManager.Instance.handIsStack = true;
                GameManager.Instance.handExtraDamage = data.damage[data.level];
                data.level++;
                break;
            case 9:
                GameManager.Instance.extraRange = data.damage[data.level];
                data.level++;
                break;
            case 10:
                GameManager.Instance.extraSpeed = data.damage[data.level];
                data.level++;
                break;
            case 11:
                GameManager.Instance.extraCriticalPer = data.damage[data.level];
                data.level++;
                break;
            case 12:
                GameManager.Instance.extraCriticalDamage = data.damage[data.level];
                data.level++;
                break;
            case 13:
                GameManager.Instance.extraFinalDamage = data.damage[data.level];
                data.level++;
                break;
            case 14:
                GameManager.Instance.extraHP = data.damage[data.level];
                GameManager.Instance.playerMaxHP += (GameManager.Instance.playerMaxHP * GameManager.Instance.extraHP);
                GameManager.Instance.playerCurHP = GameManager.Instance.playerMaxHP;
                data.level++;
                break;
        }
    }
}
