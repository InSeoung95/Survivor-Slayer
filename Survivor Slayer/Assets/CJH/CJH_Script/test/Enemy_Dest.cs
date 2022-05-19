using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Dest : MonoBehaviour
{
    private const float ENEMY_MAX_HEALTH = 100f;     //좀비의 최대체력

    public float maxhealth = ENEMY_MAX_HEALTH;
    public float currentHealth = ENEMY_MAX_HEALTH;
    [SerializeField] private Enemy_Body[] _body;
    
   
    void Update()
    {
        OnDamaged();
    }

    private void OnDamaged()
    {
        foreach (var Body in _body)
        {
            if (Body.onDamaged)
            {
                currentHealth -= Body._bodyDamage;
                Body.DamageCount++;
                Body.onDamaged = false;
                
                Body.DestroyCount();
            }
        }
    }
}
