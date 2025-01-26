using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class BubbleBehaviour : MonoBehaviour
{
    // Bubble size variables
    private float bubbleGrowthRate;
    private float maxBubbleSize;
    private bool bubbleIsGrowing = false;

    // Bubble list variables
    private static List<BubbleBehaviour> activeBubbles = new List<BubbleBehaviour>();
    private static int maxBubbles = 5;

    // Player position variables
    private Transform playerTransform;
    private bool isFollowingPlayer = true;

    // Mouse position variables
    private Vector3 launchMousePosition;
    private bool reachedTarget = false;

    [SerializeField] private float launchSpeed;
    
    [SerializeField] private UnityEvent onDeath;


    public void Initialize(float rate, float max, Transform playerTransform)
    {
        // If max bubbles reached, destroy the oldest bubble
        if (activeBubbles.Count >= maxBubbles)
        {
            BubbleBehaviour oldestBubble = activeBubbles[0];
            activeBubbles.RemoveAt(0);
            if(oldestBubble != null)
            {
                Destroy(oldestBubble.gameObject);
            }
        }

        // Add current bubble to the list
        activeBubbles.Add(this);

        this.bubbleGrowthRate = rate;
        this.maxBubbleSize = max;
        this.bubbleIsGrowing = true;
        this.playerTransform = playerTransform;
    }

    void Update()
    {
        // Make the bubble follow the player
        if (isFollowingPlayer && playerTransform != null)
        {
            // Make sure the bubble gets placed on the side of the player which is closest to the mouse location
            Vector3 directionToMouse = (playerTransform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition)).normalized;
            Vector3 lookingDirection = Vector3.Dot(directionToMouse, playerTransform.transform.right) < 0 ? playerTransform.transform.right : -playerTransform.transform.right;
            transform.position = playerTransform.position + lookingDirection * (Mathf.Sqrt(transform.localScale.magnitude) + 1f);
        }

        // Stop the current bubble from growing and following the player when mouse button is released
        if (isFollowingPlayer && Input.GetMouseButtonUp(0))
        {
            // Get the current mousePosition to shoot the bubble towards
            launchMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            launchMousePosition.z = 0;

            bubbleIsGrowing = false;
            isFollowingPlayer = false;
            reachedTarget = false;

            // Launch bubble to mouse position when mouse button is released
            Vector3 launchDirection = (launchMousePosition - transform.position).normalized;
            Rigidbody2D bubbleRigidbody = GetComponent<Rigidbody2D>();
            if (bubbleRigidbody != null)
            {
                bubbleRigidbody.AddForce(launchDirection * launchSpeed, ForceMode2D.Impulse);
            }
        }
        
        if (!isFollowingPlayer)
        {
            Rigidbody2D bubbleRigidbody = GetComponent<Rigidbody2D>();
            if (bubbleRigidbody != null)
            {
                float distanceToLaunchPoint = Vector3.Distance(transform.position, launchMousePosition);

                // Check if bubble has reached the mouse position
                if (!reachedTarget)
                {
                    CircleCollider2D bubbleCircleCollider = GetComponent<CircleCollider2D>();
                    // Smoothly approach mouse position
                    // I tried putting the 0.1f and the 7.5f in this if statements into changeable variables,
                    // but Unity did not like it. Get back to this if I have the chance
                    print(distanceToLaunchPoint);

                    if (distanceToLaunchPoint > 0.15f)
                    {
                        Vector2 directionToMouse = (launchMousePosition - transform.position).normalized;
                        float remainingDistance = distanceToLaunchPoint;
                        bubbleRigidbody.linearVelocity = directionToMouse * remainingDistance * 7.5f;
                    }
                    
                    if (distanceToLaunchPoint <= 0.15f)
                    {
                        bubbleRigidbody.linearVelocity = Vector2.zero;
                        reachedTarget = true;
                        bubbleCircleCollider.enabled = true;
                    }
                }
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
