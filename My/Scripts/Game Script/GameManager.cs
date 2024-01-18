using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Player")]
    public float playerMaxHP=120;
    public float playerCurHP=0;
    public float playerMaxEXP = 10;
    public float playerCurEXP = 0;
    public float playerGold = 0;
    public float playerCrystal = 0;
    public float playerMaxCrystal = 25000;
    public float playerLevel = 1;
    public float playerCriticalPer = 5;
    public float playerCriticalDam = 1.5f;
    public float playerDamage;
    public float playerWalkSpeed = 8;
    public float playerRunSpeed = 20;
    public float playerCrouchSpeed = 3;
    public float playerJumpForce = 7;


    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        playerCurHP = playerMaxHP;
    }

    public void GetEXP()
    {
        playerCurEXP += 1;

        if(playerCurEXP == playerMaxEXP)
        {
            LevelUP();
        }
    }

    public void LevelUP()
    {
        playerCurEXP = 0;
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
