using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathUnit : MonoBehaviour
{
    private float speed;
    private Vector3[] path;
    private int targetIndex;

    private void Start()
    {
        speed = gameObject.GetComponent<Enemy_test>().MoveSpeed;        // 좀비가 가지는 이동 스피드값
    }

    public void Targetting(GameObject target)
    {
        PathRequestManager.RequestPath(transform.position, target.transform.position, OnPathFound);
    }

    public void OnPathFound(Vector3[] newPath, bool pathSuccess)
    {
        if (pathSuccess)
        {
            path = newPath;
            targetIndex = 0;
            StopCoroutine("FollowPath");
            StartCoroutine("FollowPath");
        }
    }

    IEnumerator FollowPath()
    {
        Vector3 currentWaypoint;
        
        if(path.Length <= 0)
            yield break;
        else
        {
            currentWaypoint = path[0];
        }
        
        while(true)
        {
            if (transform.position == currentWaypoint)
            {
                targetIndex++;
                if (targetIndex >= path.Length)
                {
                    targetIndex = 0;
                    path = new Vector3[0];
                    yield break;
                }

                currentWaypoint = path[targetIndex];
            }
            transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, speed * Time.deltaTime);
            yield return null;
        }
    }

    public void OnDrawGizmos()
    {
        if (path != null)
        {
            for (int i = targetIndex; i < path.Length; i++)
            {
                Gizmos.color = Color.black;
                Gizmos.DrawCube(path[i],Vector3.one);

                if (i == targetIndex)
                {
                    Gizmos.DrawLine(transform.position,path[i]);
                }
                else
                {
                    Gizmos.DrawLine(path[i - 1], path[i]);
                }
            }
        }
    }
}
