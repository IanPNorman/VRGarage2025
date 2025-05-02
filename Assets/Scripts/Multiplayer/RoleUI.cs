using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;
using TMPro;
using UnityEngine.SceneManagement;

public class RoleUI : MonoBehaviour
{
    [Header("Buttons")]
    public Button gmJoinButton;
    public Button survJoinButton;
    public Button startGameButton;

    [Header("Button Labels")]
    public TMP_Text gmButtonText;
    public TMP_Text survButtonText;

    private void Start()
    {
        RoleManager.Instance.gameMasterCount.OnValueChanged += OnRoleChanged;
        RoleManager.Instance.survivorCount.OnValueChanged += OnRoleChanged;

        gmJoinButton.onClick.AddListener(() => ChooseRole(PlayerRole.GameMaster));
        survJoinButton.onClick.AddListener(() => ChooseRole(PlayerRole.Survivor));

        UpdateUI();
        UpdateStartGameButton(); 
    }

    private void ChooseRole(PlayerRole role)
    {
        if (NetworkManager.Singleton.IsClient || NetworkManager.Singleton.IsHost)
        {
            RoleManager.Instance.RequestRoleServerRpc(NetworkManager.Singleton.LocalClientId, role);
        }
    }

    private void OnRoleChanged(int oldVal, int newVal)
    {
        UpdateUI();
        UpdateStartGameButton();
    }

    private void UpdateUI()
    {
        int gm = RoleManager.Instance.gameMasterCount.Value;
        int sv = RoleManager.Instance.survivorCount.Value;

        gmButtonText.text = $"Game Master {gm}/1";
        survButtonText.text = $"Survivor {sv}/4";

        gmJoinButton.interactable = gm < RoleManager.MaxGameMasters;
        survJoinButton.interactable = sv < RoleManager.MaxSurvivors;
    }

    private void UpdateStartGameButton()
    {
        //bool canStart = RoleManager.Instance.gameMasterCount.Value == 1 &&
        //                RoleManager.Instance.survivorCount.Value >= 1;
        bool canStart = true;
        startGameButton.gameObject.SetActive(canStart);
        startGameButton.interactable = canStart;
    }

    public void StartGame()
    {
        if (NetworkManager.Singleton.IsServer)
        {
            NetworkManager.Singleton.SceneManager.LoadScene("DemoMap", LoadSceneMode.Single);
        }
        else
        {
            Debug.LogWarning("Only the host can start the game.");
        }
    }
}
