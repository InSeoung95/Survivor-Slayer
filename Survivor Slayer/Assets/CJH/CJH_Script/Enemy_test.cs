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
    private BoxCollider _boxCollider;   // 좀비 몸체
    private SphereCollider _sphereCollider; // 좀비 인식범위
    private GameObject Target;

    // 부위파괴 테스트용 왼팔 오른팔
    public GameObject leftArm;
    public GameObject rightArm;

    public bool testMove = false;

    private void Start()
    {
        _rigid = GetComponent<Rigidbody>();
        _boxCollider = GetComponent<BoxCollider>();
        _sphereCollider = GetComponent<SphereCollider>();
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
            dir.Normalize();
            
            Debug.Log(dir);

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
        
        else if (collision.gameObject.tag == "Player" && attackDelay < 0)
        {
            //attack test
            PlayerHealth testhealth = collision.gameObject.GetComponent<PlayerHealth>();
            testhealth.currenthealth -= 10f;
            attackDelay = 1f;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Target = other.gameObject;
            testMove = true;
            _sphereCollider.radius = 20f;  // 플레이어 인식 -> 플레이어 추적범위 : 플레이어가 추적범위(20f) 벗어나면 다시 인식(10f)으로
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            testMove = false;
            _sphereCollider.radius = 10f;
        }
    }
}
