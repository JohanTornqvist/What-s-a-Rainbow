using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PositionTracker : MonoBehaviour
{
    // Interval (in seconds) to record the position
    [SerializeField] float trackInterval = 0.5f; 
    private List<Vector3> positions = new List<Vector3>();

    void Start()
    {
        StartCoroutine(TrackPosition());
    }

    IEnumerator TrackPosition()
    {
        while (true)
        {
            positions.Add(transform.position);
            yield return new WaitForSeconds(trackInterval);
        }
    }

    // Returns a copy of the tracked positions so external scripts can use the trail.
    public List<Vector3> GetTrackedPositions()
    {
        return new List<Vector3>(positions);
    }
}
