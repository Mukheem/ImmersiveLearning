using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;

public class ForceSensorIntegration : MonoBehaviour
{
    SerialPort portNum = new SerialPort("/dev/cu.usbmodem14201", 9600);


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
            try
            {
                string serialInput = portNum.ReadLine();
                Debug.Log(serialInput);

                // Check if the received message is "TOUCH_DETECTED"
                if (serialInput.Contains("TOUCH_DETECTED HERE"))
                {
                    // Code to trigger ball movement
                    Debug.Log("The Sensor is Touched now");
                }
            }
            catch (System.Exception e)
            {
                Debug.LogWarning(e.Message);
            }
        }
    }

    void OpenConnection()
    {
        if (portNum != null)
        {
            if (portNum.IsOpen)
            {
                portNum.Close();
                Debug.Log("Closing port, because it was already open!");
            }
            else
            {
                portNum.Open();
                portNum.ReadTimeout = 9000;
                Debug.Log("Port is Opened!");
            }
        }
        else
        {
            Debug.LogWarning("Port is null! Check please");
        }
    }

    void OnApplicationQuit()
    {
        if (portNum != null && portNum.IsOpen)
        {
            portNum.Close();
            Debug.Log("Port is Closed now!");
        }
    }
}
