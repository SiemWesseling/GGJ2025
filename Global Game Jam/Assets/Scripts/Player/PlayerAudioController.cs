using UnityEngine;

public class PlayerAudioController : MonoBehaviour
{
    [SerializeField] private AudioClip jumpSFX, hurtSFX, popSFX;
    [SerializeField] private AudioSource audioSource;

    public void PlayJump()
    {
        audioSource.PlayOneShot(jumpSFX);
    }

    public void PlayHurt()
    {
        audioSource.PlayOneShot(hurtSFX);
    }

    public void PlayPop()
    {
        audioSource.PlayOneShot(popSFX);
    }
}
