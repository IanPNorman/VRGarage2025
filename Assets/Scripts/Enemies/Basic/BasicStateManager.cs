using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BasicStateManager : MonoBehaviour
{
    public BasicState currentState;
    public BasicState basicDieState;
    public bool isDead;

    public HealthHandler healthHandler;
    void Update()
    {
        RunStateMachine();
        if (healthHandler.CurrentHealth <= 0)
        {
            isDead = true;
        }
    }

    private void RunStateMachine()
    {
        if (isDead)
        {
            SwitchToNextState(basicDieState);
        }
        BasicState nextState = currentState?.RunCurrentState();

        if (nextState != null)
        {
            SwitchToNextState(nextState);
        }

    }

    private void SwitchToNextState(BasicState nextState)
    {
        currentState = nextState;
    }
}
