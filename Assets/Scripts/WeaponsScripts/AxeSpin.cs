using UnityEngine;

public class AxeSpin : MonoBehaviour
{
    public float damage = 1f;
    public Transform centerPoint;   // Typically the enemy's transform
    public float spinSpeed = 100f;  // Degrees per second

    void Update()
    {
        if (centerPoint != null)
        {
            // Spin the axe around the center point on the Y axis
            transform.RotateAround(centerPoint.position, Vector3.up, spinSpeed * Time.deltaTime);
        }
    }
}
