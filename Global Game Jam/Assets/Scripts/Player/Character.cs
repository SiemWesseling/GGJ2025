using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class Character : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpForce;

    [Header("Ground Check")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius;
    [SerializeField] private LayerMask groundLayer;

    private Rigidbody2D playerRigidBody;
    private bool isGrounded;
    private SpriteRenderer playerSpriteRenderer;
    private BubbleBehaviour bubbleBehaviour;

    [Header("Bubble Spawning")]
    [SerializeField] private GameObject bubbleToSpawn;
    [SerializeField] private float bubbleGrowthRate;
    [SerializeField] private float maxBubbleSize;


    [Header("Bouncing")]
    [SerializeField] private float bounceForce;
    [SerializeField] private float maxForce;
    [SerializeField] private float upwardBiasStrength;

    //TODO: cant blow bubble during jump now, glitches out player controller?

    void Start()
    {
        playerRigidBody = GetComponent<Rigidbody2D>();
        playerSpriteRenderer = GetComponent<SpriteRenderer>();
        bubbleBehaviour = GetComponent<BubbleBehaviour>();

        // If groundCheck is not set in inspector, create a child object
        if (groundCheck == null)
        {
            GameObject groundCheckObject = new GameObject("GroundCheck");
            groundCheckObject.transform.SetParent(transform);
            groundCheckObject.transform.localPosition = new Vector3(0, -0.5f, 0);
            groundCheck = groundCheckObject.transform;
        }
    }

    void Update()
    {
        // Handles horizontal movement
        float moveInput = Input.GetAxis("Horizontal");
        playerRigidBody.linearVelocity = new Vector2(moveInput * moveSpeed, playerRigidBody.linearVelocity.y);

        // Checks if grounded
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        // Handles jump
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            playerRigidBody.linearVelocity = new Vector2(playerRigidBody.linearVelocity.x, jumpForce);
        }

        // Spawns new bubble on mouse press, allowing multiple bubbles
        if (Input.GetMouseButtonDown(0))
        {
            // Creates a new bubble at player position
            GameObject newBubble = Instantiate(bubbleToSpawn, transform.position, Quaternion.identity);
            newBubble.transform.localScale = Vector3.zero;

            // Creates a new script instance to manage this specific bubble's growth
            BubbleBehaviour bubbleBehaviour = newBubble.AddComponent<BubbleBehaviour>();
            bubbleBehaviour.Initialize(bubbleGrowthRate, maxBubbleSize, transform, playerSpriteRenderer);
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

        // TODO: on collision with object increase the bubble array amount with 1
        if (collision.gameObject.CompareTag("BubbleAmmoIncrease"))
        {
            //bubbleBehaviour.maxBubbles += 1;
            Destroy(collision.gameObject);
        }
    }
}




