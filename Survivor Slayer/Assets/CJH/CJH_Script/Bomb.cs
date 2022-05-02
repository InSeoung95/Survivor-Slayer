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
    

    private void Start()
    {
        StartCoroutine(Explosion());
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere (transform.position, BOMB_RANGE);

    }

    IEnumerator Explosion()
    {
        yield return new WaitForSeconds(BOMB_TIME);
        foreach (var mesh in meshObj)
        {
            mesh.SetActive(false);
        }
        effectObj.SetActive(true);

        RaycastHit[] rayHits =
            Physics.SphereCastAll(transform.position, BOMB_RANGE, Vector3.up, 0f, LayerMask.GetMask("Enemy"));

        foreach (RaycastHit hitObj in rayHits)
        {
            hitObj.transform.GetComponent<Enemy_test>().HitBomb();
        }
        
        Destroy(gameObject, 2);
    }
}
