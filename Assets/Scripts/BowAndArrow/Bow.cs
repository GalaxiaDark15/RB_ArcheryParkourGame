using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bow : MonoBehaviour
{
    [SerializeField] public float reloadTime;
    [SerializeField] private Arrow arrowPrefab;
    [SerializeField] private Transform spawnPoint;

    private Arrow currentArrow;
    private bool isReloading;

    // NEW: Track ownership: is this Bow on an enemy?
    public bool isEnemyBow = false;

    public void PrepareArrow()
    {
        if (isReloading || currentArrow != null) return;
        currentArrow = Instantiate(arrowPrefab, spawnPoint);
        currentArrow.transform.localPosition = Vector3.zero;
        currentArrow.Prepare();  // Disable collider until fired
    }

    public void Fire(float firePower)
    {
        if (isReloading || currentArrow == null) return;

        var force = spawnPoint.TransformDirection(Vector3.forward * firePower);
        currentArrow.Fly(force, isEnemyBow); // Set arrow ownership
        currentArrow = null;
        StartCoroutine(ReloadAfterTime());
    }

    private IEnumerator ReloadAfterTime()
    {
        isReloading = true;
        yield return new WaitForSecondsRealtime(reloadTime);
        isReloading = false;
    }

    public bool IsReady()
    {
        return (!isReloading && currentArrow != null);
    }

    public bool IsReloading()
    {
        return isReloading;
    }

    public float ReloadTime
    {
        get { return reloadTime; }
        // optionally add a setter if you want to change it from outside
        // set { reloadTime = value; }
    }
}
