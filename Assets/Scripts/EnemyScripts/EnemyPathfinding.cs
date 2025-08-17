using UnityEngine;
using UnityEngine.AI;

public class EnemyPathfinding : MonoBehaviour
{
    public Transform playerRef;
    private NavMeshAgent agent;
    private FieldOfView fov;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        if (playerRef == null)
            playerRef = GameObject.FindGameObjectWithTag("Player").transform;

        // Get reference to FieldOfView script on the same GameObject
        fov = GetComponent<FieldOfView>();
        if(fov == null)
            Debug.LogError("FieldOfView component missing on this GameObject!");
    }

    void Update()
    {
        if (playerRef != null && fov != null)
        {
            if (fov.canSeePlayer)
            {
                agent.isStopped = false;      // Resume movement
                agent.SetDestination(playerRef.position);
            }
            else
            {
                agent.isStopped = true;       // Stop moving if player not visible or in FOV
            }
        }
    }
}
