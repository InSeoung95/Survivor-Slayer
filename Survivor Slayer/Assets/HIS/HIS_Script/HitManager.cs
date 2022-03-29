using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HitManager : MonoBehaviour
{
    //싱글톤 관리
    public static HitManager instance;
    public GameObject AttackedImage;//플레이어가 맞을 때 나오는 이미지

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
            Destroy(this.gameObject);
    }
    // Start is called before the first frame update
    void Start()
    {
       AttackedImage.SetActive(false);
    }

    // Update is called once per frame
  

    public void Attacked()
    {
        StartCoroutine("Flash"); // 0.1초 동안 보이도록.
    }

    IEnumerator Flash()
    {
        AttackedImage.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        AttackedImage.SetActive(false);
    }
}
