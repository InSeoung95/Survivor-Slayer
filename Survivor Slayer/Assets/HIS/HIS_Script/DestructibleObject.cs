using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructibleObject : MonoBehaviour
{
    private Gun gun;
    public float Health; // 파괴가능한 오브젝트의 체력.

    public GameObject Origin; // 멀쩡한 모델링
    public GameObject Broken; // 파괴된 모델링

    [SerializeField] private bool OnDestroy;
    
    public MoveObject moveObject;

    public int count;

    // Start is called before the first frame update
    void Start()
    {
        Origin.SetActive(true);
        Broken.SetActive(false);
        gun = FindObjectOfType<Gun>();
    }

    // Update is called once per frame
    void Update()
    {
        if(OnDestroy&&count<1)
        {
            StartCoroutine(GoDestroy());
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Bullet"))
        {
            collision.gameObject.SetActive(false);

            Health -= 10f;
            Debug.Log("파괴중");

            if(Health<0)
            {
                OnDestroy = true;
            }
        }
    }

    IEnumerator GoDestroy()
    {
        Origin.SetActive(false);
        Broken.SetActive(true);
        //Instantiate(Broken, transform.position, transform.rotation);

        moveObject.brokenCount++;
        Debug.Log("broken count: " + moveObject.brokenCount);
        /*
        foreach(var part in Broken)
        {

        }
         */
        count++;
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }
}
