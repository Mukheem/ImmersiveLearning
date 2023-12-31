using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;

public class Sampleforcereading : MonoBehaviour
{
    public string portName = "/dev/cu.usbmodem14201";  // Change to your Arduino port

    public int baudRate = 9600;       // Match with Arduino baud rate

    private SerialPort serialPort;

    void Start()
    {
        serialPort = new SerialPort(portName, baudRate);
        serialPort.Open();

        if (serialPort.IsOpen)
        {
            Debug.Log("Serial port opened successfully.");
        }
        else
        {
            Debug.LogError("Failed to open serial port.");
        }
    }

    void Update()
    {
        Debug.Log("trying to read Force Sensor Value: ");
        if (serialPort.IsOpen && serialPort.BytesToRead > 0)
        {
            // Read force sensor value from Arduino
            string line = serialPort.ReadLine();

            // Try to parse the received string as an integer
            if (int.TryParse(line, out int fsrValue))
            {
                Debug.Log("Force Sensor Value: " + fsrValue);

                // Your logic based on the force sensor value goes here
                // For example, you can trigger events or move objects based on the value
            }
        }
    }

    void OnDestroy()
    {
        if (serialPort != null && serialPort.IsOpen)
        {
            serialPort.Close();
            Debug.Log("Serial port closed.");
        }
    }
}
