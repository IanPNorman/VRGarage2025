using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class BasicBreakDoorState : BasicState
{
    public bool doorIsBroken;
    public bool startDamage;

    public BasicState BasicHuntPlayer;
    public NavMeshAgent agent;

    public GameObject targetDoor;

    private Finder finder;
    public HealthHandler healthHandler;
    private DoorHealth doorHealth;

    private Coroutine damageDoorCoroutine;

    public int damageDone = 0;

    public override BasicState RunCurrentState()
    {
        if (doorIsBroken)
        {
            
            return BasicHuntPlayer;
        }
        else
        {
            if (!startDamage)
            {
                startDamage = true;

                targetDoor = finder.FindNearestDoor(transform.parent.position);

                if (targetDoor != null)
                {
                    
                    damageDoorCoroutine = StartCoroutine(damageDoor());
                }
            }

            return this;
        }
    }

    void Start()
    {
        finder = FindObjectOfType<Finder>();
    }

    IEnumerator damageDoor()
    {
        healthHandler = targetDoor.GetComponent<HealthHandler>();
        doorHealth = targetDoor.GetComponent<DoorHealth>();

        if (healthHandler != null && healthHandler.CurrentHealth > 0)
        {
            doorHealth.getBarricade(damageDone).SetActive(false);
            damageDone++;

            healthHandler.HealthChanged(-1);
        }
        else
        {
            yield return new WaitForSeconds(2f);

            doorIsBroken = true;

            if (damageDoorCoroutine != null)
                StopCoroutine(damageDoorCoroutine);

            yield break;
        }

        yield return new WaitForSeconds(2f);
        damageDoorCoroutine = StartCoroutine(damageDoor());
    }

}
