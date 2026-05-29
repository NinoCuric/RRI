using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    public AudioSource source;

    public AudioClip walkSound;
    public AudioClip jumpSound;
    public AudioClip attackSound;

    public void PlayWalk()
    {
        source.PlayOneShot(walkSound);
    }
    public void PlayJump()
    {
        source.PlayOneShot(jumpSound);
    }

    public void PlayAttack()
    {
        source.PlayOneShot(attackSound);
    }
}