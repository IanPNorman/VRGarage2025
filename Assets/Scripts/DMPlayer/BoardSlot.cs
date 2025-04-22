using UnityEngine;

public class BoardSlot : MonoBehaviour
{
    public enum Side { North, South, East, West }
    public Side boardSide;

    [Header("Visuals")]
    public Material highlightMat;
    public Material hoverMat;

    [Header("Placement Settings")]
    public Vector3 placementOffset = new Vector3(0, 0.05f, 0); // Adjust in Inspector

    private Material originalMat;
    private Renderer rend;

    public bool isFilled = false;
    public Minifigure assignedFigure;

    private void Awake()
    {
        rend = GetComponent<Renderer>();
        if (rend != null)
        {
            originalMat = rend.material;
        }
    }

    public void Highlight(bool on)
    {
        if (!isFilled)
        {
            rend.material = on ? highlightMat : originalMat;
             
        }
    }

    public void ShowHoverHighlight(bool on)
    {
        if (!isFilled)
        {
            rend.material = on ? hoverMat : highlightMat;
        }
    }

    public bool TryAssignFigure(Minifigure figure)
    {
        if (isFilled) return false;

        assignedFigure = figure;
        isFilled = true;

        // Use the placement offset to raise the figure
        figure.SnapToSlot(transform.position + placementOffset);
        ShowHoverHighlight(false);
        return true;
    }
}
