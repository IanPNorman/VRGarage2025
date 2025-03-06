using System.Collections;
using System.Collections.Generic;
using Unity.Services.Analytics.Internal;
using UnityEngine;

public class SpawnEnemy : MonoBehaviour
{
    public Camera cam;

    public GameObject EnemyPrefab;

    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                Instantiate(EnemyPrefab, hit.point, EnemyPrefab.transform.rotation);
            }
        }

    }
}
