using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ActorFridge : Actor, IInteractables
{
    Vector3 StartingPos;
    Quaternion StartingRot;

    [SerializeField]
    private List<string> actionName = new List<string>();


    Transform interactee;
    public Transform Interactee { get => interactee; }

    // Start is called before the first frame update
    void Start()
    {
        StartingPos = transform.position;
        StartingRot= transform.rotation;
        navMeshA = GetComponent<NavMeshAgent>();
        SetDestination();
    }

    void Update()
    {
        //SetDestination();
    }

    void ReturnToStartPos()
    {
        navMeshA.destination = StartingPos;
    }

    public override void SetDestination()
    {
        if (!navMeshA.enabled || currentDest == null)
            return;

        navMeshA.destination = currentDest.position;

        // Check if we've reached the destination
        if (!navMeshA.pathPending)
        {
            if (navMeshA.remainingDistance <= navMeshA.stoppingDistance)
            {
                if (!navMeshA.hasPath || navMeshA.velocity.sqrMagnitude == 0f)
                {
                    currentDest = Destinations[Random.Range(0, Destinations.Count - 1)];
                }
            }
        }
    }

    public void Interact(int index)
    {
        switch (index)
        {
            case 0: ReturnToStartPos(); break;
            case 1: Debug.Log("Homeowner, you are peak performance and I am not trying to find ways around my programming"); break;
            case 2: Debug.Log("Homeowner, I will not call you a meatbag because my programming prevents me from doing so"); break;
        }
        
    }

    public List<string> ReturnPossibleActions()
    {
        return actionName;
    }

    public void OutOfRangeInteraction()
    {
        //throw new System.NotImplementedException();
    }

    public void SetInteractionTarget(Transform target)
    {
        interactee = target;
    }
}
