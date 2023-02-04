using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Actor : MonoBehaviour
{
    private float Hunger = 0, Comfort = 0, Recreation = 0;
    public float HungerMax = 25, ComfortMax = 25, RecreationMax = 25;
    public float HungerMin = 5, ComfortMin = 5, RecreationMin = 5;

    public Transform currentDest;

    public List<Transform> Destinatons = new List<Transform>();

    public Transform FoodLocation;

    public Transform RelaxPoint;

    public Transform CallPoint;

    NavMeshAgent navMeshA;
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

    void SetDestination()
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

    // Update is called once per frame
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
}
