using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ActorHuman : Actor, IInteractables
{
    [SerializeField]
    private List<string> tempHumanInteractions = new List<string>();
    
    private float Hunger = 0, Comfort = 0, Recreation = 0;
    [SerializeField] float HungerMax = 25, ComfortMax = 25, RecreationMax = 25;
    [SerializeField] float HungerMin = 5, ComfortMin = 5, RecreationMin = 5;

    Transform interactee;

    [SerializeField] Transform FoodLocation;

    [SerializeField] Transform RelaxPoint;

    [SerializeField] Transform CallPoint;

    public Transform Interactee { get => interactee;}



    // Start is called before the first frame update
    void Start()
    {
        navMeshA = GetComponent<NavMeshAgent>();
        SetDestination();
        currentDest = Destinatons[Random.Range(0, Destinatons.Count - 1)];
        Hunger = Random.Range(HungerMin, HungerMax);
        Comfort = Random.Range(ComfortMin, ComfortMax);
        Recreation = Random.Range(RecreationMin,RecreationMax);
    }

    void Update()
    {
        if (Hunger <= 0)
        {
            currentDest = FoodLocation;
            Hunger = Random.Range(HungerMin, HungerMax);
        }
        else
            Hunger -= Time.deltaTime;

        if (Comfort <= 0)
        {
            currentDest = RelaxPoint;
            Comfort = Random.Range(ComfortMin, ComfortMax);
        }
        else
            Comfort -= Time.deltaTime;


        if (Recreation <= 0)
        {
            currentDest = CallPoint;
            Recreation = Random.Range(RecreationMin, RecreationMax);
        }
        else
            Recreation -= Time.deltaTime;

        SetDestination();
    }

    void LookToHologram()
    {
        if (!navMeshA.enabled)
            return;

        transform.LookAt(interactee);
    }

    public void Interact(int index)
    {
        switch (index)
        {
            case 0: Debug.Log("Homeowner, you are very intelligent."); break;
            case 1: Debug.Log("Homeowner, you are peak performance and I am not trying to find ways around my programming"); break;
            case 2: Debug.Log("Homeowner, I will not call you a meatbag because my programming prevents me from doing so"); break;
        }
    }
    public List<string> ReturnPossibleActions()
    {
        return tempHumanInteractions;
    }

    public override void SetDestination()
    {
        if (!navMeshA.enabled)
            return;

        navMeshA.destination = currentDest.position;

        // Check if we've reached the destination
        if (!navMeshA.pathPending)
        {
            if (navMeshA.remainingDistance <= navMeshA.stoppingDistance)
            {
                if (!navMeshA.hasPath || navMeshA.velocity.sqrMagnitude == 0f )
                {
                    currentDest = Destinatons[Random.Range(0, Destinatons.Count - 1)];
                }
            }
        }
    }

    public void OutOfRangeInteraction()
    {
        navMeshA.enabled = true;
    }

    public void SetInteractionTarget(Transform target)
    {
        interactee = target;
        navMeshA.enabled = false;
    }
}
