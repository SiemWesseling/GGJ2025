using UnityEngine;

public class Character : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpForce;

    [Header("Ground Check")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius;
    [SerializeField] private LayerMask groundLayer;

    private Rigidbody2D characterRigidBody;
    private bool isGrounded;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        characterRigidBody = GetComponent<Rigidbody2D>();

        // If groundCheck is not set in inspector, create a child object
        if (groundCheck == null)
        {
            GameObject groundCheckObject = new GameObject("GroundCheck");
            groundCheckObject.transform.SetParent(transform);
            groundCheckObject.transform.localPosition = new Vector3(0, -0.5f, 0);
            groundCheck = groundCheckObject.transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Handle horizontal movement
        float moveInput = Input.GetAxis("Horizontal");
        characterRigidBody.linearVelocity = new Vector2(moveInput * moveSpeed, characterRigidBody.linearVelocity.y);

        // Check if grounded
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        // Handle jump
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            characterRigidBody.linearVelocity = new Vector2(characterRigidBody.linearVelocity.x, jumpForce);
        }
    }

    // Visualize ground check in editor
    void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
}

