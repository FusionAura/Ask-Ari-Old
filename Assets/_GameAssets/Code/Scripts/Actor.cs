using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem.EnhancedTouch;

public class Actor : MonoBehaviour
{
    public Transform currentDest;

    public List<Transform> Destinatons = new List<Transform>();

    protected NavMeshAgent navMeshA;

    protected bool wait = false;

    void Start()
    {
        navMeshA = GetComponent<NavMeshAgent>();
        SetDestination();
        currentDest = Destinatons[Random.Range(0, Destinatons.Count - 1)];

    }
    public virtual void SetDestination()
    {
        navMeshA.destination = currentDest.position;

        // Check if we've reached the destination
        if (!navMeshA.pathPending)
        {
            if (navMeshA.remainingDistance <= navMeshA.stoppingDistance)
            {
                if (!navMeshA.hasPath || navMeshA.velocity.sqrMagnitude == 0f)
                {
                    currentDest = Destinatons[Random.Range(0, Destinatons.Count - 1)];
                }
            }
        }
    }
}
