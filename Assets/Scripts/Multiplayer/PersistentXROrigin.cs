using UnityEngine;
using UnityEngine.SceneManagement;

public class PersistentXROrigin : MonoBehaviour
{
    private static PersistentXROrigin instance;

    private void Awake()
    {
        // Destroy duplicate XR Origins if one already exists
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // In your persistent XR Origin script
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "DemoMap")
        {
            transform.position = new Vector3(0f, 0f, 0f); 
        }
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }



}
