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

    // Update is called once per frame
    void Update()
    {
        CheckBullet();
        CheckGold();
        CheckCrystal();
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
}
