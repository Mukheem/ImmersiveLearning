using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public Rigidbody ballOne;
    public Rigidbody ballTwo;
    public Rigidbody ballThree;

    // Start is called before the first frame update
    void Start()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartRolling();
        }

        //ApplyFriction(ballOne);
        //ApplyFriction(ballTwo);
        //ApplyFriction(ballThree);
    }

    void StartRolling()
    {

        ballOne.AddForce(transform.right * Time.deltaTime * 10);
        ballTwo.AddForce(transform.right* Time.deltaTime * 10);
        ballThree.AddForce(transform.right * Time.deltaTime * 10);

        //ballOne.velocity = new Vector3(5f, 0f, 0f);  // Initial velocity, adjust as needed
        //ballTwo.velocity = new Vector3(5f, 0f, 0f);
        //ballThree.velocity = new Vector3(5f, 0f, 0f);
    }

    //void ApplyFriction(Rigidbody ball)
    //{
    //    Collider[] colliders = Physics.OverlapSphere(ball.position, 0.1f);

    //    foreach (Collider collider in colliders)
    //    {
    //        if (collider.CompareTag("Surface"))
    //        {
    //            PhysicMaterial surfaceMaterial = collider.material;

    //            // Get the friction values from the physics material
    //            float dynamicFriction = surfaceMaterial.dynamicFriction;

    //            // Reduce the velocity based on the friction
    //            ball.velocity *= Mathf.Pow(1 - dynamicFriction, Time.deltaTime);

    //            // Check if the velocity is below a threshold, then explicitly stop the ball
    //            if (ball.velocity.magnitude < 0.01f)
    //            {
    //                ball.velocity = Vector3.zero;
    //            }

    //            break; // Exit the loop after finding the surface
    //        }
    //    }
    //}
}
