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
