using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Gun : NetworkBehaviour
{
    public Transform bulletSpawn; // The position where the bullet will spawn
    public GameObject bulletPrefab; // The bullet prefab
    public float bulletSpeed = 20f; // The speed of the bullet
    public int maxAmmo = 10; // Maximum ammo capacity
    public float reloadTime = 2f; // Time it takes to reload
    [SerializeField] KeyCode fireKey = KeyCode.Mouse0;

    private int currentAmmo;
    private bool isReloading = false;

    void Start()
    {
        currentAmmo = maxAmmo; // Initialize the current ammo to max ammo at the start
        NetworkClient.RegisterPrefab(bulletPrefab); // Register the bullet prefab
    }

    void Update()
    {
        if (!isLocalPlayer)
        {
            return; // Ensure only the local player can shoot
        }

        if (isReloading)
        {
            return;
        }

        if (currentAmmo <= 0)
        {
            StartCoroutine(Reload());
            return;
        }

        if (Input.GetKeyDown(fireKey))
        {
            CmdShoot();
        }
    }

    [Command]
    void CmdShoot()
    {
        Shoot();
    }

    void Shoot()
    {
        currentAmmo--;

        GameObject bullet = Instantiate(bulletPrefab, bulletSpawn.position, bulletSpawn.rotation);
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("No Rigidbody found on the bullet prefab.");
            return;
        }

        rb.velocity = bulletSpawn.forward * bulletSpeed; // Apply initial velocity
        NetworkServer.Spawn(bullet); // Ensure the bullet is spawned on the server
    }

    IEnumerator Reload()
    {
        isReloading = true;
        Debug.Log("Reloading...");

        yield return new WaitForSeconds(reloadTime);

        currentAmmo = maxAmmo;
        isReloading = false;

        Debug.Log("Reloaded. Current ammo: " + currentAmmo);
    }
}
