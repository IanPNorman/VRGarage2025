using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ConnectButtonHandler : MonoBehaviour
{
    public Button connectButton;
    public string lobbySceneName = "LobbyScene"; 
    private void Start()
    {
        connectButton.onClick.AddListener(() =>
        {
            SceneManager.LoadScene(lobbySceneName);
        });
    }
}
