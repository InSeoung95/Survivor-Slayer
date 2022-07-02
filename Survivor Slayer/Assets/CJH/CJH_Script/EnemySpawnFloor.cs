using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnFloor : MonoBehaviour
{
    private bool SpawnTrigger;
    [SerializeField]private float SpawnTime = 10f;
    [SerializeField] private int SpawnNumber = 5;
    public Transform[] SpawnPoint;
    public ObjectManager _objectManager;
    private float Timer;
    

    private void Update()
    {
        if (SpawnTrigger)
        {
            Timer += Time.deltaTime;

            if (Timer > SpawnTime && SpawnNumber>0)
            {
                Timer = 0;
                SpawnNumber--;
                ZombieSpawn();
            }
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SpawnTrigger = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SpawnTrigger = false;
            Timer = 0;
        }
    }

    private void ZombieSpawn()
    {
        for (int i = 0; i < SpawnPoint.Length; i++)
        {
            _objectManager.MakeObj("Enemy_Zombie", SpawnPoint[i].position, SpawnPoint[i].rotation);
        }
    }
}
