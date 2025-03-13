using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicBreakDoorState : BasicState
{
    public bool atDoor;
    public BasicState BasicInitialMoveState;


    public override BasicState RunCurrentState()
    {
        if (atDoor)
        {

            return BasicInitialMoveState;
        }
        else
        {
            // Continue to break door until its health is 0, then break the door and have enemy enter through the door.
            // Probably play animations on repeat every x seconds, then do 1 damage. 
            return this;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            atDoor = true;
        }
    }
}
