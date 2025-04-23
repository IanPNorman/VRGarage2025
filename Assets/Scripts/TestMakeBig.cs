using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMakeBig : MonoBehaviour
{
    public GameObject XROrigin;
    void Start()
    {
        XROrigin.transform.localScale = new Vector3(5,5,5);
    }

   
}
