using DigitalRuby.LightningBolt;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class GunController : MonoBehaviour
{
    //Ȱ��ȭ ����
    public static bool isActivate = false;

    [SerializeField]
    private Gun currentGun;

    //���� �ӵ�
    [SerializeField]
    private float currentFireRate;

    //���� ����
    private bool isReload = false;
    private bool isFineSightMode = false;

    private Vector3 originPos;

    //�浹 ����
    private RaycastHit hitInfo;

    [SerializeField]
    private Camera theCam;
    private Crosshair theCrosshair;

    //Ÿ�� ����Ʈ
    [SerializeField]
    private GameObject hitEffect_Prefab;

    [SerializeField]
    private string Fire_Sound;
    [SerializeField]
    private PlayerController playerController;

    private Enemy enemy;
    [SerializeField]
    private int gunShootingStack = 0;
    [SerializeField]
    private float gunShootingSpeed = 0;

    private void Start()
    {
        originPos = Vector3.zero;

        theCrosshair = FindAnyObjectByType<Crosshair>();

    }

    // Update is called once per frame
    void Update()
    {
        if (!isActivate || GameManager.Instance.isOpenTab)
            return;

        GunFireRateCalc();
        TryFire();
        TryReload();
    }

    //���� �ӵ� ����
    private void GunFireRateCalc()
    {
        if(currentFireRate > 0)
        {
            currentFireRate -= Time.deltaTime;
        }
    }

    //�߻� �õ�
    private void TryFire()
    {
        if (Input.GetButton("Fire1") && currentFireRate <= 0 && !isReload)
        {
            Fire();
        }
    }

    //�߻� ��
    private void Fire()
    {
        if (!isReload)
        {
            if (currentGun.currentBulletCount > 0)
            {
                Shoot();
            }
            else
            {
                StartCoroutine(ReloadCoroutine());
            }
        }

    }

    //�߻� ��
    private void Shoot()
    {
        if (playerController.GetPlayerIsRun()) return;

        if (GameManager.Instance.gunIsSpeed)
        {
            if (gunShootingStack < 3) gunShootingStack++;
        }

        gunShootingSpeed = gunShootingStack * GameManager.Instance.gunSpeed;

        theCrosshair.FireAnim();
        currentGun.currentBulletCount--;
        currentFireRate = (currentGun.fireRate)-(currentGun.fireRate * gunShootingSpeed);
        SoundManager.instance.PlaySE(Fire_Sound);
        currentGun.muzzleFlash.Play();

        Hit();

        StopCoroutine(ReloadCoroutine());
        StartCoroutine(RetroActionCoroutine());
    }

    //�ǰ� ȿ��
    private void Hit()
    {
        if (Physics.Raycast(theCam.transform.position, theCam.transform.forward + 
            new Vector3(Random.Range(-theCrosshair.GetAccuracy() - currentGun.accuracy, theCrosshair.GetAccuracy() + currentGun.accuracy),
                        Random.Range(-theCrosshair.GetAccuracy() - currentGun.accuracy, theCrosshair.GetAccuracy() + currentGun.accuracy),
                        0)
            , out hitInfo, (currentGun.range) + (currentGun.range * GameManager.Instance.extraRange)))
        {
            if(hitInfo.transform.tag == "Enemy")
            {
                GameObject hitEfffect = PoolManager.instance.ActivateObj(7);
                hitEfffect.transform.position = hitInfo.point;

                enemy = hitInfo.collider.GetComponent<Enemy>();

                if(enemy != null && enemy.enemyCurrentHP > 0)
                {
                    GameManager.Instance.puzzleCriExtraDam();
                    int cri = Random.Range(0, 100);
                    float damage = currentGun.damage + (currentGun.damage * GameManager.Instance.extraDamage) +(currentGun.damage * GameManager.Instance.gunExtraDamage);
                    GameManager.Instance.playerCurDamage = damage;
                    int criPer = (int)(currentGun.criticalPer + (GameManager.Instance.extraCriticalPer * 100));
                    float cridam = currentGun.criticalDamage + (GameManager.Instance.extraCriticalDamage);

                    float adrenalineDam = GameManager.Instance.isIncreased ? damage * 1.5f : 0;

                    damage += GameManager.Instance.MaxHPDam(damage) + adrenalineDam;

                    GameManager.Instance.Judge(enemy);
                    if (cri < criPer)
                    {
                        CriticalNearbyEnemyAttack();
                        GameManager.Instance.isCritical = true;
                        enemy.enemyCurrentHP -= (damage + (damage * cridam)) + ((damage + (damage * cridam)) * GameManager.Instance.extraFinalDamage)+ GameManager.Instance.REBAddAttack(enemy, damage);
                    }
                    else
                    {
                        enemy.enemyCurrentHP -= damage + (damage * GameManager.Instance.extraFinalDamage) + GameManager.Instance.NoCriDam(damage) + GameManager.Instance.REBAddAttack(enemy, damage);
                    }

                    LightningAttack(damage);

                }  
            }
        }
    }

    //������ �õ�
    private void TryReload()
    {
        if(Input.GetKeyDown(KeyCode.R) && !isReload && currentGun.currentBulletCount < currentGun.reloadBulletCount)
        {
            gunShootingStack = 0;
            StartCoroutine(ReloadCoroutine());
        }
    }

    //������ ���
    public void CancelReload()
    {
        if (isReload)
        {
            StopCoroutine(RetroActionCoroutine());
            StopCoroutine(ReloadCoroutine());
            isReload = false;
        }
    }

    //������ �ڷ�ƾ
    IEnumerator ReloadCoroutine()
    {
        if(currentGun.carryBulletCount > 0)
        {
            isReload = true;
            currentGun.anim.SetTrigger("Reload");

            currentGun.carryBulletCount += currentGun.currentBulletCount;
            currentGun.currentBulletCount = 0;

            yield return new WaitForSeconds(currentGun.reloadTime);

            if(currentGun.carryBulletCount >= currentGun.reloadBulletCount)
            {
                currentGun.currentBulletCount = currentGun.reloadBulletCount;
                currentGun.carryBulletCount -= currentGun.reloadBulletCount;
            }
            else
            {
                currentGun.currentBulletCount = currentGun.carryBulletCount;
                currentGun.carryBulletCount = 0;
            }

            isReload = false;
        }
        else
        {
            Debug.Log("���� �Ѿ� ����");
        }
    }

    //�ݵ� �ڷ�ƾ
    IEnumerator RetroActionCoroutine()
    {
        Vector3 recoilBack = new Vector3(currentGun.retroActionForce, originPos.y, originPos.z);
        Vector3 retroActionRecoilBack = new Vector3(currentGun.retroActionFineSightForce, currentGun.fineSightOriginPos.y, currentGun.fineSightOriginPos.z);

        if (!isFineSightMode)
        {
            currentGun.transform.localPosition = originPos;

            while(currentGun.transform.localPosition.x <= currentGun.retroActionForce - 0.02f)
            {
                currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, recoilBack, 0.4f);
                yield return null;
            }

            while(currentGun.transform.localPosition != originPos)
            {
                currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, originPos, 0.1f);
                yield return null;
            }
        }
        else
        {
            currentGun.transform.localPosition = currentGun.fineSightOriginPos;

            while (currentGun.transform.localPosition.x <= currentGun.retroActionFineSightForce - 0.02f)
            {
                currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, retroActionRecoilBack, 0.4f);
                yield return null;
            }

            while (currentGun.transform.localPosition != currentGun.fineSightOriginPos)
            {
                currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, currentGun.fineSightOriginPos, 0.1f);
                yield return null;
            }
        }
    }

    //ġ��Ÿ �� �ֺ� �� ����(����)
    private void CriticalNearbyEnemyAttack()
    {
        if (GameManager.Instance.puzzleCriNearby)
        {
            Collider[] colliders = Physics.OverlapSphere(hitInfo.point, 10f, LayerMask.GetMask("Enemy"))
                .Where(collider => collider.gameObject != hitInfo.collider.gameObject)
                .ToArray();

            for (int i = 0; i < colliders.Length; i++)
            {
                Enemy nearbyEnemy = colliders[i].GetComponent<Enemy>();

                if (nearbyEnemy != null && nearbyEnemy.enemyCurrentHP > 0)
                {
                    nearbyEnemy.enemyCurrentHP -= (nearbyEnemy.enemyCurrentHP * 0.05f);
                }
            }
        }
    }

    //���� ���� �õ�
    private void LightningAttack(float damage)
    {
        if (GameManager.Instance.gunLightning)
        {
            Collider[] colliders = Physics.OverlapSphere(hitInfo.point, 15f, LayerMask.GetMask("Enemy"))
                .Where(collider => collider.gameObject != hitInfo.collider.gameObject)
                .ToArray();

            int bounceCount = (int)Mathf.Ceil(GameManager.Instance.gunLightningCount * 100);

            for (int i = 0; i < Mathf.Min(bounceCount, colliders.Length); i++)
            {
                Enemy nearbyEnemy = colliders[i].GetComponent<Enemy>();
                string attackPoint = "AttackPoint";
                StartCoroutine(ShowLightning(enemy.transform.Find(attackPoint).gameObject, nearbyEnemy.transform.Find(attackPoint).gameObject));

                if (nearbyEnemy != null && nearbyEnemy.enemyCurrentHP > 0)
                {
                    nearbyEnemy.enemyCurrentHP -= damage + (damage * GameManager.Instance.extraFinalDamage);
                }
            }
        }
    }

    //���� ���� ����Ʈ
    IEnumerator ShowLightning(GameObject start, GameObject end)
    {
        GameObject hitLightEfffect = PoolManager.instance.ActivateObj(15);
        hitLightEfffect.transform.position = hitInfo.point;
        LightningBoltScript light = hitLightEfffect.GetComponent<LightningBoltScript>();

        light.StartObject = start;
        light.EndObject = end;

        yield return new WaitForSeconds(0.5f);

        hitLightEfffect.SetActive(false);
    }

    public Gun GetGun()
    {
        return currentGun;
    }

    public bool GetFineSightMode()
    {
        return isFineSightMode;
    }

    public void GunChange(Gun gun)
    {
        if(WeaponManager.currentWeapon != null)
        {
            WeaponManager.currentWeapon.gameObject.SetActive(false);
        }

        currentGun = gun;
        WeaponManager.currentWeapon = currentGun.GetComponent<Transform>();
        WeaponManager.currentWeaponAnim = currentGun.anim;

        currentGun.transform.localPosition = Vector3.zero;
        currentGun.gameObject.SetActive(true);
        isActivate = true;
    }
}
