using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Enemy_test : MonoBehaviour
{
    private const float ENEMY_MAX_HEALTH = 10f;     //좀비 최대 체력
    private const float ENEMY_MOVESPEED = 1.6f;     //좀비 이동속도
    private const float ENEMY_ATTACK_DELAY = 1f;    //좀비 공격속도

    public float maxhealth = ENEMY_MAX_HEALTH;
    public float currentHealth = ENEMY_MAX_HEALTH;

    public float MoveSpeed = ENEMY_MOVESPEED;
    public float attackDelay = ENEMY_ATTACK_DELAY;

    private Rigidbody _rigid;
    private BoxCollider _boxCollider;   // 좀비 공격범위
    public GameObject Target;           // 좀비가 이동할 타겟
    public ObjectManager _ObjectManager;

    // 부위파괴 테스트용 왼팔 오른팔
    public GameObject leftArm;
    public GameObject rightArm;

    public bool testMove = false;

    private void Start()
    {
        _rigid = GetComponent<Rigidbody>();
        _boxCollider = GetComponent<BoxCollider>();
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

            // transform.position += dir * MoveSpeed * Time.deltaTime;
            _rigid.MovePosition(transform.position + (transform.forward * MoveSpeed * Time.deltaTime));

            Quaternion to = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, to, 1);

        }
    }

    private void Die()
    {
        if (currentHealth <= 0)
        {
            DropItem();
            gameObject.SetActive(false);
        }
    }

    private void DropItem()
    {
        int healDrop = Random.Range(0, 100);    // 힐팩 드랍률 10%
        int ammoDrop = Random.Range(0, 100);    // 탄약 드랍률 20%
        int powerDrop = Random.Range(0, 100);    // 파워게이지 드랍률 10%
        int psychoDrop = Random.Range(0, 100);    // 초능력게이지 드랍률 20%
        var dropPoint = Vector3.up * 3;
        if (healDrop < 10)
        {
            var itemposition = this.gameObject.transform.position + dropPoint;
            var itemGo = _ObjectManager.MakeObj("Item_HealPack", itemposition, Quaternion.identity);
        }
        if (ammoDrop < 20)
        {
            var itemposition = this.gameObject.transform.position + dropPoint;
            var itemGo = _ObjectManager.MakeObj("Item_Ammo", itemposition, Quaternion.identity);
        }
        if (powerDrop < 10)
        {
            var itemposition = this.gameObject.transform.position + dropPoint;
            var itemGo = _ObjectManager.MakeObj("Item_PowerGage", itemposition, Quaternion.identity);
        }
        if (psychoDrop < 20)
        {
            var itemposition = this.gameObject.transform.position + dropPoint;
            var itemGo = _ObjectManager.MakeObj("Item_Psycho", itemposition, Quaternion.identity);
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
