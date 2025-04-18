using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BasicHuntPlayer: BasicState
{
    public bool nearPlayer;

    public BasicState AttackPlayerState;
    public NavMeshAgent agent;

    private Finder finder;
    private GameObject nearestPlayer;




    public override BasicState RunCurrentState()
    {
        if (nearPlayer)
        {
            nearPlayer = false;
            return AttackPlayerState;
        }
        else
        {
            nearestPlayer = finder.FindNearestPlayer(transform.parent.position);
            if (nearestPlayer != null)
            {
                agent.SetDestination(nearestPlayer.transform.position);
                CheckDistance(nearestPlayer);
            }
            return this;
        }
    }

    void Start()
    {
        agent = GetComponentInParent<NavMeshAgent>();
        finder = FindObjectOfType<Finder>();
    }

    private void CheckDistance(GameObject target)
    {
        float distance = Vector3.Distance(transform.parent.position, target.transform.position);
        if (distance < 2)
        {
            nearPlayer = true;
        }
    }

}
