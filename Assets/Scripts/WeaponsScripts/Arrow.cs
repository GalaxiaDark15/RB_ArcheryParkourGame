using UnityEngine;

public class Arrow : MonoBehaviour
{
    public float damage;

    [SerializeField]
    private float torque;

    [SerializeField]
    private Rigidbody rigidBody;

    private bool didHit;

    public bool IsFired { get; private set; } = false;

    // NEW: Track if the arrow was fired by Enemy
    public bool isEnemyArrow = false;

    public void Fly(Vector3 force, bool firedByEnemy = false)
    {
        IsFired = true; // Arrow is now fired
        isEnemyArrow = firedByEnemy;

        gameObject.layer = LayerMask.NameToLayer("Arrows");
        rigidBody.isKinematic = false;
        rigidBody.AddForce(force, ForceMode.Impulse);
        rigidBody.AddTorque(transform.right * torque);
        transform.SetParent(null);
    }

    public void Prepare()
    {
        IsFired = false; // Arrow is only prepared, not fired yet
        didHit = false;
        isEnemyArrow = false; // Default: not from enemy
        rigidBody.isKinematic = true;
        rigidBody.velocity = Vector3.zero;
        rigidBody.angularVelocity = Vector3.zero;
        transform.SetParent(null);
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (didHit) return;
        didHit = true;

        rigidBody.velocity = Vector3.zero;
        rigidBody.angularVelocity = Vector3.zero;
        rigidBody.isKinematic = true;
        transform.SetParent(collider.transform);
    }
}
