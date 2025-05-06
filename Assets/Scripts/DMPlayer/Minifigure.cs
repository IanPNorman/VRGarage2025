using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using TMPro;

public class Minifigure : MonoBehaviour
{
    public static event System.Action<Minifigure> OnGrabbed;
    public static event System.Action<Minifigure> OnReleased;

    public GameObject enemyPrefab;
    public int maxSpawns = 5;
    private int currentSpawnCount = 0;

    [Header("UI")]
    public TextMeshProUGUI spawnCountText;

    [Header("Mana Cost")]
    public int manaCost = 2;

    private UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable grab;
    private BoardSlot currentNearbySlot = null;
    private BoardSlot assignedSlot = null;

    private void Awake()
    {
        grab = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();
        grab.selectEntered.AddListener(_ => OnGrabbed?.Invoke(this));
        grab.selectExited.AddListener(OnReleasedHandler);

        UpdateSpawnText();
    }

    private void OnReleasedHandler(SelectExitEventArgs args)
    {
        OnReleased?.Invoke(this);

        if (currentNearbySlot != null && !currentNearbySlot.isFilled)
        {
           // bool success = ManaManager.Instance?.SpendMana(manaCost) ?? true;

          /*  if (!success)
            {
                Debug.Log("Not enough mana to place minifigure.");
                return;
            }
        */
            currentNearbySlot.TryAssignFigure(this);
            grab.enabled = false;
            assignedSlot = currentNearbySlot;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out BoardSlot slot))
        {
            currentNearbySlot = slot;
            slot.ShowHoverHighlight(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out BoardSlot slot) && slot == currentNearbySlot)
        {
            slot.ShowHoverHighlight(false);
            currentNearbySlot = null;
        }
    }

    // ✅ Updated to accept rotation
    public void SnapToSlot(Vector3 slotPosition, Quaternion rotation)
    {
        transform.position = slotPosition;
        transform.rotation = rotation;

        if (TryGetComponent(out Rigidbody rb))
        {
            rb.isKinematic = true;
            rb.useGravity = false;
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        if (grab != null)
        {
            grab.enabled = false;
        }
    }

    public GameObject TrySpawnEnemy(Vector3 spawnPosition)
    {
        if (currentSpawnCount >= maxSpawns)
        {
            return null;
        }

        GameObject spawned = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
        currentSpawnCount++;
        UpdateSpawnText();

        if (currentSpawnCount >= maxSpawns)
        {
            if (assignedSlot != null)
            {
                assignedSlot.isFilled = false;
                assignedSlot.assignedFigure = null;
            }

            Destroy(gameObject);
        }

        return spawned;
    }

    private void UpdateSpawnText()
    {
        if (spawnCountText != null)
        {
            spawnCountText.text = (maxSpawns - currentSpawnCount).ToString();
        }
    }
}
