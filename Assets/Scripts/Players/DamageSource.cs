using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageSource : MonoBehaviour
{
    private int damageAmount = 1;

    private void Start()
    {

        MonoBehaviour currenActiveWeapon = ActiveWeapon.Instance.CurrentActiveWeapon;
        if(currenActiveWeapon)
        {
            damageAmount = (currenActiveWeapon as IWeapon).GetWeaponInfo().weaponDamage;
        }
       
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
            EnemyHealth enemyHealth = other.gameObject.GetComponent<EnemyHealth>();
            enemyHealth?.TakeDamage(damageAmount);
    }
}
