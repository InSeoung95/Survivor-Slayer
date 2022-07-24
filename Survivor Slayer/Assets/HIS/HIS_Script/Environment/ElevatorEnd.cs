using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorEnd : MonoBehaviour
{
    public Elevator_Ctrl elevator;
    
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag=="Player")
        {
            elevator.end = true;
        }
    }
}
