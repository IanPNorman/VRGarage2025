using UnityEngine;

public class FollowBook : MonoBehaviour
{
    public Transform target;
    public Vector3 positionOffset;
    public Vector3 rotationOffset;

    void LateUpdate()
    {
        if (target == null) return;

        transform.position = target.position + target.TransformVector(positionOffset);
        transform.rotation = target.rotation * Quaternion.Euler(rotationOffset);
    }
}
