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

    private bool didHit;

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
        // Debug.Log($"Arrow hit something: {collider.name}, tag: {collider.tag}");
        if (didHit) return;
        didHit = true;

        rigidBody.velocity = Vector3.zero;
        rigidBody.angularVelocity = Vector3.zero;
        rigidBody.isKinematic = true;
        transform.SetParent(collider.transform);
    }

}
