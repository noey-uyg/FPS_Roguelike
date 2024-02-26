using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    //중복 교체 방지
    public static bool isChangeWeapon = false;
    
    //현재 무기와 애니메이션
    public static Transform currentWeapon;
    public static Animator currentWeaponAnim;

    //현재 무기 타입
    [SerializeField]
    public static string currentWeaponType;


    //무기 교체 딜레이
    [SerializeField]
    private float changeWeaponDelayTime;
    //무기 교체가 끝난 시점
    [SerializeField]
    private float changeWeaponEndDelayTime;

    //무기 관리
    [SerializeField]
    private Gun[] guns;
    [SerializeField]
    private CloseWeapon[] hands;
    [SerializeField]
    private CloseWeapon[] axes;

    //무기 접근
    private Dictionary<string, Gun> gunDic = new Dictionary<string, Gun>();
    private Dictionary<string, CloseWeapon> handDic = new Dictionary<string, CloseWeapon>();
    private Dictionary<string, CloseWeapon> axeDic = new Dictionary<string, CloseWeapon>();

    //컨트롤러 변경
    [SerializeField]
    private GunController theGunController;
    [SerializeField]
    private HandController theHandController;
    [SerializeField]
    private AxeController theAxeController;

    void Start()
    {
        for (int i = 0; i < guns.Length; i++)
        {
            gunDic.Add(guns[i].gunName, guns[i]);
        }
        for (int i = 0; i < hands.Length; i++)
        {
            handDic.Add(hands[i].closeWeaponName, hands[i]);
        }
        for (int i = 0; i < axes.Length; i++)
        {
            axeDic.Add(axes[i].closeWeaponName, axes[i]);
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.isOpenTab || GameManager.Instance.mainScene) return;

        if (currentWeaponType == null)
        {
            StartCoroutine(ChangeWeaponCoroutine("HAND", "맨손"));
            currentWeaponType = "HAND";
            HandController.isActivate = true;
        }

        if (!isChangeWeapon)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                //맨손 교체 실행
                StartCoroutine(ChangeWeaponCoroutine("HAND", "맨손"));
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                //서브머신건 교체 실행
                StartCoroutine(ChangeWeaponCoroutine("GUN", "SubMachineGun1"));
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                //도끼 교체 실행
                StartCoroutine(ChangeWeaponCoroutine("AXE", "Axe"));
            }
        }
    }

    public IEnumerator ChangeWeaponCoroutine(string type, string name)
    {
        isChangeWeapon = true;
        currentWeaponAnim.SetTrigger("Weapon_Out");

        yield return new WaitForSeconds(changeWeaponDelayTime);

        CancelPreWeaponAction();
        WeaponChange(type, name);

        yield return new WaitForSeconds(changeWeaponEndDelayTime);

        currentWeaponType = type;
        isChangeWeapon = false;
    }

    //전 무기 애니메이션 비활성화
    private void CancelPreWeaponAction()
    {
        switch (currentWeaponType)
        {
            case "GUN":
                theGunController.CancelReload();
                GunController.isActivate = false;
                break;
            case "HAND":
                HandController.isActivate = false;
                break;
            case "AXE":
                AxeController.isActivate = false;
                break;
        }
    }

    //무기 교체
    private void WeaponChange(string type, string name)
    {
        GameManager.Instance.Adrenaline();
        if(type == "GUN")
        {
            theGunController.GunChange(gunDic[name]);
            GameManager.Instance.playerData.playerCurDamage = gunDic[name].damage;
        }
        else if(type == "HAND")
        {
            theHandController.CloseWeaponChange(handDic[name]);
            GameManager.Instance.playerData.playerCurDamage = handDic[name].damage;
        }
        else if (type == "AXE")
        {
            theAxeController.CloseWeaponChange(axeDic[name]);
            GameManager.Instance.playerData.playerCurDamage = axeDic[name].damage;
        }
            
    }


}
