using UnityEngine;

public class DMUi : MonoBehaviour
{
    [Header("Target to face")]
    public Transform targetCamera;

    void LateUpdate()
    {
        if (targetCamera == null && Camera.main != null)
            targetCamera = Camera.main.transform;

        if (targetCamera != null)
        {
            Vector3 direction = transform.position - targetCamera.position;
            transform.rotation = Quaternion.LookRotation(direction);
        }
    }
}
