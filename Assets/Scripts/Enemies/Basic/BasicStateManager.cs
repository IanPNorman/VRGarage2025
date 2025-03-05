using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BasicStateManager : MonoBehaviour
{
    public BasicState currentState;
    public BasicState BossRatDead;
    public bool isDead;
    void Update()
    {
        RunStateMachine();

    }

    private void RunStateMachine()
    {
        if (isDead)
        {
            SwitchToNextState(BossRatDead);
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
