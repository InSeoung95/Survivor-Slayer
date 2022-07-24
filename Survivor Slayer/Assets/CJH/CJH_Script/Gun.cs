using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Gun : MonoBehaviour
{
    public float accuracy;
    public float fireRate = 0.1f;  //연사속도
    public float reloadTime = 1f;
    
    public int[] BULLETSPEED = { 20, 30, 40 };          // 총알 탄속
    public int[] reloadBulletCount = { 30, 45, 60 };       // 총알 재장전 갯수
    public int[] PlasmaBombCount = { 1, 2, 3 };    // 플라즈마 포 사용 가능 횟수
    public int currentBulletCount = 30;      // 현재 탄창의 갯수
    public int maxBulletCount = 120;   // 플레이어의 최대 탄창갯수
    public int carryBulletCount = 120;        // 현재 소유한 탄창 갯수

    public float retroActionForce;  // 반동세기
    public float retroActionFineSightForce; // 정조준시 반동 세기

    public int[] upgradeRate = {0,0,0};       // 데미지, 탄창수, 게이지폭탄   0/12 단계
    
    public Vector3 fineSightOriginPos;  //조준점 위치
    public ParticleSystem muzzleFlash;  // 총구 섬광
    public AudioClip fireSound; // 총기발사 소리
    public AudioClip reloadSound;   //재장전 소리
    public AudioClip chargeSound;   // 플라즈마 폭탄 차징소리
    public AudioClip PlasmaShot;    // 플라즈마 폭탄 발사

    public GameObject PlasmaBomb;
    public GameObject[] _GunPart;
    public Material[] _GunPartMaterials;
    public Material[] _GunChangeMat;
    [SerializeField] private Material _gunMat;
    
    //인성 추가
    public Animator gunAnim;
    public Slider PlasmaUI;

    void Start()
    {
        gunAnim=GetComponent<Animator>();
    }

    public void GunUpgrade(UpgradeType Type)
    {
        if (Type == UpgradeType.Damage)
        {
            if (upgradeRate[0] < 3)
            {
                upgradeRate[0]++;
                var GunMesh = _GunPart[0].gameObject.GetComponent<MeshRenderer>();
                _GunChangeMat[0] = _GunPartMaterials[upgradeRate[0]];
                GunMesh.materials = _GunChangeMat;
                Debug.Log("총업그레이드 1");
            }
        }

        if (Type == UpgradeType.Bullet)
        {
            if (upgradeRate[1] < 3)
            {
                upgradeRate[1]++;
                var GunMesh = _GunPart[1].gameObject.GetComponent<MeshRenderer>();
                _GunChangeMat[0] = _GunPartMaterials[upgradeRate[1]];
                GunMesh.materials = _GunChangeMat;
                Debug.Log("총업그레이드 2");
            }
        }

        if (Type == UpgradeType.GunGage)
        {
            if (upgradeRate[2] < 3)
            {
                upgradeRate[2]++;
                PlasmaUI.maxValue += 20; // 플라즈마 UI 게이지 상승.
                Debug.Log("총업그레이드 3");
            }
        }

    }

    public void PlasmaCount(int num)
    {
        if (num == 1)
        {
            var GunMesh = _GunPart[2].gameObject.GetComponent<MeshRenderer>();
            _GunChangeMat[0] = _GunPartMaterials[0];
            GunMesh.materials = _GunChangeMat;
        }
        else if(num ==2)
        {
            var GunMesh = _GunPart[2].gameObject.GetComponent<MeshRenderer>();
            _GunChangeMat[0] = _GunPartMaterials[1];
            GunMesh.materials = _GunChangeMat;
        }
        else if(num ==3)
        {
            var GunMesh = _GunPart[2].gameObject.GetComponent<MeshRenderer>();
            _GunChangeMat[0] = _GunPartMaterials[2];
            GunMesh.materials = _GunChangeMat;
        }
        else
        {
            var GunMesh = _GunPart[2].gameObject.GetComponent<MeshRenderer>();
            _GunChangeMat[0] = _gunMat;
            GunMesh.materials = _GunChangeMat;
        }
    }
}
