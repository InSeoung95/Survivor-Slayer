using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class EnemySpawn : MonoBehaviour
{
    private const float SPAWNTIME = 10f;    //?뚰솚 ?쒓컙
    private const int SPAWNNUMBER = 5;     //?뚰솚 ?잛닔
    public ObjectManager _objectManager;
    private float SpawnTimer;
    private int SpawnNumber;
    
    void Update()
    {
        SpawnTimer += Time.deltaTime;
        if (SpawnTimer > SPAWNTIME && SpawnNumber < SPAWNNUMBER)
        {
            SpawnTimer = 0;
            SpawnNumber++;
            ZombieSpawn();
        }
    }

    private void ZombieSpawn()
    {
        _objectManager.MakeObj("Enemy_Zombie", transform.position, transform.rotation);

    }
}
