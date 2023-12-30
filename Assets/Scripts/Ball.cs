using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Ball : MonoBehaviour
{
    private AudioSource narration;

    private float narrationLenght = 106.0f;
    public float forceMultiplier = 10f;
   

    void Start()
    {
        narration = GameObject.Find("Balls").GetComponent<AudioSource>();

        StartCoroutine(AllowSensing());

        // Get the Rigidbody component attached to the ball.
        Rigidbody rb = GetComponent<Rigidbody>();

        // Check if a Rigidbody component is attached.
        if (rb == null)
        {
            Debug.LogError("Rigidbody component not found.");
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    void ApplyForceToBall()
    {
        // Apply an initial force to the ball in a specific direction.
        Rigidbody rb = GetComponent<Rigidbody>();
        Vector3 initialForce = new Vector3(1f, 0f, 0f);

        rb.AddForce(initialForce * forceMultiplier, ForceMode.Impulse);
    }

    public IEnumerator AllowSensing()
    {
        narration.Play();
        yield return new WaitForSeconds(narrationLenght);

        if (ArduinoIntegration.isTouchDetected)
        {
            ApplyForceToBall();
            yield return new WaitForSeconds(20);
            ArduinoIntegration.isTouchDetected = false; // Reset the flag so it won't be detected again
        }
    }


}
