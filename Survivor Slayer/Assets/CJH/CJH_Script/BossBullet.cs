using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBullet : MonoBehaviour
{
    public Transform BulletPoint;       // 보스 총알 목적지
    [SerializeField] private float BulletSpd;
    [SerializeField] private float Size;

    private void Start()
    {
        gameObject.transform.localScale = Vector3.one * Size;
    }

    private void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, BulletPoint.position, BulletSpd);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            Destroy(gameObject);
        }
        else if (other.gameObject.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }
        else if(other.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }
}
