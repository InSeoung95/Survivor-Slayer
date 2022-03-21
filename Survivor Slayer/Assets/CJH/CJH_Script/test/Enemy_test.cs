using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Enemy_test : MonoBehaviour
{
    private const float ENEMY_MAX_HEALTH = 10f;     //醫鍮?理쒕? 泥대젰
    private const float ENEMY_MOVESPEED = 1.6f;     //醫鍮??대룞?띾룄
    private const float ENEMY_ATTACK_DELAY = 1f;    //醫鍮?怨듦꺽?띾룄

    public float maxhealth = ENEMY_MAX_HEALTH;
    public float currentHealth = ENEMY_MAX_HEALTH;

    public float MoveSpeed = ENEMY_MOVESPEED;
    public float attackDelay = ENEMY_ATTACK_DELAY;

    private Rigidbody _rigid;
    private BoxCollider _boxCollider;   // 醫鍮?怨듦꺽踰붿쐞
    public GameObject Target;           // 醫鍮꾧? ?대룞???寃?
    private PathUnit _pathUnit;
    public ObjectManager _ObjectManager;

    // 遺?꾪뙆愿??뚯뒪?몄슜 ?쇳뙏 ?ㅻⅨ??
    public GameObject leftArm;
    public GameObject rightArm;

    public bool testMove = false;

    private void Start()
    {
        _rigid = GetComponent<Rigidbody>();
        _boxCollider = GetComponent<BoxCollider>();
        _ObjectManager = GameObject.Find("ObjectManager").GetComponent<ObjectManager>();
        _pathUnit = GetComponent<PathUnit>();
    }

    private void Update()
    {
        Move();
        //Attack
        attackDelay -= Time.deltaTime;
        Die();
    }

    private void Move()
    {
        if (testMove)
        {
            Vector3 dir = Target.transform.position - transform.position;
            dir.y = 0;
            dir.Normalize();
            
            _pathUnit.Targetting(Target);

            Quaternion to = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, to, 1);

        }
    }

    private void Die()
    {
        if (currentHealth <= 0)
        {
            DropItem();
            currentHealth = ENEMY_MAX_HEALTH;
            gameObject.SetActive(false);
        }
    }

    private void DropItem()
    {
        int healDrop = Random.Range(0, 100);    // ?먰뙥 ?쒕엻瑜?10%
        int ammoDrop = Random.Range(0, 100);    // ?꾩빟 ?쒕엻瑜?20%
        int powerDrop = Random.Range(0, 100);    // ?뚯썙寃뚯씠吏 ?쒕엻瑜?10%
        int psychoDrop = Random.Range(0, 100);    // 珥덈뒫?κ쾶?댁? ?쒕엻瑜?20%
        var dropPoint = Vector3.up * 1;
        if (healDrop < 10)
        {
            var itemposition = this.gameObject.transform.position + dropPoint;
            _ObjectManager.MakeObj("Item_HealPack", itemposition, Quaternion.identity);
        }
        if (ammoDrop < 20)
        {
            var itemposition = this.gameObject.transform.position + dropPoint;
            _ObjectManager.MakeObj("Item_Ammo", itemposition, Quaternion.identity);
        }
        if (powerDrop < 10)
        {
            var itemposition = this.gameObject.transform.position + dropPoint;
            _ObjectManager.MakeObj("Item_PowerGage", itemposition, Quaternion.identity);
        }
        if (psychoDrop < 20)
        {
            var itemposition = this.gameObject.transform.position + dropPoint;
            _ObjectManager.MakeObj("Item_Psycho", itemposition, Quaternion.identity);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Bullet")
        {
            currentHealth -= 1f;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player" && attackDelay < 0)
        {
            //attack test
            PlayerInfo testhealth = other.gameObject.GetComponent<PlayerInfo>();
            testhealth.currenthealth -= 10f;
            //인성 수정
            testhealth.onDamaged= true; // 플레이어 공격 받는 상태 true;
            HitManager hm = FindObjectOfType<HitManager>();
            hm.Attacked();
            //
            attackDelay = 1f;
        }
        else if (other.gameObject.tag == "Base" && attackDelay < 0)
        {
            Base testBaseHealth = other.gameObject.GetComponent<Base>();
            testBaseHealth.baseHealth -= 10f;
            attackDelay = 1f;
        }
    }

   
}
