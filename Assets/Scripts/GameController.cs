using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO.Ports;

public class GameController : MonoBehaviour
{
    // Serial Port to which Arduino is connected
    SerialPort arduinoPort = new SerialPort("/dev/cu.usbmodem11201", 9600);

    public AudioClip introClip;
    AudioSource audioSource;
    GameObject gravityText;

    // Method to connect/disconnect Arduino
    public void ConnectionWithArduino(bool makeConnection)
    {
        try
        {
            
            if (makeConnection)
            {
                Debug.Log("Connecting with Arduino...");
                arduinoPort.Open();
               
                if (arduinoPort.IsOpen)
                {
                    Debug.LogAssertion("Connection with Arduino established...");
                }
            }
            else
            {
                if (arduinoPort.IsOpen)
                {
                    Debug.Log("Disconnecting Arduino...");
                    arduinoPort.Close();
                    if (!arduinoPort.IsOpen)
                    {
                        Debug.LogAssertion("Connection with Arduino is now broke...");
                    }
                }
            }
            
           
        }
        catch (System.Exception)
        {
            throw;
        }
    }

    // Method to read data from Arduino
    public int ReadFromArduino()
    {
        int valueFromArduinoSensor = 0;

        //arduinoPort.ReadTimeout = 50000;
        if (arduinoPort.IsOpen)
        {
            valueFromArduinoSensor = Int32.Parse(arduinoPort.ReadLine());
            Debug.Log("Data From Arduino:" + valueFromArduinoSensor);
        }

        return valueFromArduinoSensor;
    }

    public void Start()
    {
        StartCoroutine(IntroNarration());
    }

    IEnumerator IntroNarration()
    {
        Debug.Log("+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++");
        if (!audioSource.isPlaying)
        {
            audioSource.clip = introClip;
            audioSource.Play();
            gravityText.SetActive(true);
        }
        yield return new WaitForSeconds(introClip.length);
    }
}
