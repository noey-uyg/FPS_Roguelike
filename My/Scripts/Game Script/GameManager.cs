using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    //JSON으로 변환할 것.
    [Header("Player Data")]
    public PlayerData playerData;

    [Header("Setting")]
    public SettingData settingData;

    [Header("Traits")]
    public PlayerTraitsData playerTraitsData;

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
    public bool isClear = false;

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
    public bool isOpenPause = false;
    public bool scrollisInitialized = false;
    public int scrollCount = 0;
    public bool isCritical = false;
    public bool criCoroutine = false;

    [Header("UI")]
    public LevelUPUI levelUpUi;
    public GameObject PauseUI;
    public GameObject ClearUI;
    Text clearUiLevelText;
    Text clearUiWaveText;
    Text clearText;
    Text deadText;

    public static event System.Action OnWaveStart;
    public static event System.Action OnBossWave;
    public static event System.Action OnEliteWave;
    public Gun currentGun;
    public CloseWeapon weaponAxe;
    public CloseWeapon weaponHand;

    private void Awake()
    {
        LoadPlayerDataToJson();
        LoadPlayerSettingDataToJson();
        LoadPlayerTraitsDataToJson();
        Instance = this;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        InitAwake();
    }

    private void Start()
    {
        ClearUIInit();
        PlayerInit();
    }

    private void Update()
    {
        PuzzleKillAblity();
        PauseESC();
        if (isOpenTab || isOpenPause || mainScene)
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

        if(playerData.playerCurHP <= 0)
        {
            if(playerData.playerResur > 0)
            {
                canPlayerMove = false;
                ResurrectPlayer();
            }
            else
            {
                playerData.isDead = true;
            }
        }

        if (playerData.isDead)
        {
            ClearUIShow();
        }
    }

    //부활
    private float resurrectionTime = 2f; // 부활까지 걸리는 시간
    private float resurrectionTimer = 0f; // 부활까지 남은 시간

    public void ResurrectPlayer()
    {
        resurrectionTimer += Time.deltaTime;

        if(resurrectionTimer > resurrectionTime)
        {
            canPlayerMove = true;
            playerData.playerCurHP = playerData.playerMaxHP;
            playerData.playerResur--;
            resurrectionTimer = 0f;
            PuzzleResurDam();
        }
    }

    //ClearUIInit
    public void ClearUIInit()
    {
        Text[] texts = ClearUI.GetComponentsInChildren<Text>();
        clearUiLevelText = texts[0];
        clearUiWaveText = texts[1];
        deadText = texts[2];
        clearText = texts[3];
    }

    //사망 or 클리어 시 UI
    public void ClearUIShow()
    {
        isOpenTab = true;
        clearUiLevelText.text = string.Format("Level:" + playerData.playerLevel);
        clearUiWaveText.text = string.Format("Wave:" + wave);
        deadText.gameObject.SetActive(playerData.isDead);
        clearText.gameObject.SetActive(isClear);

        ClearUI.SetActive(true);
    }

    public void ClearUIHide()
    {
        isOpenTab = false;
        ClearUI.SetActive(false);
    }

    //Esc키
    public void PauseESC()
    {
        if (mainScene || isOpenTab) return;
        
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!isOpenPause)
            {
                PauseShow();
            }
            else
            {
                PauseHide();
            }
        }
    }

    //PauseShow
    public void PauseShow()
    {
        isOpenPause = true;
        PauseUI.SetActive(true);
    }

    //PauseHide
    public void PauseHide()
    {
        isOpenPause = false;
        PauseUI.SetActive(false);
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
        ClearUIHide();
        InitSystem();

        //플레이어 기본 초기화
        InitPlayerBasic();
        isClear = false;

        //특성 수치 적용
        playerData.playerMaxHP += playerTraitsData.traitsMaxHP;
        playerData.playerGold += playerTraitsData.traitsCoin;
        playerData.playerResur += playerTraitsData.traitsResur;
        extraDamage += playerTraitsData.traitsExtraDam;
        extraCriticalPer += playerTraitsData.traitsCriPer;
        gunExtraDamage += playerTraitsData.traitsGunDam;
        weaponAxe.attackDelay -= weaponAxe.attackDelay * playerTraitsData.traitsAxeSpeed;
        weaponHand.range += weaponHand.range * playerTraitsData.traitsHandRange;
    }

    //플레이어 기본 스탯 초기화
    public void InitPlayerBasic()
    {
        playerData.playerMaxHP = 120;
        playerData.playerCurHP = playerData.playerMaxHP;
        playerData.playerNextEXP = playerData.playerExp[0];
        playerData.playerCurEXP = 0;
        playerData.playerGold = 0;
        playerData.playerLevel = 1;
        playerData.playerCriticalPer = 5;
        playerData.playerCriticalDam = 1.5f;
        playerData.playerWalkSpeed = 8;
        playerData.playerRunSpeed = 13;
        playerData.playerCrouchSpeed = 3;
        playerData.playerJumpForce = 0;
        playerData.playerResur = 0;
        playerData.isDead = false;
}

    public void InitSystem()
    {
        if (!mainScene)
        {
            PauseHide();
            mainScene = true;
            gameIsStart = false;
            wave = 0;
            SoundManager.instance.PlayBGM("BGM_Main");
        }
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
  

    #region Scroll
    //치명적이지 않은 공격
    public float NoCriDam(float damage)
    {
        if (scrollNoCriDam) return 0;

        return damage * 0.3f;
    }

    //허세쟁이
    public float MaxHPDam(float damage)
    {
        if (!isScrollMaxHpDam) return 0;

        return damage * 0.3f;
    }

    //탄력 육체
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
        float healingAmount = playerData.playerMaxHP * 0.3f;
        playerData.playerCurHP = Mathf.Min(playerData.playerCurHP + healingAmount, playerData.playerMaxHP);
    }
    #endregion

    //기습 공격
    public float SurpirseAttack(Enemy enemy, float damage)
    {
        if (!isSurpriseAttack) return 0;

        if(enemy.enemyCurrentHP > enemy.enemyCurrentHP * 0.7f)
        {
            return damage * 0.3f;
        }

        return 0;
    }

    //아드레날린
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

    //심판자
    public void Judge(Enemy enemy)
    {
        if (!isJudge) return;

        int ranNum = Random.Range(0, 100);

        if(ranNum < 5)
        {
            enemy.enemyCurrentHP -= enemy.maxHP * 0.1f;
        }
    }

    //엘리트 킬러
    public void EliteKiler()
    {
        if (!isEliteKiller) return;

        extraDamage += 0.1f;
    }

    //파괴자
    public void DestroyerScroll()
    {
        if (!isDestroyer) return;

        extraDamage += 0.01f;
    }
    #endregion

    #region Traits
    //보스,강화,엘리트 몹 추가 데미지
    public float REBAddAttack(Enemy enemy, float damage)
    {
        if (enemy.isBoss || enemy.isReinforced || enemy.isElite)
        {
            return damage * playerTraitsData.traitsAddDam;
        }
        return 0;
    }
    #endregion

    #region Puzzle
    public void PuzzleCriExtraDam()
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
            playerData.playerMaxHP += playerData.playerMaxHP * extraHP;
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
            playerData.playerCurHP = playerData.playerMaxHP;
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

    //부활시 데미지, 속도, 생명력 증가
    public void PuzzleResurDam()
    {
        if (puzzleResurDam)
        {
            extraDamage += 0.1f;
            extraSpeed += 0.1f;
            playerData.playerMaxHP += playerData.playerMaxHP * 0.1f;
        }
    }
    #endregion

    public void LevelUP()
    {
        playerData.playerCurEXP = 0;
        playerData.playerNextEXP = playerData.playerExp[Mathf.Min(playerData.playerLevel, playerData.playerExp.Length)];
        playerData.playerLevel += 1;
        PuzzleLevelUPAblity();
        levelUpUi.Show();
    }

    public void GetEXP()
    {
        playerData.playerCurEXP += 1;

        if (playerData.playerCurEXP >= playerData.playerNextEXP)
        {
            LevelUP();
        }
    }

    public void GetHP()
    {
        playerData.playerCurHP += playerData.playerMaxHP * 0.3f;

        if (playerData.playerCurHP > playerData.playerMaxHP)
        {
            playerData.playerCurHP = playerData.playerMaxHP;
        }
    }

    public void GetGold()
    {
        int coinRanNum = Random.Range(0, 100);

        if(coinRanNum < playerTraitsData.traitsDoubleCoinPer)
        {
            playerData.playerGold += 2;
        }

        playerData.playerGold += 1;
    }

    public void GetCrystal()
    {
        if (playerData.playerCrystal >= playerData.playerMaxCrystal) return;
        playerData.playerCrystal += 1;
    }

    public void GetBullet()
    {
        currentGun.carryBulletCount += 30;

        if(currentGun.carryBulletCount >= currentGun.maxBulletCount)
        {
            currentGun.carryBulletCount = currentGun.maxBulletCount;
        }
    }

    //플레이어 데이터
    [ContextMenu("To Json Data")]
    public void SavePlayerDataToJson()
    {
        InitPlayerBasic();
        if (playerData.playerCrystal >= playerData.playerMaxCrystal) playerData.playerCrystal = playerData.playerMaxCrystal;
        string jsonData = JsonUtility.ToJson(playerData, true);
        string path = Path.Combine(Application.persistentDataPath + "/playerData.json");
        File.WriteAllText(path, jsonData);
        Debug.Log("저장완료");
    }

    [ContextMenu("From Json Data")]
    public void LoadPlayerDataToJson()
    {
        string path = Path.Combine(Application.persistentDataPath + "/playerData.json");
        if (File.Exists(path))
        {
            string jsonData = File.ReadAllText(path);
            playerData = JsonUtility.FromJson<PlayerData>(jsonData);
            Debug.Log("불러오기 완료");
        }
        else
        {
            playerData = new PlayerData();
            Debug.Log("파일생성 완료");
        }
    }

    //설정 데이터
    [ContextMenu("To Json Data")]
    public void SavePlayerSettingDataToJson()
    {
        string jsonData = JsonUtility.ToJson(settingData, true);
        string path = Path.Combine(Application.persistentDataPath + "/settingData.json");
        File.WriteAllText(path, jsonData);
        Debug.Log("저장완료");
    }

    [ContextMenu("From Json Data")]
    public void LoadPlayerSettingDataToJson()
    {
        string path = Path.Combine(Application.persistentDataPath + "/settingData.json");
        if (File.Exists(path))
        {
            string jsonData = File.ReadAllText(path);
            settingData = JsonUtility.FromJson<SettingData>(jsonData);
            Debug.Log("불러오기 완료");
        }
        else
        {
            settingData = new SettingData();
            Debug.Log("파일생성 완료");
        }
    }

    //특성 데이터
    [ContextMenu("To Json Data")]
    public void SavePlayerTraitsDataToJson()
    {
        string jsonData = JsonUtility.ToJson(playerTraitsData, true);
        string path = Path.Combine(Application.persistentDataPath + "/playerTraitsData.json");
        File.WriteAllText(path, jsonData);
        Debug.Log("저장완료");
    }

    [ContextMenu("From Json Data")]
    public void LoadPlayerTraitsDataToJson()
    {
        string path = Path.Combine(Application.persistentDataPath + "/playerTraitsData.json");
        if (File.Exists(path))
        {
            string jsonData = File.ReadAllText(path);
            playerTraitsData = JsonUtility.FromJson<PlayerTraitsData>(jsonData);
            Debug.Log("불러오기 완료");
        }
        else
        {
            playerTraitsData = new PlayerTraitsData();
            Debug.Log("파일생성 완료");
        }
    }
}

//플레이어의 데이터
public class PlayerData
{
    public float playerMaxHP = 120;
    public float playerCurHP = 0;
    public float playerNextEXP = 0;
    public float playerCurEXP = 0;
    public float playerGold = 0;
    public float playerCrystal;
    public float playerMaxCrystal = 25000;
    public int playerLevel = 1;
    public float playerCriticalPer = 5;
    public float playerCriticalDam = 1.5f;
    public float playerCurDamage;
    public float playerWalkSpeed = 8;
    public float playerRunSpeed = 13;
    public float playerCrouchSpeed = 3;
    public float playerJumpForce = 0;
    public int playerResur = 0;
    public float[] playerExp = {12,19,28,42,56,63,70,81,93,109,121,136,155,166,189,190,200,210,213,217,220,228,
        231,233,235,236,245,251,274,288,297,324,356,378,403,456,484,499,518,553,9999};
    public bool isDead = false;
}

//설정 데이터
public class SettingData
{
    public float mouseSensitivity = 1;
    public float soundEffectVolume = 1;
    public float soundBgmVolume = 1;
}

//특성 데이터
public class PlayerTraitsData
{
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
}
