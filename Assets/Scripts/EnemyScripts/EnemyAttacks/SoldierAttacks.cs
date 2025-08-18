using System.Collections;
using UnityEngine;

public class SoldierAttacks : MonoBehaviour
{

    private Transform playerRef;
    private FieldOfView fov;
    private EnemyHealth enemyHealth;
    private EnemyPathfinding pathfinding;

    [SerializeField] private Collider axeCollider;      // Assign axe collider here (trigger collider)

    private bool canAttack = true;

    void Start()
    {
        playerRef = GameObject.FindGameObjectWithTag("Player")?.transform;
        fov = GetComponent<FieldOfView>();
        enemyHealth = GetComponent<EnemyHealth>();
        pathfinding = GetComponent<EnemyPathfinding>();

        if (axeCollider != null)
            axeCollider.enabled = false;  // Disable collider initially
        else
            Debug.LogError("Axe Collider is not assigned on SoldierAttacks script!");
    }

    void Update()
    {
        if (playerRef == null || fov == null || enemyHealth == null || enemyHealth.enemyHealth <= 0 || pathfinding == null)
            return;

        // Attack only if player is visible and allowed to attack
        if (fov.canSeePlayer && canAttack)
        {
            StartCoroutine(PerformAttack());
        }
    }

    private IEnumerator PerformAttack()
    {
        canAttack = false;

        // Pause movement during attack
        pathfinding.PauseMovement();

        // Enable axe collider to detect hits
        if (axeCollider != null)
            axeCollider.enabled = true;

        // Duration of attack animation/hit window (simulate attack)
        yield return new WaitForSeconds(0.5f);

        if (axeCollider != null)
            axeCollider.enabled = false;

        // Resume movement after attack
        pathfinding.ResumeMovement();

        canAttack = true;
    }
}
