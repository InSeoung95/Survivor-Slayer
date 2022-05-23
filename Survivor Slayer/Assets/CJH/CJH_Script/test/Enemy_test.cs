using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;
using UnityEngine.VFX;

public class Enemy_test : MonoBehaviour
{
    private const float ENEMY_MOVESPEED = 1.6f;     //좀비의 이동속도
    private const float ENEMY_ZOMBIE_DAMAGE = 10f;  //일반좀비 공격력
    private const float ENEMY_ATTACK_DELAY = 2f;    //좀비의 공격속도

    private Enemy_Dest _enemyDest;                  // 좀비 체력 및 부위별 체력관리, 팔/다리 제거인식
    public float MoveSpeed = ENEMY_MOVESPEED;
    public float attackDelay = ENEMY_ATTACK_DELAY;

    private Rigidbody _rigid;
    private BoxCollider _boxCollider;   // 좀비의 공격범위
    private float _boxColliderSize;
    public GameObject Target;           // 좀비가 공격할 목표
    private PlayerInfo _player;         // 좀비가 가져올 플레이어 정보

    [SerializeField]private Material[] _materials;      // 마테리얼 보관용
    private Material[] _ZombieMaterial;                 // 좀비 마테리얼 확인용
    
    // private PathUnit _pathUnit;         // A*길찾기 오류해결하면 navmesh지우고 이걸로 사용
    private NavMeshAgent _nav;
    public ObjectManager _ObjectManager;

    public bool testMove = false;
    public bool chasePlayer = false;
    public bool isDeath = false;
    private Animator _anim;
    private float animSpeed;                 // 좀비별 애니메이션 스피드

    // 인성 수정
    public VisualEffect hitEffect;   // 좀비 피격 이펙트
    public ParticleSystem hitEffect2;// 피격 피격 파티클 
    public AudioClip deadSound;// 좀비 사망 사운드.

    private AudioSource audioSource;
    private EnemySpawn enemySpawn;
    //

    private void Start()
    {
        _ZombieMaterial = GetComponentInChildren<SkinnedMeshRenderer>().materials;
        var i = Random.Range(0, 100);
        if (i < 50)
        {
            _ZombieMaterial[0] = _materials[1];     // 투명화로 교체
            GetComponentInChildren<SkinnedMeshRenderer>().materials = _ZombieMaterial;
        }
        else
        {
            _ZombieMaterial[0] = _materials[0];     // 원본으로 교체
            GetComponentInChildren<SkinnedMeshRenderer>().materials = _ZombieMaterial;
        }

        _enemyDest = GetComponent<Enemy_Dest>();
        _rigid = GetComponent<Rigidbody>();
        _boxCollider = GetComponent<BoxCollider>();
        _boxColliderSize = _boxCollider.size.z + _boxCollider.center.z;     // 좀비 공격범위(위치+크기)
        _ObjectManager = GameObject.Find("ObjectManager").GetComponent<ObjectManager>();
        audioSource = GetComponent<AudioSource>();
        
        //_pathUnit = GetComponent<PathUnit>();          // a* 해결하면 교체
        _nav = GetComponent<NavMeshAgent>();            // a* 해결하면 _navmesh지우고 a*로 교체
        _anim = GetComponentInChildren<Animator>();
        
        animSpeed = Random.Range(10, 25+1) * 0.1f;
        _anim.speed = animSpeed;
        
        //인성 추가
        enemySpawn = FindObjectOfType<EnemySpawn>();
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
        if (Target == null && !chasePlayer)
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
            {
                Target = _ObjectManager.Player.gameObject;
                chasePlayer = true;
            }

            _nav.isStopped = false;
            _nav.speed = MoveSpeed;
            testMove = true;
        }
        
        if (testMove)
        {
            Vector3 dir = Target.transform.position - transform.position;
            dir.y = 0;
            dir.Normalize();
            
            _nav.SetDestination(Target.transform.position);
            if(_enemyDest.isLeg)
                _anim.SetBool("isCrawl",true);
            _anim.SetBool("isRun", testMove);
            
            Quaternion to = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, to, 1);
            
        }
    }

    public void HitBomb()
    {
        _enemyDest.currentHealth -=5;
    }

    private void Die()
    {
        if ( _enemyDest.currentHealth <= 0)
        {
            _enemyDest.currentHealth =  _enemyDest.maxhealth;
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

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Bullet")
        {
            //인성 수정
            
            ContactPoint contactPoint = collision.contacts[0];
            hitEffect2.transform.position = contactPoint.point;
            hitEffect2.transform.rotation = Quaternion.LookRotation(contactPoint.normal);
            hitEffect2.Play();

            /*
            hitEffect.transform.position = contactPoint.point;
            hitEffect.transform.rotation = Quaternion.LookRotation(contactPoint.normal);
            hitEffect.Play();
             */
            //
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player" && attackDelay < 0)
        {
            if (_player == null)
                _player = other.gameObject.GetComponent<PlayerInfo>();

            StartCoroutine(Attack());
            attackDelay = ENEMY_ATTACK_DELAY * 1.5f;
        }
        else if (other.gameObject.tag == "Base" && attackDelay < 0)
        {
            Base testBaseHealth = other.gameObject.GetComponentInParent<Base>();
            if (testBaseHealth.state != Base.State.Enemy_Occupation)
            {
                testBaseHealth.baseHealth -= ENEMY_ZOMBIE_DAMAGE;
                StartCoroutine(Attack());
                attackDelay = ENEMY_ATTACK_DELAY * 1.5f;
            }
            else if (Target == other.gameObject.transform.parent.gameObject && !chasePlayer)       
            {
                //Base 상태가 Enemy_occupation일때 목적지가 base와 같을경우
                Target = null;
            }
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
        _nav.isStopped = true;
        _nav.velocity = Vector3.zero;
        if (_enemyDest.isArm)
        {
            _anim.SetBool("isHeadbutt", true);
        }
        else
        {
            _anim.SetBool("isAttack", true);
        }

        yield return new WaitForSeconds(ENEMY_ATTACK_DELAY / animSpeed);

        RaycastHit _hit;
        if (Physics.Raycast(transform.position + Vector3.up, transform.forward, out _hit, _boxColliderSize,
                LayerMask.GetMask("Player")))
        {
            _player.currenthealth -= ENEMY_ZOMBIE_DAMAGE;
            _player.onDamaged= true; // 플레이어 공격 받는 상태 true;
            UIManager.instance.PlayerAttacked();
        }
        
        testMove = true;
        _nav.isStopped = false;
        _anim.SetBool("isHeadbutt", false);
        _anim.SetBool("isAttack", false);
       
    }

    IEnumerator Death()
    {
        _anim.SetBool("isDeath", true);
        audioSource.PlayOneShot(deadSound);
        yield return new WaitForSeconds(3f / animSpeed);
        
        DropItem();
        _anim.SetBool("isDeath", false);
        
        chasePlayer = false;
        Target = null;
        gameObject.SetActive(false);
        
    }
   
}
