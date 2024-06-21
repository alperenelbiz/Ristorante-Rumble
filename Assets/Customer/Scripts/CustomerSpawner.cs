using UnityEngine;
using System.Collections;

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
                Debug.Log("doldu");
                break;
            }
        }
    }

    private void SpawnCustomer()
    {
        Restaurant selectedRestaurant = restaurants[Random.Range(0, restaurants.Length)];
        Chair emptyChair = selectedRestaurant.GetEmptyChair();

        if (emptyChair != null)
        {
            Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
            GameObject customer = Instantiate(customerPrefab, spawnPoint.position, Quaternion.identity);
            customer.GetComponent<Customer>().MoveTo(emptyChair);
            emptyChair.IsOccupied = true;

            Debug.Log("müþteri þuna gidiyor: " + emptyChair.name);
        }
        else
        {
            Debug.Log("sandalye kalmadý");
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
}
