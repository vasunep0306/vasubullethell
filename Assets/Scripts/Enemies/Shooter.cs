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

    private IEnumerator ShootRoutine()
    {
        isShooting = true;

        float startAngle, currentAngle, angleStep;

        TargetConeOfInfluence(out startAngle, out currentAngle, out angleStep);

        for (int i = 0; i < burstCount; i++)
        {
            for (int j = 0; j < projectilesPerBurst; j++)
            {
                Vector2 pos = FindBulletSpawnPos(currentAngle);

                GameObject newBullet = Instantiate(bulletPrefab, pos, Quaternion.identity);
                newBullet.transform.right = newBullet.transform.position - transform.position;

                if (newBullet.TryGetComponent(out Projectile projectile))
                {
                    projectile.UpdateMoveSpeed(bulletMoveSpeed);
                }

                currentAngle += angleStep;
            }

            currentAngle = startAngle;

            yield return new WaitForSeconds(timeBetweenBursts);
            TargetConeOfInfluence(out startAngle, out currentAngle, out angleStep);
        }

        yield return new WaitForSeconds(restTime);
        isShooting = false;
    }


    /// <summary>
    /// Calculates the start, current, and end angles of the cone of influence for targeting the player.
    /// </summary>
    /// <param name="startAngle">The start angle of the cone of influence.</param>
    /// <param name="currentAngle">The current angle of the cone of influence.</param>
    /// <param name="angleStep">The angle step between each projectile in a burst.</param>
    private void TargetConeOfInfluence(out float startAngle, out float currentAngle, out float angleStep)
    {
        // Calculate the direction from this object to the player
        Vector2 targetDirection = PlayerController.Instance.transform.position - transform.position;

        // Calculate the angle from this object to the player
        float targetAngle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg;

        // Set the start, current, and end angles to the target angle
        startAngle = targetAngle;
        float endAngle = targetAngle;
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
