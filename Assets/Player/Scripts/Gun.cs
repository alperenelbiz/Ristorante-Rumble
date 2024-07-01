using UnityEngine;
using System.Collections;

public class Gun : MonoBehaviour
{
    public Transform bulletSpawn; // The position where the bullet will spawn
    public GameObject bulletPrefab; // The bullet prefab
    public float bulletSpeed = 20f; // The speed of the bullet
    public int maxAmmo = 10; // Maximum ammo capacity
    public float reloadTime = 2f; // Time it takes to reload

    private int currentAmmo;
    private bool isReloading = false;

    void Start()
    {
        currentAmmo = maxAmmo; // Initialize the current ammo to max ammo at the start
    }

    void Update()
    {
        if (isReloading)
            return;

        if (currentAmmo <= 0)
        {
            StartCoroutine(Reload());
            return;
        }

        if (Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }
    }

    void Shoot()
    {
        currentAmmo--;

        GameObject bullet = Instantiate(bulletPrefab, bulletSpawn.position, bulletSpawn.rotation);
        bullet.layer = LayerMask.NameToLayer("Bullet"); // Set the bullet's layer to "Bullet"
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        rb.velocity = bulletSpawn.forward * bulletSpeed;

        Debug.Log("Shot fired. Current ammo: " + currentAmmo);
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
