using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    //JSON으로 변환할 것.
    [Header("Player Data")]
    public float playerMaxHP=120;
    public float playerCurHP=0;
    public float playerNextEXP = 0;
    public float playerCurEXP = 0;
    public float playerGold = 0;
    public float playerCrystal = 0;
    public float playerMaxCrystal = 25000;
    public int playerLevel = 1;
    public float playerCriticalPer = 5;
    public float playerCriticalDam = 1.5f;
    public float playerDamage;
    public float playerWalkSpeed = 8;
    public float playerRunSpeed = 20;
    public float playerCrouchSpeed = 3;
    public float playerJumpForce = 7;
    public float[] playerExp = {12,19,28,42,56,63,70,81,93,109,121,136,155,166,189,190,200,210,213,217,220,228,
        231,233,235,236,245,251,274,288,297,324,356,378,403,456,484,499,518,553,9999};

    public bool canPlayerMove = true;
    public bool isOpenTab = false;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {        
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        playerNextEXP = playerExp[0];
        playerCurHP = playerMaxHP;
    }

    private void Update()
    {
        if(isOpenTab)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            canPlayerMove = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            canPlayerMove = true;
        }
    }

    public void GetEXP()
    {
        playerCurEXP += 1;

        if(playerCurEXP >= playerNextEXP)
        {
            LevelUP();
        }
    }

    public void LevelUP()
    {
        playerCurEXP = 0;
        playerNextEXP = playerExp[playerLevel];
        playerLevel += 1;
    }

    public void GetHP()
    {
        playerCurHP += playerMaxHP * 0.3f;

        if (playerCurHP > playerMaxHP)
        {
            playerCurHP = playerMaxHP;
        }
    }

    public void GetGold()
    {
        playerGold += 1;
    }

    public void GetCrystal()
    {
        if (playerCrystal >= playerMaxCrystal) return;
        playerCrystal += 1;
    }
}
