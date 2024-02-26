using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    [SerializeField]
    private GunController theGunController;
    private Gun currentGun;

    [SerializeField]
    private GameObject go_BulletHUD;

    [SerializeField]
    private Text[] text_bullet;
    [SerializeField]
    private Text text_Gold;
    [SerializeField]
    private Text text_Crystal;

    [SerializeField]
    private Text text_Start;

    // Update is called once per frame
    void Update()
    {
        CheckBullet();
        CheckGold();
        CheckCrystal();
        PressItStart();
    }

    private void CheckBullet()
    {
        currentGun = theGunController.GetGun();
        text_bullet[0].text = currentGun.carryBulletCount.ToString();
        text_bullet[1].text = currentGun.reloadBulletCount.ToString();
        text_bullet[2].text = currentGun.currentBulletCount.ToString();
    }

    private void CheckGold()
    {
        text_Gold.text = GameManager.Instance.playerGold.ToString();
    }

    private void CheckCrystal()
    {
        text_Crystal.text = GameManager.Instance.playerCrystal.ToString();
    }

    private void PressItStart()
    {
        if (!GameManager.Instance.gameIsStart)
        {
            text_Start.gameObject.SetActive(true);
            text_Start.text = "웨이브 시작하기" + "<color=red> (F) </color>" + GameManager.Instance.curGameStartPushTime.ToString("F1") + "/" + GameManager.Instance.maxGameStartPushTime;
        }
        else
        {
            text_Start.gameObject.SetActive(false);
        }

    }
}
