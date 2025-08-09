using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public float enemyHealth = 100f;

    void Start()
    {

    }

    void Update()
    {

    }

    void OnTriggerEnter(Collider other)
    {
        // Check if the collider belongs to an arrow object
        Arrow arrow = other.GetComponent<Arrow>();
        if (arrow != null)
        {
            Debug.Log("Enemy detected hit by arrow");
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
