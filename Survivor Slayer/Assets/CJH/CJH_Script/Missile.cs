using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour
{
    private const float MISSILE_RANGE = 10f;        // 미사일 폭파거리
    private const float MISSILE_DAMAGE = 10f;       // 미사일 데미지
    public Transform _target;
    private float turningForce = 0.8f;
    
    [SerializeField]private float maxspeed = 20f;
    private float accelAmount = 1f;
    [SerializeField]private float lifetime = 10f;
    [SerializeField]private float speed = 10f;
    [SerializeField] private GameObject explosion;
    [SerializeField] private GameObject[] Body;
    private Rigidbody _rigidbody;
    private bool DestroyTrigger;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        Destroy(gameObject, lifetime);
    }

    private void Update()
    {
        if (!DestroyTrigger)
        {
            if (maxspeed > speed)
            {
                speed += accelAmount * Time.deltaTime;
            }

            transform.Translate(new Vector3(0, 0, speed * Time.deltaTime));
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!DestroyTrigger)
        {
            RaycastHit[] rayHits =
                Physics.SphereCastAll(transform.position, MISSILE_RANGE, Vector3.up, 0f, LayerMask.GetMask("Player"));

            foreach (RaycastHit hitObj in rayHits)
            {
                hitObj.transform.GetComponent<PlayerInfo>().HitBomb(MISSILE_DAMAGE);
            }
        }

        DestroyTrigger = true;
        _rigidbody.velocity = Vector3.zero;
        _rigidbody.angularVelocity = Vector3.zero;
        for (int i = 0; i < Body.Length; i++)
        {
            Body[i].SetActive(false);
        }
        explosion.gameObject.SetActive(true);
        Destroy(gameObject, 1.5f);
    }
    
}
