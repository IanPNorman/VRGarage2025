using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using static UnityEngine.XR.Interaction.Toolkit.Inputs.Haptics.HapticsUtility;

public class TriggerHaptic : MonoBehaviour
{
    public XRBaseController controller;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<XRBaseController>();
    }

    public void HapticFeedback(float amplitude, float duration)
    {
        if (controller != null)
        {
            controller.SendHapticImpulse(amplitude, duration);
        }
        else
        {
            Debug.LogWarning("XRBaseController not assigned to TriggerHaptic.");
        }
    }
}
