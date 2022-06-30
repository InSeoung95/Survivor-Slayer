using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructibleObject : MonoBehaviour
{
    private Gun gun;
    public float Health; // 파괴가능한 오브젝트의 체력.

    public GameObject Origin; // 멀쩡한 모델링
    public GameObject Broken; // 파괴된 모델링

    private bool OnDestroy=false;
    
    public MoveObject moveObject;

    // Start is called before the first frame update
    void Start()
    {
        //Origin.SetActive(true);
        gun = FindObjectOfType<Gun>();
    }

    // Update is called once per frame
    void Update()
    {
        if(OnDestroy)
        {
            StartCoroutine(GoDestroy());
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag=="Bullet")
        {
            Health -= gun.damage;
            Debug.Log("파괴중");

            if(Health<0)
            {
                OnDestroy = true;
            }
        }
    }

    IEnumerator GoDestroy()
    {
        //Origin.SetActive(false);
        //Destroy(gameObject);
        Instantiate(Broken, transform.position, transform.rotation);

        moveObject.brokenCount++;
        Debug.Log("broken count: " + moveObject.brokenCount);
        /*
        foreach(var part in Broken)
        {

        }
         */
        yield return new WaitForSeconds(2f);
        Destroy(Broken);
    }
}
