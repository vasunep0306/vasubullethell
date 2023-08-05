using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 22f;
    [SerializeField] private GameObject particleOnHitPrefabVFX;
    [SerializeField] private float projectileRange = 10f;
    [SerializeField] private bool isEnemyProjectile = false;

    private WeaponInfo weaponInfo;
    private Vector3 startPosition;

    private void Start()
    {
        startPosition = transform.position;
    }

    private void Update()
    {
        MoveProjectile();
        DetectFireDistance();
    }


    public void UpdateProjectileRange(float projectileRange)
    {
        this.projectileRange = projectileRange;
    }

    public void UpdateMoveSpeed(float moveSpeed)
    {
        this.moveSpeed = moveSpeed;
    }



    /// <summary>
    /// Handles the collision of the object with other objects.
    /// </summary>
    /// <param name="other">The other collider involved in the collision.</param>
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Get the EnemyHealth, Indestructable, and PlayerHealth components from the other object
        EnemyHealth enemyHealth = other.gameObject.GetComponent<EnemyHealth>();
        Indestructable indestructible = other.gameObject.GetComponent<Indestructable>();
        PlayerHealth player = other.gameObject.GetComponent<PlayerHealth>();

        // If the other object is not a trigger and has one of the above components
        if (!other.isTrigger && (enemyHealth || indestructible || player))
        {
            // If the other object is a player and this is an enemy projectile, or if the other object is an enemy and this is not an enemy projectile
            if ((player && isEnemyProjectile) || (enemyHealth && !isEnemyProjectile))
            {
                // Make the player take damage
                player?.TakeDamage(1, transform);

                // Instantiate a particle effect at this position
                Instantiate(particleOnHitPrefabVFX, transform.position, transform.rotation);

                // Destroy this object
                Destroy(gameObject);
            }
            // If the other object is indestructible
            else if (!other.isTrigger && indestructible)
            {
                // Instantiate a particle effect at this position
                Instantiate(particleOnHitPrefabVFX, transform.position, transform.rotation);

                // Destroy this object
                Destroy(gameObject);
            }
        }
    }


    private void MoveProjectile()
    {
        transform.Translate(Vector3.right * Time.deltaTime * moveSpeed);
    }

    private void DetectFireDistance()
    {
        if (Vector3.Distance(transform.position, startPosition) > projectileRange)
        {
            Destroy(gameObject);
        }
    }

}
