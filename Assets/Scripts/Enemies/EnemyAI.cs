using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private float roamChangeDirFloat = 2f;
    [SerializeField] private float attackRange = 0f;
    [SerializeField] private MonoBehaviour enemyType;
    [SerializeField] private float attackCooldown = 2f;
    [SerializeField] private bool stopMovingWhileAttacking = false;

    private bool canAttack = true;

    private enum State
    {
        Roaming,
        Attacking
    }

    private State state;
    private EnemyPathfinding enemyPathfinding;
    private Vector2 roamPosition;
    private float timeRoaming = 0f;


    private void Awake()
    {
        enemyPathfinding = GetComponent<EnemyPathfinding>();
        state = State.Roaming;
    }


    private void Start()
    {
        roamPosition = GetRoamingPosition();
    }


    private void Update()
    {
        MovementStateControl();
    }

    /// <summary>
    /// Controls the movement state of the object.
    /// </summary>
    private void MovementStateControl()
    {
        // Switch statement to handle different movement states
        switch (state)
        {
            // Default case is to roam
            default:
            case State.Roaming:
                Roaming();
                break;

            // If attacking, call the Attacking function
            case State.Attacking:
                Attacking();
                break;
        }
    }


    /// <summary>
    /// Handles the attacking behavior of the object.
    /// </summary>
    private void Attacking()
    {
        // If the player is out of attack range, switch to roaming state
        if (Vector2.Distance(transform.position, PlayerController.Instance.transform.position) > attackRange)
        {
            state = State.Roaming;
        }

        // If the object can attack, perform an attack
        if (attackRange != 0 && canAttack)
        {
            // Set canAttack to false to prevent multiple attacks
            canAttack = false;

            // Call the Attack method on the enemyType object
            (enemyType as IEnemy).Attack();

            // If stopMovingWhileAttacking is true, stop moving
            if (stopMovingWhileAttacking)
            {
                enemyPathfinding.StopMoving();
            }
            // Otherwise, move towards the roam position
            else
            {
                enemyPathfinding.MoveTo(roamPosition);
            }

            // Start the AttackCooldownRoutine to wait for the next attack
            StartCoroutine(AttackCooldownRoutine());
        }
    }


    /// <summary>
    /// Handles the roaming behavior of the object.
    /// </summary>
    private void Roaming()
    {
        // Increment the time spent roaming
        timeRoaming += Time.deltaTime;

        // Move towards the roam position
        enemyPathfinding.MoveTo(roamPosition);

        // If the player is within attack range, switch to attacking state
        if (Vector2.Distance(transform.position, PlayerController.Instance.transform.position) < attackRange)
        {
            state = State.Attacking;
        }

        // If it's time to change direction, get a new roam position
        if (timeRoaming > roamChangeDirFloat)
        {
            roamPosition = GetRoamingPosition();
        }
    }



    //private IEnumerator RoamingRoutine()
    //{
    //    while(state == State.Roaming)
    //    {
    //        Vector2 roamPosition = GetRoamingPosition();
    //        enemyPathFinding.MoveTo(roamPosition);
    //        yield return new WaitForSeconds(roamChangeDirFloat);
    //    }
    //}


    /// <summary>
    /// Coroutine that waits for the attack cooldown time before allowing the object to attack again.
    /// </summary>
    private IEnumerator AttackCooldownRoutine()
    {
        // Wait for the attack cooldown time
        yield return new WaitForSeconds(attackCooldown);

        // Set canAttack to true to allow the object to attack again
        canAttack = true;
    }



    /// <summary>
    /// Returns a random roaming position for the object.
    /// </summary>
    private Vector2 GetRoamingPosition()
    {
        // Reset the time spent roaming
        timeRoaming = 0f;

        // Return a random normalized position
        return new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
    }

}
