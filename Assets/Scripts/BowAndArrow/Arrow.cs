using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    [SerializeField]
    private float damage;

    [SerializeField]
    private float torque;

    [SerializeField]
    private Rigidbody rigidBody;

    private string enemyTag;

    private bool didHit;

    public void SetEnemyTag(string enemyTag)
    {
        this.enemyTag = enemyTag;
    }

    public void Fly(Vector3 force)
    {
        rigidBody.isKinematic = false;
        rigidBody.AddForce(force, ForceMode.Impulse);
        rigidBody.AddTorque(transform.right * torque);
        transform.SetParent(null);
    }

    void OnTriggerEnter(Collider collider)
    {
        if (didHit) return;
        didHit = true;

        if (collider.CompareTag(enemyTag))
        {
            // var health = collider.GetComponent<HealthController>();
            // health.ApplyDamage(damage);
            Debug.Log("Arrow landed on enemy");
        }

        rigidBody.velocity = Vector3.zero;
        rigidBody.angularVelocity = Vector3.zero;
        rigidBody.isKinematic = true;
        transform.SetParent(collider.transform);
    }
}
