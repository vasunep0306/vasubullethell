using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public bool FacingLeft { get => facingLeft; set => facingLeft = value; }
    public static PlayerController Instance;

    [SerializeField] private float dashSpeed;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float dashTime;
    [SerializeField] private TrailRenderer myTrailRender;

    private PlayerControls playerControls;

    private Vector2 movement;
    private Rigidbody2D rb;
    private Animator myAnimator;
    private SpriteRenderer mySpriteRenderer;

    private bool facingLeft = false;
    

    private void Awake()
    {
        Instance = this;
        playerControls = new PlayerControls();
        rb = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        mySpriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        playerControls.Combat.Dash.performed += _ => Dash();
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
            FacingLeft = true;
        } 
        else
        {
            mySpriteRenderer.flipX = false;
            FacingLeft = false;
        }
    }

    private void Dash()
    {
        moveSpeed *= dashSpeed;
        myTrailRender.emitting = true;
        StartCoroutine(EndDashRoutine());
    }

    private IEnumerator EndDashRoutine()
    {
        yield return new WaitForSeconds(dashTime);
        moveSpeed /= dashSpeed;
        myTrailRender.emitting = false;
    }
}
