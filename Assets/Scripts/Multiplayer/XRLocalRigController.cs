using UnityEngine;
using Unity.Netcode;

public class XRLocalRigController : MonoBehaviour
{
    [Header("Role-Specific Objects")]
    public GameObject[] gameMasterObjects;
    public GameObject[] survivorObjects;

    public void ApplyRole(PlayerRole role)
    {
        Debug.Log($"[XRLocalRigController] Applying role: {role}");

        switch (role)
        {
            case PlayerRole.GameMaster:
                SetActiveArray(gameMasterObjects, true);
                SetActiveArray(survivorObjects, false);
                break;

            case PlayerRole.Survivor:
                SetActiveArray(gameMasterObjects, false);
                SetActiveArray(survivorObjects, true);
                break;

            default:
                SetActiveArray(gameMasterObjects, false);
                SetActiveArray(survivorObjects, false);
                break;
        }
    }

    private void SetActiveArray(GameObject[] objects, bool active)
    {
        foreach (var obj in objects)
        {
            if (obj != null)
                obj.SetActive(active);
        }
    }
}
