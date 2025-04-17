using UnityEngine;

public class DoorHealth : MonoBehaviour
{
    public GameObject[] numBarricades;
    

    void Awake()
    {
        if (numBarricades == null || numBarricades.Length == 0)
        {
            // Fill array with direct children
            int count = transform.childCount;
            numBarricades = new GameObject[count];

            for (int i = 0; i < count; i++)
            {
                numBarricades[i] = transform.GetChild(i).gameObject;
            }

            Debug.Log(numBarricades[0]);
        }
    }


    public GameObject getBarricade(int num)
    {
        Debug.Log(numBarricades[num]);
        return numBarricades[num];   
    }
}
