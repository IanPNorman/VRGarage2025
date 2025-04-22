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
        if (healthHandler == null)
            healthHandler = targetDoor.GetComponent<HealthHandler>();

        if (doorHealth == null)
            doorHealth = targetDoor.GetComponent<DoorHealth>();

        if (healthHandler == null || doorHealth == null)
        {
            yield break;
        }

        while (healthHandler.CurrentHealth > 0)
        {
            GameObject barricade = doorHealth.getBarricade(damageDone);
            GameObject repairdBaricade = doorHealth.getRepairable(damageDone); //This gets the repairable barricade 
            if (barricade != null)
                barricade.SetActive(false);
            if (repairdBaricade != null)
                repairdBaricade.SetActive(true); // This sets the repairdable barriade as true

            damageDone++;
            healthHandler.HealthChanged(-1);

            yield return new WaitForSeconds(2f);
        }
        doorIsBroken = true;

        damageDoorCoroutine = null;
    }

}
