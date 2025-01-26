using UnityEngine;

public class BubbleAudioController : MonoBehaviour
{
    [SerializeField] private AudioClip popSFX;
    public AudioSource audioSource;

    public void PlaySFX()
    {
        audioSource.PlayOneShot(popSFX);
    }
}
