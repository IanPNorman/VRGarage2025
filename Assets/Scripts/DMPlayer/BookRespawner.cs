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
    }

    void Update()
    {
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
        GameObject clone = Instantiate(bookPrefab, transform.position, transform.rotation);
    }
}
