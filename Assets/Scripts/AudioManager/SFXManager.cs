using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXManager : MonoBehaviour
{
    public AudioClip arrowSound;
    public AudioClip drawBowSound;
    private AudioSource audioSource;

    void Awake()
    {
        // Get the AudioSource component, or add one if it doesn't exist
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    public void playArrowSound()
    {
        if (arrowSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(arrowSound);
        }
    }

    public void playDrawBowSound()
    {
        if (drawBowSound != null && audioSource != null)
        {
            if (audioSource.clip != drawBowSound || !audioSource.isPlaying)
            {
                audioSource.loop = true;
                audioSource.clip = drawBowSound;
                audioSource.Play();
            }
        }
    }

    public void stopDrawBowSound()
    {
        if (audioSource != null && audioSource.clip == drawBowSound && audioSource.isPlaying)
        {
            audioSource.Stop();
            audioSource.loop = false;
            audioSource.clip = null;
        }
    }
}
