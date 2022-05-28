using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Body : MonoBehaviour
{
    public enum BodyName
    {
        Head,
        Body,
        Arm,
        Leg,
        Other
    }
    
    public float _bodyHealth;                        // 부위별 최대체력
    public float _bodyDamage;                       // 부위별(머리, 몸통, 팔다리) 데미지
    public bool onDamaged;                          // 피격 판정
    public BodyName _BodyName;                      // 부위 인식용 태그
    public int DamageMaxCount;
    public int DamageCount = 0;                    // n회 이상 피격시 부위파괴되는것 카운트
    
    public ParticleSystem hitEffectBlood;               // 피격 피격 파티클 
    [SerializeField] private GameObject effect;     // 부위 파괴시 나타나는 이펙트
    [SerializeField] private Transform effectTransform; //보이는 몸체의 위치와 위치의 position이 안맞아서 따로 찾기

    [SerializeField] private GameObject _JacketBody;        // 좀비 갑옷파괴용(갑옷파괴시 몸통의 콜라이더를 active)

    public SkinnedMeshRenderer _renderer;
    [SerializeField] private Material[] ZombieMaterial;
    [SerializeField] private Material[] BurserkMaterial;

    private void Start()
    {
        _renderer = GetComponent<SkinnedMeshRenderer>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Bullet")
        {
            ContactPoint contactPoint = collision.contacts[0];
            hitEffectBlood.transform.position = contactPoint.point;
            hitEffectBlood.transform.rotation = Quaternion.LookRotation(contactPoint.normal);
            hitEffectBlood.Play();
            
            onDamaged = true;
            collision.gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Bullet")
        {

            hitEffectBlood.transform.position = other.transform.position;
            hitEffectBlood.transform.rotation = Quaternion.LookRotation(other.transform.up);
            hitEffectBlood.Play();
            
            onDamaged = true;
            other.gameObject.SetActive(false);
        }
    }

    public void DestroyCount()
    {
        if (effect)
        {
            effect.gameObject.transform.position = effectTransform.position;
            effect.gameObject.SetActive(true);
        }

        DamageCount = 0;
        gameObject.SetActive(false);

        // 갑옷파괴시 몸통콜라이더 active하는것
        if (_JacketBody != null)
        {
            Collider _collider = _JacketBody.GetComponent<Collider>();
            _collider.enabled = true;
        }

    }

    public void GetBurserk()
    {
        _renderer.materials = BurserkMaterial;
    }

}
