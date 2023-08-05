using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour, IEnemy
{
    // Bullet prefab to be instantiated when the enemy attacks
    [SerializeField] private GameObject bulletPrefab;

    // Method to make the enemy attack the player
    public void Attack()
    {
        // Calculate the direction from the enemy to the player
        Vector2 targetDirection = PlayerController.Instance.transform.position - transform.position;

        // Instantiate a new bullet at the enemy's position
        GameObject newBullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);

        // Set the bullet's direction towards the player
        newBullet.transform.right = targetDirection;
    }
}

