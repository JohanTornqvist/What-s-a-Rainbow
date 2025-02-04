using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformMoveScript : MonoBehaviour
{
    public Vector3 pointA;
    public Vector3 pointB;
    public float speed = 2.0f;
    private Vector3 target;

    void Start()
    {
        target = pointA;
    }

    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, target) < 0.1f)
        {
            target = (target == pointA) ? pointB : pointA;
        }
    }
}