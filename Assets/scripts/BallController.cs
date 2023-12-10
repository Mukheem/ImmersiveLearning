using System.Collections;
using System.Collections.Generic;
// BallController.cs

using UnityEngine;

public class BallController : MonoBehaviour
{
    public float forceMagnitude = 10f;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ApplyForce();
        }
    }

    void ApplyForce()
    {
        Vector3 force = new Vector3(forceMagnitude, 0, 0);
        rb.AddForce(force, ForceMode.Impulse);
    }
}

