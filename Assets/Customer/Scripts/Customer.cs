using UnityEngine;
using UnityEngine.AI;

public class Customer : MonoBehaviour
{
    private NavMeshAgent agent;
    private Chair targetChair;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        
    }

    public void MoveTo(Chair chair)
    {
        targetChair = chair;
        if (agent != null)
        {
            agent.SetDestination(chair.transform.position);
        }
       
    }

    private void Update()
    {
        if (targetChair != null && agent.remainingDistance < 0.1f)
        {
            SitOnChair();
        }
    }

    private void SitOnChair()
    {
        transform.position = targetChair.transform.position;
        transform.rotation = targetChair.transform.rotation;
        
    }
}
