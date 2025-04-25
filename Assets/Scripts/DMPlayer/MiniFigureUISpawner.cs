using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class MiniFigureUISpawner : MonoBehaviour
{
    [Header("Prefab to spawn when selected")]
    public GameObject miniFigurePrefab;

    [Header("Mana Cost of the Minifigure")]
    public int manaCost = 2;

    [Header("Optional: spawn offset (else spawns in hand)")]
    public Transform spawnPoint;

    private UnityEngine.XR.Interaction.Toolkit.Interactables.XRBaseInteractable interactable;
    private UnityEngine.XR.Interaction.Toolkit.Interactors.IXRHoverInteractor currentInteractor;

    private void Awake()
    {
        interactable = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRBaseInteractable>();
    }

    private void OnEnable()
    {
        if (interactable != null)
        {
            interactable.hoverEntered.AddListener(OnHoverEntered);
            interactable.hoverExited.AddListener(OnHoverExited);
            interactable.selectEntered.AddListener(OnSelectEntered);
        }
    }

    private void OnDisable()
    {
        if (interactable != null)
        {
            interactable.hoverEntered.RemoveListener(OnHoverEntered);
            interactable.hoverExited.RemoveListener(OnHoverExited);
            interactable.selectEntered.RemoveListener(OnSelectEntered);
        }
    }

    private void OnHoverEntered(HoverEnterEventArgs args)
    {
        currentInteractor = args.interactorObject;
    }

    private void OnHoverExited(HoverExitEventArgs args)
    {
        currentInteractor = null;
    }

    private void OnSelectEntered(SelectEnterEventArgs args)
    {
        if (miniFigurePrefab == null || args.interactorObject == null)
        {
            Debug.LogWarning("[MiniFigureUISpawner] Prefab or interactor missing.");
            return;
        }

        // ✅ Check for mana before spawning
        if (ManaManager.Instance != null && ManaManager.Instance.GetCurrentMana() < manaCost)
        {
            Debug.Log("[MiniFigureUISpawner] Not enough mana to spawn.");
            return;
        }

        // ✅ Spend mana
        ManaManager.Instance?.SpendMana(manaCost);

        // Spawn at hand location or optional spawn point
        Transform handTransform = args.interactorObject.transform;
        Vector3 spawnPos = spawnPoint != null ? spawnPoint.position : handTransform.position;
        Quaternion spawnRot = spawnPoint != null ? spawnPoint.rotation : handTransform.rotation;

        GameObject spawned = Instantiate(miniFigurePrefab, spawnPos, spawnRot);

        if (spawned.TryGetComponent(out UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable grabInteractable))
        {
            UnityEngine.XR.Interaction.Toolkit.Interactors.XRBaseInteractor baseInteractor = args.interactorObject as UnityEngine.XR.Interaction.Toolkit.Interactors.XRBaseInteractor;
            var interactionManager = baseInteractor?.interactionManager;

            if (interactionManager != null)
            {
                interactionManager.SelectEnter(
                    (UnityEngine.XR.Interaction.Toolkit.Interactors.IXRSelectInteractor)args.interactorObject,
                    (UnityEngine.XR.Interaction.Toolkit.Interactables.IXRSelectInteractable)grabInteractable
                );
            }
            else
            {
                Debug.LogWarning("[MiniFigureUISpawner] InteractionManager not found.");
            }
        }
        else
        {
            Debug.LogWarning("[MiniFigureUISpawner] Spawned prefab missing XRGrabInteractable.");
        }
    }
}
