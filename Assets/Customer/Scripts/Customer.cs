using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class Customer : MonoBehaviour
{
    private NavMeshAgent agent;
    private Chair targetChair;
    private Transform spawnPoint;
    private bool hasOrdered = false;
    private bool isWaiting = false;
    private bool isLeaving = false;
    private float patience;
    private float patienceTime = 10f; // Time in seconds the customer will wait
    private float waitStartTime;
    private float destroyDelay = 5f; // Time in seconds before the customer is destroyed after reaching the spawn point

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        if (agent == null)
        {
            Debug.LogError("NavMeshAgent component is missing on customer prefab.");
        }
    }

    public void MoveTo(Chair chair, Transform spawnPoint)
    {
        targetChair = chair;
        this.spawnPoint = spawnPoint;
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

        // Trigger the order event
        GiveOrder();
    }

    private void GiveOrder()
    {
        Debug.Log("Customer has given an order and is waiting.");
        // Additional logic to handle the order can be placed here
    }

    private void Leave()
    {
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
