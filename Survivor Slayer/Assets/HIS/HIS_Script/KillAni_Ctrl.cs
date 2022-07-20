using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using Cinemachine;

public class KillAni_Ctrl : MonoBehaviour
{
    private bool isPlaying=false; // 애니메이션이 재생 중인지.
    public bool isEnemyFront;// 적이 정면을 보고 있는지. rotation.y값이 -90~90일때.
    [SerializeField]
    private Transform mainCamera; //메인 카메라 좌표.
    private ParticleSystem bloodEf;

    private PlayableDirector playableDirector;

    public PlayableDirector[] KillAniGroup;



    public CinemachineVirtualCamera playerCam; // 컨트롤 할 플레이어 가상 카메라

    public GameObject Player_Model; // 플레이어 캐릭터. 평소에 비활성화. 확정킬 재생 때 활성화.
    public Transform LeftArmIK;
    public Transform RightArmIK;

    public Transform GunLeftHandle;
    public Transform GunRightHandle;

    public TimelineAsset[] KillAniTimelines=new TimelineAsset[6]; // 재생할 확정킬 애니메이션들.

    public enum KillAniType //확정킬 애니메이션 종류
    {
        Stand_Front,
        Stand_Back,
        Stnad_Side,
        Lie_Front,
        Lie_Back,
        Lie_Side
    }
    // Start is called before the first frame update
    void Start()
    {
        playableDirector = GetComponent<PlayableDirector>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {

        //조건: 적이 1.부위파괴로인한 그로기 상태인지, 2.플레이어와 적 확정킬 콜라이더에 닿았는지, 3.확정킬 애니메이션 재생 중 아닌지. 4.적이 누워있는 상태인지
        if(other.tag=="KillAni"&&!isPlaying)
        {
            KillAniEnemyData enemyInform = other.GetComponentInParent<KillAniEnemyData>(); //적으로부터 KillAniEnemyDate 클래스 정보 받아온다.

            if(enemyInform.isGroggy) // 적이 현재 그로기 상태인지.
            {
                KillAniType aniType;

                isPlaying = true; //애니메이션 재생 중으로 변경.
                Player_Model.SetActive(true); // 플레이어 모델링 활성화.
                //확정킬 애니 타입 체크
                float enemyAngle;
                enemyAngle = enemyInform.transform.eulerAngles.y;
                Debug.Log("적 각도:"+enemyAngle);
                if (enemyAngle < 90 && enemyAngle > -90)
                    isEnemyFront = true;
                else
                    isEnemyFront = false;

                bloodEf = enemyInform.bloodEf;

                Vector3 dir = enemyInform.transform.position - transform.position;
                Debug.Log("로컬 포지션:"+enemyInform.transform.TransformPoint(enemyInform.transform.position));
                //Vector3 dir = other.transform.position - transform.position;
                dir.y = 0f;

                Quaternion rot = Quaternion.LookRotation(dir.normalized);

                //카메라, 플레이어 정면 보도록.
                mainCamera.rotation = rot;
                transform.rotation = rot;

                //Debug.Log(rot.eulerAngles);

                //float angle = rot.eulerAngles.y;

                //float angle = Mathf.Atan2(enemyInform.transform.position.y - transform.position.y, enemyInform.transform.position.x - transform.position.x)*Mathf.Rad2Deg;
                //float angle = Mathf.Atan2(enemyInform.transform.position.x - transform.position.x, enemyInform.transform.position.y - transform.position.y) * Mathf.Rad2Deg;
                //float angle = Vector3.SignedAngle(Vector3.up,transform.position, enemyInform.transform.position);
                //float angle = Mathf.Atan2(transform.position.z - enemyInform.transform.position.z, transform.position.x - enemyInform.transform.position.x)*Mathf.Rad2Deg;

                //각도 구하기 첫번째 방법.
               
                
                Transform enemyTransform = enemyInform.transform;
                float angle = Mathf.Atan2(enemyTransform.position.z - transform.position.z,
                    enemyTransform.position.x - transform.position.x) * Mathf.Rad2Deg; ;

                if (!isEnemyFront)//적이 뒤돌아 있을 때 
                    angle = -angle;
                 
                 
                //누워있을 때는 적 z축을 y로.
                
                //Transform enemyTransform = other.transform;
                
                /*
                float angle = Mathf.Atan2(enemyTransform.TransformVector( enemyTransform.position).z - transform.position.z,
                    enemyTransform.TransformVector(enemyTransform.position).x - transform.position.x) * Mathf.Rad2Deg;
                 */

                //각도 구하기 2번째 방법ㅐ

                //Vector3 player_dir = transform.forward;
                //Vector3 enemy_dir = transform.InverseTransformVector(enemyInform.transform.forward); // 월드 좌표계 벡터를 로컬 좌표계 벡터로 변환.
                //float angle = Vector3.Angle(enemy_dir, player_dir);
                /*

                Vector3 referenceForward = transform.forward;/*기준이 되는 reference 벡터
                Vector3 referenceRight = Vector3.Cross(Vector3.up, referenceForward);
                Vector3 newDirection = enemyInform.transform.forward;/*각도를 구하고자 하는 입력 Direction

                float angle = Vector3.Angle(newDirection, referenceForward);
                float sign = Mathf.Sign(Vector3.Dot(newDirection, referenceRight));
                float finalAngle = sign * angle;
                 */

                //각도 3번째
                /*
                Vector3 Player = transform.position;
                Vector3 enemy = enemyInform.transform.position;

                Vector3 v = enemy - Player;

                float angle = Mathf.Atan2(v.y, v.x) * Mathf.Rad2Deg;
                 */
                //각도 4번째
                /*
                Vector3 player = transform.position;
                Vector3 enemy = enemyInform.transform.position;

                float angle=Quaternion.FromToRotation(Vector3.up,enemy-player).eulerAngles.z;
                 */

                //각도 5번째
                /*
                Vector3 enemyDir = enemyInform.transform.TransformVector(enemyInform.transform.forward);

                float angle = Vector3.SignedAngle(Vector3.up, enemyDir, transform.forward);

                 */
                Debug.Log("anlge: " + angle);
                //누워있을 때는 적 z축을 y로.
                //Mathf.Atan2(enemyInform.transform.position.y - transform.position.z, enemyInform.transform.position.x - transform.position.x) * Mathf.Rad2Deg;

                //Debug.Log(Quaternion.FromToRotation(Vector3.up,dir).eulerAngles.z);
                //Debug.Log(Quaternion.FromToRotation(Vector3.up, dir).eulerAngles.y);


                //어느 타입 애니메이션 재생할지
                aniType = GetAniType(angle); // 재생할 애니 타입 받아오고
                PlayKillAni(aniType, enemyInform); // 받아온 애니 타입에 맞는 킬애니 재생
                /*
                playableDirector.Play();
                */

            }


        }
    }
   
    public bool CheckIsPlaying()
    {
        return isPlaying;
    }

    public KillAniType GetAniType(float _angle) // 플레이어와 적의 각도에 따라 재생할 확정킬 애니 결정
    {
        KillAniType aniType;

        if (_angle > 0 && _angle <= 180)
        {
            aniType = KillAniType.Stand_Back;
        }
        else
            aniType = KillAniType.Stand_Front;

        Debug.Log("AniType: "+aniType);
        return aniType;
    }
    public void PlayKillAni(KillAniType _aniType, KillAniEnemyData _enemyInform) // 확정킬 애니 재생함수
    {
        switch (_aniType)
        {
            case KillAniType.Stand_Front:
                {
                    playerCam.LookAt = _enemyInform.targetInforms[0].CameraLookAt;

                    //카메라가 돌아가는 것 y,z축 고정해야 된다.
                    //playerCam.transform.rotation = Quaternion.Euler(0, 0, 0);
                    //playerCam.transform.rotation=

                    //playerCam.LookAt.position = Mathf.Lerp(playerCa, _enemyInform.targetInforms[0].CameraLookAt.position, Time.deltaTime);
                    //playableDirector.Play(KillAniTimelines[0]);
                    //playerCam.LookAt.position = Vector3.Lerp(new Vector3(0,0,0), _enemyInform.targetInforms[0].CameraLookAt.position, Time.deltaTime * 1);
                    KillAniGroup[0].Play();
                    break;
                }
            case KillAniType.Stand_Back:
                {
                    playerCam.LookAt = _enemyInform.targetInforms[1].CameraLookAt;

                    //RightArmIK = _enemyInform.targetInforms[1].RifhtArmTarget;
                    //LeftArmIK = _enemyInform.targetInforms[1].LeftArmIKTarget;

                    //LeftArmIK.GetComponent<UpdateRigTarget>().target= _enemyInform.targetInforms[1].LeftArmIKTarget;
                    //RightArmIK.GetComponent<UpdateRigTarget>().target= _enemyInform.targetInforms[1].RifhtArmTarget;
                    LeftArmIK = _enemyInform.targetInforms[1].LeftArmTarget;
                    RightArmIK = _enemyInform.targetInforms[1].RifhtArmTarget;

                    //playableDirector.Play(KillAniTimelines[1]);
                    KillAniGroup[1].Play();
                    break;
                }
            case KillAniType.Stnad_Side:
                {
                    break;
                }

            case KillAniType.Lie_Front:
                {

                    break;
                }
            case KillAniType.Lie_Back:
                {
                    break;
                }
            case KillAniType.Lie_Side:
                {
                    break;
                }
        }
        Debug.Log("확정킬 애내 재생");
        StartCoroutine(playingTime(_enemyInform));
        
    }
    IEnumerator playingTime(KillAniEnemyData _enemyInform)
    {
        yield return new WaitForSeconds(1f);
        isPlaying = false; // 확정킬 애니 재생 중 false;
        Player_Model.SetActive(false); // 플레이어 모델 다시 비활성화
        _enemyInform.GetComponent<Enemy_test>().isDeath = true; // 적 죽이기.
        _enemyInform.isGroggy = false;
        Debug.Log("is Playing: " + isPlaying);

        //각 변수들 초기화.
        playerCam.LookAt = null;
        //LeftArmIK.GetComponent<UpdateRigTarget>().target = GunLeftHandle;
        //RightArmIK.GetComponent<UpdateRigTarget>().target = GunRightHandle;
        LeftArmIK = null;
        RightArmIK = null;
        //playerCam.transform.rotation= Quaternion.Euler(mainCamera.transform.rotation.x,0,0);
        //mainCamera.transform.localRotation= Quaternion.Euler(mainCamera.transform.rotation.x, 0, 0);
        //mainCamera.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));

    }
    public void PlayBloodEffect()
    {
        bloodEf.Play();
    }
}
