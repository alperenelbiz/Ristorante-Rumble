using UnityEngine;
using Mirror;

public class Bullet : NetworkBehaviour
{
    public float lifeTime = 2f; // Time in seconds before the bullet is destroyed
    public float damage = 10f; // Amount of damage the bullet deals
    public float bulletSpeed = 20f; // Speed of the bullet

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("No Rigidbody found on the bullet.");
            return;
        }

        // Ensure the Rigidbody is set up correctly
        rb.isKinematic = false; // Enable physics interactions
        rb.velocity = transform.forward * bulletSpeed; // Apply initial velocity

        // Destroy the bullet after its lifetime expires
        Destroy(gameObject, lifeTime);
    }

    [ServerCallback]
    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            PlayerHealth playerHealth = collider.gameObject.GetComponent<PlayerHealth>();

            if (playerHealth != null)
            {
                //playerHealth.TakeDamage(damage);
                Destroy(gameObject); // Destroy the bullet upon collision
            }
        }
        else
        {
            Destroy(gameObject); // Destroy the bullet if it hits something else
        }
    }

    [ServerCallback]
    void FixedUpdate()
    {
        if (!isServer) return;

        // Update bullet position and velocity on the server
        rb.velocity = transform.forward * bulletSpeed;
    }
}
