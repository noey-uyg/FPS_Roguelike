using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Player")]
    private float walkSpeed;
    private float runSpeed;
    private float crouchSpeed;
    private float jumpForce;
    private float currentSpeed;

    private Rigidbody myRigid;
    private CapsuleCollider capsuleCollider;
    private GunController theGunController;
    private Crosshair theCrosshair;

    private bool isWalk = false;
    private bool isRun = false;
    private bool isGround = true;
    private bool isCrouch = false;

    private Vector3 lastPos;

    [Header("Camera")]
    public float mouseSensitivity;
    [SerializeField]
    private float cameraRotationLimit;
    private float currentCameraRoataionX;
    [SerializeField]
    private Camera myCamera;
    [SerializeField]
    private float crouhPosY;
    private float originPosY;
    private float currentPosY;

    // Start is called before the first frame update
    void Start()
    {
        InitPlayerInfo();

        myRigid = GetComponent<Rigidbody>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        theGunController = FindAnyObjectByType<GunController>();
        theCrosshair = FindAnyObjectByType<Crosshair>();

        currentSpeed = walkSpeed;
        originPosY = myCamera.transform.localPosition.y;
        currentPosY = originPosY;
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.Instance.canPlayerMove || GameManager.Instance.mainScene) return;

        mouseSensitivity = GameManager.Instance.mouseSensitivity;
        IsGround();
        TryJump();
        TryRun();
        TryCrouch();
        Move();
        CameraRotation();
        CharacterRotation();
    }

    private void InitPlayerInfo()
    {
        walkSpeed = (GameManager.Instance.playerWalkSpeed);
        runSpeed = (GameManager.Instance.playerRunSpeed);
        crouchSpeed = (GameManager.Instance.playerCrouchSpeed);
        jumpForce = GameManager.Instance.playerJumpForce;
    }

    private void FixedUpdate()
    {
        MoveCheck();
    }

    //�ɱ� �õ�
    private void TryCrouch()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            Crouch();
        }
    }

    //�ɱ�
    private void Crouch()
    {
        if (isWalk)
        {
            isWalk = false;
            theCrosshair.WalkingAnim(isWalk);
        }

        isCrouch = !isCrouch;
        theCrosshair.CrouchingAnim(isCrouch);
        if (isCrouch)
        {
            currentSpeed = crouchSpeed;
            currentPosY = crouhPosY;
        }
        else
        {
            currentSpeed = walkSpeed;
            currentPosY = originPosY;
        }

        StartCoroutine(CrouchCoroutine());
    }

    //�ɱ� ���� ����
    IEnumerator CrouchCoroutine()
    {
        float posY = myCamera.transform.localPosition.y;
        int count = 0;

        while(posY != currentPosY)
        {
            count++;

            posY = Mathf.Lerp(posY, currentPosY, 0.1f);
            myCamera.transform.localPosition = new Vector3(0, posY, 0);
            if (count > 15) break;

            yield return null;
        }
        myCamera.transform.localPosition = new Vector3(0, currentPosY, 0);
    }

    //���� �õ�
    private void TryJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGround)
        {
            Jump();
        }
    }

    //���� üũ
    private void IsGround()
    {
        isGround = Physics.Raycast(transform.position, Vector3.down, capsuleCollider.bounds.extents.y + 0.3f);
        theCrosshair.JumpAnim(!isGround);
    }

    //���� 
    private void Jump()
    {
        if (isCrouch)
        {
            Crouch();
        }

        myRigid.velocity = transform.up * jumpForce;
    }

    //�޸��� �õ�
    private void TryRun()
    {
        if (Input.GetKey(KeyCode.LeftShift) && !isCrouch)
        {
            Running();
        }

        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            RunningCancle();
        }
    }

    //�޸���
    private void Running()
    {
        isRun = true;
        theCrosshair.RunningAnim(isRun);
        currentSpeed = runSpeed;
    }

    //�޸��� ���
    private void RunningCancle()
    {
        isRun = false;
        theCrosshair.RunningAnim(isRun);
        currentSpeed = walkSpeed;
    }

    //�÷��̾� ������
    private void Move()
    {
        float moveDirX = Input.GetAxisRaw("Horizontal");
        float moveDirZ = Input.GetAxisRaw("Vertical");

        Vector3 moveHorizontal = transform.right * moveDirX;
        Vector3 moveVertical = transform.forward * moveDirZ;

        float FinalSpeed;
        FinalSpeed = currentSpeed + (currentSpeed * GameManager.Instance.extraSpeed);

        Vector3 velocity = (moveHorizontal + moveVertical).normalized * FinalSpeed;

        myRigid.MovePosition(transform.position + velocity * Time.deltaTime);
    }

    //������ üũ
    private void MoveCheck()
    {
        if (!isRun && !isCrouch && isGround)
        {
            if (Vector3.Distance(lastPos, transform.position) >= 0.01f)
            {
                isWalk = true;
            }
            else
            {
                isWalk = false;
            }

            theCrosshair.WalkingAnim(isWalk);
            lastPos = transform.position; 
        }

    }

    //ĳ���� �¿� ȸ��
    private void CharacterRotation()
    {
        float rotationY = Input.GetAxisRaw("Mouse X");
        Vector3 characterRotationY = new Vector3(0f, rotationY, 0f) * mouseSensitivity;

        myRigid.MoveRotation(myRigid.rotation * Quaternion.Euler(characterRotationY));
    }

    //ī�޶� ���� ����
    private void CameraRotation()
    {
        float rotationX = Input.GetAxisRaw("Mouse Y");
        float cameraRotationX = rotationX * mouseSensitivity;
        currentCameraRoataionX -= cameraRotationX;
        currentCameraRoataionX = Mathf.Clamp(currentCameraRoataionX, -cameraRotationLimit, cameraRotationLimit);

        myCamera.transform.localEulerAngles = new Vector3(currentCameraRoataionX, 0, 0);
    }

    public bool GetPlayerIsRun()
    {
        return isRun;
    }

}
