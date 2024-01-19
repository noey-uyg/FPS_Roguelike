using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusController : MonoBehaviour
{
    [SerializeField]
    private Image[] image_Gauge;

    private const int HP = 0, EXP = 1;

    // Update is called once per frame
    void LateUpdate()
    {
        GaugeUpdate();
    }

    private void GaugeUpdate()
    {
        image_Gauge[HP].fillAmount = HPGauge();
        image_Gauge[EXP].fillAmount = EXPGauge();
    }

    private float HPGauge()
    {
        float maxHP = GameManager.Instance.playerMaxHP;
        float currentHP = GameManager.Instance.playerCurHP;

        return currentHP / maxHP;
    }

    private float EXPGauge()
    {
        float maxEXP = GameManager.Instance.playerNextEXP;
        float currentEXP = GameManager.Instance.playerCurEXP;

        return currentEXP / maxEXP;
    }
}
