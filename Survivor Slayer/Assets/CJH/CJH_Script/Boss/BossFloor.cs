using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BossFloor : MonoBehaviour
{
    private Player_Info _onPlayerInfo;
    [SerializeField] private Material[] _OnPoison;
    [SerializeField] private Material[] _OnHeal;
    [SerializeField] private Material[] _DisFloor;
    [SerializeField] private BossFloorChild[] _floor;
    public bool _Trigger;                             
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

            int count = Random.Range(5, 10);
            
            for (int i = 0; i < _floor.Length; i++)
            {
                int changeFloorNum = Random.Range(0, 1+1);
                if (changeFloorNum == 1 && count > 0)
                {
                    _floor[i].FloorTrigger = true;
                    if(_floor[i].floorType)
                        _floor[i].ChangeMaterials(_OnHeal);
                    else
                        _floor[i].ChangeMaterials(_OnPoison);

                    count--;
                }
                else
                {
                    _floor[i].FloorTrigger = false;
                    _floor[i].ChangeMaterials(_DisFloor);
                }
            }
        }
    }

    public void GroundRollBack()
    {
        _Trigger = false;
        for (int i = 0; i < _floor.Length; i++)
        {
            _floor[i].FloorTrigger = false;
            _floor[i].ChangeMaterials(_DisFloor);
        }
    }

    public void ChangePoisonToHeal()
    {
        for (int i = 0; i < _floor.Length; i++)
        {
            _floor[i].floorType= true;
        }
    }
}