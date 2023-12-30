using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;

public class MoveGift : MonoBehaviour
{
    public string portName = "/dev/cu.usbmodem14201";  // Change to your Arduino port
    public int baudRate = 9600;       // Match with Arduino baud rate

    public float forceThreshold = 800.0f;  // Adjust the threshold based on your FSR characteristics
    public float moveSpeed = 0.5f;
    public float moveDistance = 0.1f;  // Distance to move when force threshold is surpassed

    private SerialPort serialPort;
    private bool isMoving = false;
    private Vector3 originalPosition;

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

        // Store the original position of the gift
        originalPosition = transform.position;
    }

    void Update()
    {
        if (serialPort.IsOpen)
        {
            try
            {
                // Read the FSR value from Arduino
                int fsrValue = int.Parse(serialPort.ReadLine());
                Debug.Log("FSR Value: " + fsrValue);
                Debug.Log("Static friction is appied, push harder to overcome the static friction and move the gift");

                // Check if the force exceeds the threshold
                if (fsrValue > forceThreshold && !isMoving)
                {
                    // Apply force to move the gift
                    float moveAmount = fsrValue / forceThreshold;
                    StartCoroutine(MoveGift1(moveAmount));

                    Debug.Log("Moving the gift. You have overcome static friction!");
                    //Debug.Log("Gift Position: " + transform.position);
                }
            }
            catch (System.Exception ex)
            {
                Debug.LogError("Error reading from Arduino: " + ex.Message);
            }
        }
    }

    IEnumerator MoveGift1(float moveAmount)
    {
        isMoving = true;

        // Calculate the target position
        Vector3 targetPosition = originalPosition + Vector3.left * moveDistance * moveAmount;

        // Move the gift smoothly
        while (transform.position != targetPosition)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
            yield return null;
        }

        isMoving = false;
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
