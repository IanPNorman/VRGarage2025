using UnityEngine;

public class TeleportNode : MonoBehaviour
{
    public TeleportNode leftNeighbor;
    public TeleportNode rightNeighbor;
    public Transform faceToward;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;

        if (leftNeighbor != null)
        {
            Gizmos.DrawLine(transform.position, leftNeighbor.transform.position);
            DrawArrow(transform.position, leftNeighbor.transform.position, Color.green);
        }

        if (rightNeighbor != null)
        {
            Gizmos.DrawLine(transform.position, rightNeighbor.transform.position);
            DrawArrow(transform.position, rightNeighbor.transform.position, Color.cyan);
        }

        if (faceToward != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(transform.position, faceToward.position);
            DrawArrow(transform.position, faceToward.position, Color.yellow);
        }
    }

    private void DrawArrow(Vector3 from, Vector3 to, Color color)
    {
        Gizmos.color = color;
        Vector3 direction = (to - from).normalized;
        Vector3 right = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 160, 0) * Vector3.forward;
        Vector3 left = Quaternion.LookRotation(direction) * Quaternion.Euler(0, -160, 0) * Vector3.forward;
        float arrowHeadLength = 0.3f;
        float arrowHeadAngle = 25.0f;

        Gizmos.DrawLine(to, to - direction * arrowHeadLength + right * 0.2f);
        Gizmos.DrawLine(to, to - direction * arrowHeadLength + left * 0.2f);
    }
}
