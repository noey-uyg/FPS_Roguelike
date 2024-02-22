using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    //JSON으로 변환할 것.
    [Header("Player Data")]
    public float playerMaxHP = 120;
    public float playerCurHP = 0;
    public float playerNextEXP = 0;
    public float playerCurEXP = 0;
    public float playerGold=0;
    public float playerCrystal;
    public float playerMaxCrystal = 25000;
    public int playerLevel = 1;
    public float playerCriticalPer = 5;
    public float playerCriticalDam = 1.5f;
    public float playerCurDamage;
    public float playerWalkSpeed = 8;
    public float playerRunSpeed = 20;
    public float playerCrouchSpeed = 3;
    public float playerJumpForce = 7;
    public int playerResur = 0;
    public float[] playerExp = {12,19,28,42,56,63,70,81,93,109,121,136,155,166,189,190,200,210,213,217,220,228,
        231,233,235,236,245,251,274,288,297,324,356,378,403,456,484,499,518,553,9999};

    [Header("Setting")]
    public float mouseSensitivity;
    public float soundEffectVolume;
    public float soundBgmVolume;

    [Header("Traits")]
    public int traitsCoin = 0;
    public bool traitsShopUseAwake = false;
    public float traitsDoubleCoinPer = 0;
    public float traitsDiscountShop = 0;
    public bool traitsScrollPer = false;
    public float traitsExtraDam = 0;
    public float traitsCriPer = 0;
    public float traitsGunDam = 0;
    public float traitsAxeSpeed = 0;
    public float traitsHandRange = 0;
    public float traitsAddDam = 0;
    public int traitsMaxHP = 0;
    public int traitsResur = 0;
    public float traitsReduceDam = 0;
    public bool traitsShopRefrsh = false;

    [Header("AxeAwakening")]
    public float axeExtraDamage = 0;
    public bool axeBleeding = false;
    public float axeBleedingDamage = 0;
    public bool axeFear = false;
    public float axeFearDamage = 0;

    [Header("GunAwakening")]
    public bool gunLightning = false;
    public float gunLightningCount = 0;
    public bool gunIsSpeed = false;
    public float gunSpeed = 0;
    public float gunExtraDamage = 0;

    [Header("HandAwakening")]
    public bool handWave = false;
    public float handWaveDamage = 0;
    public bool handIsStack = false;
    public float handStackDamage = 0;
    public float handExtraDamage = 0;

    [Header("CommonAwakening")]
    public float extraRange = 0;
    public float extraDamage = 0;
    public float extraSpeed = 0;
    public float extraCriticalPer = 0;
    public float extraCriticalDamage = 0;
    public float extraFinalDamage = 0;
    public float extraHP = 0;

    [Header("GameElements")]
    public bool mainScene = true;
    public bool gameIsStart = false;
    public float maxGameStartPushTime = 1f;
    public float curGameStartPushTime = 0f;
    public AwakeningData[] awakeDatas;

    [Header("Wave")]
    public float difficultyLevel = 0;
    public int[] waveMaxKill = { 1000, 3000, 5000, 1 };
    public int enemyKilledNum = 0;
    public int allEnemyKill = 0;
    public int maxEnemyKilledNum = 0;
    public int eliteEnemyKilledNum = 0;
    public int eliteSpawnCount = 3;
    public int wave = 0;
    public bool isEliteWave = false;

    [Header("Puzzle")]
    public bool puzzleDam = false;
    public bool puzzleHP = false;
    public bool puzzleSpeed = false;
    public bool puzzleLevelDam = false;
    public bool puzzleLevelHP = false;
    public bool puzzleLevelSpeed = false;
    public bool puzzleHandDam = false;
    public bool puzzleGunDam = false;
    public bool puzzleAxeDam = false;
    public bool puzzleScrollDam = false;
    public bool puzzleCriEnemyDam = false;
    public bool puzzleResur = false;
    public bool puzzleResurDam = false;
    public bool puzzleKillNearby = false;
    public bool puzzleCriNearby = false;

    [Header("ETC")]
    public bool canPlayerMove = true;
    public bool isOpenTab = false;
    public bool scrollisInitialized = false;
    public int scrollCount = 0;
    public bool isCritical = false;
    public bool criCoroutine = false;

    [Header("UI")]
    public LevelUPUI levelUpUi;

    public static event System.Action OnWaveStart;
    public static event System.Action OnBossWave;
    public static event System.Action OnEliteWave;
    public Gun currentGun;
    public CloseWeapon weaponAxe;
    public CloseWeapon weaponHand;

    private void Awake()
    {
        Instance = this;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        InitAwake();
    }

    private void Start()
    {
        PlayerInit();
    }

    private void Update()
    {
        PuzzleKillAblity();
        if (isOpenTab || mainScene)
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
        GameStart();
        EliteTime();
        EneWave();
    }

    //각성 초기화
    public void InitAwake()
    {
        for (int i = 0; i < awakeDatas.Length; i++)
        {
            awakeDatas[i].level = 0;
        }
    }

    //플레이어 초기화
    public void PlayerInit()
    {
        //플레이어 기본 초기화
        playerMaxHP += playerMaxHP * extraHP;
        playerNextEXP = playerExp[0];
        playerCurHP = playerMaxHP;

        //특성 수치 적용
        playerMaxHP += traitsMaxHP;
        playerGold += traitsCoin;
        playerResur += traitsResur;
        extraDamage += traitsExtraDam;
        extraCriticalPer += traitsCriPer;
        gunExtraDamage += traitsGunDam;
        weaponAxe.attackDelay -= weaponAxe.attackDelay * traitsAxeSpeed;
        weaponHand.range += weaponHand.range * traitsHandRange;
    }

    //웨이브시작하기
    public void GameStart()
    {
        if (!gameIsStart)
        {
            if (Input.GetKey(KeyCode.F))
            {
                curGameStartPushTime += Time.deltaTime;

                if (curGameStartPushTime >= maxGameStartPushTime)
                {
                    gameIsStart = true;
                    curGameStartPushTime = 0f;
                    maxEnemyKilledNum = waveMaxKill[wave];
                    wave++;

                    if (wave == 4)
                    {
                        OnBossWave?.Invoke();
                        return;
                    }
                    OnWaveStart?.Invoke();
                }
            }
            if (Input.GetKeyUp(KeyCode.F))
            {
                curGameStartPushTime = 0f;
            }
        }
    }

    public void EliteTime()
    {
        if (!gameIsStart) return;

        if (enemyKilledNum >= maxEnemyKilledNum)
        {
            isEliteWave = true;
            enemyKilledNum = 0;
            OnEliteWave?.Invoke();
        }
    }

    //웨이브 끝내기
    public void EneWave()
    {
        if (eliteEnemyKilledNum >= eliteSpawnCount)
        {
            gameIsStart = false;
            isEliteWave = false;
            eliteEnemyKilledNum = 0;
        }
    }

    //보스,강화,엘리트 몹 추가 데미지
    public float REBAddAttack(Enemy enemy, float damage)
    {
        if (enemy.isBoss || enemy.isReinforced || enemy.isElite)
        {
            return damage * traitsAddDam;
        }
        return 0;
    }

    //장착된 퍼즐

    public void puzzleCriExtraDam()
    {
        if (isCritical && puzzleCriEnemyDam)
        {
            if (!criCoroutine)
            {
                StartCoroutine(CriticalExtraDam());
            }

        }
    }

    IEnumerator CriticalExtraDam()
    {
        criCoroutine = true;
        extraDamage += 0.25f;

        yield return new WaitForSeconds(5f);

        extraDamage -= 0.25f;
        isCritical = false;
        criCoroutine = false;
    }

    public void PuzzleScrollDam()
    {
        if (puzzleScrollDam)
        {
            extraDamage = scrollCount * 0.02f;
        }
    }

    public void PuzzleKillAblity()
    {
        if(allEnemyKill == 50)
        {
            Kill50Dam();
            Kill50HP();
            Kill50Speed();
            allEnemyKill = 0;
        }
    }

    public void Kill50Dam()
    {
        if (puzzleDam)
        {
            extraDamage += (allEnemyKill / 50) * 0.01f;
        }
    }

    public void Kill50HP()
    {
        if (puzzleHP)
        {
            extraHP += (allEnemyKill / 50) * 0.01f;
            playerMaxHP += playerMaxHP * extraHP;
        }
    }

    public void Kill50Speed()
    {
        if (puzzleSpeed)
        {
            extraSpeed += ((allEnemyKill / 50) * 0.001f);
        }
    }

    public void LevelUPDam()
    {
        if (puzzleLevelDam)
        {
            extraDamage += 0.015f;
        }
    }

    public void LevelUPHP()
    {
        if (puzzleLevelHP)
        {
            playerCurHP = playerMaxHP;
        }
    }

    public void LevelUPSpeed()
    {
        if (puzzleLevelSpeed)
        {
            extraSpeed += 0.002f;
        }
    }

    public void PuzzleLevelUPAblity()
    {
        LevelUPDam();
        LevelUPHP();
        LevelUPSpeed();
    }

    public void LevelUP()
    {
        playerCurEXP = 0;
        playerNextEXP = playerExp[Mathf.Min(playerLevel, playerExp.Length)];
        playerLevel += 1;
        PuzzleLevelUPAblity();
        levelUpUi.Show();
    }

    public void GetEXP()
    {
        playerCurEXP += 1;

        if (playerCurEXP >= playerNextEXP)
        {
            LevelUP();
        }
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
        int coinRanNum = Random.Range(0, 100);

        if(coinRanNum < traitsDoubleCoinPer)
        {
            playerGold += 2;
        }

        playerGold += 1;
    }

    public void GetCrystal()
    {
        if (playerCrystal >= playerMaxCrystal) return;
        playerCrystal += 1;
    }

    public void GetBullet()
    {
        currentGun.carryBulletCount += 30;

        if(currentGun.carryBulletCount >= currentGun.maxBulletCount)
        {
            currentGun.carryBulletCount = currentGun.maxBulletCount;
        }
    }
}
