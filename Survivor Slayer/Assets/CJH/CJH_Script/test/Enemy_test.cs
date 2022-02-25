using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Enemy_test : MonoBehaviour
{
    public float maxhealth = 10f;
    public float currentHealth = 10f;

    public float MoveSpeed = 1.6f;
    public float attackDelay = 1f;

    private Rigidbody _rigid;
    private BoxCollider _boxCollider;   // 좀비 공격범위
    public GameObject Target;
    public GameObject[] ItemPrefab;     // 힐팩, 탄약, 파워게이지, 초능력게이지

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
            Destroy(this.gameObject);
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
            var itemGo = Instantiate<GameObject>(this.ItemPrefab[0]);
            itemGo.transform.position = this.gameObject.transform.position + dropPoint;
        }
        if (ammoDrop < 20)
        {
            var itemGo = Instantiate<GameObject>(this.ItemPrefab[1]);
            itemGo.transform.position = this.gameObject.transform.position + dropPoint;
        }
        if (powerDrop < 10)
        {
            var itemGo = Instantiate<GameObject>(this.ItemPrefab[2]);
            itemGo.transform.position = this.gameObject.transform.position + dropPoint;
        }
        if (psychoDrop < 20)
        {
            var itemGo = Instantiate<GameObject>(this.ItemPrefab[3]);
            itemGo.transform.position = this.gameObject.transform.position + dropPoint;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Bullet")
        {
            currentHealth -= 1f;
            Destroy(collision.gameObject, 0.5f);
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
