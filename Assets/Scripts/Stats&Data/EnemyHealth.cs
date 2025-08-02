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

    public void TakeDamage()
    {
        enemyHealth -= 50f;

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
