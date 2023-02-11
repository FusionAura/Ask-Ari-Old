
using System.Collections.Generic;
using UnityEngine;

public interface IInteractables
{
    void Interact(int index);

    List<string> ReturnPossibleActions();

    void OutOfRangeInteraction();

    void SetInteractionTarget(Transform target);
}
