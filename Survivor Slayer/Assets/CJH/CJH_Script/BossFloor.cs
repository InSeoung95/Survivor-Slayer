using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BossFloor : MonoBehaviour
{
    private Player_Info _onPlayerInfo;
    [SerializeField] private Material[] _OnFloor;
    [SerializeField] private Material[] _DisFloor;
    [SerializeField] private BossFloorChild[] _floor;
    public bool _Trigger;                               // 트리거 조건 지금은 없음 차후에 true올리는거 생각
    private float Timer;
    [SerializeField]private float TimerMax = 10f;           // %초마다 타일이 바뀜
    

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            _onPlayerInfo = collision.gameObject.GetComponent<Player_Info>();
           
        }
    }

    private void Update()
    {
        Timer += Time.deltaTime;
        if (_Trigger && Timer>TimerMax)
        {
            Timer = 0;
            
            for (int i = 0; i < _floor.Length; i++)
            {
                int changeFloorNum = Random.Range(0, 1+1);
                if (changeFloorNum == 1)
                {
                    _floor[i].FloorTrigger = true;
                    _floor[i].ChangeMaterials(_OnFloor);
                }
                else
                {
                    _floor[i].FloorTrigger = false;
                    _floor[i].ChangeMaterials(_DisFloor);
                }
            }
        }
    }
}