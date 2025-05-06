using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;

public class StartButton : MonoBehaviour
{
    public Button startGameButton;
    public RoleUI roleUI;

    private void Start()
    {
        startGameButton.gameObject.SetActive(false);
        RoleManager.Instance.gameMasterCount.OnValueChanged += CheckStartCondition;
        RoleManager.Instance.survivorCount.OnValueChanged += CheckStartCondition;

        startGameButton.onClick.AddListener(() =>
        {
            RoleManager.Instance.StartGameServerRpc();
        });
    }

    private void CheckStartCondition(int oldVal, int newVal)
    {
        bool canStart = RoleManager.Instance.AllRolesAssigned;

        if (NetworkManager.Singleton.IsHost || NetworkManager.Singleton.IsServer)
        {
            startGameButton.gameObject.SetActive(canStart);
        }
    }
}
