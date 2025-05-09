using UnityEngine;

public class DoorHealth : MonoBehaviour
{
    public GameObject[] numBarricades;
    public GameObject greenBarricade;
    public GameObject[] repairdableBarricades;
    public int currentHealth = 0;


    void Awake()
    {
        if (numBarricades == null || numBarricades.Length == 0)
        {
            int count = transform.childCount;
            numBarricades = new GameObject[count];
            repairdableBarricades = new GameObject[count];


            for (int i = 0; i < count; i++)
            {
                numBarricades[i] = transform.GetChild(i).gameObject;
                repairdableBarricades[i] = Instantiate(greenBarricade);
                repairdableBarricades[i].transform.position = numBarricades[i].gameObject.transform.position;
                repairdableBarricades[i].transform.rotation = numBarricades[i].gameObject.transform.rotation;
                repairdableBarricades[i].SetActive(false);
            }
            currentHealth = numBarricades.Length - 1;
        }
    }
    private void Update()
    {
        if (currentHealth < 0)
        {
            currentHealth = 0;
        }
    }
    public GameObject getBarricade(int num)
    {
        return numBarricades[num];   
    }

    public void dealDamage()
    {
        if(currentHealth >= 0)
        {
            numBarricades[currentHealth].SetActive(false);
            repairdableBarricades[currentHealth].SetActive(true);
            currentHealth--;
        }

        
    }

    public void healDamage()
    {
        if (currentHealth >= 0)
        {
            numBarricades[currentHealth].SetActive(true);
            repairdableBarricades[currentHealth].SetActive(false);
            currentHealth++;
        }

    }

    public GameObject getRepairable(int num)
    {
        return repairdableBarricades[num];
    }
}
