using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Destructible_Obj : MonoBehaviour
{
    public float Health; // 오브젝트 체력.
    [SerializeField]
    float currentHealth;
    private Gun gun;
    public GameObject Explosion;// 폭발 효과 프리팹.
    public bool isDeath=false;// 죽으면 true;
    int count = 0;
    //UI 관련
    public Slider HP_Bar;
    public Image Fill;
    public Gradient gradient;

    private void Start()
    {
        currentHealth = Health;
        gun = FindObjectOfType<Gun>();
        HP_Bar.maxValue = Health;
        HP_Bar.value = Health;
    }

    private void Update()
    {
        if(currentHealth<1&&count==0)
        {
            Death();
        }
    }

    private void OnCollisionEnter(Collision _other)
    {
        if(_other.gameObject.tag=="Bullet"&&currentHealth>0)
        {
            currentHealth -= 10f;

            HP_Bar.value = currentHealth;
            Fill.color = gradient.Evaluate(HP_Bar.normalizedValue);
        }
    }

    public void Death()
    {
        count++;
        isDeath = true;
        Instantiate(Explosion, transform);

        Destroy(gameObject, 5f);//5초 뒤 소멸;
    }
    public bool CheckDeath()
    {
        return isDeath;
    }
}
