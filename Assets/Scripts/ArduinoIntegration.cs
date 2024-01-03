using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;

public class ArduinoIntegration : MonoBehaviour
{
    SerialPort portNum = new SerialPort("/dev/cu.usbmodem13201", 9600);

    private int charCount = 26;
    // Flag to indicate touch detection
    public static bool isTouchDetected = false;

    // Start is called before the first frame update
    void Start()
    {
        OpenConnection();
        new System.Threading.Thread(CheckSerial).Start();
    }

    void CheckSerial()
    {
        while (true)
        {

            if (portNum.IsOpen)
            {
                Debug.Log("It is opened");
                string a = portNum.ReadExisting();
                Debug.Log(a);
                if(a.Length >= charCount)
                    a = a.Substring(a.Length - charCount);
                if (a.Contains("TOUCH_PRESSED"))
                {
                    // Set the flag to true when touch pressed
                    isTouchDetected = true;
                    Debug.Log("The Sensor is Touched");
                }
                if (a.Contains("TOUCH_RELEASE"))
                {
                    // Set the flag to true when touch released
                    isTouchDetected = false;
                    Debug.Log("The Sensor released");
                }
            }
            System.Threading.Thread.Sleep(200);
        }
    }

    // Update is called once per frame
    void Update()
    {
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