using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;

public class MoveGift1 : MonoBehaviour
{
    public string portName = "/dev/cu.usbmodem14201";  // Change to your Arduino port
    public int baudRate = 9600;       // Match with Arduino baud rate

    public float forceThreshold = 800.0f;  // Adjust the threshold based on your FSR characteristics
    public float moveSpeed = 0.5f;
    public float moveDistance = 0.1f;  // Distance to move when force threshold is surpassed

    public AudioClip audioClip1;
    public AudioClip audioClip2;
    public AudioClip audioClip3;
    public AudioClip audioClip4;
    public AudioClip audioClip5;

    private SerialPort serialPort;
    private bool isMoving = false;
    private Vector3 originalPosition;
    private AudioSource audioSource;

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

        // Initialize AudioSource
        audioSource = GetComponent<AudioSource>();
        Debug.Log("before playallaudioclips");

        // Start playing audio clips sequentially
        StartCoroutine(PlayAllAudioClips());
    }

    IEnumerator PlayAllAudioClips()
    {
        Debug.Log("in PlayALLAudioClip");
        yield return StartCoroutine(PlayAudioClip(audioClip1));
        yield return StartCoroutine(PlayAudioClip(audioClip2));
        yield return StartCoroutine(PlayAudioClip(audioClip3));

        while (true)
        {
            // Read the FSR value from Arduino
            int fsrValue = int.Parse(serialPort.ReadLine());
            Debug.Log("FSR Value: " + fsrValue);
            Debug.Log("Static friction is applied, push harder to overcome the static friction and move the gift");
            yield return StartCoroutine(PlayAudioClip(audioClip4));

            // Check if the force exceeds the threshold
            if (fsrValue > forceThreshold && !isMoving)
            {
                // Apply force to move the gift
                float moveAmount = fsrValue / forceThreshold;
                StartCoroutine(MoveGift(moveAmount));

                Debug.Log("Moving the gift. You have overcome static friction!");
                // Debug.Log("Gift Position: " + transform.position);
                yield return StartCoroutine(PlayAudioClip(audioClip5));
                break;
            }
        }
    }

    IEnumerator MoveGift(float moveAmount)
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

    IEnumerator PlayAudioClip(AudioClip clip)
    {
        
        if (clip != null && audioSource != null && !audioSource.isPlaying)
        {
            Debug.Log("in PlayAudioClip IF");
            // Assign the audio clip and play
            audioSource.clip = clip;
            audioSource.Play();

            // Wait for the clip to finish playing
            yield return new WaitForSeconds(clip.length);
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
