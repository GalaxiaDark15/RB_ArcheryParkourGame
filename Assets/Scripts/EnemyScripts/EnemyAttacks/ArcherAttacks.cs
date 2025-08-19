using UnityEngine;
using System.Collections;

public class ArcherAttacks : MonoBehaviour
{
    public float attackRange = 20f;
    public float attackCooldown = 2f;

    private float cooldownTimer = 0f;
    private Transform playerRef;
    private FieldOfView fieldOfView;
    private Bow bow;
    private EnemyHealth enemyHealth;

    private void Start()
    {
        playerRef = GameObject.FindGameObjectWithTag("Player")?.transform;
        fieldOfView = GetComponent<FieldOfView>();
        bow = GetComponentInChildren<Bow>();
        enemyHealth = GetComponent<EnemyHealth>();

        // Prepare arrow initially for a quick first shot
        if (bow != null && !bow.IsReady())
            bow.PrepareArrow();
    }

    private void Update()
    {
        if (playerRef == null || fieldOfView == null || bow == null || enemyHealth == null)
            return;

        // Only shoot if alive
        if (enemyHealth.enemyHealth <= 0f)
            return;

        cooldownTimer -= Time.deltaTime;

        float distanceToPlayer = Vector3.Distance(transform.position, playerRef.position);

        if (fieldOfView.canSeePlayer && distanceToPlayer <= attackRange && bow.IsReady())
        {
            RotateTowardsPlayer();

            if (cooldownTimer <= 0f)
            {
                bow.Fire(100f); // Adjust power as needed
                cooldownTimer = attackCooldown;
                // Reload next arrow after cooldown asynchronously
                StartCoroutine(PrepareNextArrowAfterReload());
            }
        }
    }

    private void RotateTowardsPlayer()
    {
        Vector3 lookPos = new Vector3(playerRef.position.x, transform.position.y, playerRef.position.z);
        Vector3 direction = (lookPos - transform.position).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 5f * Time.deltaTime);
    }

    private IEnumerator PrepareNextArrowAfterReload()
    {
        float reloadTime = bow.ReloadTime;
        yield return new WaitForSeconds(reloadTime);

        if (bow != null && !bow.IsReady())
        {
            bow.PrepareArrow();
        }
    }
}
