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
        Arrow arrow = other.GetComponent<Arrow>();
        // Player only takes damage from arrows fired by enemies
        if (arrow != null && arrow.IsFired && arrow.isEnemyArrow)
        {
            Debug.Log("Player hit by enemy-fired arrow!");
            DecreaseHealth(arrow.damage);  // Uses Arrow's damage value
        }
    }

    private void DecreaseHealth(float amount)
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
