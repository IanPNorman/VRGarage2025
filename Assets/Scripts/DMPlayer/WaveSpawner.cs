using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


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
       // Debug.Log("WaveSpawner: Start() called.");
        StartCoroutine(WaveLoop());
    }

    private IEnumerator WaveLoop()
    {
        while (true)
        {
          //  Debug.Log("WaveSpawner: Waiting for next wave...");
            yield return new WaitForSeconds(waveInterval);
         //   Debug.Log("WaveSpawner: Spawning wave now.");
            SpawnWave();
        }
    }

    private void SpawnWave()
{
    Dictionary<BoardSlot.Side, List<BoardSlot>> groupedSlots = new();

    foreach (BoardSlot slot in slots)
    {
        if (!groupedSlots.ContainsKey(slot.boardSide))
            groupedSlots[slot.boardSide] = new List<BoardSlot>();

        groupedSlots[slot.boardSide].Add(slot);
    }

    foreach (var kvp in groupedSlots)
    {
        List<BoardSlot> sideSlots = kvp.Value;
        Transform spawnZone = GetSpawnZone(kvp.Key);
        if (spawnZone == null || spawnZone.childCount == 0) continue;

        // Start coroutine for each side
        StartCoroutine(SpawnFromSideSorted(sideSlots, spawnZone));
    }
}


private IEnumerator SpawnFromSideSorted(List<BoardSlot> sideSlots, Transform spawnZone)
{
    // Filter and order slots by name or a slot ID
    var orderedSlots = sideSlots
        .Where(s => s.isFilled && s.assignedFigure != null)
        .OrderBy(s => s.name) // or s.slotNumber if available
        .ToList();

    foreach (BoardSlot slot in orderedSlots)
    {
        Minifigure fig = slot.assignedFigure;

        for (int i = 0; i < enemiesPerMinifigure; i++)
        {
            if (fig == null) break;

            int index = Random.Range(0, spawnZone.childCount);
            Transform spawnPoint = spawnZone.GetChild(index);
            GameObject spawned = fig.TrySpawnEnemy(spawnPoint.position);

            if (spawned == null) break;

            yield return new WaitForSeconds(1.5f);
        }
    }
}


    private IEnumerator SpawnEnemiesWithDelay(Minifigure figure, Transform spawnZone, int count)
{
    for (int i = 0; i < count; i++)
    {
        if (figure == null) yield break;

        int index = Random.Range(0, spawnZone.childCount);
        Transform spawnPoint = spawnZone.GetChild(index);
        Vector3 spawnPos = spawnPoint.position;

        GameObject spawned = figure.TrySpawnEnemy(spawnPos);
        if (spawned == null)
        {
            // The figure is out of spawns and destroyed itself
            yield break;
        }

        yield return new WaitForSeconds(1.5f);
    }
}

    private Vector3 GetRandomPointOnLine(Transform start, Transform end)
{
    float t = Random.Range(0f, 1f);
    return Vector3.Lerp(start.position, end.position, t);
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
