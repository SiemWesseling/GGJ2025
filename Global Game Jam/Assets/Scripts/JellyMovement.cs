using UnityEngine;

public class JellyMovement : MonoBehaviour
{

    [SerializeField] private float amplitude = 1f; // How high the object moves
    [SerializeField] private float frequency = 1f; // How fast the object moves

    private Vector3 startPos;

    void Start()
    {
        // Record the initial position of the object
        startPos = transform.position;
    }

    void Update()
    {
        // Calculate the new Y position based on a sine wave
        float newY = startPos.y + Mathf.Sin(Time.time * frequency) * amplitude;

        // Update the object's position
        transform.position = new Vector3(startPos.x, newY, startPos.z);
    }
}
