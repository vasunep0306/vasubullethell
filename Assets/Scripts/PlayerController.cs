using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed;

    private PlayerControls playerControls;

    private Vector2 movement;
    private Rigidbody2D rb;
    private Animator myAnimator;
    private SpriteRenderer mySpriteRenderer;

    private void Awake()
    {
        playerControls = new PlayerControls();
        rb = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        mySpriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        playerControls.Enable();
    }


    private void Update()
    {
        PlayerInput();
    }

    private void FixedUpdate()
    {
        AdjustPlayerFacingPosition();
        Move();
    }

    private void PlayerInput()
    {
        movement = playerControls.Movement.Move.ReadValue<Vector2>();

        myAnimator.SetFloat("moveX", movement.x);
        myAnimator.SetFloat("moveY", movement.y);
    }

    private void Move()
    {
        rb.MovePosition(rb.position + movement * (moveSpeed * Time.fixedDeltaTime));
    }

    private void AdjustPlayerFacingPosition()
    {
        Vector3 mousePos = Input.mousePosition;
        Vector3 playerScreenPosition = Camera.main.WorldToScreenPoint(transform.position);

        if(mousePos.x < playerScreenPosition.x)
        {
            // flip player sprite
            mySpriteRenderer.flipX = true;
        } 
        else
        {
            mySpriteRenderer.flipX = false;
        }
    }
}
