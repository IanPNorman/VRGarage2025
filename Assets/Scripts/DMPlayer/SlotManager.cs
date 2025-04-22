using System.Collections.Generic;
using UnityEngine;

public class BoardSlotManager : MonoBehaviour
{
    public BoardSlot.Side side;
    public List<BoardSlot> slots = new();

    private void OnEnable()
    {
        Minifigure.OnGrabbed += HandleMinifigureGrabbed;
        Minifigure.OnReleased += HandleMinifigureReleased;
    }

    private void OnDisable()
    {
        Minifigure.OnGrabbed -= HandleMinifigureGrabbed;
        Minifigure.OnReleased -= HandleMinifigureReleased;
    }

    private void HandleMinifigureGrabbed(Minifigure figure)
    {
        foreach (var slot in slots)
        {
            if (!slot.isFilled && slot.boardSide == side)
                slot.Highlight(true);
        }
    }

    private void HandleMinifigureReleased(Minifigure figure)
    {
        foreach (var slot in slots)
        {
            slot.Highlight(false);
        }
    }
}
