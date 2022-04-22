using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;
using UnityEngine.VFX;

public class Enemy_test : MonoBehaviour
{
    private const float ENEMY_MAX_HEALTH = 10f;     //좀비의 최대체력
    private const float ENEMY_MOVESPEED = 1.6f;     //좀비의 이동속도
    private const float ENEMY_ZOMBIE_DAMAGE = 10f;  //일반좀비 공격력
    private const float ENEMY_ATTACK_DELAY = 2f;    //좀비의 공격속도

    public float maxhealth = ENEMY_MAX_HEALTH;
    public float currentHealth = ENEMY_MAX_HEALTH;
    public float MoveSpeed = ENEMY_MOVESPEED;
    public float attackDelay = ENEMY_ATTACK_DELAY;

    private Rigidbody _rigid;
    private BoxCollider _boxCollider;   // 좀비의 공격범위
    public GameObject Target;           // 좀비가 공격할 목표
    private PlayerInfo _player;         // 좀비가 가져올 플레이어 정보
    
    // private PathUnit _pathUnit;         // A*길찾기 오류해결하면 navmesh지우고 이걸로 사용
    private NavMeshAgent _nav;
    public ObjectManager _ObjectManager;

    // 부분파괴 테스트용 팔
    public GameObject leftArm;
    public GameObject rightArm;

    public bool testMove = false;
    private Animator _anim;

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
        audioSource = GetComponent<AudioSource>();
        
        //_pathUnit = GetComponent<PathUnit>();          // a* 해결하면 교체
        _nav = GetComponent<NavMeshAgent>();            // a* 해결하면 _navmesh지우고 a*로 교체
        _anim = GetComponentInChildren<Animator>();
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
        if (Target == null)
        {
            int search = 0;
            while (search < 4)
            {
                int ran = Random.Range(0, 4);
                if (_ObjectManager.EnemyTargetBase[ran].state != Base.State.Enemy_Occupation)
                {
                    Target = _ObjectManager.EnemyTargetBase[ran].gameObject;
                    break;
                }
                search++;
            }
            if (search == 4)
                Target = _ObjectManager.Player.gameObject;
            testMove = true;
        }
        
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
        if (currentHealth <= 0)
        {
            DropItem();
            currentHealth = ENEMY_MAX_HEALTH;
            Target = null;
            
            //인성 추가
            audioSource.PlayOneShot(deadSound);
            
            gameObject.SetActive(false);
        }
    }

    private void DropItem()
    {
        int healDrop = Random.Range(0, 100);    // 힐팩 드랍률10%
        int ammoDrop = Random.Range(0, 100);    // 탄약 드랍률20%
        int powerDrop = Random.Range(0, 100);    // 파워게이지 드랍률10%
        int psychoDrop = Random.Range(0, 100);    // 초능력게이지 드랍률20%
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
            Vector3 dir = transform.position-collision.transform.position;
            hitEffect.transform.rotation = Quaternion.LookRotation(dir);
            hitEffect.Play();
            //
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player" && attackDelay < 0)
        {
            if (_player == null)
                _player = other.gameObject.GetComponent<PlayerInfo>();
            
       
            _player.onDamaged= true; // 플레이어 공격 받는 상태 true;
            UIManager.instance.PlayerAttacked();

            StartCoroutine(Attack());
            attackDelay = ENEMY_ATTACK_DELAY * 1.5f;
        }
        else if (other.gameObject.tag == "Base" && attackDelay < 0)
        {
            Base testBaseHealth = other.gameObject.GetComponent<Base>();
            testBaseHealth.baseHealth -= ENEMY_ZOMBIE_DAMAGE;
            
            StartCoroutine(Attack());
            attackDelay = ENEMY_ATTACK_DELAY;
        }
        else if (other.gameObject.tag == "ObstacleWall" && attackDelay < 0)
        {
            ObstacleWall testWallHealth = other.gameObject.GetComponent<ObstacleWall>();
            testWallHealth.WallHealth -= ENEMY_ZOMBIE_DAMAGE;
            
            StartCoroutine(Attack());
            attackDelay = ENEMY_ATTACK_DELAY;
        }
    }

    IEnumerator Attack()
    {
        testMove = false;
        _anim.SetBool("isAttack", true);
        yield return new WaitForSeconds(ENEMY_ATTACK_DELAY);

        RaycastHit _hit;
        if (Physics.Raycast(transform.position + Vector3.up, transform.forward, out _hit, 2f,
                LayerMask.GetMask("Player")))
        {
            _player.currenthealth -= ENEMY_ZOMBIE_DAMAGE;
        }
        
        testMove = true;
        _anim.SetBool("isAttack", false);
    }
   
}
