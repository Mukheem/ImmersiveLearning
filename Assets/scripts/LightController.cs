using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightController : MonoBehaviour

{
    public Light roomLight;
    public float initialIntensity = 0.1f; // Adjust this value for the desired initial intensity
    public float intensityChangeSpeed = 1.0f;
    public Color yellowColor = Color.yellow;
    public Color pinkColor = new Color(1.0f, 0.5f, 0.5f); // Adjust RGB values for the desired pink color

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Light is off");
        // Set the initial intensity of the room light
        roomLight.intensity = initialIntensity;
        // Set the initial color to pink
        roomLight.color = pinkColor;
        // Set ambient light to black
        RenderSettings.ambientLight = Color.black;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            roomLight.intensity += intensityChangeSpeed * Time.deltaTime;
            roomLight.color = yellowColor;
            Debug.Log("Light is on. Current intensity: " + roomLight.intensity);
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            roomLight.intensity -= intensityChangeSpeed * Time.deltaTime;
            roomLight.intensity = Mathf.Max(0, roomLight.intensity);
            roomLight.color = pinkColor; // Maintain pinkColor
            Debug.Log("Light is off. Current intensity: " + roomLight.intensity);
        }
    }
}
