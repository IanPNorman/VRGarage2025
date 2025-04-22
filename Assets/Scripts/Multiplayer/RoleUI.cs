using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;
using TMPro;

public class RoleUI : MonoBehaviour
{
    [Header("Buttons")]
    public Button gmJoinButton;
    public Button survJoinButton;

    [Header("Button Labels")]
    public TMP_Text gmButtonText;
    public TMP_Text survButtonText;

    private void Start()
    {
        // Subscribe to value changes to update UI across network
        RoleManager.Instance.gameMasterCount.OnValueChanged += UpdateUI;
        RoleManager.Instance.survivorCount.OnValueChanged += UpdateUI;

        // Button listeners
        gmJoinButton.onClick.AddListener(() => ChooseRole(PlayerRole.GameMaster));
        survJoinButton.onClick.AddListener(() => ChooseRole(PlayerRole.Survivor));

        // Initialize display
        UpdateUI(0, 0);
    }

    private void ChooseRole(PlayerRole role)
    {
        if (NetworkManager.Singleton.IsClient || NetworkManager.Singleton.IsHost)
        {
            RoleManager.Instance.RequestRoleServerRpc(NetworkManager.Singleton.LocalClientId, role);
        }
    }

    private void UpdateUI(int oldVal, int newVal)
    {
        int gm = RoleManager.Instance.gameMasterCount.Value;
        int sv = RoleManager.Instance.survivorCount.Value;

        gmButtonText.text = $"Game Master {gm}/1";
        survButtonText.text = $"Survivor {sv}/4";

        gmJoinButton.interactable = gm < RoleManager.MaxGameMasters;
        survJoinButton.interactable = sv < RoleManager.MaxSurvivors;
    }
}
