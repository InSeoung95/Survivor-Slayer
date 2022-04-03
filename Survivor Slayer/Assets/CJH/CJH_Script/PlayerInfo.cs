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
        
    public Slider HPSlider;
    public Text HPText;
    //?몄꽦 異붽?
    public bool onDamaged = false; // ?뚮젅?댁뼱媛 ?곕?吏 諛쏅뒗 ?곹깭?몄?
    //
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
