using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public float playerHealth = 100f;

    private void Update()
    {
        // if (Input.GetKeyDown(KeyCode.R))
        // {
        //     DecreaseHealth(10f);
        // }
    }

    private void DecreaseHealth(float amount)
    {
        playerHealth -= amount;
        playerHealth = Mathf.Max(playerHealth, 0f); // Prevent health going below 0
        Debug.Log("Player health decreased. Current health: " + playerHealth);
    }
}
