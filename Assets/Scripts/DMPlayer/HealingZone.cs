using UnityEngine;

public class HealingZone : MonoBehaviour
{
    public float duration = 5f;
    public float healPerSecond = 5f;
    public float radius = 3f;
    public LayerMask enemyLayer;

    private float timer = 0f;

    private void Update()
    {
        timer += Time.deltaTime;

        // Heal enemies within radius
        Collider[] hits = Physics.OverlapSphere(transform.position, radius, enemyLayer);
        foreach (var hit in hits)
        {
            //EnemyHealth enemy = hit.GetComponent<EnemyHealth>();
            //if (enemy != null)
           // {
                //enemy.Heal(healPerSecond * Time.deltaTime);
           // }
        }

        if (timer >= duration)
        {
            Destroy(gameObject);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
