using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicInitialMoveState : BasicState
{
    public bool atDoor;
    public BasicState BasicBreakDoorState;


    public override BasicState RunCurrentState()
    {
        if (atDoor)
        {

            return BasicBreakDoorState;
        }
        else
        {
            return this;
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            atDoor = true;
        }
    }
}
