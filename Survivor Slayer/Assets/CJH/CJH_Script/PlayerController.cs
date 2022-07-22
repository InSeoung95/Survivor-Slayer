using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float walkSpeed = 200f;
    [SerializeField] private float runSpeed = 250f;
    private float applySpeed;
    
    [SerializeField] private float lookSensitivity;
    [SerializeField] private float cameraRotationLimit;
    private float cameraRotationX = 0f;

    [SerializeField] private Camera _mycamera;
    private Rigidbody _myRigid;

    private bool isRun = false;
    public bool isMoving = false;
    private bool isBorder = false; // 이동할 벡터를 border가 검사해서 앞에 layer가 있으면 못지나가게 설정
    private RaycastHit hit;
    //인성 추가
    private Crosshair crosshair;
    private KillAni_Ctrl aniCtrl; // 확정킬 제어 변수

    void Start()
    {
        _myRigid = GetComponent<Rigidbody>();
        applySpeed = walkSpeed;
        crosshair = FindObjectOfType<Crosshair>();
        aniCtrl = GetComponent<KillAni_Ctrl>();
    }

    
    void Update()
    {
        TryRun();
        Move();
        CameraRotation();
        CaracterRotation();
        
    }
    
    private void TryRun()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            isRun = true;
            applySpeed = runSpeed;
        }

        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            isRun = false;
            applySpeed = walkSpeed;
        }
    }

    private void Move()
    {
        float _moveDirX = Input.GetAxisRaw("Horizontal");
        float _moveDirZ = Input.GetAxisRaw("Vertical");
        if (_moveDirX != 0 || _moveDirZ != 0)
            isMoving = true;
        else
            isMoving = false;
        //인성 추가
        crosshair.MoveOnCrosshair(isMoving);

        Vector3 _moveHorizontal = transform.right * _moveDirX;
        Vector3 _moveVertical = transform.forward * _moveDirZ;
        Vector3 _moveVector = (_moveHorizontal + _moveVertical).normalized;
        Vector3 _velocity = _moveVector * applySpeed;
        
        isBorder = Physics.Raycast(transform.position, _moveVector, out hit, 2.5f);
        Debug.DrawRay(transform.position + new Vector3(0,0.1f,0) , _moveVector * 1, Color.blue);
        if(isBorder)
        {
            if (hit.transform.CompareTag("Wall") || hit.transform.CompareTag("Base"))
                isBorder = true;
            else
                isBorder = false;
        }

        if (!isBorder&&!aniCtrl.CheckIsPlaying()&&!UIManager.instance.OnInteract) // 조건 추가. 확정킬 재생 중일때 and 상호 작용 중 이동 X
        {
            _myRigid.MovePosition(_myRigid.position + _velocity * Time.deltaTime);
            
        }
        
    }

    private void CameraRotation()
    {
       
        if(!UIManager.instance.OnInteract&&!aniCtrl.CheckIsPlaying()) //인성 추가
        {
            float _xRotation = Input.GetAxisRaw("Mouse Y");
            float _cameraRotationX = _xRotation * lookSensitivity;
            cameraRotationX -= _cameraRotationX;
            cameraRotationX = Mathf.Clamp(cameraRotationX, -cameraRotationLimit, cameraRotationLimit);

            _mycamera.transform.localEulerAngles = new Vector3(cameraRotationX, 0, 0);
        }
       
    }

    private void CaracterRotation()
    {
        if(!UIManager.instance.OnInteract&& !aniCtrl.CheckIsPlaying())// 인성 추가
        {
            float _yRotation = Input.GetAxisRaw("Mouse X");
            Vector3 _characterRatationY = new Vector3(0, _yRotation, 0) * lookSensitivity;
            _myRigid.MoveRotation(_myRigid.rotation * Quaternion.Euler(_characterRatationY));
        }
        
    }
}
