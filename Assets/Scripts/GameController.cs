using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{

    public Rigidbody ballOne;
    public Rigidbody ballTwo;
    public Rigidbody ballThree;

    public float frictionMultiplier = 0.99f; // Adjust based on the desired friction
    //public float stopThreshold = 0.1f; // Adjust based on when you consider the ball stopped



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartRolling();
        }

        ApplyFriction(ballOne);
        ApplyFriction(ballTwo);
        ApplyFriction(ballThree);
    }

    void StartRolling()
    {
        ballOne.velocity = new Vector3(10f, 0f, 0f);  // Initial velocity, adjust as needed
        ballTwo.velocity = new Vector3(10f, 0f, 0f);
        ballThree.velocity = new Vector3(10f, 0f, 0f);
    }

    void ApplyFriction(Rigidbody ball)
    {
        // Gradually slow down the ball based on the friction multiplier
        ball.velocity *= frictionMultiplier;

        // If the velocity is below the stop threshold, consider the ball stopped
        //if (ball.velocity.magnitude < stopThreshold)
        //{
        //    ball.velocity = Vector3.zero;
        //}
    }
}
