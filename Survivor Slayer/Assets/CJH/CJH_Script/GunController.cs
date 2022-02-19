using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{
    [SerializeField] private Gun currentGun;
    public GameObject bullet;
    public Transform bulletPos;

    public float currentFireRate;      //연사속도

    private AudioSource _audioSource;

    public bool isReload = false;
    public bool isFineSightMode = false;

    [SerializeField] private Vector3 originPos;     //조준전 포지션

    private RaycastHit hitinfo;
    [SerializeField] private Camera thecam;
    [SerializeField] private GameObject hit_effect_prefab;

    private void Start()
    {
        // _audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        GunFireRateCalc();
        TryFire();
        TryReload();
        TryFineSight();

    }

    private void GunFireRateCalc()
    {
        if (currentFireRate > 0)
            currentFireRate -= Time.deltaTime;
        
    }

    private void TryFire()
    {
        if (Input.GetButton("Fire1") && currentFireRate <= 0 && !isReload)
        {
            if (currentGun.currentBulletCount > 0)
                Fire();
        }
    }

    private void Fire()
    {
        if (!isReload)
        {
            if(currentGun.carryBulletCount > 0)
                Shoot();
            else
            {
                CancelFineSight();
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
        // PlaySE(currentGun.fireSound);

        // Hit();   //raycast 방식인데 bullet생성이 더 좋은거라 생각 나중에 삭제해서 통합
        Vector3 v = thecam.transform.position - bulletPos.transform.position;
        var angle = Mathf.Atan2(v.y, v.x) * Mathf.Rad2Deg;

        Physics.Raycast(thecam.transform.position, thecam.transform.forward, out hitinfo);
        bulletPos.LookAt(hitinfo.point);

        GameObject intantBullet = Instantiate(bullet, bulletPos.position, bulletPos.rotation);
        Rigidbody bulletRigid = intantBullet.GetComponent<Rigidbody>();
        bulletRigid.velocity = bulletPos.forward * 80;

        // StopAllCoroutines();
        // StartCoroutine(RetroActionCoroutine());
    }

    private void Hit()
    {
        if (Physics.Raycast(thecam.transform.position, thecam.transform.forward, out hitinfo))
        {
            var hiteffect = Instantiate(hit_effect_prefab, hitinfo.point, Quaternion.LookRotation(hitinfo.normal));
            Destroy(hiteffect,2f);
            Debug.Log(hitinfo);
        }
    }

    private void TryReload()
    {
        if (Input.GetKeyDown(KeyCode.R) && !isReload && currentGun.currentBulletCount < currentGun.reloadBulletCount)
        {
            CancelFineSight();
            StartCoroutine(ReloadCoroutine());
        }
    }

    IEnumerator ReloadCoroutine()
    {
        if (currentGun.carryBulletCount > 0)
        {
            // currentGun.animation.Settrigger("Reload");   // 재장전 애니메이션 호출
            isReload = true;

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

    private void TryFineSight()
    {
        if (Input.GetButtonDown("Fire2") && !isReload)
        {
            FineSight();
        }
    }

    private void CancelFineSight()
    {
        if(isFineSightMode)
            FineSight();
    }

    private void FineSight()
    {
        isFineSightMode = !isFineSightMode;
        //  currentGun.anim.SetBool("FineSightMode", isFineSightMode);

        if (isFineSightMode)
        {
            StopAllCoroutines();
            StartCoroutine(FineSightActivateCoroutine());
        }
        else
        {
            StopAllCoroutines();
            StartCoroutine(FineSightDeactivateCoroutine());
        }
    }

    //정조준 활성화
    IEnumerator FineSightActivateCoroutine()
    {
        while (currentGun.transform.localPosition != currentGun.fineSightOriginPos)
        {
            currentGun.transform.localPosition =
                Vector3.Lerp(currentGun.transform.localPosition, currentGun.fineSightOriginPos, 0.2f);
            yield return null;
        }
    }
    
    //정조준 비활성화
    IEnumerator FineSightDeactivateCoroutine()
    {
        while (currentGun.transform.localPosition != originPos)
        {
            currentGun.transform.localPosition =
                Vector3.Lerp(currentGun.transform.localPosition, originPos, 0.2f);
            yield return null;
        }
    }

    //반동 코루틴
    IEnumerator RetroActionCoroutine()
    {
        Vector3 recoilBack = new Vector3(originPos.x, originPos.y, currentGun.retroActionForce);
        Vector3 retroActionRecoilBack = new Vector3(currentGun.fineSightOriginPos.x, currentGun.fineSightOriginPos.y,
            currentGun.retroActionForce);
        
        if (!isFineSightMode)
        {
            currentGun.transform.localPosition = originPos;
            
            // 반동시작
            while (currentGun.transform.localPosition.z <= currentGun.retroActionForce - 0.02f)
            {
                currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, recoilBack, 0.4f);
                yield return null;
            }
            
            // 원위치로

            while (currentGun.transform.localPosition != originPos)
            {
                currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, originPos, 0.1f);
                yield return null;
            }
        }
        else
        {
            currentGun.transform.localPosition = currentGun.fineSightOriginPos;
            
            // 반동시작
            while (currentGun.transform.localPosition.z <= currentGun.retroActionFineSightForce - 0.02f)
            {
                currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, retroActionRecoilBack, 0.4f);
                yield return null;
            }
            
            // 원위치로

            while (currentGun.transform.localPosition != currentGun.fineSightOriginPos)
            {
                currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, currentGun.fineSightOriginPos, 0.1f);
                yield return null;
            }
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
