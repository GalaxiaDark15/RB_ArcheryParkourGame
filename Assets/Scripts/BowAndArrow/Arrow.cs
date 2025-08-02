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

    private string enemyTag = "Enemy";  // Default to "Enemy"

    private bool didHit;

    public void SetEnemyTag(string enemyTag)
    {
        this.enemyTag = enemyTag;
    }

    public void Fly(Vector3 force)
    {
        gameObject.layer = LayerMask.NameToLayer("Arrows");
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
            EnemyHealth enemyHealth = collider.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage();  // Call TakeDamage on the enemy
            }
            Debug.Log("Arrow landed on enemy");
        }

        rigidBody.velocity = Vector3.zero;
        rigidBody.angularVelocity = Vector3.zero;
        rigidBody.isKinematic = true;
        transform.SetParent(collider.transform);
    }
}
