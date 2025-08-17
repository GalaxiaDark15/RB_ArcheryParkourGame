using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXManager : MonoBehaviour
{
    public AudioClip arrowSound;
    public AudioClip drawBowSound;
    public AudioClip footstepsSound;
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
            // Play draw bow sound without interrupting other sounds
            audioSource.PlayOneShot(drawBowSound);
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


    public void playFastFootstepsSound()
    {
        if (footstepsSound != null && audioSource != null)
        {
            if (audioSource.clip != footstepsSound || !audioSource.isPlaying)
            {
                audioSource.pitch = 1.5f;
                audioSource.volume = 0.5f;
                audioSource.loop = true;
                audioSource.clip = footstepsSound;
                audioSource.Play();
            }
            else
            {
                // If already playing footsteps, just update pitch
                audioSource.pitch = 1.5f;
            }
        }
    }

    public void playFootstepsSound()
    {
        if (footstepsSound != null && audioSource != null)
        {
            if (audioSource.clip != footstepsSound || !audioSource.isPlaying)
            {
                audioSource.pitch = 1.0f;
                audioSource.volume = 0.5f;
                audioSource.loop = true;
                audioSource.clip = footstepsSound;
                audioSource.Play();
            }
            else
            {
                audioSource.pitch = 1.0f;
            }
        }
    }

    public void stopFootstepsSound()
    {
        if (audioSource != null && audioSource.clip == footstepsSound && audioSource.isPlaying)
        {
            audioSource.Stop();
            audioSource.loop = false;
            audioSource.pitch = 1.0f;
            audioSource.clip = null;
        }
    }

    public void pauseAllSound()
    {
        if (audioSource != null && audioSource.isPlaying)
        {
            audioSource.Pause();
        }
    }

    public void resumeAllSound()
    {
        if (audioSource != null)
        {
            audioSource.UnPause();
        }
    }


}
