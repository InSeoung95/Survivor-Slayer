using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator_Ctrl : MonoBehaviour
{
    private Animator eleAnim; // 엘리베이터 애니메이터
    InteractDoor door;
    [SerializeField]
    private GameObject Player;

    public ParticleSystem[] explosions;// 엘리베이터로 탈출 시 재생될 이펙트들
    public float BombDelay;// 폭발 이펙트 딜레이 시간

    // Start is called before the first frame update
    void Start()
    {
        eleAnim = GetComponent<Animator>();
        door = GetComponentInChildren<InteractDoor>();
        Player = GameObject.Find("Player");
    }

    private void Update()
    {
        if(eleAnim.GetBool("Move"))
        {
            Player.transform.position = new Vector3(Player.transform.position.x, gameObject.transform.position.y, Player.transform.position.z);

            StartCoroutine(Explosion());
        }
    }
    IEnumerator Explosion() // 연쇄 폭발 코루틴
    {
        yield return new WaitForSeconds(2f);

        foreach(var explosion in explosions)
        {
            explosion.Play();

            yield return new WaitForSeconds(BombDelay);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag=="Player")
        {
            door.Activate = false;

            eleAnim.SetBool("Move", true);
        }
    }
}
