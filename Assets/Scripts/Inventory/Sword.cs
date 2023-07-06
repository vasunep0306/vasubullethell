using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This class implements the sword weapon functionality for the player.
public class Sword : MonoBehaviour, IWeapon
{
    [SerializeField] private GameObject slashAnimPrefab; //The prefab for the slash animation.
    [SerializeField] private Transform slashAnimSpawnPoint; //The spawn point for the slash animation.
    [SerializeField] private Transform weaponCollider; //The collider for the weapon.
    [SerializeField] private float swordAttackCD = .5f; //The cooldown time for the sword attack.

    
    private Animator myAnimator; //The reference to the animator component.
    private PlayerController playerController; //The reference to the player controller component.
    private ActiveWeapon activeWeapon; //The reference to the active weapon component.

    

    private GameObject slashAnim; //The reference to the slash animation game object.

    private void Awake()
    {
        //Get the components from the parent game object.
        playerController = GetComponentInParent<PlayerController>();
        activeWeapon = GetComponentInParent<ActiveWeapon>();
        myAnimator = GetComponent<Animator>();
        //playerControls = new PlayerControls(); //Initialize the player controls.
    }

   

    private void Update()
    {
        MouseFollowWithOffset(); //Call the method to make the weapon follow the mouse position with an offset.
       
    }

    



    public void Attack()
    {
        //If the attack button is pressed and not currently attacking, perform an attack.
     
           // isAttacking = true; //Set the attacking flag to true.
            myAnimator.SetTrigger("Attack"); //Trigger the attack animation.
            weaponCollider.gameObject.SetActive(true); //Enable the weapon collider.

            //Instantiate and parent the slash animation game object at the spawn point position.
            slashAnim = Instantiate(slashAnimPrefab, slashAnimSpawnPoint.position, Quaternion.identity);
            slashAnim.transform.parent = this.transform.parent;

            StartCoroutine(AttackCDRoutine()); //Start a coroutine to wait for the cooldown time before attacking again.
        
    }


    public void DoneAttackingAnimEvent()
    {
        weaponCollider.gameObject.SetActive(false); //Disable the weapon collider when done attacking. This is called by an animation event.
    }

    public void SwingUpFlipAnimEvent()
    {
        slashAnim.gameObject.transform.rotation = Quaternion.Euler(-180f, 0f, 0f); //Flip the slash animation when swinging up. This is called by an animation event.

        if (playerController.FacingLeft) //If facing left, also flip the sprite horizontally.
        {
            slashAnim.GetComponent<SpriteRenderer>().flipX = true;
        }

    }

    public void SwingDownFlipAnimEvent()
    {
        slashAnim.gameObject.transform.rotation = Quaternion.Euler(0f, 0f, 0f); //Reset the slash animation when swinging down. This is called by an animation event.

        if (playerController.FacingLeft) //If facing left, also flip the sprite horizontally.
        {
            slashAnim.GetComponent<SpriteRenderer>().flipX = true;
        }

    }


    private void MouseFollowWithOffset()
    {
        Vector3 mousePos = Input.mousePosition; //Get the mouse position in screen space.
        Vector3 playerScreenPosition = Camera.main.WorldToScreenPoint(playerController.transform.position); //Get the player position in screen space.
        float angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg; //Calculate the angle between the mouse and the origin in degrees.

        if (mousePos.x < playerScreenPosition.x) //If the mouse is on the left side of the player, rotate and flip the weapon accordingly.
        {
            activeWeapon.transform.rotation = Quaternion.Euler(0f, -180f, angle);
            weaponCollider.transform.rotation = Quaternion.Euler(0f, -180f, 0f);
        }
        else //If the mouse is on the right side of the player, rotate and reset the weapon accordingly.
        {
            activeWeapon.transform.rotation = Quaternion.Euler(0f, 0f, angle);
            weaponCollider.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        }
    }


    private IEnumerator AttackCDRoutine()
    {
        yield return new WaitForSeconds(swordAttackCD); //Wait for the cooldown time.
        ActiveWeapon.Instance.ToggleIsAttacking(false);
        //isAttacking = false; //Set the attacking flag to false.
    }
}
