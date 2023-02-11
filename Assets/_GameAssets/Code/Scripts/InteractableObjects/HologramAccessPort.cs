using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HologramAccessPort : MonoBehaviour, IInteractables
{
    [SerializeField]
    private PlayerController playerController;

    [SerializeField]
    private List<string> actionName = new List<string>();

    [SerializeField] Transform interactee;

    void Awake()
    {
        playerController = (PlayerController)FindObjectOfType(typeof(PlayerController));
    }

    public void Interact(int index)
    {
        switch (index)
        {
            case 0: SwitchToOSMode(); break;
            case 1: Debug.Log("Return to sender"); break;
            case 2: Debug.Log("Repairing Port"); break;
        }
    }
    
    void SwitchToOSMode()
    {
        playerController.ForceOSMode();
    }

    public List<string> ReturnPossibleActions()
    {
        return actionName;
    }

    public void OutOfRangeInteraction()
    {
        
    }

    public void SetInteractionTarget(Transform target)
    {
        interactee = target;
    }
}
