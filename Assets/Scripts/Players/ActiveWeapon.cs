using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveWeapon : Singleton<ActiveWeapon>
{
    public MonoBehaviour CurrentActiveWeapon { get; private set; }
    private PlayerControls playerControls; //The reference to the player controls.
    private bool attackButtonDown, isAttacking = false; //The flags for the attack button state and the attacking state.
    private float timeBetweenAttacks;

    protected override void Awake()
    {
        base.Awake();

        playerControls = new PlayerControls();
    }

    private void OnEnable()
    {
        playerControls.Enable(); //Enable the player controls.
    }

    private void Start()
    {
        //Subscribe to the attack input events for starting and stopping attacking.
        playerControls.Combat.Attack.started += _ => StartAttacking();
        playerControls.Combat.Attack.canceled += _ => StopAttacking();
        AttackCooldown();
    }

    private void Update()
    {
        Attack(); //Call the method to perform an attack if possible.
    }

    public void NewWeapon(MonoBehaviour newWeapon)
    {
        CurrentActiveWeapon = newWeapon;
        AttackCooldown();
        timeBetweenAttacks = (CurrentActiveWeapon as IWeapon).GetWeaponInfo().weaponCooldown;

    }


    public void WeaponNull()
    {
        CurrentActiveWeapon = null;
    }


    private void AttackCooldown()
    {
        isAttacking = true;
        StopAllCoroutines();
        StartCoroutine(TimeBetweenAttacksRoutine());
    }

    private IEnumerator TimeBetweenAttacksRoutine()
    {
        yield return new WaitForSeconds(timeBetweenAttacks);
        isAttacking = false;
    }



    private void Attack()
    {
        if(attackButtonDown && !isAttacking)
        {
            AttackCooldown();
            (CurrentActiveWeapon as IWeapon).Attack();
        }
        
    }


    private void StartAttacking()
    {
        attackButtonDown = true; //Set the attack button flag to true.
    }

    private void StopAttacking()
    {
        attackButtonDown = false; //Set the attack button flag to false.
    }
}
