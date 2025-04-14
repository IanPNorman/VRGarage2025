using UnityEngine;

public class BoardSlot : MonoBehaviour
{
    public enum Side { North, South, East, West }
    public Side boardSide;

    [HideInInspector] public bool isFilled = false;

    private Renderer rend;
    private Material originalMat;
    public Material highlightMat;

    private void Awake()
    {
        rend = GetComponent<Renderer>();
        originalMat = rend.material;
    }

    public void Highlight(bool enable)
    {
        rend.material = enable ? highlightMat : originalMat;
    }
}
