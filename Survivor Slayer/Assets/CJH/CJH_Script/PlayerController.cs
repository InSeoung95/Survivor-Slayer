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

    [SerializeField]private Camera _mycamera;
    private Rigidbody _myRigid;

    private bool isRun = false;
    private bool isBorder = false; // 이동시 벽 충돌체크
    
    void Start()
    {
        _myRigid = GetComponent<Rigidbody>();
        applySpeed = walkSpeed;
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

        Vector3 _moveHorizontal = transform.right * _moveDirX;
        Vector3 _moveVertical = transform.forward * _moveDirZ;
        Vector3 _moveVector = (_moveHorizontal + _moveVertical).normalized;
        Vector3 _velocity = _moveVector * applySpeed;
        
        isBorder = Physics.Raycast(transform.position, _moveVector, 1, LayerMask.GetMask("Wall"));
        if (!isBorder)
        {
            // transform.position += _velocity * Time.deltaTime;
            _myRigid.MovePosition(transform.position + _velocity * Time.deltaTime);
        }
    }

    private void CameraRotation()
    {
        float _xRotation = Input.GetAxisRaw("Mouse Y");
        float _cameraRotationX = _xRotation * lookSensitivity;
        cameraRotationX -= _cameraRotationX;
        cameraRotationX = Mathf.Clamp(cameraRotationX, -cameraRotationLimit, cameraRotationLimit);

        _mycamera.transform.localEulerAngles = new Vector3(cameraRotationX, 0, 0);
    }

    private void CaracterRotation()
    {
        float _yRotation = Input.GetAxisRaw("Mouse X");
        Vector3 _characterRatationY = new Vector3(0, _yRotation, 0) * lookSensitivity;
        _myRigid.MoveRotation(_myRigid.rotation * Quaternion.Euler(_characterRatationY));
    }
}
