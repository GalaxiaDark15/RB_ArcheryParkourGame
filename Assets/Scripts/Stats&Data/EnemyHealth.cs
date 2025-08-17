using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public float enemyHealth = 100f;

    void OnTriggerEnter(Collider other)
    {
        Arrow arrow = other.GetComponent<Arrow>();
        if (arrow != null && arrow.IsFired) // Only take damage if arrow is fired
        {
            Debug.Log("Enemy detected hit by fired arrow");
            TakeDamage();
        }
    }



    public void TakeDamage()
    {
        enemyHealth -= 50f;
        Debug.Log(enemyHealth);

        if (enemyHealth <= 0f)
        {
            EnemyDeath();
        }
    }

    public void EnemyDeath()
    {
        Destroy(gameObject);
    }
}
