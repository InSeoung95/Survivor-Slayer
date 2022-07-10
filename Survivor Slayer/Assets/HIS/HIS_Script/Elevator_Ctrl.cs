using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator_Ctrl : MonoBehaviour
{
    private Animator eleAnim; // 엘리베이터 애니메이터
    InteractDoor door;
    public GameObject Player;

    // Start is called before the first frame update
    void Start()
    {
        eleAnim = GetComponent<Animator>();
        door = GetComponentInChildren<InteractDoor>();
    }

    private void Update()
    {
        if(eleAnim.GetBool("Move"))
        {
            Player.transform.position = new Vector3(Player.transform.position.x, gameObject.transform.position.y, Player.transform.position.z);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag=="Player")
        {
            door.Activate = false;

            eleAnim.SetBool("Move", true);
        }
    }
}
