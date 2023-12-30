using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float pushForceMultiplier = 2f;
    public float staticFrictionThreshold = 500;  // Adjust the threshold as needed
    public float staticFrictionForce = 5f;        // Adjust the static friction force as needed

    private Rigidbody girlRigidbody;
    private SerialPort sp = new SerialPort("/dev/cu.usbmodem14201", 9600);

    void Start()
    {
        girlRigidbody = GetComponent<Rigidbody>();

        if (girlRigidbody == null)
        {
            Debug.LogError("Rigidbody not found on Girl GameObject!");
        }

        if (sp != null)
        {
            sp.Open();
            Debug.Log("Script is running. PlayerController");
        }
        else
        {
            Debug.LogError("SerialPort is null. Check if the correct COM port is specified.");
        }
    }

    void Update()
    {
        MoveGirl();
    }

    void MoveGirl()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(horizontal, 0f, vertical);
        girlRigidbody.AddForce(movement * moveSpeed);
        Debug.Log("Girl is moving.");

        // Print Santa's position for debugging
        Debug.Log("Girl Position: " + transform.position);

        // Print Santa's velocity for debugging
        Debug.Log("Girl Velocity: " + girlRigidbody.velocity.magnitude);

        if (sp.IsOpen)
        {
            string forceData = sp.ReadLine();

            // Attempt to parse forceData into an integer
            if (int.TryParse(forceData, out int forceValue))
            {
                Debug.Log("Force Sensor Reading: " + forceValue);

                // Check if Santa is near the gift's collider
                Collider giftCollider = null; // Replace this with the actual reference to the gift's collider
                float distanceToGift = Vector3.Distance(transform.position, giftCollider.transform.position);

                // If Santa is near the gift, attempt to push it
                if (distanceToGift < 2f) // Adjust the distance as needed
                {
                    // Map forceValue to push force
                    float pushForce = forceValue * pushForceMultiplier;

                    // Apply static friction if the force sensor reading suggests static friction
                    if (forceValue > staticFrictionThreshold && girlRigidbody.velocity.magnitude < 0.1f)
                    {
                        // Apply static friction force to oppose the attempted motion
                        girlRigidbody.AddForce(-girlRigidbody.velocity.normalized * staticFrictionForce, ForceMode.Impulse);
                        Debug.Log("Static friction applied!");
                    }
                    else
                    {
                        // Apply force to the gift in the forward direction of Santa
                        // Note: Replace 'other' with the actual reference to the gift's Rigidbody
                        giftCollider.GetComponent<Rigidbody>().AddForce(transform.forward * pushForce, ForceMode.Impulse);
                        Debug.Log("Girl pushed the gift with force: " + pushForce);
                    }
                }
            }
            else
            {
                Debug.LogError("Failed to parse force sensor reading. Received: " + forceData);
            }
        }
    }
}
