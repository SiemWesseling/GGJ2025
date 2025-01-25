using UnityEngine;

public class BubbleBehaviour : MonoBehaviour
{
    private float bubbleGrowthRate;
    private float maxBubbleSize;
    private bool bubbleIsGrowing = false;
    private Transform playerTransform;
    private bool isFollowing = true;

    [SerializeField] private float launchSpeed;

    public void Initialize(float rate, float max, Transform playerTransform)
    {
        this.bubbleGrowthRate = rate;
        this.maxBubbleSize = max;
        this.bubbleIsGrowing = true;
        this.playerTransform = playerTransform;
    }

    void Update()
    {
        // Make the bubble follow the player
        if (isFollowing && playerTransform != null)
        {
            transform.position = playerTransform.position;
        }

        // Stop the current bubble from growing and following the player when mouse button is released
        if (isFollowing && Input.GetMouseButtonUp(0))
        {
            bubbleIsGrowing = false;
            isFollowing = false;
            
            // Get the current mousePosition
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0;

            // Launch bubble to mouse position when mouse button is released
            Vector3 launchDirection = (mousePos - transform.position).normalized;
            Rigidbody2D bubbleRigidbody = GetComponent<Rigidbody2D>();
            if (bubbleRigidbody != null)
            {
                bubbleRigidbody.linearVelocity = launchDirection * launchSpeed;
                Debug.Log(bubbleRigidbody.linearVelocity);
            }

        }

        // Grow current bubble while mouse button is held
        if (bubbleIsGrowing && transform.localScale.x < maxBubbleSize)
        {
            Vector3 newScale = transform.localScale + Vector3.one * bubbleGrowthRate * Time.deltaTime;
            transform.localScale = Vector3.Min(newScale, Vector3.one * maxBubbleSize);
        }
    }
}
