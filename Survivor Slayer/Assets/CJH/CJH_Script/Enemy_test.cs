using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_test : MonoBehaviour
{
    public float maxhealth = 10f;
    public float currentHealth = 10f;

    private Rigidbody rigid;
    private BoxCollider boxCollider;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        boxCollider = GetComponent<BoxCollider>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Bullet")
        {
            currentHealth -= 1f;
            Destroy(collision.gameObject, 0.5f);
        }
    }
}
