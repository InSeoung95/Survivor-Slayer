using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations.Rigging;
using Random = UnityEngine.Random;
using UnityEngine.VFX;

public class Enemy_test : MonoBehaviour
{
    private const float ENEMY_MOVESPEED = 5f;     //좀비의 이동속도  1.6f = default
    private const float ENEMY_ZOMBIE_DAMAGE = 10f;  //일반좀비 공격력
    private const float ENEMY_ATTACK_DELAY = 1.3f;    //좀비의 공격속도

    private Enemy_Dest _enemyDest;                  // 좀비 체력 및 부위별 체력관리, 팔/다리 제거인식
    public float MoveSpeed = ENEMY_MOVESPEED;
    public float attackDelay = ENEMY_ATTACK_DELAY;

    private Rigidbody _rigid;
    private NavAround _navAround;       // 좀비의 주위에 플레이어 검출해서 이동불가
    private BoxCollider _boxCollider;   // 좀비의 공격범위
    private float _boxColliderSize;
    public GameObject Target;           // 좀비가 공격할 목표
    private PlayerInfo _player;         // 좀비가 가져올 플레이어 정보

    // private PathUnit _pathUnit;         // A*길찾기 오류해결하면 navmesh지우고 이걸로 사용
    private NavMeshAgent _nav;
    public ObjectManager _ObjectManager;

    public bool testMove = false;
    public bool chasePlayer = false;
    public bool isDeath = false;
    private bool isAttacked;
    private Animator _anim;
    private float animSpeed;                 // 좀비별 애니메이션 스피드

    // 인성 수정
    public VisualEffect hitEffect;   // 좀비 피격 이펙트
    public AudioClip BurserkSound;  // 광폭화 사운드
    public AudioClip deadSound;// 좀비 사망 사운드.

    private AudioSource audioSource;
    [SerializeField] private AudioClip[] ZombieHowling;     // 좀비가 울어댈 소리
    private int howling;
    
    private EnemySpawn enemySpawn;
    public Material Dissolve;// 좀비 사망 시 소멸 효과.
    public SkinnedMeshRenderer[] Bodys; // 이펙트 적용할 좀비 몸통들.
    public Material GroggyEffect; // 좀비 그로기 상태 시 적용할 이펙트

    public KillAniEnemyData killAniData;// 킬 애니 데이터
    private Enemy_Body _Body;
    private Material[] DefaultMaterial; // 기본 적용된 머테리얼.
    private KillAni_Ctrl playerKilAni;

    public Rig _Rig;
    

    private void Start()
    {
        _enemyDest = GetComponent<Enemy_Dest>();
        _rigid = GetComponent<Rigidbody>();
        _navAround = GetComponentInChildren<NavAround>();
        _boxCollider = GetComponent<BoxCollider>();
        _boxColliderSize = _boxCollider.size.z + _boxCollider.center.z;     // 좀비 공격범위(위치+크기)
        _ObjectManager = GameObject.Find("ObjectManager").GetComponent<ObjectManager>();
        audioSource = GetComponent<AudioSource>();
        
        _nav = GetComponent<NavMeshAgent>();         
        _anim = GetComponentInChildren<Animator>();
        
        animSpeed = Random.Range(10, 25+1) * 0.1f;
        _anim.SetFloat("RunRatio", Random.Range(0,10+1));
        _anim.speed = animSpeed;
        
        howling = Random.Range(0, 2 + 1);
        audioSource.clip = ZombieHowling[howling];
        audioSource.Play();
        audioSource.loop = true;
        
        //인성 추가
        enemySpawn = FindObjectOfType<EnemySpawn>();
        killAniData = GetComponent<KillAniEnemyData>();
        _Body = GetComponent<Enemy_Body>();
        DefaultMaterial = new Material[Bodys.Length];
        for(int i=0;i<DefaultMaterial.Length;++i)
        {
            DefaultMaterial[i] = Bodys[i].material;
        }
        playerKilAni = FindObjectOfType<KillAni_Ctrl>();
    }

    private void Update()
    {
        Move();
        //Attack
        Burserk();
        attackDelay -= Time.deltaTime;
        Die();
        
    }

    private void Move()
    {
        if (Target == null && !chasePlayer)
        {
            if (_ObjectManager.stage == 1)
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
            }
            else
            {
                Target = _ObjectManager.Player.gameObject;
                chasePlayer = true;
            }

            _nav.isStopped = false;
            _nav.speed = MoveSpeed + (animSpeed * 0.2f);
            testMove = true;
        }

        PlayerIn();
        
        if (testMove && !isDeath)
        {
            
            //인성 추가
            if(killAniData.isGroggy)
            {
                OnGroggy();
                _nav.speed = 0f;
            }
            else
            {
                Vector3 dir = Target.transform.position - transform.position;
                dir.y = 0;
                dir.Normalize();


                _nav.SetDestination(Target.transform.position);
                if (_enemyDest.isLeg)
                {
                    _anim.SetBool("isCrawl", true);
                    MoveSpeed *= 0.7f; // 인성 수정 초기값 : 0.1f
                    //인성 추가
                    //killAniData.isCrawl = true;

                    _enemyDest.isLeg = false;
                }
                _anim.SetBool("isRun", testMove);

                _nav.speed = MoveSpeed + (animSpeed * 0.05f);
                Quaternion to = Quaternion.LookRotation(dir);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, to, 1);
            }



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
        _enemyDest.currentHealth -= 500f;

    }

    private void Burserk()
    {
        if (_enemyDest.isBurserk)
        {
            MoveSpeed *= 2.5f;
            _anim.speed += 1.5f;
            audioSource.PlayOneShot(BurserkSound);
            
            _enemyDest.isBurserk = false;
        }
    }

    private void Die()
    {
        if ( _enemyDest.currentHealth <= 0 && !isDeath)
        {
            isDeath = true;
            testMove = false;
            _nav.isStopped = true;
            _nav.velocity = Vector3.zero;

            MoveSpeed = ENEMY_MOVESPEED;
            _anim.speed = animSpeed;
            killAniData.isGroggy = false;
            _Rig.weight = 0f;

            if (_ObjectManager.stage == 1)
            {
                UIManager.instance.CurrentEnemyNum--;
                UIManager.instance.UpdateLeftEnemy(UIManager.instance.CurrentEnemyNum);
            }

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

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && attackDelay < 0&&!playerKilAni.isPlaying) // 확정킬 애니 재생 중일 땐 데미지 X
        {
            if (_player == null)
                _player = other.gameObject.GetComponent<PlayerInfo>();

            StartCoroutine(Attack());
            attackDelay = ENEMY_ATTACK_DELAY * 1.5f;
        }
        else if (other.gameObject.CompareTag("Base") && attackDelay < 0)
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
        else if (other.gameObject.CompareTag("ObstacleWall") && attackDelay < 0)
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
        isAttacked = true;
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
            _player.OnDamage(ENEMY_ZOMBIE_DAMAGE);
            _player.onDamaged= true; // 플레이어 공격 받는 상태 true;
            UIManager.instance.PlayerAttacked();
        }
        
        testMove = true;
        _nav.isStopped = false;
        isAttacked = false;
        _anim.SetBool("isHeadbutt", false);
        _anim.SetBool("isAttack", false);
       
    }

    IEnumerator Death()
    {
        _anim.SetBool("isDeath", true);
        audioSource.clip = deadSound;
        audioSource.PlayOneShot(deadSound);
        yield return new WaitForSeconds(1.5f / animSpeed);
        foreach(var Body in Bodys) // 소멸효과
        {
            Body.material = Dissolve;
        }
        yield return new WaitForSeconds(1.5f / animSpeed);
        
        DropItem();
        _anim.SetBool("isDeath", false);
        chasePlayer = false;
        Target = null;
        
        audioSource.clip = ZombieHowling[howling];
        audioSource.loop = true;
        
        for(int i=0;i<Bodys.Length;++i)
        {
            Bodys[i].material = DefaultMaterial[i];
        }
        
        isDeath = false;
        _navAround.InPlayer = false;
        _enemyDest.currentHealth = _enemyDest.maxhealth;
        gameObject.SetActive(false);
        
    }
   public void OnGroggy()
    {
        StartCoroutine(Groggy(killAniData));
    }
    IEnumerator Groggy(KillAniEnemyData _data)
    {
        if (killAniData.isCrawl)
        {
            _anim.SetBool("CrawlGroggy", true);
        }
        else
            _anim.SetBool("RunGroggy", true);

        //_anim.speed = 0f; // 이게 일시정지인가?
        //Debug.Log("그로기 상태 On");
        foreach(var body in Bodys)
        {
            body.material = GroggyEffect;
        }
        yield return new WaitForSeconds(_data.GroggyTime);//그로기 시작만큼 멈춤

        if (killAniData.isCrawl)
        {
            _anim.SetBool("CrawlGroggy", false);
        }
        else
            _anim.SetBool("RunGroggy", false);//그로기 애니메이션 해제.

        //_anim.speed = 1f; // 다시 애니메이션 시작.
        killAniData.isGroggy = false;
        
        for(int i=0;i<Bodys.Length;++i)
        {
            Bodys[i].material = DefaultMaterial[i];
        }
    }
}
