using UnityEngine;

public class PlayerAudioController : MonoBehaviour
{
    [SerializeField] private AudioClip jumpSFX, hurtSFX;
    [SerializeField] private AudioSource audioSource;

    public void PlayJump()
    {
        audioSource.PlayOneShot(jumpSFX);
    }

    public void PlayHurt()
    {
        audioSource.PlayOneShot(hurtSFX);
    }
}
