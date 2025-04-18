using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    public float waveInterval = 15f;
    public int enemiesPerMinifigure = 5;

    public List<BoardSlot> slots; // Assign via inspector or dynamically

    public Transform northZone;
    public Transform southZone;
    public Transform eastZone;
    public Transform westZone;

    private void Start()
    {
        Debug.Log("WaveSpawner: Start() called.");
        StartCoroutine(WaveLoop());
    }

    private IEnumerator WaveLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(waveInterval);
            SpawnWave();
        }
    }

    private void SpawnWave()
    {
        Debug.Log("WaveSpawner: Checking slots...");
        foreach (var slot in slots)
        {
            Debug.Log($"Checking slot: {slot.name} — Filled: {slot.isFilled}");
            if (slot.isFilled && slot.assignedFigure != null)
            {
                
                Debug.Log($"Slot {slot.name} is filled. Assigned enemy prefab: {slot.assignedFigure.enemyPrefab}");
                GameObject enemyPrefab = slot.assignedFigure.enemyPrefab;
                if (enemyPrefab == null) continue;

                Transform spawnZone = GetSpawnZone(slot.boardSide);
                if (spawnZone == null) continue;

                for (int i = 0; i < enemiesPerMinifigure; i++)
                {
                    Vector3 offset = Random.insideUnitSphere * 2f;
                    offset.y = 0;

                    Vector3 spawnPos = spawnZone.position + offset;

                    Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
                }
            }
        }
    }

    private Transform GetSpawnZone(BoardSlot.Side side)
    {
        switch (side)
        {
            case BoardSlot.Side.North: return northZone;
            case BoardSlot.Side.South: return southZone;
            case BoardSlot.Side.East: return eastZone;
            case BoardSlot.Side.West: return westZone;
            default: return null;
        }
    }

    #if UNITY_EDITOR
private void OnDrawGizmos()
{
    Gizmos.color = Color.red;
    if (northZone) Gizmos.DrawWireSphere(northZone.position, 2f);

    Gizmos.color = Color.blue;
    if (southZone) Gizmos.DrawWireSphere(southZone.position, 2f);

    Gizmos.color = Color.green;
    if (eastZone) Gizmos.DrawWireSphere(eastZone.position, 2f);

    Gizmos.color = Color.yellow;
    if (westZone) Gizmos.DrawWireSphere(westZone.position, 2f);
}
#endif

}
