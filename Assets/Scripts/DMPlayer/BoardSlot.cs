using UnityEngine;

public class BoardSlot : MonoBehaviour
{
    public enum Side { North, South, East, West }
    public Side boardSide;

    [Tooltip("Offset added to the position when snapping a minifigure")]
    public Vector3 placementOffset = new Vector3(0f, 0.05f, 0f);

    [Tooltip("The rotation (in degrees) the figure should face when placed")]
    public Vector3 placementRotation = Vector3.zero;

    public Material highlightMat;
    public Material hoverMat;

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
        if (!isFilled && rend != null)
        {
            rend.material = on ? highlightMat : originalMat;
        }
    }

    public void ShowHoverHighlight(bool on)
    {
        if (!isFilled && rend != null)
        {
            rend.material = on ? hoverMat : highlightMat;
        }
    }

    public bool TryAssignFigure(Minifigure figure)
    {
        if (isFilled) return false;

        assignedFigure = figure;
        isFilled = true;

        Vector3 finalPos = transform.position + placementOffset;
        Quaternion finalRot = Quaternion.Euler(placementRotation);
        figure.SnapToSlot(finalPos, finalRot);

        ShowHoverHighlight(false);
        return true;
    }
}
