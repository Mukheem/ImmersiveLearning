using UnityEngine;

public class Ball : MonoBehaviour
{
    public float forceMultiplier = 10f; // Adjust this value to control the force applied.

    void Start()
    {
        // Get the Rigidbody component attached to the ball.
        Rigidbody rb = GetComponent<Rigidbody>();

        // Check if a Rigidbody component is attached.
        if (rb != null)
        {
            // Do not apply the initial force here
        }
        else
        {
            Debug.LogError("Rigidbody component not found. Make sure to attach a Rigidbody to the ball GameObject.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Check if the touch is detected
        if (ArduinoIntegration.isTouchDetected)
        {
            ApplyForceToBall();
            ArduinoIntegration.isTouchDetected = false; // Reset the flag so it won't be detected again
        }
    }

    void ApplyForceToBall()
    {
        // Apply an initial force to the ball in a specific direction.
        Rigidbody rb = GetComponent<Rigidbody>();
        Vector3 initialForce = new Vector3(1f, 0f, 0f); // Change the direction as needed.
        rb.AddForce(initialForce * forceMultiplier, ForceMode.Impulse);
    }
}
