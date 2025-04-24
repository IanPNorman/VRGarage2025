using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class bodySocket
{
    // Give the range of game object relative to headset
    public GameObject gameObject;
    [Range(0.01f, 1f)] 
    public float heightRatio;
}

public class BodySocketInventory : MonoBehaviour
{
    public GameObject HMD; // Head Mounted Display
    public bodySocket[] bodySockets;

    private Vector3 _currentHMDPosition;
    private Quaternion _currentHMDRotation;

    // Update is called once per frame
    void Update()
    {
        _currentHMDPosition = HMD.transform.position; // Get current position and rotation of headset
        _currentHMDRotation = HMD.transform.rotation;
        // Update bodySocket positions
        foreach(var bodySocket in bodySockets)
        {
            UpdateBodySocketHeight(bodySocket);
        }
        UpdateSocketInventory();
    }

    // Update bodySocket height
    private void UpdateBodySocketHeight(bodySocket bodySocket)
    {
        bodySocket.gameObject.transform.position = new Vector3(bodySocket.gameObject.transform.position.x, 
                                                                _currentHMDPosition.y * bodySocket.heightRatio,
                                                                bodySocket.gameObject.transform.position.z);
    }

    private void UpdateSocketInventory()
    {
        transform.position = new Vector3(_currentHMDPosition.x, 0, _currentHMDPosition.z); // No adjusting by y here as y is adjusted separately
        transform.rotation = new Quaternion(transform.rotation.x, _currentHMDRotation.y, transform.rotation.z, _currentHMDRotation.w); // Rotating along y axis
    }
}
