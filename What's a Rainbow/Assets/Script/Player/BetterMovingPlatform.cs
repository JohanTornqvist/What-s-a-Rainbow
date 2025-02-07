using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.GameCenter;

public class BetterMovingPlatform : MonoBehaviour
{
    public Rigidbody2D center;
    public Rigidbody2D point1;
    public Rigidbody2D point2;
    public Rigidbody2D platform;
    public float moveSpeed = 2f;
    public float moveDistance = 3f;
    public bool moveDiretion;

    private void Start()
    {
        if(platform.position.y <= center.position.y)
        {
            moveDiretion = false;
        }
        else
        {
            moveDiretion = true;
        }
        
    }

    void FixedUpdate()
    {
        center.position = (point1.position + point2.position)/2;
    }
}
