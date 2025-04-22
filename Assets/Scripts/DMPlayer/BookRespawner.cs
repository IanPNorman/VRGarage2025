using UnityEngine;


public class BookRespawner : MonoBehaviour
{
    public GameObject bookPrefab;
    private UnityEngine.XR.Interaction.Toolkit.Interactors.XRSocketInteractor socket;
    private float checkInterval = 3f;
    private float timer = 0f;

    void Awake()
    {
        socket = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactors.XRSocketInteractor>();
        if (socket == null)
            Debug.LogError("[BookRespawner] No XRSocketInteractor found on object: " + gameObject.name);
    }

    void Update()
    {
        if (socket == null) return;

        if (socket.hasSelection) return;

        timer += Time.deltaTime;
        if (timer >= checkInterval)
        {
            SpawnBook();
            timer = 0f;
        }
    }

    private void SpawnBook()
    {
        if (bookPrefab == null)
        {
            Debug.LogWarning("[BookRespawner] No book prefab assigned.");
            return;
        }

        Instantiate(bookPrefab, transform.position, transform.rotation);
    }
}
