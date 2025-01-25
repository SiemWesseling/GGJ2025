using UnityEngine;

public class Character : MonoBehaviour
{
    private CharacterController characterController;

    // Movement
    [SerializeField] private float speed;

    // Jumping
    [SerializeField] private float gravity;
    [SerializeField] private float jumpHeight;

    // Ground Checking
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private LayerMask groundMask;

    private bool isGrounded;

    // Velocity
    private Vector3 velocity;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        characterController = GetComponent<CharacterController>();

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
        isGrounded = Physics.CheckSphere(transform.position, groundCheckDistance, groundMask);

        if(isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;   // Small value to keep the character grounded
        }

        // Jump logic
        if(Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        // Movement logic
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector2 move = transform.right * moveHorizontal + transform.forward * moveVertical;
        characterController.Move(move * Time.deltaTime * speed);

        // Apply gravity to character
        velocity.y += gravity * Time.deltaTime;

        // Move the character with gravity
        characterController.Move(velocity * Time.deltaTime);
    }
}
