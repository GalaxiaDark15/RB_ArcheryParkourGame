using UnityEngine;
using UnityEngine.AI;

public class EnemyPathfinding : MonoBehaviour
{
    public Transform playerRef;
    private NavMeshAgent agent;
    private FieldOfView fov;

    // Set how close the enemy gets before stopping to attack
    public float stoppingDistance = 3f;   // Match this with your attack range

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        if (playerRef == null)
            playerRef = GameObject.FindGameObjectWithTag("Player").transform;

        fov = GetComponent<FieldOfView>();
        if (fov == null)
            Debug.LogError("FieldOfView component missing on this GameObject!");

        // Assign stopping distance to NavMeshAgent
        agent.stoppingDistance = stoppingDistance;
    }

    void Update()
    {
        if (playerRef != null && fov != null)
        {
            if (fov.canSeePlayer)
            {
                if (agent.isStopped == true)
                    agent.isStopped = false;

                agent.SetDestination(playerRef.position);
            }
            else
            {
                if (agent.isStopped == false)
                    agent.isStopped = true;

                agent.ResetPath();
            }
        }
    }

    // Public methods to pause and resume movement if needed (attack control)
    public void PauseMovement()
    {
        if (agent != null)
        {
            agent.isStopped = true;
            agent.velocity = Vector3.zero;
            agent.ResetPath();
        }
    }

    public void ResumeMovement()
    {
        if (agent != null)
        {
            agent.isStopped = false;
        }
    }
}
