using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveInventory : Singleton<ActiveInventory>
{
    /// <summary>
    /// This class handles the active inventory slot for the player.
    /// </summary>
    private int activeSlotIndexNum = 0; //The index number of the active slot.

    private PlayerControls playerControls; //The reference to the player controls.

    protected override void Awake()
    {
        base.Awake();
        playerControls = new PlayerControls(); //Initialize the player controls.
    }

    private void Start()
    {
        //Subscribe to the keyboard input event for changing the active slot.
        playerControls.Inventory.KeyBoard.performed += ctx => ToggleActiveSlot((int)ctx.ReadValue<float>());

    }

    private void OnEnable()
    {
        playerControls.Enable(); //Enable the player controls.
    }

    public void EquipStartingWeapon()
    {
        ToggleActiveHighlight(0);
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

        Transform activeSlot = transform.GetChild(activeSlotIndexNum);
        InventorySlot slot = activeSlot.GetComponentInChildren<InventorySlot>();
        WeaponInfo weaponInfo = slot.GetWeaponInfo();

        if (weaponInfo == null)
        {
            ActiveWeapon.Instance.WeaponNull();
            return;
        }

        GameObject weaponToSpawn = weaponInfo.weaponPrefab;
        GameObject newWeapon = Instantiate(weaponToSpawn, ActiveWeapon.Instance.transform.position, Quaternion.identity);
        ActiveWeapon.Instance.transform.rotation = Quaternion.Euler(0, 0, 0);
        newWeapon.transform.parent = ActiveWeapon.Instance.transform;
        ActiveWeapon.Instance.NewWeapon(newWeapon.GetComponent<MonoBehaviour>());

    }
}
