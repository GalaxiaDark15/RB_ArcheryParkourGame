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
        Debug.Log($"[TimeManager] Awake: originalFixedDeltaTime = {originalFixedDeltaTime}");
    }

    public void DoBulletTime()
    {
        if (!isBulletTime)
        {
            bulletTimeElapsed = 0f; // reset timer here
            Debug.Log("[TimeManager] DoBulletTime called. Starting bullet time.");
            StartCoroutine(BulletTimeCoroutine());
        }
        else
        {
            Debug.Log("[TimeManager] DoBulletTime called but bullet time already active.");
        }
    }

    private IEnumerator BulletTimeCoroutine()
    {
        isBulletTime = true;
        Time.timeScale = slowDownFactor;
        Time.fixedDeltaTime = Time.timeScale * 0.02f;
        Debug.Log("[TimeManager] BulletTimeCoroutine started. Time scale set to " + Time.timeScale);

        if (weaponController != null)
        {
            Debug.Log("[TimeManager] WeaponController found. Setting bullet time fire values.");
            weaponController.SetBulletTimeFireValues();
        }

        while (bulletTimeElapsed < bulletTimeDuration)
        {
            Debug.Log($"[TimeManager] BulletTimeCoroutine looping: elapsed={bulletTimeElapsed:F2}s, isPaused={isPaused}");

            if (!isPaused)
            {
                bulletTimeElapsed += Time.unscaledDeltaTime;
            }
            else
            {
                Debug.Log("[TimeManager] Pause detected, timer NOT incremented this frame.");
            }

            yield return null;
        }

        Debug.Log($"[TimeManager] Bullet time duration reached: {bulletTimeElapsed:F2}s >= {bulletTimeDuration}s.");
        EndBulletTime();
    }

    public void DoNormalTime()
    {
        Debug.Log("[TimeManager] DoNormalTime called. Stopping bullet time and resetting time scale.");
        StopAllCoroutines();
        EndBulletTime();
    }

    private void EndBulletTime()
    {
        Time.timeScale = 1f;
        Time.fixedDeltaTime = originalFixedDeltaTime;
        isBulletTime = false;
        bulletTimeElapsed = 0f;
        Debug.Log("[TimeManager] Bullet time ended. Time scale reset to 1.");

        if (weaponController != null)
        {
            weaponController.ResetFireValues();
            Debug.Log("[TimeManager] WeaponController fire values reset.");
        }
    }

    public void SaveTimeScaleBeforePause()
    {
        savedTimeScale = Time.timeScale;
        Debug.Log($"[TimeManager] Saved timeScale: {savedTimeScale}");
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
            Debug.Log($"[TimeManager] Restored saved time scale after resume: {Time.timeScale}");
        }
        else
        {
            Time.timeScale = 1f;
            Time.fixedDeltaTime = originalFixedDeltaTime;
            Debug.Log("[TimeManager] Restored normal time scale = 1 after resume");
        }
    }

}
