using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class XROriginScaler : MonoBehaviour
{
    public GameObject xrOrigin; 
    public float scaleMultiplier = 6f;

    private void Start()
    {
        if (xrOrigin == null)
        {
            xrOrigin = GameObject.FindWithTag("XROrigin");

            if (xrOrigin == null)
            {
                Debug.LogError("XR Origin not assigned or found!");
                return;
            }
        }
        xrOrigin.transform.localScale = Vector3.one * scaleMultiplier;
    }
}
