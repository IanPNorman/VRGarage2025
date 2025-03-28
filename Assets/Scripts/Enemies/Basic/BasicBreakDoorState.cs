using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BasicBreakDoorState : BasicState
{
    public bool doorIsBroken;

    public bool startDamage;

    public BasicState BasicInitialMoveState;

    public NavMeshAgent agent;

    public GameObject[] allDoors;

    public float distanceTo;

    public float lowestDistance;

    public int arrNum;

    public GameObject targetDoor;

    public HealthHandler healthHandler;

    private Coroutine damageDoorCoroutine;



    public override BasicState RunCurrentState()
    {
        if (doorIsBroken)
        {
            Debug.Log("Done with breakDoor state");
            return BasicInitialMoveState;
        }
        else
        {
            if (!startDamage)
            {
                startDamage = true;

                targetDoor = nearestDoor();

                damageDoorCoroutine = StartCoroutine(damageDoor());
            }
            return this;
        }
    }

    void Start() // Simplify this and put into external class for easier access instead of each state having to call it.
    {
        allDoors = GameObject.FindGameObjectsWithTag("Door");
    }

    private GameObject nearestDoor()
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

        return allDoors[arrNum];
    }

    IEnumerator damageDoor()
    {
        healthHandler = targetDoor.GetComponent<HealthHandler>();
        if (healthHandler.CurrentHealth > 0)
        {
            healthHandler.HealthChanged(-1);
        }

        else
        {
            yield return new WaitForSeconds(2f);

            doorIsBroken = true;

            StopCoroutine(damageDoorCoroutine);
        }

        yield return new WaitForSeconds(2f);

        StartCoroutine(damageDoor());
    }



}
