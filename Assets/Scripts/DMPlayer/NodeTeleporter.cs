using UnityEngine;

public class NodeTeleporter : MonoBehaviour
{
    public TeleportNode currentNode;
    public Transform playerRig; // XR Rig root, usually under Camera Offset
    public float transitionSpeed = 5f;
    public float inputThreshold = 0.5f;

    private bool movedThisInput = false;

    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal"); // Keyboard or XR joystick

        if (!movedThisInput)
        {
            if (horizontalInput < -inputThreshold && currentNode.leftNeighbor != null)
            {
                MoveToNode(currentNode.leftNeighbor);
                movedThisInput = true;
            }
            else if (horizontalInput > inputThreshold && currentNode.rightNeighbor != null)
            {
                MoveToNode(currentNode.rightNeighbor);
                movedThisInput = true;
            }
        }

        // Reset when stick returns to center
        if (Mathf.Abs(horizontalInput) < 0.1f)
        {
            movedThisInput = false;
        }
    }

    void MoveToNode(TeleportNode targetNode)
    {
        playerRig.position = targetNode.transform.position;

        // Face the center
        if (targetNode.faceToward != null)
        {
            Vector3 lookDirection = (targetNode.faceToward.position - playerRig.position);
            lookDirection.y = 0;
            if (lookDirection != Vector3.zero)
                playerRig.forward = lookDirection.normalized;
        }

        currentNode = targetNode;
    }
}
