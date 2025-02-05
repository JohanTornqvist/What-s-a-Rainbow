using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PositionTracker : MonoBehaviour
{
    [SerializeField] float trackInterval = 0.5f;
    [SerializeField] int maxPositions = 1000; 
    private List<Vector3> positions = new List<Vector3>();
    private Collider2D col;

    void Start()
    {
        col = GetComponent<Collider2D>();
        StartCoroutine(TrackPosition());
    }

    IEnumerator TrackPosition()
    {
        while (true)
        {
            Vector3 currentPos = (col != null) ? col.bounds.center : transform.position;
            if (positions.Count >= maxPositions)
            {
                positions.RemoveAt(0); 
            }
            positions.Add(currentPos);
            yield return new WaitForSeconds(trackInterval);
        }
    }

    /// Returns a copy of the tracked positions.
    public List<Vector3> GetTrackedPositions()
    {
        return new List<Vector3>(positions);
    }
}
