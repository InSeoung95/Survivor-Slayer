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
    public Animator PropellerAnim;

  

    // Start is called before the first frame update
    void Start()
    {
        recordTime = 0;
        obj = GetComponentInChildren<Destructible_Obj>();
        //PropellerAnim.GetComponentInChildren<Animator>();
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
        else
            PropellerAnim.speed = 0f;

    }

    void CheckDeath()
    {

    }
}
