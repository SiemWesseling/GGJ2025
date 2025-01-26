using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;

public class Character : MonoBehaviour
{
    [Header("Movement Settings")] 
    [SerializeField] private float moveSpeed;

    [Header("Jumping Settings")]
    [SerializeField] private float jumpForce;
    [SerializeField] private float normalGravityScale;
    [SerializeField] private float increasedGravityScale;
    private int framesSinceJump = 0;
    private bool hasJumped = false;

    [Header("Ground Check")] 
    [SerializeField] private Transform groundCheck;


    [SerializeField] private float groundCheckRadius;
    [SerializeField] private LayerMask groundLayer;

    private Rigidbody2D playerRigidBody;

    private float _moveInput;

    private float moveInput
    {
        get => _moveInput;
        set
        {
            if (value < 0) spriteRenderer.flipX = true;
            if (value > 0) spriteRenderer.flipX = false;
            if (value != 0) animator.SetBool("PlayerIsRunning", true);
            else animator.SetBool("PlayerIsRunning", false);
            _moveInput = value;
        }
    }

    private bool _isGrounded;

    private bool isGrounded
    {
        get => _isGrounded;
        set
        {
            if (isJumping && value)
            {
                isJumping = false;
                animator.SetTrigger("PlayerHasLanded");
            }

            _isGrounded = value;
        }
    }

    private bool isJumping;


    [Header("Bubble Spawning")] [SerializeField]
    private GameObject bubbleToSpawn;

    [SerializeField] private float bubbleGrowthRate;
    [SerializeField] private float maxBubbleSize;

    [Header("Animator")] [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer spriteRenderer;


    [Header("Bouncing")] [SerializeField] private float bounceForce;
    [SerializeField] private float maxForce;
    [SerializeField] private float upwardBiasStrength;

    private GameObject currentRespawnPoint;

    [SerializeField] private UnityEvent onHit, onJump;

    //TODO: cant blow bubble during jump now, glitches out player controller?

    void Start()
    {
        playerRigidBody = GetComponent<Rigidbody2D>();

        // If groundCheck is not set in inspector, create a child object
        if (groundCheck == null)
        {
            GameObject groundCheckObject = new GameObject("GroundCheck");
            groundCheckObject.transform.SetParent(transform);
            groundCheckObject.transform.localPosition = new Vector3(0, -0.5f, 0);
            groundCheck = groundCheckObject.transform;
        }
    }

    private void FixedUpdate()
    {
        if (hasJumped)
        {
            framesSinceJump++;

            if (framesSinceJump == 20)
            {
                playerRigidBody.gravityScale = increasedGravityScale;
            }
        }
    }


    void Update()
    {
        // Handles horizontal movement
        moveInput = Input.GetAxis("Horizontal");
        // Handle horizontal movement
        moveInput = Input.GetAxis("Horizontal");
        playerRigidBody.linearVelocity = new Vector2(moveInput * moveSpeed, playerRigidBody.linearVelocity.y);

        if (playerRigidBody.linearVelocityY <= 0)
        {
            // Checks if grounded
            isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        }

        // Check if grounded
        if (playerRigidBody.linearVelocityY <= 0)
        {
            isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        }

        // Handles jump
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            isJumping = true;
            onJump.Invoke();
            animator.SetTrigger("PlayerIsJumping");
            playerRigidBody.linearVelocity = new Vector2(playerRigidBody.linearVelocity.x, jumpForce);
            hasJumped = true;
            framesSinceJump = 0;
            playerRigidBody.gravityScale = normalGravityScale;
        }

        // Spawns new bubble on mouse press, allowing multiple bubbles
        if (Input.GetMouseButtonDown(0))
        {
            // Creates a new bubble at player position
            GameObject newBubble = Instantiate(bubbleToSpawn, transform.position, Quaternion.identity);
            newBubble.transform.localScale = Vector3.zero;

            // Creates a new script instance to manage this specific bubble's growth
            BubbleBehaviour bubbleBehaviour = newBubble.AddComponent<BubbleBehaviour>();
            bubbleBehaviour.Initialize(bubbleGrowthRate, maxBubbleSize, transform, spriteRenderer);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Bubble"))
        {
            // TODO: maak dit minder clunky
            Vector2 incomingVelocity = playerRigidBody.linearVelocity;
            Vector2 reflectedVelocity = Vector2.Reflect(incomingVelocity, collision.contacts[0].normal);
            Vector2 upwardBias = Vector2.up * upwardBiasStrength;
            Vector2 totalForce = (reflectedVelocity.normalized * bounceForce) + upwardBias;
            playerRigidBody.linearVelocity = Vector2.ClampMagnitude(totalForce, maxForce);
            Destroy(collision.gameObject);
        }

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Damage"))
        {
            onHit.Invoke();
            gameObject.transform.position = currentRespawnPoint.transform.position;
        }

        if (other.gameObject.CompareTag("Respawn"))
        {
            currentRespawnPoint = other.gameObject;
        }
    }
}




