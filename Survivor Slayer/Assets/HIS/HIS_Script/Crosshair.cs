using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crosshair : MonoBehaviour
{
    [SerializeField]
    private GameObject Crosshair_HUD;
    public float Run_GunAccuracy=0.05f;// 달릴 때 적용할 총 명중도.
    private float Gun_Accuracy;// 총 정확도.
    private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void MoveOnCrosshair(bool _isMove) // 플레이어 움직일 때 나오는 크로스 헤어
    {
        anim.SetBool("isMove", _isMove);
    }
    public void FireOnCrosshair() //총 발사시 나오는 크로스 헤어
    {
        anim.SetTrigger("isFire");
    }
    public float GetAccuracy()
    {
        if (anim.GetBool("isMove"))
            Gun_Accuracy = Run_GunAccuracy;
        else
            Gun_Accuracy = 0f;

        return Gun_Accuracy;
    }
}
