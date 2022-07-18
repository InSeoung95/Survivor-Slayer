using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class EnemySpawn : MonoBehaviour
{
    private struct Stage
    {
        public int Round;                  // 라운드당 적들 숫자 설정
        public float RoundTime;            // 라운드당 제한시간 시간 넘어가면 다음스테이지로 넘어가고 적들 버프
        public int Zombie;                 // 일반좀비 숫자
        public int ArmorZombie;            // 갑옷좀비 숫자
        public int tower;                  // 포탑 숫자
        public int AllEnemy;
    }

    private Stage _stage;
    public Transform[] SpawnPoint;
    private const float SPAWNTIME = 10f;    // 스폰시간
    public ObjectManager _objectManager;
    private float SpawnTimer;
    private int SpawnNumber;        // 라운드당 소환된 적들

    private float sinematicTime = 30f;
    private bool sinematicTriger = false;
   

    private void Start()
    {
        _stage.Round = 1;
        _stage.RoundTime = 150;
        _stage.Zombie = 40;
        _stage.ArmorZombie = 10;
        _stage.tower = 0;
        _stage.AllEnemy = 30;       // 적은 총 50마리씩 나옴 // 인성 수정 30마리로.

        //인성 추가
        UIManager.instance.UpdateRound(_stage.Round);
      
    }

    void Update()
    {
        SpawnTimer += Time.deltaTime;

        if (SpawnTimer > sinematicTime && !sinematicTriger)
        {
            sinematicTriger = true;
            SpawnTimer = 0;
        }
        
        if (SpawnTimer > SPAWNTIME && sinematicTriger)
        {
            SpawnTimer = 0;
            ZombieSpawn();
        }
    }

    private void ZombieSpawn()
    {
        switch (_stage.Round)
        {
            case 1:
                for (int i = 0; i < SpawnPoint.Length; i++)
                {
                if (i == 0)
                {
                    _objectManager.MakeObj("Enemy_Fog", SpawnPoint[i].position, SpawnPoint[i].rotation);
                    SpawnNumber++;
                    }
                    else
                    {
                    _objectManager.MakeObj("Enemy_Zombie", SpawnPoint[i].position, SpawnPoint[i].rotation);
                    SpawnNumber++;
                    }
                    //인성 추가
                    UIManager.instance.UpdateRound(_stage.Round);
                }
                if (SpawnNumber>=_stage.AllEnemy)
                {
                    SpawnNumber = 0;
                    _stage.Round++;
                    _stage.Zombie = 35;
                    _stage.ArmorZombie = 10;
                    _stage.tower = 5;
                }
                UIManager.instance.CurrentEnemyNum += SpawnPoint.Length;
                break;
            case 2:
                /*
                for (int i = 0; i < SpawnPoint.Length; i++)
                {
                    _objectManager.MakeObj("Enemy_Zombie", SpawnPoint[i].position, SpawnPoint[i].rotation);
                    SpawnNumber++;
                }
                break;
                 */
            case 3:
                break;
            case 4:
                break;
            case 5:
                break;
        }
        

    }
    
}
