using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PathRequestManager : MonoBehaviour
{
    private Queue<PathRequest> pathRequestQueue = new Queue<PathRequest>();     //길찾기 요청을 순서에 맞춰 큐에 담기
    private PathRequest currentPathRequest; // 현재 처리할 길찾기

    private static PathRequestManager instance;
    private PathFinding PathFinding;

    private bool isProcessingPath;

    private void Awake()
    {
        instance = this;
        PathFinding = GetComponent<PathFinding>();
    }

    // 오브젝트가 길찾기 요청하는 함수
    public static void RequestPath(Vector3 pathStart, Vector3 pathEnd, UnityAction<Vector3[], bool> callback)
    {
        PathRequest newRequest = new PathRequest(pathStart, pathEnd, callback);
        instance.pathRequestQueue.Enqueue(newRequest);
        instance.TryProcessNext();
    }
    
    //큐 순서대로 길찾기 요청을 꺼내 Pathfinding 시작
    void TryProcessNext()
    {
        if (!isProcessingPath && pathRequestQueue.Count > 0)
        {
            currentPathRequest = pathRequestQueue.Dequeue();
            isProcessingPath = true;
            PathFinding.StartFindPath(currentPathRequest.pathStart, currentPathRequest.pathEnd);
        }
    }

    // 길찾기 완료된요청을 처리후 오브젝트에 이동시작명령을 콜백
    public void FinishedProcessingPath(Vector3[] path, bool success)
    {
        currentPathRequest.callback(path, success);
        isProcessingPath = false;
        TryProcessNext();
    }
    
}
struct PathRequest
{
    public Vector3 pathStart;
    public Vector3 pathEnd;
    public UnityAction<Vector3[], bool> callback;

    public PathRequest(Vector3 nStart, Vector3 nEnd, UnityAction<Vector3[], bool> nCall)
    {
        pathStart = nStart;
        pathEnd = nEnd;
        callback = nCall;
    }
}
