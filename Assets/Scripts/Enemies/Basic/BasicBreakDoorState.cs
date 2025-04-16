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
    public HealthHandler healthHandler;

    private Coroutine damageDoorCoroutine;

    private Finder finder;

    public override BasicState RunCurrentState()
    {
        if (doorIsBroken)
        {
            Debug.Log("Done with breakDoor state");
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

        if (healthHandler != null && healthHandler.CurrentHealth > 0)
        {
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
