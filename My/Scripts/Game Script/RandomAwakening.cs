using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomAwakening : MonoBehaviour
{
    public AwakeningData[] datas;
    public AwakeningData data;

    private void OnEnable()
    {
        while (true)
        {
            int ranNum = Random.Range(0, datas.Length);

            if (datas[ranNum].level < datas[ranNum].damage.Length)
            {
                data = datas[ranNum];
                break;
            }
        }
    }

    public void AwakeUpgrade()
    {
        Debug.Log(data.awakeningName + "°­È­");
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
                GameManager.Instance.playerData.playerMaxHP += (GameManager.Instance.playerData.playerMaxHP * GameManager.Instance.extraHP);
                GameManager.Instance.playerData.playerCurHP = GameManager.Instance.playerData.playerMaxHP;
                data.level++;
                break;
        }
    }
}

