using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;

public class LightArduino : MonoBehaviour
{
    public Light sceneLight;
    SerialPort serialPort;

    void Start()
    {
        // Open the serial port (change COM3 to your Arduino port)
        serialPort = new SerialPort("/dev/cu.usbmodem14201", 9600);
        serialPort.Open();


        // Display initial message
        Debug.Log("Script is running la.");
        Debug.Log("Press 'L' to toggle the light on and off.");
        sceneLight.intensity = 0f;
    }

    void Update()
    {
        // Check for user input
        if (Input.GetKeyDown(KeyCode.L))
        {
            ToggleLight();
        }
    }

    void ToggleLight()
    {
        // Send the "Toggle" command to Arduino
        SendDataToArduino("L");
        Debug.Log("Sent command to toggle the light.");
        if (sceneLight.intensity == 0f) // scene light
        {
            // If the light is off, turn it on
            sceneLight.intensity = 1f; // Adjust the intensity as needed
            Debug.Log("Light turned on.");
        }
        else
        {
            // If the light is on, turn it off
            sceneLight.intensity = 0f;
            Debug.Log("Light turned off.");
        }
    }

    void SendDataToArduino(string data)
    {
        // Send data to Arduino
        serialPort.WriteLine(data);
    }

    void OnDestroy()
    {
        // Close the serial port when the application quits
        serialPort.Close();
    }
}
