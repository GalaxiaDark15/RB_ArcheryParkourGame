using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public float slowDownFactor = 0.2f;
    public float bulletTimeDuration = 10f;

    // Reference WeaponController
    public WeaponController weaponController;

    private bool isBulletTime = false;
    private float originalFixedDeltaTime;

    void Awake()
    {
        originalFixedDeltaTime = Time.fixedDeltaTime;
    }

    public void DoBulletTime()
    {
        if (!isBulletTime)
            StartCoroutine(BulletTimeCoroutine());
    }

    private IEnumerator BulletTimeCoroutine()
    {
        isBulletTime = true;
        Time.timeScale = slowDownFactor;
        Time.fixedDeltaTime = Time.timeScale * 0.02f;
        Debug.Log("Is in Bullet Time");

        // Tell WeaponController to set bullet time fire values
        if (weaponController != null)
            Debug.Log("weapon controller exists!");
            weaponController.SetBulletTimeFireValues();

        float timer = 0f;
        while (timer < bulletTimeDuration)
        {
            yield return null;
            timer += Time.unscaledDeltaTime;
        }

        Time.timeScale = 1f;
        Time.fixedDeltaTime = originalFixedDeltaTime;
        isBulletTime = false;
        Debug.Log("Exited Bullet Time");

        // Tell WeaponController to reset fire values
        if (weaponController != null)
            weaponController.ResetFireValues();
    }

    public void DoNormalTime()
    {
        StopAllCoroutines();
        Time.timeScale = 1f;
        Time.fixedDeltaTime = originalFixedDeltaTime;
        isBulletTime = false;
        Debug.Log("Exited Bullet Time (manually)");

        if (weaponController != null)
            weaponController.ResetFireValues();
    }
}
