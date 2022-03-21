using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player_Info : MonoBehaviour
{
    public float maxHealth = 100;
    public float currenthealth = 100;
    public float maxPower = 100;
    public float currentPower = 0;
    public float maxPsycho = 100;
    public float currentPsycho = 0;
    //인성 수정
    private bool onDamaged = false; // 플레이어가 공격받는 상태인지.

    public float currentBasePoint = 0;      // ?ы솕???쒖젙?놁씠 怨꾩냽 ?볦뿬???ъ슜?섎뒗 ?먯썝

    public Slider HPSlider;
    public Text HPText;

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
}
