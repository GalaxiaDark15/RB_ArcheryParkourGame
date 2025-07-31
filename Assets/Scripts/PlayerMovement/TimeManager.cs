using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public float slowDownFactor = 0.2f; // Time is 20% realtime speed
    public float bulletTimeDuration = 10f;

    public WeaponController weaponController;

    private bool isBulletTime = false;
    private float originalFixedDeltaTime;
    private float bulletTimeElapsed = 0f;

    private float savedTimeScale = 1f;

    // Is the game paused?
    public bool isPaused = false;

    void Awake()
    {
        originalFixedDeltaTime = Time.fixedDeltaTime;
    }

    public void DoBulletTime()
    {
        if (!isBulletTime)
        {
            bulletTimeElapsed = 0f; // reset timer here
            StartCoroutine(BulletTimeCoroutine());
        }
        else
        {
        }
    }

    private IEnumerator BulletTimeCoroutine()
    {
        isBulletTime = true;
        Time.timeScale = slowDownFactor;
        Time.fixedDeltaTime = Time.timeScale * 0.02f;

        if (weaponController != null)
        {
            weaponController.SetBulletTimeFireValues();
        }

        while (bulletTimeElapsed < bulletTimeDuration)
        {

            if (!isPaused)
            {
                bulletTimeElapsed += Time.unscaledDeltaTime;
            }

            yield return null;
        }

        EndBulletTime();
    }

    public void DoNormalTime()
    {
        StopAllCoroutines();
        EndBulletTime();
    }

    private void EndBulletTime()
    {
        Time.timeScale = 1f;
        Time.fixedDeltaTime = originalFixedDeltaTime;
        isBulletTime = false;
        bulletTimeElapsed = 0f;

        if (weaponController != null)
        {
            weaponController.ResetFireValues();
        }
    }

    public void SaveTimeScaleBeforePause()
    {
        savedTimeScale = Time.timeScale;
        Time.timeScale = 0f;  // Actually pause now
        Time.fixedDeltaTime = 0f;
    }

    public void RestoreTimeScaleAfterResume()
    {
        // Restore saved timeScale only if bullet time is active
        if (isBulletTime)
        {
            Time.timeScale = savedTimeScale;
            Time.fixedDeltaTime = Time.timeScale * 0.02f;
        }
        else
        {
            Time.timeScale = 1f;
            Time.fixedDeltaTime = originalFixedDeltaTime;
        }
    }

}
