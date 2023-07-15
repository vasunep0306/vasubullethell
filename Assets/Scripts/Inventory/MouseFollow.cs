using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseFollow : MonoBehaviour
{
    private void Update()
    {
        FaceMouse();
    }

    // This function makes the object face the mouse cursor
    private void FaceMouse()
    {
        // Get the mouse position on the screen
        Vector3 mousePosition = Input.mousePosition;
        // Convert the mouse position to world coordinates
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);

        // Calculate the direction vector from the object to the mouse
        Vector2 direction = transform.position - mousePosition;

        // Rotate the object to face the opposite direction of the mouse
        transform.right = -direction;
    }


}
