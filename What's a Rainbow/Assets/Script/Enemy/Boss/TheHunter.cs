using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class TheHunter : MonoBehaviour
{
    [SerializeField] float moveSpeed = 5f;         
    [SerializeField] float chaseInterval = 0.1f;     

    private PositionTracker playerTracker;           
    private Transform playerTransform;              
    private List<Vector3> chasePath = new List<Vector3>();
    private int currentTargetIndex = 0;

    void Start()
    {
        // Find the player GameObject by tag and get its components.
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
            playerTracker = player.GetComponent<PositionTracker>();

            if (playerTracker != null)
            {
                StartCoroutine(ChasePlayer());
            }
            else
            {
                Debug.LogWarning("Player does not have a PositionTracker component!");
            }
        }
        else
        {
            Debug.LogWarning("No GameObject found with the 'Player' tag!");
        }
    }

    IEnumerator ChasePlayer()
    {
        while (true)
        {
            // Get the latest trail of the player's positions.
            chasePath = playerTracker.GetTrackedPositions();

            // If there are positions in the list, follow them one by one.
            if (chasePath.Count > 0)
            {
                // Ensure our target index is within range. Reset if needed.
                if (currentTargetIndex >= chasePath.Count)
                {
                    currentTargetIndex = 0;
                }

                Vector3 targetPos = chasePath[currentTargetIndex];

                // Move the enemy towards the target position.
                transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);

                // If the enemy is close enough to the target, move on to the next point.
                if (Vector3.Distance(transform.position, targetPos) < 0.1f)
                {
                    currentTargetIndex++;
                }
            }

            yield return new WaitForSeconds(chaseInterval);
        }
    }
}
