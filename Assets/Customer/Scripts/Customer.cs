using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using System.Collections.Generic;

public class Customer : MonoBehaviour
{
    private NavMeshAgent agent;
    private Chair targetChair;
    private Restaurant rest;
    private Transform spawnPoint;
    private bool hasOrdered = false;
    private bool isWaiting = false;
    private bool isLeaving = false;
    private bool isEating = false;
    private float patience;
    [SerializeField] private float patienceTime = 10f; 
    private float waitStartTime;
    private float eatingTime = 5f; 
    private float destroyDelay = 5f;
    private Meal chosenMeal;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        if (agent == null)
        {
            Debug.LogError("NavMeshAgent component is missing on customer prefab.");
        }
    }

    public void MoveTo(Chair chair, Transform spawnPoint, Restaurant restaurant)
    {
        targetChair = chair;
        this.spawnPoint = spawnPoint;
        this.rest = restaurant;

        if (agent != null)
        {
            agent.SetDestination(chair.transform.position);
        }
        else
        {
            Debug.LogError("NavMeshAgent component is not initialized.");
        }
    }

    private void Update()
    {
        if (!hasOrdered && targetChair != null && agent.remainingDistance < 0.1f)
        {
            SitOnChair();
        }

        if (isWaiting)
        {
            patience = patienceTime - (Time.time - waitStartTime);
            if (patience <= 0)
            {
                rest.DecreaseReputation(5);
                Leave();
            }
        }

        if (isLeaving && spawnPoint != null && agent.remainingDistance < 0.1f)
        {
            StartCoroutine(DestroyAfterDelay());
        }
        
    }

    private void SitOnChair()
    {
        transform.position = targetChair.transform.position;
        transform.rotation = targetChair.transform.rotation;
        hasOrdered = true;
        isWaiting = true;
        waitStartTime = Time.time;

        GiveOrder();
    }

    private void GiveOrder()
    {
        List<Meal> allMeals = OvenManager.Instance.GetAllAvailableMeals();
        foreach (Meal meal in allMeals)
        {
            Debug.Log("Available meal: " + meal.name);
        }
        Debug.Log("Customer has given an order and is waiting.");
        chosenMeal = allMeals[Random.Range(0, allMeals.Count)];
        Debug.Log("Chosen Meal: " + chosenMeal.name);
    }

    public void ReceiveFood(Meal meal)
    {
        if (meal == chosenMeal)
        {
            StartCoroutine(EatAndLeave());
        }
        else
        {
            Debug.Log("Wrong meal! Customer is still waiting.");
            Leave();
        }
    }

    private IEnumerator EatAndLeave()
    {
        isWaiting = false;
        isEating = true;
        Debug.Log("Customer is eating.");
        yield return new WaitForSeconds(eatingTime);
        Debug.Log("Customer finished eating and is leaving.");
        isEating = false;
        if (rest != null)
        {
            rest.totalMoney += chosenMeal.price;
            rest.IncreaseReputation(5);
        }
        Leave();
    }

    private void Leave()
    {
        Debug.Log(targetChair);
        isWaiting = false;
        targetChair.IsOccupied = false;
        targetChair = null;
        
        isLeaving = true;

        if (agent != null && spawnPoint != null)
        {
            agent.SetDestination(spawnPoint.position);
        }
    }

    private IEnumerator DestroyAfterDelay()
    {
        yield return new WaitForSeconds(destroyDelay);
        Destroy(gameObject);
    }
}
