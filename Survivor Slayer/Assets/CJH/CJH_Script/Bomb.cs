using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    private const float BOMB_TIME = 5f;     // 폭탄 소환후 폭팔시간
    [SerializeField] private float BOMB_RANGE = 10;     // 폭탄 폭발범위
    public GameObject[] meshObj;
    public GameObject effectObj;
    //인성 추가
    private AudioSource audioSource;
    [Header("폭탄 사운드")]
    public AudioClip WaitBomb; // 폭탄 대기 시 나는 소리
    public AudioClip OnBomb; // 터질 때 나는 소리

    private void Start()
    {
        audioSource= GetComponent<AudioSource>();
        StartCoroutine(Explosion());
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere (transform.position, BOMB_RANGE);
    }

    IEnumerator Explosion()
    {
        //인성 추가
        audioSource.loop = true;
        audioSource.clip = WaitBomb;
        audioSource.Play();
        //
        yield return new WaitForSeconds(BOMB_TIME);
        foreach (var mesh in meshObj)
        {
            mesh.SetActive(false);
        }
        effectObj.SetActive(true);
        // 인성 추가
        audioSource.Stop();
        audioSource.PlayOneShot(OnBomb);
        //
        RaycastHit[] rayHits =
            Physics.SphereCastAll(transform.position, BOMB_RANGE, Vector3.up, 0f, LayerMask.GetMask("Enemy"));

        foreach (RaycastHit hitObj in rayHits)
        {
            hitObj.transform.GetComponent<Enemy_test>().HitBomb();
        }
        
        Destroy(gameObject, 2);
    }
}
