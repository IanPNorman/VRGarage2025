using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Finder : MonoBehaviour
{
    public GameObject[] allDoors;
    public GameObject[] allPlayers;

    void Start()
    {
        allDoors = GameObject.FindGameObjectsWithTag("Door");
        
    }

    private void Update()
    {
        allPlayers = GameObject.FindGameObjectsWithTag("Player");
    }

    public GameObject FindNearestDoor(Vector3 position)
    {
        return FindNearestObject(allDoors, position);
    }

    public GameObject FindNearestPlayer(Vector3 position)
    {
        return FindNearestObject(allPlayers, position);
    }

    private GameObject FindNearestObject(GameObject[] objects, Vector3 position)
    {
        if (objects == null || objects.Length == 0) return null;

        GameObject closest = objects[0];
        float minDistance = Vector3.Distance(position, closest.transform.position);

        for (int i = 1; i < objects.Length; i++)
        {
            float distance = Vector3.Distance(position, objects[i].transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                closest = objects[i];
            }
        }

        return closest;
    }
}
