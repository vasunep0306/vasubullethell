using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    private PlayerControls playerControls;
    private Animator myAnimator;
    private PlayerController playerController;
    private ActiveWeapon activeWeapon;

    private void Awake()
    {
        playerController = GetComponentInParent<PlayerController>();
        activeWeapon = GetComponentInParent<ActiveWeapon>();
        myAnimator = GetComponent<Animator>();
        playerControls = new PlayerControls();
    }

    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void Start()
    {
        playerControls.Combat.Attack.started += _ => Attack();
    }

    private void Update()
    {
        MouseFollowWithOffset();
    }

    private void Attack()
    {
        myAnimator.SetTrigger("Attack");
    }


    private void MouseFollowWithOffset()
    {
        Vector3 mousePos = Input.mousePosition;
        Vector3 playerScreenPosition = Camera.main.WorldToScreenPoint(playerController.transform.position);
        float angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;

        if(mousePos.x < playerScreenPosition.x)
        {
            activeWeapon.transform.rotation = Quaternion.Euler(0, -180, angle);
        }
        else
        {
            activeWeapon.transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }
}
