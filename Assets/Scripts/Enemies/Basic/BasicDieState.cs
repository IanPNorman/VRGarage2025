using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class BasicDieState : BasicState
{
    private bool animationStarted = false;
    private Coroutine deathCoroutine;

    public GameObject enemyObject;
    public float rotationSpeed = 90f; 

    public override BasicState RunCurrentState()
    {
        if (!animationStarted)
        {
            animationStarted = true;
            deathCoroutine = StartCoroutine(PlayDeathAnimation());
        }

        return this;
    }

    private IEnumerator PlayDeathAnimation()
    {
        float currentX = enemyObject.transform.eulerAngles.x;
        float targetX = 90f;

        while (Mathf.Abs(Mathf.DeltaAngle(currentX, targetX)) > 0.5f)
        {
            currentX = Mathf.MoveTowardsAngle(currentX, targetX, rotationSpeed * Time.deltaTime);
            Vector3 currentEuler = enemyObject.transform.eulerAngles;
            enemyObject.transform.eulerAngles = new Vector3(currentX, currentEuler.y, currentEuler.z);
            yield return null;
        }


        yield return new WaitForSeconds(1f);

        Destroy(enemyObject);
    }
}
