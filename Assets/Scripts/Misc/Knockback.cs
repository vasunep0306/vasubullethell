using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knockback : MonoBehaviour
{
    public bool gettingKnockedBack { get; private set; }
    [SerializeField] private float knockBackTime = .2f;

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void GetKnockBack(Transform damageSource, float knockBackThrust)
    {
        gettingKnockedBack = true;
        // Call the CalculateKnockBackForce method and multiply the result by the rb mass
        Vector2 force = CalculateKnockBackForce(damageSource, knockBackThrust) * rb.mass;

        // Add the force to the rb as an impulse
        rb.AddForce(force, ForceMode2D.Impulse);

        StartCoroutine(KnockRoutine());
    }

    private Vector2 CalculateKnockBackForce(Transform damageSource, float knockBackThrust)
    {
        // Calculate the difference vector between the transform position and the damage source position
        Vector2 difference = transform.position - damageSource.position;

        // Normalize the difference vector and multiply it by the knock back thrust
        Vector2 force = difference.normalized * knockBackThrust;

        // Return the force vector
        return force;
    }

    private IEnumerator KnockRoutine()
    {
        yield return new WaitForSeconds(knockBackTime);
        rb.velocity = Vector2.zero;
        gettingKnockedBack = false;
    }
}
