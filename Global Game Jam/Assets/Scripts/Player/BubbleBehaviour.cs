using UnityEngine;

public class BubbleBehaviour : MonoBehaviour
{
    private float bubbleGrowthRate;
    private float maxBubbleSize;
    private bool bubbleIsGrowing = false;

    public void Initialize(float rate, float max)
    {
        bubbleGrowthRate = rate;
        maxBubbleSize = max;
        bubbleIsGrowing = true;
    }

    void Update()
    {
        // Stop the current bubble growing when mouse button is released
        if (Input.GetMouseButtonUp(0))
        {
            bubbleIsGrowing = false;
        }

        // Grow current bubble while mouse button is held
        if (bubbleIsGrowing && transform.localScale.x < maxBubbleSize)
        {
            Vector3 newScale = transform.localScale + Vector3.one * bubbleGrowthRate * Time.deltaTime;
            transform.localScale = Vector3.Min(newScale, Vector3.one * maxBubbleSize);
        }
    }
}
