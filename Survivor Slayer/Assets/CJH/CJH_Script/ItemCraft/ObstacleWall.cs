using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleWall : MonoBehaviour
{
    private const float WALLTIMER = 10f;
    
    public float WallHealth = 100;      // 벽 체력
    private float WallTimer = WALLTIMER;      // 벽 제한시간

    private void Update()
    {
        Broken();
        WallTimer -= Time.deltaTime;
    }

    private void Broken()
    {
        if(WallHealth <= 0 || WallTimer <= 0)
            Destroy(gameObject);
    }
}
