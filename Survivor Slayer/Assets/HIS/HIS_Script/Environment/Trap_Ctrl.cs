using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap_Ctrl : MonoBehaviour
{
    public float Speed; // 트랩이 이동할 속도.
    public float time;// 트랩이 한 번 이동하는 시간.
    private float recordTime;
    private bool GoFront;
    Destructible_Obj obj;
    [SerializeField]
    private bool isDeath;
    
    private PlayerInfo playerHealth;
    public int trapDamage; // 트랩의 데미지.
    public float DamageDelay; // 트랩이 플레이어에게 데미지를 주는 시간 간격.
    [SerializeField] private bool isDamage;

  

    // Start is called before the first frame update
    void Start()
    {
        recordTime = 0;
        obj = GetComponentInChildren<Destructible_Obj>();
        //PropellerAnim.GetComponentInChildren<Animator>();
        playerHealth = FindObjectOfType<PlayerInfo>();
    }

    // Update is called once per frame
    void Update()
    {
        isDeath = obj.CheckDeath();

        if (!isDeath)
        {
            recordTime += Time.deltaTime;

            if (recordTime > time)
            {
                GoFront = !GoFront;
                recordTime = 0;
            }


            if (GoFront)
            {
                transform.Translate(Vector3.right * Speed * Time.deltaTime);
            }
            else
            {
                transform.Translate(Vector3.left * Speed * Time.deltaTime);
            }
        }

    }
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player" && !isDamage)
        {
            StartCoroutine(OnDamage());
        }
    }

    IEnumerator OnDamage()
    {
        isDamage = true;
        //playerHealth.currenthealth -= trapDamage;
        playerHealth.OnDamage(trapDamage);
        UIManager.instance.PlayerAttacked();
        yield return new WaitForSeconds(DamageDelay);
        isDamage = false;
    }
}


