using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour, IEnemy
{
    
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float bulletMoveSpeed;
    [SerializeField] private float restTime = 1f;
    [SerializeField] private int burstCount;
    [SerializeField] private float timeBetweenBursts;
    [SerializeField] private int projectilesPerBurst;
    [SerializeField] [Range(0, 359)] private float angleSpread;
    [SerializeField] private float startingDistance = 0.1f;
    [SerializeField] private bool stagger;
    [SerializeField] private bool oscillate;


    private bool isShooting = false;

    public void Attack()
    {
        if (!isShooting)
        {
            StartCoroutine(ShootRoutine());
        }
    }

    /// <summary>
    /// Coroutine for shooting projectiles in bursts.
    /// </summary>
    /// <returns>IEnumerator for coroutine.</returns>
    private IEnumerator ShootRoutine()
    {
        isShooting = true;

        // Initialize variables for targeting cone of influence
        float startAngle, currentAngle, angleStep, endAngle;
        float timeBetweenProjectiles = 0f;

        // Calculate initial targeting cone of influence
        TargetConeOfInfluence(out startAngle, out currentAngle, out angleStep, out endAngle);

        // Calculate time between projectiles if staggering is enabled
        if (stagger) { timeBetweenProjectiles = timeBetweenBursts / projectilesPerBurst; }

        // Loop through each burst
        for (int i = 0; i < burstCount; i++)
        {
            // Recalculate targeting cone of influence if not oscillating
            if (!oscillate)
            {
                TargetConeOfInfluence(out startAngle, out currentAngle, out angleStep, out endAngle);
            }

            // Recalculate targeting cone of influence if oscillating and on even numbered burst
            if (oscillate && i % 2 != 1)
            {
                TargetConeOfInfluence(out startAngle, out currentAngle, out angleStep, out endAngle);
            }
            // Reverse targeting cone of influence if oscillating and on odd numbered burst
            else if (oscillate)
            {
                currentAngle = endAngle;
                endAngle = startAngle;
                startAngle = currentAngle;
                angleStep *= -1;
            }

            // Loop through each projectile in burst
            for (int j = 0; j < projectilesPerBurst; j++)
            {
                // Find spawn position for projectile
                Vector2 pos = FindBulletSpawnPos(currentAngle);

                // Instantiate projectile and set its direction
                GameObject newBullet = Instantiate(bulletPrefab, pos, Quaternion.identity);
                newBullet.transform.right = newBullet.transform.position - transform.position;

                // Update projectile move speed if it has a Projectile component
                if (newBullet.TryGetComponent(out Projectile projectile))
                {
                    projectile.UpdateMoveSpeed(bulletMoveSpeed);
                }

                // Increment current angle by angle step
                currentAngle += angleStep;

                // Wait between projectiles if staggering is enabled
                if (stagger) { yield return new WaitForSeconds(timeBetweenProjectiles); }
            }

            // Reset current angle to start angle
            currentAngle = startAngle;

            // Wait between bursts if not staggering
            if (!stagger) { yield return new WaitForSeconds(timeBetweenBursts); }
        }

        // Wait rest time before ending shooting routine
        yield return new WaitForSeconds(restTime);
        isShooting = false;
    }





    /// <summary>
    /// Calculates the start, current, and end angles of the cone of influence for targeting the player.
    /// </summary>
    /// <param name="startAngle">The start angle of the cone of influence.</param>
    /// <param name="currentAngle">The current angle of the cone of influence.</param>
    /// <param name="angleStep">The angle step between each projectile in a burst.</param>
    private void TargetConeOfInfluence(out float startAngle, out float currentAngle, out float angleStep, out float endAngle)
    {
        // Calculate the direction from this object to the player
        Vector2 targetDirection = PlayerController.Instance.transform.position - transform.position;

        // Calculate the angle from this object to the player
        float targetAngle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg;

        // Set the start, current, and end angles to the target angle
        startAngle = targetAngle;
        endAngle = targetAngle;
        currentAngle = targetAngle;

        // Initialize halfAngleSpread and angleStep to 0
        float halfAngleSpread = 0f;
        angleStep = 0;

        // If angleSpread is not 0
        if (angleSpread != 0)
        {
            // Calculate the angle step between each projectile in a burst
            angleStep = angleSpread / (projectilesPerBurst - 1);

            // Calculate half of the angle spread
            halfAngleSpread = angleSpread / 2f;

            // Calculate the start and end angles of the cone of influence
            startAngle = targetAngle - halfAngleSpread;
            endAngle = targetAngle + halfAngleSpread;

            // Set the current angle to the start angle
            currentAngle = startAngle;
        }
    }


    /// <summary>
    /// Calculates the spawn position of a bullet based on the current angle.
    /// </summary>
    /// <param name="currentAngle">The current angle of the cone of influence.</param>
    /// <returns>The spawn position of the bullet.</returns>
    private Vector2 FindBulletSpawnPos(float currentAngle)
    {
        // Calculate the x and y coordinates of the spawn position
        float x = transform.position.x + startingDistance * Mathf.Cos(currentAngle * Mathf.Deg2Rad);
        float y = transform.position.y + startingDistance * Mathf.Sin(currentAngle * Mathf.Deg2Rad);

        // Create a new Vector2 with the calculated coordinates
        Vector2 pos = new Vector2(x, y);

        // Return the spawn position
        return pos;
    }


}
