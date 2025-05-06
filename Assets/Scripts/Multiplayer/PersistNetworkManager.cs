using UnityEngine;

public class PersistNetworkManager : MonoBehaviour
{
    private static PersistNetworkManager instance;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }
}
