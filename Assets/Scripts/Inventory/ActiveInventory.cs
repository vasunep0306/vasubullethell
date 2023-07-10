using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveInventory : MonoBehaviour
{
    /// <summary>
    /// This class handles the active inventory slot for the player.
    /// </summary>
    private int activeSlotIndexNum = 0; //The index number of the active slot.

    private PlayerControls playerControls; //The reference to the player controls.

    private void Awake()
    {
        playerControls = new PlayerControls(); //Initialize the player controls.
    }

    private void Start()
    {
        //Subscribe to the keyboard input event for changing the active slot.
        playerControls.Inventory.KeyBoard.performed += ctx => ToggleActiveSlot((int)ctx.ReadValue<float>());

        ToggleActiveHighlight(0);
    }

    private void OnEnable()
    {
        playerControls.Enable(); //Enable the player controls.
    }

    private void ToggleActiveSlot(int numValue)
    {
        //Call the method to toggle the active highlight with the input value minus one.
        ToggleActiveHighlight(numValue - 1);
    }

    private void ToggleActiveHighlight(int indexNum)
    {
        activeSlotIndexNum = indexNum; //Set the active slot index number to the input value.

        //Loop through all the inventory slots and disable their highlight game objects.
        foreach (Transform inventorySlot in this.transform)
        {
            inventorySlot.GetChild(0).gameObject.SetActive(false);
        }

        //Enable the highlight game object of the active slot.
        this.transform.GetChild(activeSlotIndexNum).GetChild(0).gameObject.SetActive(true);

        ChangeActiveWeapon();
    }

    private void ChangeActiveWeapon()
    {
        if (ActiveWeapon.Instance.CurrentActiveWeapon != null)
        {
            Destroy(ActiveWeapon.Instance.CurrentActiveWeapon.gameObject);
        }
        if (!transform.GetChild(activeSlotIndexNum).GetComponentInChildren<InventorySlot>())
        {
            ActiveWeapon.Instance.WeaponNull();
            return;
        }

        GameObject weaponToSpawn = transform.GetChild(activeSlotIndexNum)
                                        .GetComponentInChildren<InventorySlot>()
                                         .GetWeaponInfo().weaponPrefab;

        GameObject newWeapon = Instantiate(weaponToSpawn, ActiveWeapon.Instance.transform.position, Quaternion.identity);
        newWeapon.transform.parent = ActiveWeapon.Instance.transform;

        ActiveWeapon.Instance.NewWeapon(newWeapon.GetComponent<MonoBehaviour>());



    }
}
