using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using UnityEngine.VFX;
using Random = UnityEngine.Random;

public class BossZombie : MonoBehaviour
{
    [SerializeField]private const float ENEMY_HEALTH = 1000;
    [SerializeField] private const float ENEMY_BURSERK = 300;
    private const float ENEMY_MOVESPEED = 5f;     //좀비의 이동속도  1.6f = default
    private const float ENEMY_ZOMBIE_DAMAGE = 10f;  //일반좀비 공격력
    private const float ENEMY_ATTACK_DELAY = 5.0f;    //좀비의 공격속도
    private const float ENEMY_ATTACK_ANITIME = 1.8f;     // 좀비 공격 애니메이션 타임
    public float _EnemyHealth = ENEMY_HEALTH;
    public float MoveSpeed = ENEMY_MOVESPEED;
    public float attackDelay = ENEMY_ATTACK_DELAY;

    private Rigidbody _rigid;
    private BoxCollider _boxCollider;   // 좀비의 공격범위
    private bool AttackIn;
    private bool isAttacked;
    private float _boxColliderSize;
    public GameObject Target;           // 좀비가 공격할 목표
    private PlayerInfo _player;         // 좀비가 가져올 플레이어 정보
    private NavMeshAgent _nav;
    [SerializeField] private NavAround _navAround;

    public bool testMove;
    public bool chasePlayer;
    private bool isBurserk;
    private bool isPlasma;
    public bool isDeath;
    private Animator _anim;

    public VisualEffect hitEffect;   // 좀비 피격 이펙트
    public ParticleSystem hitEffectBlood; 
    public AudioClip BurserkSound;  // 광폭화 사운드
    public AudioClip deadSound;     // 좀비 사망 사운드.
    public AudioClip FirstJumpSound;    // 보스좀비 첫점프시 howling
    public AudioClip[] ZombieAttackSound;       // 보스좀비 공격소리

    private AudioSource audioSource;
    [SerializeField] private AudioClip ZombieHowling;     // 좀비가 울어댈 소리

    private EnemySpawn enemySpawn;
    public Material Dissolve;// 좀비 사망 시 소멸 효과.
    public SkinnedMeshRenderer[] Bodys; // 이펙트 적용할 좀비 몸통들.
    
    public SkinnedMeshRenderer _renderer;
    [SerializeField] private Material[] BurserkMaterial;
    private Material[] DefaultMaterial; // 기본 적용된 머테리얼.

    [SerializeField] private ParticleSystem[] JumpEffect;           // 점프시 충격파 이펙트
    private bool isJumpping;
    
    public Slider FinalBossHP_Bar;
    public GameObject _3rdCanvas;
    [SerializeField] private GameObject[] _UI;
    

    //인성 추가
    public InteractDoor door;
    
    private void Start()
    {
        _rigid = GetComponent<Rigidbody>();
        _boxCollider = GetComponent<BoxCollider>();
        _boxColliderSize = _boxCollider.size.z + _boxCollider.center.z;     // 좀비 공격범위(위치+크기)
        audioSource = GetComponent<AudioSource>();
        
        _nav = GetComponent<NavMeshAgent>();         
        _anim = GetComponentInChildren<Animator>();
        
        FinalBossHP_Bar.maxValue = ENEMY_HEALTH;
        FinalBossHP_Bar.value = _EnemyHealth;

        // audioSource.clip = ZombieHowling;
        // audioSource.Play();
        // audioSource.loop = true;
        
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
            
            FinalBossHP_Bar.value = _EnemyHealth;
            collision.gameObject.SetActive(false);
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && attackDelay < 0)
        {
            if (_player == null)
                _player = other.gameObject.GetComponent<PlayerInfo>();

            var attackType = Random.Range(0, 1 + 1);

            if (attackType == 0)
            {
                StartCoroutine(Attack());
            }
            else
            {
                StartCoroutine(JumpAttack());
            }
            attackDelay = ENEMY_ATTACK_DELAY / _anim.speed;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            AttackIn = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            AttackIn = false;
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
        if(isJumpping)
            PlayerIn();
        
        if (testMove)
        {
            Vector3 dir = Target.transform.position - transform.position;
            dir.y = 0;
            dir.Normalize();
            _nav.SetDestination(Target.transform.position);
            
            _anim.SetBool("isRun", testMove);
            _anim.SetBool("isIdle",!testMove);
            Quaternion to = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, to, 1);
        }
    }

    private void PlayerIn()
    {
        if (_navAround.InPlayer)
        {
            testMove = false;
            _anim.SetBool("isRun", false);
            _anim.SetBool("isIdle",true);
            
            _nav.isStopped = true;
            _nav.velocity = Vector3.zero;
        }
        else if (!_navAround.InPlayer && !isDeath)
        {
            testMove = true;
            _anim.SetBool("isRun", true);
            _anim.SetBool("isIdle",false);
            
            if(!isAttacked)
                _nav.isStopped = false;
        }
    }

    public void HitBomb()
    {
        if (!isPlasma)
        {
            _EnemyHealth -= 150f;
            FinalBossHP_Bar.value = _EnemyHealth;
            isPlasma = true;
            StartCoroutine(timewait());
        }
    }
    
    IEnumerator timewait()
    {
        yield return new WaitForSeconds(2f);
        isPlasma = false;
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
        if (_EnemyHealth <= 0 && !isDeath)
        {
            _nav.isStopped = true;
            _nav.velocity = Vector3.zero;
            testMove = false;
            isDeath = true;
            door.Activate = true; // 마지막 문 활성화.
            FinalBossHP_Bar.gameObject.SetActive(false);
            foreach (var ui in _UI)
            {
                ui.gameObject.SetActive(false);
            }
            StartCoroutine("Death");
        }
    }

    IEnumerator Attack()
    {
        testMove = false;
        _nav.isStopped = true;
        _nav.velocity = Vector3.zero;

        isAttacked = true;
        _anim.SetBool("isAttack", true);
        yield return new WaitForSeconds(ENEMY_ATTACK_ANITIME / _anim.speed);
        JumpEffect[1].gameObject.SetActive(true);
        audioSource.PlayOneShot(ZombieAttackSound[0]);
        
        if (AttackIn)
        {
            _player.OnDamage(ENEMY_ZOMBIE_DAMAGE);
            _player.onDamaged= true;
            UIManager.instance.PlayerAttacked();
        }
        testMove = true;
        _nav.isStopped = false;
        isAttacked = false;
        JumpEffect[1].gameObject.SetActive(false);
        _anim.SetBool("isAttack", false);
       
    }

    IEnumerator Death()
    {
        _anim.SetBool("isDeath", true);
        audioSource.PlayOneShot(deadSound);
        yield return new WaitForSeconds(1.5f/ _anim.speed);
        foreach(var Body in Bodys) // 소멸효과
        {
            Body.material = Dissolve;
        }
        yield return new WaitForSeconds(1.5f/ _anim.speed);
        
        _anim.SetBool("isDeath", false);
        chasePlayer = false;
        Target = null;
        isDeath = false;
        gameObject.SetActive(false);
    }

    public IEnumerator FirstJump()
    {
        _anim.SetBool("isZumpFirst",true);
        audioSource.PlayOneShot(FirstJumpSound);
        _nav.velocity = Vector3.forward * 15f;

        yield return new WaitForSeconds(1.5f/ _anim.speed);
        JumpEffect[0].gameObject.SetActive(true);
        _anim.SetBool("isZumpFirst",false);
        _nav.velocity = Vector3.zero;
        yield return new WaitForSeconds(3f/ _anim.speed);
        
        JumpEffect[0].gameObject.SetActive(false);
        isJumpping = true;
    }

    IEnumerator JumpAttack()
    {
        testMove = false;
        _nav.isStopped = true;
        _nav.velocity = Vector3.zero;
        _anim.SetBool("isZump",true);
        isAttacked = true;
        yield return new WaitForSeconds(2f / _anim.speed);
        JumpEffect[1].gameObject.SetActive(true);
        audioSource.PlayOneShot(ZombieAttackSound[1]);
        yield return new WaitForSeconds(0.7f / _anim.speed);
        
        if (AttackIn)
        {
            _player.OnDamage(ENEMY_ZOMBIE_DAMAGE);
            _player.onDamaged= true;
            UIManager.instance.PlayerAttacked();
        }
        
        testMove = true;
        _nav.isStopped = false;
        isAttacked = false;
        JumpEffect[1].gameObject.SetActive(false);
        _anim.SetBool("isZump", false);

    }
}
