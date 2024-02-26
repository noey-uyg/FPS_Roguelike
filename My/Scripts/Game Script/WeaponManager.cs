using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    //�ߺ� ��ü ����
    public static bool isChangeWeapon = false;
    
    //���� ����� �ִϸ��̼�
    public static Transform currentWeapon;
    public static Animator currentWeaponAnim;

    //���� ���� Ÿ��
    [SerializeField]
    public static string currentWeaponType;


    //���� ��ü ������
    [SerializeField]
    private float changeWeaponDelayTime;
    //���� ��ü�� ���� ����
    [SerializeField]
    private float changeWeaponEndDelayTime;

    //���� ����
    [SerializeField]
    private Gun[] guns;
    [SerializeField]
    private CloseWeapon[] hands;
    [SerializeField]
    private CloseWeapon[] axes;

    //���� ����
    private Dictionary<string, Gun> gunDic = new Dictionary<string, Gun>();
    private Dictionary<string, CloseWeapon> handDic = new Dictionary<string, CloseWeapon>();
    private Dictionary<string, CloseWeapon> axeDic = new Dictionary<string, CloseWeapon>();

    //��Ʈ�ѷ� ����
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
            StartCoroutine(ChangeWeaponCoroutine("HAND", "�Ǽ�"));
            currentWeaponType = "HAND";
            HandController.isActivate = true;
        }

        if (!isChangeWeapon)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                //�Ǽ� ��ü ����
                StartCoroutine(ChangeWeaponCoroutine("HAND", "�Ǽ�"));
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                //����ӽŰ� ��ü ����
                StartCoroutine(ChangeWeaponCoroutine("GUN", "SubMachineGun1"));
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                //���� ��ü ����
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

    //�� ���� �ִϸ��̼� ��Ȱ��ȭ
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

    //���� ��ü
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
