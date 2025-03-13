using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BasicInitialMoveState : BasicState
{
    public bool atDoor;

    public BasicState BasicBreakDoorState;

    public NavMeshAgent agent;  

    public GameObject[] allDoors;

    public float distanceTo;

    public float lowestDistance;

    public int arrNum;

    public override BasicState RunCurrentState()
    {
        if (atDoor)
        {

            return BasicBreakDoorState;
        }
        else
        {
            agent.SetDestination(nearestDoor());
            return this;
        }
    }

    void Start()
    {
        allDoors = GameObject.FindGameObjectsWithTag("Door"); // If map ever expands and more doors are added while the game is going this wont work 
        agent = GetComponentInParent<NavMeshAgent>();
    }
    
    private Vector3 nearestDoor()
    {
        
        lowestDistance = Vector3.Distance(this.transform.parent.position, allDoors[0].transform.position);
        for (int i = 0; i < allDoors.Length; i++)
        {
            distanceTo = Vector3.Distance(this.transform.parent.position, allDoors[i].transform.position);
            if (distanceTo <= lowestDistance)
            {
                lowestDistance = distanceTo;
                arrNum = i;
            }
        }
        return allDoors[arrNum].transform.position;
    }
}
