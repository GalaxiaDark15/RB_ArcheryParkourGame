using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public float playerHealth = 100f;

    private void Update()
    {
        // Uncomment for manual testing in editor:
        // if (Input.GetKeyDown(KeyCode.R))
        // {
        //     DecreaseHealth(10f);
        // }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Previous arrow code remains...
        Arrow arrow = other.GetComponent<Arrow>();
        if (arrow != null && arrow.IsFired && arrow.isEnemyArrow)
        {
            Debug.Log("Player hit by enemy-fired arrow!");
            DecreaseHealth(arrow.damage);
        }

        // New melee damage detection from Soldier's axe
        if (other.gameObject.layer == LayerMask.NameToLayer("EnemyLayer"))
        {
            AxeSpin axeSpin = other.GetComponent<AxeSpin>();
            if (axeSpin != null)
            {
                DecreaseHealth(axeSpin.damage);
                Debug.Log("Player hit by soldier's melee attack with damage: " + axeSpin.damage);
            }
        }

    }


    public void DecreaseHealth(float amount)
    {
        playerHealth -= amount;
        playerHealth = Mathf.Max(playerHealth, 0f); // Prevent health going below 0
        Debug.Log("Player health decreased. Current health: " + playerHealth);
        if (playerHealth <= 0f)
        {
            PlayerDeath();
        }
    }

    private void PlayerDeath()
    {
        Debug.Log("Player Dead :(");
    }
}
