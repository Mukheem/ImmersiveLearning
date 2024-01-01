using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;

public class MoveGift1 : MonoBehaviour
{
    public string portName = "/dev/cu.usbmodem14101";  // Change to your Arduino port
    public int baudRate = 9600;       // Match with Arduino baud rate

    public float forceThreshold;  // Adjust the threshold based on your FSR characteristics

    public AudioClip audioClip1;
    public AudioClip audioClip2;
    public AudioClip audioClip3;
    public AudioClip audioClip4;

    public float moveSpeed;
    public float moveDistance; // Distance to move when force threshold is surpassed

    private SerialPort serialPort;
    private AudioSource audioSource;

    private bool isClipPlaying = false;
    private bool clip2Played = false;

    

    void Start()    // checking for port here
    {
        serialPort = new SerialPort(portName, baudRate);
        Debug.Log(serialPort.IsOpen);
        serialPort.Open();

        if (serialPort.IsOpen)
        {
            Debug.Log("Serial port opened successfully.");
        }
        else
        {
            Debug.LogError("Failed to open serial port.");
        }

        // Initialize AudioSource
        audioSource = GetComponent<AudioSource>();

        // Start playing audio clips sequentially
        StartCoroutine(PlayAllAudioClips());
    }

    IEnumerator PlayAllAudioClips()
    {
        yield return StartCoroutine(PlayAudioClip(audioClip1));

        // Check for user input (L key) after playing audio clip 1
        yield return StartCoroutine(WaitForUserInput(KeyCode.L));

        // Play audio clip 2
        yield return StartCoroutine(PlayAudioClip(audioClip2));

        // Now continuously read the force sensor value
        StartCoroutine(ReadForceSensor());


    }

    IEnumerator PlayAudioClip(AudioClip clip)
    {
        if (clip != null && audioSource != null && !audioSource.isPlaying)
        {
            // Assign the audio clip and play
            audioSource.clip = clip;
            audioSource.Play();

            // Wait for the clip to finish playing
            while (audioSource.isPlaying)
            {
                isClipPlaying = true;
                yield return null;
            }

            isClipPlaying = false;
        }
    }

    IEnumerator WaitForUserInput(KeyCode key) // waiting for L key
    {
        while (!Input.GetKeyDown(key))
        {
            yield return null;
        }
    }

    IEnumerator ReadForceSensor()   // reading force sensor values for arduino
    {
        while (true)
        {
            if (serialPort.IsOpen && serialPort.BytesToRead > 0 && !isClipPlaying)
            {
                // Read force sensor value from Arduino
                string line = serialPort.ReadLine();
                
                // Try to parse the received string as an integer
                if (int.TryParse(line, out int fsrValue))
                {
                    Debug.Log("Force Sensor Value: " + fsrValue);


                    // Move the gift if the force sensor value is more than threshhold
                    if (fsrValue > forceThreshold)
                    {
                        MoveGift();
                        

                     yield return StartCoroutine(PlayAudioClip(audioClip3));

                    }
                }
            }
            yield return null;
        }
    }

    void MoveGift()
    {
        // Move the gift smoothly
        transform.Translate(Vector3.right * moveSpeed * moveDistance * Time.deltaTime);

        Debug.Log("Moving the gift. You have overcome static friction!");

        
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
