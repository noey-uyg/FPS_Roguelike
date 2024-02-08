using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crosshair : MonoBehaviour
{
    [SerializeField]
    private Animator animator;

    private float gunAccuracy;

    [SerializeField]
    private GameObject go_CrosshairHUD;
    [SerializeField]
    private GunController theGunController;

    public void WalkingAnim(bool flag)
    {
        if (GameManager.Instance.mainScene) return;
        WeaponManager.currentWeaponAnim.SetBool("Walk", flag);
        animator.SetBool("Walking", flag);
    }

    public void RunningAnim(bool flag)
    {
        if (GameManager.Instance.mainScene) return;
        WeaponManager.currentWeaponAnim.SetBool("Run", flag);
        animator.SetBool("Running", flag);
    }

    public void JumpAnim(bool flag)
    {
        if (GameManager.Instance.mainScene) return;
        animator.SetBool("Running", flag);
    }

    public void CrouchingAnim(bool flag)
    {
        if (GameManager.Instance.mainScene) return;
        animator.SetBool("Crouching", flag);
    }

    public void FineSightAnim(bool flag)
    {
        if (GameManager.Instance.mainScene) return;
        animator.SetBool("FineSight", flag);
    }

    public void FireAnim()
    {
        if (GameManager.Instance.mainScene) return;
        if (animator.GetBool("Walking"))
        {
            animator.SetTrigger("Walk_Fire");
        }
        else if (animator.GetBool("Crouching"))
        {
            animator.SetTrigger("Crouch_Fire");
        }
        else
        {
            animator.SetTrigger("Idle_Fire");
        }
    }

    public float GetAccuracy()
    {
        if (animator.GetBool("Walking"))
        {
            gunAccuracy = 0.06f;
        }
        else if (animator.GetBool("Crouching"))
        {
            gunAccuracy = 0.015f;
        }
        else if(theGunController.GetFineSightMode())
        {
            gunAccuracy = 0.001f;
        }
        else
        {
            gunAccuracy = 0.035f;
        }

        return gunAccuracy;
    }
}
