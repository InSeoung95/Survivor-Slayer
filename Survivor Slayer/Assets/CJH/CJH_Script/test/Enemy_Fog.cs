using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;
using UnityEngine.VFX;

public class Enemy_Fog : MonoBehaviour
{
    private const float ENEMY_MAX_HEALTH = 100f;     //좀비의 최대체력
    private const float ENEMY_MOVESPEED = 5f;     //좀비의 이동속도  1.6f = default

    private float _enemyHealth = ENEMY_MAX_HEALTH ;
    public float MoveSpeed = ENEMY_MOVESPEED;

    private Rigidbody _rigid;
    public GameObject Target;           // 좀비가 공격할 목표
    
    private NavMeshAgent _nav;
    public ObjectManager _ObjectManager;
    [SerializeField] private GameObject Fog;

    [SerializeField] private float FogBombDamage = 10f;
    private float FogTimer;
    public float FogTime = 10f;
    private float FogBombTimer;
    private float FogBombTime = 10f;
    private bool FogTrigger;
    private bool FogBombTrigger;

    public bool testMove = false;
    public bool chasePlayer = false;
    public bool isDeath = false;
    private Animator _anim;

    public VisualEffect hitEffect;   // 좀비 피격 이펙트
    public AudioClip deadSound;     // 좀비 사망 사운드.

    private AudioSource audioSource;
    private EnemySpawn enemySpawn;

    //인성 추가
    public Material ScreenEffect; // 화면에 효과를 줄 머테리얼
    public float ScreenEffctTime; // 화면 효과 지속 시간.

    private void Start()
    {
        _rigid = GetComponent<Rigidbody>();
        _ObjectManager = GameObject.Find("ObjectManager").GetComponent<ObjectManager>();
        audioSource = GetComponent<AudioSource>();
        _nav = GetComponent<NavMeshAgent>();         
        _anim = GetComponentInChildren<Animator>();
        
        enemySpawn = FindObjectOfType<EnemySpawn>();

        Target = _ObjectManager.Player.gameObject;
        testMove = true;
    }
    
    private void Update()
    {
        Move();
        FogTimeUpdate();
        Die();
    }
    
    private void Move()
    {

        if (testMove)
        {
            Vector3 dir = Target.transform.position - transform.position;
            dir.y = 0;
            dir.Normalize();
            
            _nav.SetDestination(Target.transform.position);
           
            _anim.SetBool("isRun", testMove);
            
            Quaternion to = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, to, 1);
            
        }
    }
    
    private void Die()
    {
        if ( _enemyHealth <= 0)
        {
            _enemyHealth = ENEMY_MAX_HEALTH ;
            _nav.isStopped = true;
            _nav.velocity = Vector3.zero;
            testMove = false;
            isDeath = true;
            UIManager.instance.CurrentEnemyNum--;
        }

        if (isDeath)
        {
            isDeath = false;
            StartCoroutine("Death");
        }
    }
    
    private void DropItem()
    {
        int healDrop = Random.Range(0, 100);    // 힐팩 드랍률10%
        int ammoDrop = Random.Range(0, 100);    // 탄약 드랍률20%
        //int powerDrop = Random.Range(0, 100);    // 파워게이지 드랍률10%
        //int psychoDrop = Random.Range(0, 100);    // 초능력게이지 드랍률20%
        var dropPoint = Vector3.up * 1;
        //var dropPoint = new Vector3(Random.Range(-1, 1),1, 0);
        
        if (healDrop < 100)
        {
            var itemposition = this.gameObject.transform.position + dropPoint;
            _ObjectManager.MakeObj("Item_HealPack", itemposition, Quaternion.identity);
        }
        if (ammoDrop < 100)
        {
            var itemposition = this.gameObject.transform.position + dropPoint + Vector3.right;
            _ObjectManager.MakeObj("Item_Ammo", itemposition, Quaternion.identity);
        }
        /*
        if (powerDrop < 100)
        {
            var itemposition = this.gameObject.transform.position + dropPoint - Vector3.right;
            _ObjectManager.MakeObj("Item_PowerGage", itemposition, Quaternion.identity);
        }
        if (psychoDrop < 0)
        {
            var itemposition = this.gameObject.transform.position + dropPoint;
            _ObjectManager.MakeObj("Item_Psycho", itemposition, Quaternion.identity);
        }
         */
    }
    
    public void HitBomb()
    {
        _enemyHealth -=5;
    }

    private void FogBomb()
    {
        RaycastHit[] rayHits =
            Physics.SphereCastAll(transform.position, 10f, Vector3.up, 0f, LayerMask.GetMask("Player"));
        foreach (RaycastHit hitObj in rayHits)
        {
            hitObj.transform.GetComponent<PlayerInfo>().HitBomb(FogBombDamage);
            hitObj.transform.GetComponent<PlayerInfo>().StartCoroutine("ScreenPollution");
        }
        Debug.Log("포그 좀비 효과");

        gameObject.SetActive(false);
        
       
    }
   
    private void FogTimeUpdate()
    {
        FogTimer += Time.deltaTime;
        
        if (FogTrigger)
        {
            if (FogTimer >= FogTime)
            {
                var fog =Instantiate(Fog, gameObject.transform.position, gameObject.transform.rotation);
                Destroy(fog, 3f);
                FogTimer = 0;
            }
        }
        
        if (FogBombTrigger)
        {
            FogBombTimer += Time.deltaTime;
            if (FogBombTimer >= FogBombTime)
            {
                FogBombTrigger = false;
                FogBomb();
            }
        }
    }
    
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Bullet")
        {
            ContactPoint contactPoint = collision.contacts[0];
            hitEffect.transform.position = contactPoint.point;
            hitEffect.transform.rotation = Quaternion.LookRotation(contactPoint.normal);
            hitEffect.Play();

            var damage = collision.gameObject.GetComponent<Bullet>()
                .Damage[collision.gameObject.GetComponent<Bullet>().UpgradeRate];
            _enemyHealth -= 15f * damage;
            
            collision.gameObject.SetActive(false);
            
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            FogTrigger = true;
            FogBombTrigger = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            FogTrigger = false;
            FogBombTrigger = false;
        }
    }

    IEnumerator Death()
    {
        _anim.SetBool("isDeath", true);
        audioSource.PlayOneShot(deadSound);
        yield return new WaitForSeconds(3f);
        
        DropItem();
        _anim.SetBool("isDeath", false);
        
        chasePlayer = false;
        Target = null;
        gameObject.SetActive(false);
        
    }
}
