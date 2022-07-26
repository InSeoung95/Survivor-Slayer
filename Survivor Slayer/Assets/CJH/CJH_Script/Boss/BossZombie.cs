using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.VFX;

public class BossZombie : MonoBehaviour
{
    [SerializeField]private const float ENEMY_HEALTH = 1000;
    [SerializeField] private const float ENEMY_BURSERK = 300;
    private const float ENEMY_MOVESPEED = 5f;     //좀비의 이동속도  1.6f = default
    private const float ENEMY_ZOMBIE_DAMAGE = 10f;  //일반좀비 공격력
    private const float ENEMY_ATTACK_DELAY = 5.0f;    //좀비의 공격속도
    public float _EnemyHealth = ENEMY_HEALTH;
    public float MoveSpeed = ENEMY_MOVESPEED;
    public float attackDelay = ENEMY_ATTACK_DELAY;

    private Rigidbody _rigid;
    private BoxCollider _boxCollider;   // 좀비의 공격범위
    private float _boxColliderSize;
    public GameObject Target;           // 좀비가 공격할 목표
    private PlayerInfo _player;         // 좀비가 가져올 플레이어 정보
    private NavMeshAgent _nav;

    public bool testMove;
    public bool chasePlayer;
    private bool isBurserk;
    public bool isDeath;
    private Animator _anim;

    public VisualEffect hitEffect;   // 좀비 피격 이펙트
    public ParticleSystem hitEffectBlood; 
    public AudioClip BurserkSound;  // 광폭화 사운드
    public AudioClip deadSound;     // 좀비 사망 사운드.

    private AudioSource audioSource;
    [SerializeField] private AudioClip ZombieHowling;     // 좀비가 울어댈 소리

    private EnemySpawn enemySpawn;
    public Material Dissolve;// 좀비 사망 시 소멸 효과.
    public SkinnedMeshRenderer[] Bodys; // 이펙트 적용할 좀비 몸통들.
    
    public SkinnedMeshRenderer _renderer;
    [SerializeField] private Material[] BurserkMaterial;
    private Material[] DefaultMaterial; // 기본 적용된 머테리얼.
    
    private void Start()
    {
        _rigid = GetComponent<Rigidbody>();
        _boxCollider = GetComponent<BoxCollider>();
        _boxColliderSize = _boxCollider.size.z + _boxCollider.center.z;     // 좀비 공격범위(위치+크기)
        audioSource = GetComponent<AudioSource>();
        
        _nav = GetComponent<NavMeshAgent>();         
        _anim = GetComponentInChildren<Animator>();
        
        audioSource.clip = ZombieHowling;
        audioSource.Play();
        audioSource.loop = true;
    }

    private void Update()
    {
        Move();
        attackDelay -= Time.deltaTime;
        Die();
        
    }
    
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            ContactPoint contactPoint = collision.contacts[0];
            hitEffectBlood.transform.position = contactPoint.point;
            hitEffectBlood.transform.rotation = Quaternion.LookRotation(contactPoint.normal);
            hitEffectBlood.Play();

            var bulletDamage = collision.gameObject.GetComponent<Bullet>()
                .Damage[collision.gameObject.GetComponent<Bullet>().UpgradeRate];
            onDamage(bulletDamage * 10);
            
            collision.gameObject.SetActive(false);
        }
    }

    private void onDamage(float Damage)
    {
        _EnemyHealth -= Damage;
        if (_EnemyHealth < ENEMY_BURSERK && !isBurserk)
        {
            isBurserk = true;
            StartCoroutine(Burserk());
        }
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

    public void HitBomb()
    {
        _EnemyHealth -= 50;
    }

    IEnumerator Burserk()
    {
        MoveSpeed *= 1.5f;
        _anim.speed += 0.5f;
        audioSource.PlayOneShot(BurserkSound);

        foreach (var Body in Bodys)
        {
            _renderer = Body.gameObject.GetComponent<SkinnedMeshRenderer>();
            _renderer.materials = BurserkMaterial;
        }
        yield return null;
    }

    private void Die()
    {
        if (_EnemyHealth <= 0)
        {
            _nav.isStopped = true;
            _nav.velocity = Vector3.zero;
            testMove = false;
            isDeath = true;
        }
        if (isDeath)
        {
            isDeath = false;
            StartCoroutine("Death");
        }
    }
    

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && attackDelay < 0)
        {
            if (_player == null)
                _player = other.gameObject.GetComponent<PlayerInfo>();

            StartCoroutine(Attack());
            attackDelay = ENEMY_ATTACK_DELAY;
        }
    }

    IEnumerator Attack()
    {
        testMove = false;
        _nav.isStopped = true;
        _nav.velocity = Vector3.zero;
        
        _anim.SetBool("isAttack", true);
        yield return new WaitForSeconds(ENEMY_ATTACK_DELAY / _anim.speed);

        RaycastHit _hit;
        if (Physics.Raycast(transform.position + Vector3.up, transform.forward, out _hit, _boxColliderSize,
                LayerMask.GetMask("Player")))
        {
            _player.OnDamage(ENEMY_ZOMBIE_DAMAGE);
            _player.onDamaged= true;
            UIManager.instance.PlayerAttacked();
        }
        
        testMove = true;
        _nav.isStopped = false;
        _anim.SetBool("isAttack", false);
       
    }

    IEnumerator Death()
    {
        _anim.SetBool("isDeath", true);
        audioSource.PlayOneShot(deadSound);
        yield return new WaitForSeconds(1.5f);
        foreach(var Body in Bodys) // 소멸효과
        {
            Body.material = Dissolve;
        }
        yield return new WaitForSeconds(1.5f);
        
        _anim.SetBool("isDeath", false);
        chasePlayer = false;
        Target = null;
        
        gameObject.SetActive(false);
    }
}
