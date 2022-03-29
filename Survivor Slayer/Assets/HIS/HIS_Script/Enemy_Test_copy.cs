using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.VFX;

public class Enemy_Test_copy : MonoBehaviour
{
    private const float ENEMY_MAX_HEALTH = 10f;     //??궰??嶺뚣끉裕? 嶺뚳퐢???
    private const float ENEMY_MOVESPEED = 1.6f;     //??궰?????????쒖┣
    private const float ENEMY_ATTACK_DELAY = 1f;    //??궰????ㅻ?????쒖┣

    public float maxhealth = ENEMY_MAX_HEALTH;
    public float currentHealth = ENEMY_MAX_HEALTH;

    public float MoveSpeed = ENEMY_MOVESPEED;
    public float attackDelay = ENEMY_ATTACK_DELAY;

    private Rigidbody _rigid;
    private BoxCollider _boxCollider;   // ??궰????ㅻ??곗띁由곈겫?곕쭊
    public GameObject Target;           // ??궰???? ??????????
    private PathUnit _pathUnit;
    public ObjectManager _ObjectManager;

    // ?遊붋?熬곥굥??????裕?筌뤾쑴?????몄냻 ???섎??
    public GameObject leftArm;
    public GameObject rightArm;

    public bool testMove = false;

    // 인성 수정
    public VisualEffect hitEffect;   // 좀비 피격 이펙트

    public AudioClip deadSound;// 좀비 사망 사운드.

    private AudioSource audioSource;
    //

    private void Start()
    {
        _rigid = GetComponent<Rigidbody>();
        _boxCollider = GetComponent<BoxCollider>();
        _ObjectManager = GameObject.Find("ObjectManager").GetComponent<ObjectManager>();
        _pathUnit = GetComponent<PathUnit>();

        //인성 수정
        hitEffect = GetComponentInChildren<VisualEffect>();
        audioSource = GetComponent<AudioSource>();
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

            //인성 추가
            audioSource.PlayOneShot(deadSound);
        }
    }

    private void DropItem()
    {
        int healDrop = Random.Range(0, 100);    // ??믨퀡????類ㅻ옐??10%
        int ammoDrop = Random.Range(0, 100);    // ?熬곣뫖????類ㅻ옐??20%
        int powerDrop = Random.Range(0, 100);    // ????뽭뇦猿딆뒩?醫묒?? ??類ㅻ옐??10%
        int psychoDrop = Random.Range(0, 100);    // ?貫???鰲???? ??類ㅻ옐??20%
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
            //인성 수정
            hitEffect.transform.position = collision.transform.position;
            Vector3 dir = transform.position - collision.transform.position;
            hitEffect.transform.rotation = Quaternion.LookRotation(dir);
            hitEffect.Play();
            //
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player" && attackDelay < 0)
        {
            //attack test
            PlayerInfo testhealth = other.gameObject.GetComponent<PlayerInfo>();
            testhealth.currenthealth -= 10f;
            //?紐꾧쉐 ??륁젟
            testhealth.onDamaged = true; // ???쟿??곷선 ?⑤벀爰?獄쏆룆???怨밴묶 true;
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
