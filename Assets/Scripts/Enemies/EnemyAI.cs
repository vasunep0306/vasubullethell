using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private float roamChangeDirFloat = 2f;
    [SerializeField] private float attackRange = 5f;
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


    private void Attacking()
    {
        if (Vector2.Distance(transform.position, PlayerController.Instance.transform.position) > attackRange)
        {
            state = State.Roaming;
        }

        if (attackRange != 0 && canAttack)
        {
            canAttack = false;
            (enemyType as IEnemy).Attack();

            if(stopMovingWhileAttacking)
            {
                enemyPathfinding.StopMoving();
            }
            else
            {
                enemyPathfinding.MoveTo(roamPosition);
            }
            StartCoroutine(AttackCooldownRoutine());
        }
    }

    private void Roaming()
    {
        timeRoaming += Time.deltaTime;

        enemyPathfinding.MoveTo(roamPosition);

        if (Vector2.Distance(transform.position, PlayerController.Instance.transform.position) < attackRange)
        {
            state = State.Attacking;
        }

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


    private IEnumerator AttackCooldownRoutine()
    {
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }


    private Vector2 GetRoamingPosition()
    {
        timeRoaming = 0f;
        return new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
    }
}
