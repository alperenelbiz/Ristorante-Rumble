using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CustomerSpawner : MonoBehaviour
{
    public GameObject customerPrefab;
    public Restaurant[] restaurants;
    public Transform[] spawnPoints;

    private void Start()
    {
      
        StartCoroutine(SpawnCustomers());
    }

    private IEnumerator SpawnCustomers()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(1f, 5f));
            if (AnyEmptyChairs())
            {
                SpawnCustomer();
            }
            else
            {
                Debug.Log("All chairs are occupied. Stopping customer spawn.");
                break;
            }
        }
    }

    private void SpawnCustomer()
    {
        Chair emptyChair = GetRandomEmptyChair();

        if (emptyChair != null)
        {
            Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
            GameObject customer = Instantiate(customerPrefab, spawnPoint.position, Quaternion.identity);
            customer.GetComponent<Customer>().MoveTo(emptyChair);
            emptyChair.IsOccupied = true;

            Debug.Log("Customer spawned and moving to chair: " + emptyChair.name);
        }
        else
        {
            Debug.Log("No empty chairs available.");
        }
    }

    private bool AnyEmptyChairs()
    {
        foreach (Restaurant restaurant in restaurants)
        {
            if (restaurant.GetEmptyChair() != null)
            {
                return true;
            }
        }
        return false;
    }

    private Chair GetRandomEmptyChair()
    {
        List<Chair> emptyChairs = new List<Chair>();

        foreach (Restaurant restaurant in restaurants)
        {
            foreach (Chair chair in restaurant.chairs)
            {
                if (!chair.IsOccupied)
                {
                    emptyChairs.Add(chair);
                }
            }
        }

        if (emptyChairs.Count > 0)
        {
            return emptyChairs[Random.Range(0, emptyChairs.Count)];
        }
        return null;
    }
}
