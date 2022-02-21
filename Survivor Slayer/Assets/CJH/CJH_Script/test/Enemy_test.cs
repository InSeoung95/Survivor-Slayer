using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_test : MonoBehaviour
{
    public float maxhealth = 10f;
    public float currentHealth = 10f;

    public float MoveSpeed = 1.6f;
    public float attackDelay = 1f;

    private Rigidbody _rigid;
    private BoxCollider _boxCollider;   // 좀비 공격범위
    public GameObject Target;

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
        if(currentHealth <= 0)
            Destroy(this.gameObject);
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
            PlayerHealth testhealth = other.gameObject.GetComponent<PlayerHealth>();
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
