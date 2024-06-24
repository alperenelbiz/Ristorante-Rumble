using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CustomerSpawner : MonoBehaviour
{
    public GameObject customerPrefab;
    public Restaurant[] restaurants;
    public Transform[] spawnPoints;
    private bool stopSpawning = false;

    private void Start()
    {
        if (customerPrefab == null || restaurants.Length == 0 || spawnPoints.Length == 0)
        {
            Debug.LogError("Missing references in CustomerSpawner script!");
            return;
        }

        StartCoroutine(SpawnCustomers());
    }

    private IEnumerator SpawnCustomers()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(1f, 5f));
            if (!stopSpawning)
            {
                if (AnyEmptyChairs())
                {
                    SpawnCustomer();
                }
                else
                {
                    Debug.Log("All chairs are occupied.");
                    stopSpawning = true;
                }
            }
            else
            {
                // Check again if any chairs are free
                if (AnyEmptyChairs())
                {
                    stopSpawning = false;
                }
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
            Restaurant restaurant = FindRestaurantWithChair(emptyChair);
            customer.GetComponent<Customer>().MoveTo(emptyChair, spawnPoint, restaurant);
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

    private Restaurant FindRestaurantWithChair(Chair chair)
    {
        foreach (Restaurant restaurant in restaurants)
        {
            foreach (Chair restChair in restaurant.chairs)
            {
                if (restChair == chair)
                {
                    return restaurant;
                }
            }
        }
        return null;
    }
}
