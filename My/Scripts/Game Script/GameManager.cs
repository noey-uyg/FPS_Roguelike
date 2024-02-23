using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    //JSON���� ��ȯ�� ��.
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

    [Header("Scroll")]
    public bool scrollNoCriDam = false;
    public bool isScrollMaxHpDam = false;
    public bool isReduceDamHPHeal = false;
    public bool isSurpriseAttack = false;
    public bool isAdrenaline = false;
    public bool isJudge = false;
    public bool isBloodCurse = false;
    public bool isEliteKiller = false;
    public bool isGrabber = false;
    public int grabberCount = 0;
    public bool isLifeCurse = false;
    public bool isDestroyer = false;
    public bool isGiveMe = false;
    public int refreshCount = 0;

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

    //���� �ʱ�ȭ
    public void InitAwake()
    {
        for (int i = 0; i < awakeDatas.Length; i++)
        {
            awakeDatas[i].level = 0;
        }
    }

    //�÷��̾� �ʱ�ȭ
    public void PlayerInit()
    {
        //�÷��̾� �⺻ �ʱ�ȭ
        playerMaxHP += playerMaxHP * extraHP;
        playerNextEXP = playerExp[0];
        playerCurHP = playerMaxHP;

        //Ư�� ��ġ ����
        playerMaxHP += traitsMaxHP;
        playerGold += traitsCoin;
        playerResur += traitsResur;
        extraDamage += traitsExtraDam;
        extraCriticalPer += traitsCriPer;
        gunExtraDamage += traitsGunDam;
        weaponAxe.attackDelay -= weaponAxe.attackDelay * traitsAxeSpeed;
        weaponHand.range += weaponHand.range * traitsHandRange;
    }

    //���̺�����ϱ�
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

    //���̺� ������
    public void EneWave()
    {
        if (eliteEnemyKilledNum >= eliteSpawnCount)
        {
            gameIsStart = false;
            isEliteWave = false;
            eliteEnemyKilledNum = 0;
        }
    }

    //��ũ��
    //ġ�������� ���� ����
    public float NoCriDam(float damage)
    {
        if (scrollNoCriDam) return 0;

        return damage * 0.3f;
    }

    //�㼼����
    public float MaxHPDam(float damage)
    {
        if (!isScrollMaxHpDam) return 0;

        return damage * 0.3f;
    }

    //ź�� ��ü
    #region PlayerHeal
    bool isHealing = false;
    public void TakeHeal()
    {
        if (!isReduceDamHPHeal) return;

        if (!isHealing)
        {
            StartCoroutine(StartHealingCoroutine());
        }    
    }

    IEnumerator StartHealingCoroutine()
    {
        isHealing = true;

        float timer = 0f;
        while (timer < 5f)
        {
            Heal();
            yield return new WaitForSeconds(1f);
            timer += 1f;
        }

        isHealing = false;
    }

    void Heal()
    {
        float healingAmount = playerMaxHP * 0.3f;
        playerCurHP = Mathf.Min(playerCurHP + healingAmount, playerMaxHP);
    }
    #endregion

    //��� ����
    public float SurpirseAttack(Enemy enemy, float damage)
    {
        if (!isSurpriseAttack) return 0;

        if(enemy.enemyCurrentHP > enemy.enemyCurrentHP * 0.7f)
        {
            return damage * 0.3f;
        }

        return 0;
    }

    //�Ƶ巹����
    #region Adrenaline
    public bool isIncreased = false;

    public void Adrenaline()
    {
        if (!isAdrenaline) return;

        StartCoroutine(AdreanilneCoroutine());

    }

    IEnumerator AdreanilneCoroutine()
    {
        isIncreased = true;

        yield return new WaitForSeconds(5f);

        isIncreased = false;
    }
    #endregion

    //������
    public void Judge(Enemy enemy)
    {
        if (!isJudge) return;

        int ranNum = Random.Range(0, 100);

        if(ranNum < 5)
        {
            enemy.enemyCurrentHP -= enemy.maxHP * 0.1f;
        }
    }

    //����Ʈ ų��
    public void EliteKiler()
    {
        if (!isEliteKiller) return;

        extraDamage += 0.1f;
    }

    //�ı���
    public void DestroyerScroll()
    {
        if (!isDestroyer) return;

        extraDamage += 0.01f;
    }

    //Ư�� 
    //����,��ȭ,����Ʈ �� �߰� ������
    public float REBAddAttack(Enemy enemy, float damage)
    {
        if (enemy.isBoss || enemy.isReinforced || enemy.isElite)
        {
            return damage * traitsAddDam;
        }
        return 0;
    }

    //������ ����
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
