using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;

public class WaveSpawner : MonoBehaviour
{
    public float setupTime = 30f;
    public float countdownThreshold = 5f;
    public int enemiesPerMinifigure = 5;

    public TMP_Text countdownText;
    public GameObject countdownUI;
    public GameObject setupPhaseUI;

    public List<BoardSlot> slots;

    public Transform northZone;
    public Transform southZone;
    public Transform eastZone;
    public Transform westZone;

    private float roundTimer = 0f;
    private bool roundActive = false;
    private bool countdownShown = false;

    private void Start()
    {
        BeginSetupPhase();
    }

    private void Update()
    {
        if (!roundActive)
        {
            roundTimer += Time.deltaTime;
            float timeLeft = setupTime - roundTimer;

            if (timeLeft <= countdownThreshold && !countdownShown)
            {
                countdownShown = true;
                StartCoroutine(CountdownCoroutine((int)countdownThreshold));
            }

            if (timeLeft <= 0f)
            {
                StartWave();
            }
        }
        else
        {
            // Only end round if all enemies are dead
            if (GameObject.FindGameObjectsWithTag("Enemy").Length == 0)
            {
                BeginSetupPhase();
            }
        }
    }

    private void BeginSetupPhase()
    {
        Debug.Log("WaveSpawner: Setup Phase started.");
        ManaManager.Instance?.ResetManaForRound();
        roundActive = false;
        roundTimer = 0f;
        countdownShown = false;

        if (setupPhaseUI != null)
            setupPhaseUI.SetActive(true);
    }

    private void StartWave()
    {
        Debug.Log("WaveSpawner: Starting round.");
        roundActive = true;

        if (setupPhaseUI != null)
            setupPhaseUI.SetActive(false);

        SpawnWave();
    }

    private IEnumerator CountdownCoroutine(int seconds)
    {
        if (countdownUI != null) countdownUI.SetActive(true);

        for (int i = seconds; i > 0; i--)
        {
            if (countdownText != null)
                countdownText.text = i.ToString();

            yield return new WaitForSeconds(1f);
        }

        if (countdownUI != null) countdownUI.SetActive(false);
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

            StartCoroutine(SpawnFromSideSorted(sideSlots, spawnZone));
        }
    }

    private IEnumerator SpawnFromSideSorted(List<BoardSlot> sideSlots, Transform spawnZone)
    {
        var orderedSlots = sideSlots
            .Where(s => s.isFilled && s.assignedFigure != null)
            .OrderBy(s => s.name)
            .ToList();

        foreach (BoardSlot slot in orderedSlots)
        {
            Minifigure fig = slot.assignedFigure;

            for (int i = 0; i < enemiesPerMinifigure; i++)
            {
                if (fig == null) break;

                Transform start = spawnZone.Find("StartPoint");
                Transform end = spawnZone.Find("EndPoint");

                if (start == null || end == null)
                {
                    Debug.LogWarning($"Missing spawn line points under {spawnZone.name}");
                    yield break;
                }

                Vector3 spawnPos = GetRandomPointOnLine(start, end);
                GameObject spawned = fig.TrySpawnEnemy(spawnPos);

                if (spawned == null) break;

                yield return new WaitForSeconds(1.5f);
            }
        }
    }

    private Vector3 GetRandomPointOnLine(Transform start, Transform end)
    {
        float t = Random.Range(0f, 1f);
        return Vector3.Lerp(start.position, end.position, t);
    }

    private Transform GetSpawnZone(BoardSlot.Side side)
    {
        return side switch
        {
            BoardSlot.Side.North => northZone,
            BoardSlot.Side.South => southZone,
            BoardSlot.Side.East => eastZone,
            BoardSlot.Side.West => westZone,
            _ => null,
        };
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
