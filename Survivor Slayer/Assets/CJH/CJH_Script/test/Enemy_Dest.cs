using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Dest : MonoBehaviour
{
    private const float ENEMY_MAX_HEALTH = 100f;     //좀비의 최대체력

    public float maxhealth = ENEMY_MAX_HEALTH;
    public float currentHealth = ENEMY_MAX_HEALTH;
    [SerializeField] private Enemy_Body[] _body;

    public bool isArm;
    public bool isLeg;
    public bool isBurserk;
    //인성 추가
    KillAniEnemyData killAnidata;
    private Animator anim;
    private Enemy_test enemy_Test;

    private void Start()
    {
        killAnidata = GetComponent<KillAniEnemyData>();
        anim = GetComponentInChildren<Animator>();
        enemy_Test = GetComponent<Enemy_test>();
    }

    private void OnEnable()
    {
        isArm = false;
        isLeg = false;
        isBurserk = false;
        foreach (var Body in _body)
        {
            Body.gameObject.SetActive(true);
        }
    }

    void Update()
    {
        OnDamaged();
        ChangeBurserk();
    }

    private void OnDamaged()
    {
        foreach (var Body in _body)
        {
            if (Body.onDamaged)
            {
                currentHealth -= Body._bodyDamage * Body.BulletDamage;
                Body._bodyHitPoint += Body._bodyDamage * Body.BulletDamage; 
                Body.onDamaged = false;

                if(Body._bodyHitPoint >= Body._bodyMaxHealth)
                {
                    // 부위별 피격데미지가 부위별 최대체력 이상 이면 실행되어 파괴된 부위이름을 받아 파괴된것 체크하고 activeFalse

                    //StartCoroutine(Groggy());
                    
                    //enemy_Test.testMove = false;
                    switch (Body._BodyName)
                    {
                        case Enemy_Body.BodyName.Arm :
                            {
                                isArm = true;
                                killAnidata.isGroggy = true;// 적 킬애니 데이터에 Groggy상태라는 것을 true로.
                            }
                            
                            break;
                        case Enemy_Body.BodyName.Leg :
                            {
                                isLeg = true;
                                killAnidata.isGroggy = true;
                                killAnidata.isCrawl = true;// 적 킬애니 데이터에 누운 상태라는 것을 true로 알려줌.
                            }
                            break;
                        case Enemy_Body.BodyName.Other :
                            isBurserk = true;
                            break;
                    }
                    
                    Body.DestroyCount();
                }
                
            }
        }
    }
    /*
    IEnumerator Groggy()
    {
        //killAnidata.isGroggy = true; // 좀비 그로기 상태 bool 값 true
        //anim.SetBool("RunGroggy", true); // 런그로기 애니메이션 true;
        //anim.speed = 0f; // 이게 일시정지인가?
        //Debug.Log("그로기 상태 On");
        //yield return new WaitForSeconds(killAnidata.GroggyTime);//그로기 시작만큼 멈춤
        //anim.speed = 1f; // 다시 애니메이션 시작.
    }
     */
    private void ChangeBurserk()
    {
        if (isBurserk)
        {
            foreach (var Body in _body)
                Body.GetBurserk();
        }
    }
}
