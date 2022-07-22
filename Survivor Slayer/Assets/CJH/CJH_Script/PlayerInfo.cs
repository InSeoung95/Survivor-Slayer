using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInfo : MonoBehaviour
{
    public float maxHealth = 100;
    public float currenthealth = 100;
    public float maxPower = 100;
    public float currentPower = 0;
    public float maxPsycho = 100;
    public float currentPsycho = 0;
    
    public float currentBasePoint = 0;      // 플레이어가 가지고있는 거점재화
    //인성 추가
    public int revivalCount;// 부활 가능 횟수
        
    public Slider HPSlider;
    public Text HPText;
    //인성 수정
    public bool onDamaged = false; // 플레이어가 데미지 받는지

  
    //
    [SerializeField] private GameObject _gameObject;
    public GameObject GameObject => _gameObject;

    private void Awake()
    {
        HPSlider.maxValue = maxHealth;
        HPSlider.value = currenthealth;
    }

    private void Update()
    {
        HPSlider.value = currenthealth;
        HPText.text = currenthealth.ToString();
    }

    public void HitBomb(float Bombdamage)
    {
        currenthealth -= Bombdamage;
    }

    public void OnDamage(float damage)
    {
        currenthealth -= damage;
    }
}
