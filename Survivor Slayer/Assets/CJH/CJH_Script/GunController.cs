//using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GunController : MonoBehaviour
{
    private const int BULLET_SPEED = 80;        // 총알 탄속
    
    [SerializeField] private Gun currentGun;
    public Transform bulletPos;
    public ObjectManager _ObjectManager;

    public float currentFireRate;      //연사속도

    private AudioSource _audioSource;

    public bool isReload = false;

    [SerializeField] private Vector3 originPos;     //조준전 포지션

    private RaycastHit hitinfo;
    [SerializeField] private Camera thecam;

    //인성 추가
    private Animator playerAnim;
    private FlashLight flashLight;
    private Crosshair crosshair;
    private KillAni_Ctrl aniCtrl; // 확정킬 제어 변수


    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        playerAnim = GetComponent<Animator>();
        flashLight = GetComponentInChildren<FlashLight>();
        crosshair = FindObjectOfType<Crosshair>();
        aniCtrl = GetComponent<KillAni_Ctrl>();
    }

    void Update()
    {
        GunFireRateCalc();
        TryFire();
        TryReload();

    }

    private void GunFireRateCalc()
    {
        if (currentFireRate > 0)
            currentFireRate -= Time.deltaTime;
        
    }

    private void TryFire()
    {
        if (Input.GetButton("Fire1") && currentFireRate <= 0 && !isReload&&!UIManager.instance.mapActive&& !aniCtrl.CheckIsPlaying()) // 인성 수정. 맵이 켜지지 않을 때 조건 추가
        {
            if (currentGun.currentBulletCount > 0)
                Fire();
        }
    }

    private void Fire()
    {
        if (!isReload)
        {
            if(currentGun.currentBulletCount > 0)
                Shoot();
            else
            {
                StartCoroutine(ReloadCoroutine());
            }
        }
    }

    private void Shoot()
    {
      
        // 무기 탄창수 감소
        currentGun.currentBulletCount--;
        currentFireRate = currentGun.fireRate;
        
        // 무기발사시 불꽃,효과음효과
        currentGun.muzzleFlash.Play();
        PlaySE(currentGun.fireSound);
        flashLight.Flash(); // 플래쉬 효과 추가
        crosshair.FireOnCrosshair();// 총 발사 크로스헤어
        currentGun.gunAnim.SetTrigger("isFire");

        // Hit();   //raycast 방식인데 bullet생성이 더 좋은거라 생각 나중에 삭제해서 통합
        Vector3 v = thecam.transform.position - bulletPos.transform.position;
        var angle = Mathf.Atan2(v.y, v.x) * Mathf.Rad2Deg;
        //인성 수정: // 레이 케스트에 랜덤값을 줘서 탄이 퍼지도록
        Physics.Raycast(thecam.transform.position, thecam.transform.forward+
            new Vector3(Random.Range(-crosshair.GetAccuracy()-currentGun.accuracy, crosshair.GetAccuracy() + currentGun.accuracy)
                        , Random.Range(-crosshair.GetAccuracy() - currentGun.accuracy, crosshair.GetAccuracy() + currentGun.accuracy)
                        ,0)
            ,out hitinfo);
        Debug.DrawRay(thecam.transform.position, thecam.transform.forward * hitinfo.distance, Color.red);

        bulletPos.LookAt(hitinfo.point);
       
        GameObject intantBullet = _ObjectManager.MakeObj("Bullet", bulletPos.position, bulletPos.rotation);
        Rigidbody bulletRigid = intantBullet.GetComponent<Rigidbody>();
        bulletRigid.velocity = bulletPos.forward * BULLET_SPEED;
        
        // StopAllCoroutines();
        // StartCoroutine(RetroActionCoroutine());
    }

    private void TryReload()
    {
        if (Input.GetKeyDown(KeyCode.R) && !isReload && currentGun.currentBulletCount < currentGun.reloadBulletCount)
        {
            StartCoroutine(ReloadCoroutine());
        }
    }

    IEnumerator ReloadCoroutine()
    {
        if (currentGun.carryBulletCount > 0)
        {
            // currentGun.animation.Settrigger("Reload");   // 재장전 애니메이션 호출
            //인성 추가
            playerAnim.SetTrigger("Reload");
            //
            isReload = true;
            PlaySE(currentGun.reloadSound);
            currentGun.carryBulletCount += currentGun.currentBulletCount; //남은 탄창 최대 탄창에 +
            currentGun.currentBulletCount = 0;
            
            yield return new WaitForSeconds(currentGun.reloadTime);

            if (currentGun.carryBulletCount >= currentGun.reloadBulletCount)
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
    }
    
    private void PlaySE(AudioClip _clip)
    {
        _audioSource.clip = _clip;
        _audioSource.Play();
    }

    public Gun GetGun()
    {
        return currentGun;
    }
}
