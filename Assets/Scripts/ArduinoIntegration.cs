using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;

public class ArduinoIntegration : MonoBehaviour
{
    SerialPort portNum = new SerialPort("/dev/cu.usbmodem113201", 9600);


    // Flag to indicate touch detection
    public static bool isTouchDetected = false; 

    // Start is called before the first frame update
    void Start()
    {
        OpenConnection();
    }

    // Update is called once per frame
    void Update()
    {
        if (portNum.IsOpen)
        {
            Debug.Log("It is opened");
            string a = portNum.ReadLine();
            Debug.Log(a);
            if (a.Contains("TOUCH_DETECTED"))
            {
                // Set the flag to true when touch is detected
                isTouchDetected = true; 
                Debug.Log("The Sensor is Touched");
            }

        }
    }

    void OpenConnection()
    {

        try
        {
            Debug.Log("Connecting to Arduino ... ");
            portNum.Open();
            Debug.Log("Connection to Arduino completed");

        }
        catch (System.Exception)
        {
            throw;
        }
        
    }

    void OnApplicationQuit()
    {
        try
        {
            if (portNum.IsOpen)
            {
                Debug.Log("disconnecting from Arduino ...");
                portNum.Close();
                if (!portNum.IsOpen)
                {
                    Debug.Log("Connection from Arduino stopped");
                }
            }
        }
        catch (System.Exception)
        {
            throw;
        }
    }
}
