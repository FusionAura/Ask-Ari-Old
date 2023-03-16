using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Appliance : Actor, IInteractables
{
    public void Interact(int index)
    {
        throw new System.NotImplementedException();
    }

    public void OutOfRangeInteraction()
    {
        throw new System.NotImplementedException();
    }

    public List<string> ReturnPossibleActions()
    {
        throw new System.NotImplementedException();
    }

    public void SetInteractionTarget(Transform target)
    {
        throw new System.NotImplementedException();
    }

}
