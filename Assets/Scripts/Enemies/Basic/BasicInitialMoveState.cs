using UnityEngine.AI;
using UnityEngine;

public class BasicInitialMoveState : BasicState
{
    public bool atDoor;

    public BasicState BasicBreakDoorState;
    public NavMeshAgent agent;

    public float movespeed = 2f;

    private Finder finder;
    private GameObject nearestDoor;

    public float stopBeforeReach;
    public override BasicState RunCurrentState()
    {
        if (atDoor)
        {
            return BasicBreakDoorState;
        }
        else
        {
            nearestDoor = finder.FindNearestDoor(transform.parent.position);
            if (nearestDoor != null)
            {
                agent.SetDestination(nearestDoor.transform.position);
                CheckDistance(nearestDoor);
            }

            return this;
        }
    }

    void Start()
    {
        agent = GetComponentInParent<NavMeshAgent>();
        finder = FindObjectOfType<Finder>();
        agent.speed = movespeed;
    }

    private void CheckDistance(GameObject target)
    {
        float distance = Vector3.Distance(transform.parent.position, target.transform.position);
        if (distance < stopBeforeReach)
        {
            Debug.Log("Stopped at distance: " + distance);
            atDoor = true;
        }
    }
}
