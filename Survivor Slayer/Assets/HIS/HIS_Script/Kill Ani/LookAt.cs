using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAt : MonoBehaviour
{
    public Transform Target;// 바라볼 타겟.


    public void LookAtTarget(Transform _target) // 타겟을 바라보는 함수.
    {
        Debug.Log(_target.gameObject.name + "바라봄");
        this.gameObject.transform.LookAt(_target);
    }
}
