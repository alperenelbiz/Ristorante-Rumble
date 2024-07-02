using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float lifeTime = 2f; // Time in seconds before the bullet is destroyed
    public float damage = 10f; // Amount of damage the bullet deals

    void Start()
    {
        Destroy(gameObject, lifeTime); // Destroy the bullet after its lifetime expires
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
        PlayerHealth playerHealth = collider.gameObject.GetComponent<PlayerHealth>();

            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
                Destroy(gameObject); // Destroy the bullet upon collision
            }
        

        else
        {
            Destroy(gameObject); // Destroy the bullet if it hits something else
        }
        }
    }
}
