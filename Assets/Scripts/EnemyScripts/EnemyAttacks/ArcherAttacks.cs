using UnityEngine;

public class ArcherAttack : MonoBehaviour
{
    public float attackRange = 20f;
    public float attackCooldown = 2f;

    private float cooldownTimer = 0f;
    private Transform playerRef;
    private FieldOfView fieldOfView;
    private Bow bow;

    private void Start()
    {
        playerRef = GameObject.FindGameObjectWithTag("Player").transform;
        fieldOfView = GetComponent<FieldOfView>();
        bow = GetComponentInChildren<Bow>();
    }

    private void Update()
    {
        if (playerRef == null || fieldOfView == null || bow == null)
            return;

        cooldownTimer -= Time.deltaTime;

        float distanceToPlayer = Vector3.Distance(transform.position, playerRef.position);

        if (fieldOfView.canSeePlayer && distanceToPlayer <= attackRange && bow.IsReady())
        {
            RotateTowardsPlayer();

            if (cooldownTimer <= 0f)
            {
                bow.Fire(50f);  // Example firePower, adjust if needed
                cooldownTimer = attackCooldown;
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
}
