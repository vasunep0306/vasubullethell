using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour, IEnemy
{
    // Bullet prefab to be instantiated when the enemy attacks
    [SerializeField] private GameObject bulletPrefab;

    // Speed at which the bullet moves
    [SerializeField] private float bulletMoveSpeed;

    // Time the enemy rests after shooting a burst of bullets
    [SerializeField] private float restTime = 1f;

    // Number of bullets in a burst
    [SerializeField] private int burstCount;

    // Time between each bullet in a burst
    [SerializeField] private float timeBetweenBursts;

    // Flag to indicate if the enemy is currently shooting
    private bool isShooting = false;

    // Method to make the enemy attack the player
    public void Attack()
    {
        // If the enemy is not already shooting, start the ShootRoutine coroutine
        if (!isShooting)
        {
            StartCoroutine(ShootRoutine());
        }
    }

    // Coroutine to shoot a burst of bullets at the player
    private IEnumerator ShootRoutine()
    {
        // Set isShooting to true to indicate that the enemy is currently shooting
        isShooting = true;

        // Shoot a burst of bullets
        for (int i = 0; i < burstCount; i++)
        {
            // Calculate the direction from the enemy to the player
            Vector2 targetDirection = PlayerController.Instance.transform.position - transform.position;

            // Instantiate a new bullet at the enemy's position
            GameObject newBullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);

            // Set the bullet's direction towards the player
            newBullet.transform.right = targetDirection;

            // If the newBullet has a Projectile component, update its move speed
            if (newBullet.TryGetComponent(out Projectile projectile))
            {
                projectile.UpdateMoveSpeed(bulletMoveSpeed);
            }

            // Wait for timeBetweenBursts before shooting the next bullet
            yield return new WaitForSeconds(timeBetweenBursts);
        }

        // Wait for restTime before allowing the enemy to shoot again
        yield return new WaitForSeconds(restTime);

        // Set isShooting to false to indicate that the enemy has finished shooting
        isShooting = false;
    }
}
