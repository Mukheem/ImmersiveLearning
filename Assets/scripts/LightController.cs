using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightController : MonoBehaviour
{
    public Light roomLight;
    public float initialIntensity = 0.1f; // Adjust this value for the desired initial intensity
    public float intensityChangeSpeed = 0.1f;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Light is off");
        // Set the initial intensity of the room light
        roomLight.intensity = initialIntensity;
    }

    // Update is called once per frame
    void Update()
    {


        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                roomLight.intensity += intensityChangeSpeed * Time.deltaTime;
                Debug.Log("Light is on. Current intensity: " + roomLight.intensity);
            }

            if (Input.GetKeyDown(KeyCode.DownArrow))  // Corrected line
            {
                roomLight.intensity -= intensityChangeSpeed * Time.deltaTime;
                roomLight.intensity = Mathf.Max(0, roomLight.intensity);
                Debug.Log("Light is off. Current intensity: " + roomLight.intensity);
            }
        }
    }
}