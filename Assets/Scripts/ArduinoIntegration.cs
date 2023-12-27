using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;

public class ArduinoIntegration : MonoBehaviour
{
    SerialPort portNum = new SerialPort("/dev/cu.usbmodem13201", 9600);
    public static bool isTouchDetected = false; // Flag to indicate touch detection

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
                isTouchDetected = true; // Set the flag to true when touch is detected
                Debug.Log("The Sensor is Touched");
            }

        }
        //if (portNum.IsOpen)
        //{
        //    string a = portNum.ReadLine();
        //    Debug.Log(a);
        //}
        //if (portNum.IsOpen)
        //{
        //    try
        //    {

        //        string serialInput = portNum.ReadLine();
        //        Debug.Log(serialInput);

        //        // Check if the received message is "TOUCH_DETECTED"
        //        if (serialInput.Contains("TOUCH_DETECTED"))
        //        {
        //            isTouchDetected = true; // Set the flag to true when touch is detected
        //            Debug.Log("The Sensor is Touched");
        //        }
        //    }
        //    catch (System.Exception e)
        //    {
        //        Debug.LogWarning(e.Message);
        //    }
        //}
    }

    void OpenConnection()
    {

        //Debug.Log("Connecting to Arduino");
        //portNum.Open();

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
        
        //if (portNum.IsOpen)
        //{
        //    Debug.Log("It is opened");
        //    string a = portNum.ReadLine();
        //    Debug.Log(a);
        //}


        //if (portNum != null)
        //{
        //    if (portNum.IsOpen)
        //    {
        //        portNum.Close();
        //        Debug.Log("Closing port, because it was already open!");
        //    }
        //    else
        //    {
        //        portNum.Open();
        //        portNum.ReadTimeout = 9000;
        //        Debug.Log("Port Opened!");
        //    }
        //}
        //else
        //{
        //    Debug.LogWarning("Port is null!");
        //}

        //Debug.Log("I am here");
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
        //if (portNum != null && portNum.IsOpen)
        //{
        //    portNum.Close();
        //    Debug.Log("Port Closed!");
        //}
    }
}
