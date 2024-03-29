using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private int startingHealth = 3;
    [SerializeField] private GameObject deathVFXPrefab;
    [SerializeField] private float knockbackThrust = 15f;


    private int currentHealth;
    private Knockback knockback;
    private Flash flash;

    private void Awake()
    {
        knockback = GetComponent<Knockback>();
        flash = GetComponent<Flash>();
    }

    private void Start()
    {
        currentHealth = startingHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        knockback.GetKnockBack(PlayerController.Instance.transform, knockbackThrust);
        StartCoroutine(flash.FlashRoutine());
        StartCoroutine(CheckDeathRoutine());
    }

    private IEnumerator CheckDeathRoutine()
    {
        yield return new WaitForSeconds(flash.GetRestoreMatTime());
        DetectDeath();
    }

    private void DetectDeath()
    {
        // Check if current health is zero or less
        if (currentHealth <= 0)
        {
            Instantiate(deathVFXPrefab, transform.position, Quaternion.identity);
            GetComponent<PickupSpawnner>().DropItems();
            Destroy(gameObject);
        }
    }
}
