using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public float accuracy;
    public float fireRate = 0.1f;  //연사속도
    public float reloadTime = 1f;

    public float damage;
    public int reloadBulletCount ;       // 총알 재장전 갯수
    public int currentBulletCount = 30;      // 현재 탄창의 갯수
    private int maxBulletCount = 120;   // 플레이어의 최대 탄창갯수
    public int carryBulletCount = 120;        // 현재 소유한 탄창 갯수

    public float retroActionForce;  // 반동세기
    public float retroActionFineSightForce; // 정조준시 반동 세기

    public Vector3 fineSightOriginPos;  //조준점 위치
    public ParticleSystem muzzleFlash;  // 총구 섬광
    public AudioClip fireSound; // 총기발사 소리

    void Start()
    {
        
    }

   
    void Update()
    {
        
    }
}
