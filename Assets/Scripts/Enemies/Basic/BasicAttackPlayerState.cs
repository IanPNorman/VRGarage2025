using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class BasicAttackPlayerState : BasicState
{
    public bool finishedAttack;
    public BasicState BasicHuntPlayer;


    private Coroutine attackCoroutine;
    private BoxCollider attackCollider;

    public float attackDuration = 1.5f;

    public override BasicState RunCurrentState()
    {
        if (finishedAttack)
        {
            finishedAttack = false;
            return BasicHuntPlayer;
        }
        else
        {
            if (attackCoroutine == null)
            {
                attackCoroutine = StartCoroutine(PerformAttack());
            }

            return this;
        }
    }

    void Start()
    {
        attackCollider = GetComponent<BoxCollider>();

        if (attackCollider != null)
            attackCollider.enabled = false;
    }

    private IEnumerator PerformAttack()
    {
        if (attackCollider != null)
            attackCollider.enabled = true;

        yield return new WaitForSeconds(attackDuration);

        if (attackCollider != null)
            attackCollider.enabled = false;

        finishedAttack = true;
        attackCoroutine = null;
    }

}
