using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DRONE : MonoBehaviour
{
    private NavMeshAgent _agent;
    public Transform[] waypoint;
    private int waypointIndex;
    private Vector3 target;
    private float itemTimer;
    [SerializeField] private float ITEMTIME = 60f;
    private int _enum = 0;

    public GameObject[] UpgradeBox;

    private void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        UpdateDestination();
    }

    private void Update()
    {
        itemTimer += Time.deltaTime;
        if (Vector3.Distance(transform.position, target) < 1)
        {
            UpdateDestination();
            InterateWaypointIndex();
            if (itemTimer > ITEMTIME)
            {
                var box = Instantiate(UpgradeBox[_enum], gameObject.transform.position + Vector3.up * 3, Quaternion.identity);
                _enum++;
                if (_enum > 2)
                    _enum = 0;
                itemTimer = 0;
            }
        }
    }

    private void UpdateDestination()
    {
        target = waypoint[waypointIndex].position;
        _agent.SetDestination(target);
    }

    private void InterateWaypointIndex()
    {
        waypointIndex++;
        if (waypointIndex == waypoint.Length)
        {
            waypointIndex = 0;
        }
    }
}
