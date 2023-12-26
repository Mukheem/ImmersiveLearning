using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO.Ports;
using WebSocketSharp;
using UnityEngine.Video;
using UnityEngine.Playables;

public class GameController : MonoBehaviour
{
    // Serial Port to which Arduino is connected
    SerialPort arduinoPort = new SerialPort("/dev/cu.usbmodem11201", 9600);
    // Websocet Service
    WebSocket ws;
    String esp32IPAddress = "172.20.10.11";
    String esp32WebsocketPort = "81";

    public AudioClip introClip;
    public AudioClip introToPracticalClip;
    AudioSource audioSource;
    GameObject gravityText;
    GameObject videoPlayerQuad;
    GameObject virtualCamZoomToQuad;
    GameObject virtualCamFocusOnBall;
    PlayableDirector zoomToQuadPlayableDirector;

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




    // Method to connect/disconnect ESP32
    public void ConnectWithESP32()
    {
        Debug.Log("Connecting Unity with ESP32 via Websockets...");
        ws = new WebSocket("ws://" + esp32IPAddress + ":" + esp32WebsocketPort);
        ws.OnOpen += (sender, e) =>
        {
            Debug.Log("WebSocket connected");
            ws.Send("Hello from Unity!");
        };
        ws.OnMessage += (sender, e) =>
        {
            Debug.Log("Received message: " + e.Data);

        };
        ws.Connect();
        Debug.Log("Websocket state - " + ws.ReadyState);
    }

    private void OnEnable()
    {

        gravityText = GameObject.FindGameObjectWithTag("IntroText");
        videoPlayerQuad = GameObject.FindGameObjectWithTag("VideoPlayer");
        virtualCamZoomToQuad = GameObject.FindGameObjectWithTag("ZoomToQuad");
        virtualCamFocusOnBall = GameObject.FindGameObjectWithTag("CMFocusOnBall");
        zoomToQuadPlayableDirector = GameObject.FindGameObjectWithTag("ZoomToQuad").GetComponent<PlayableDirector>();

        ActivateCamera("ZoomToQuad");
        videoPlayerQuad.SetActive(false);
        gravityText.SetActive(false);
        audioSource = GetComponent<AudioSource>();
    }

    public void Start()
    {

        StartCoroutine(IntroNarration());
        //StartCoroutine(IntroToPractical());
    }

    IEnumerator IntroNarration()
    {
        Debug.Log("Playing IntroNarration Coroutine...");
        if (!audioSource.isPlaying)
        {
            // audioSource.clip = introClip;
            audioSource.PlayOneShot(introClip);

            gravityText.SetActive(true);
        }
        yield return new WaitForSeconds(introClip.length);

        videoPlayerQuad.SetActive(true);
        StartCoroutine(PlayVideo());

    }

    IEnumerator PlayVideo()
    {
        zoomToQuadPlayableDirector.Play();
        VideoPlayer videoPlayer = videoPlayerQuad.GetComponent<VideoPlayer>();
        Debug.Log("Playing Video...");
        if (!videoPlayer.isPlaying)
        {
            videoPlayer.Play();
        }
        Debug.Log("Length is " + (float)videoPlayer.length);
        yield return new WaitForSeconds((float)videoPlayer.length - 33.0f);
        //yield return new WaitForSeconds(7);
        StartCoroutine(ZomOutFromQuad(videoPlayer)); // Starts co-routine to move back camera to original position. Parameter videoPlayer is used to close the quad when playing is finished.
    }

    IEnumerator ZomOutFromQuad(VideoPlayer videoPlayer)
    {


        float dt = (float)zoomToQuadPlayableDirector.duration;

        while (dt > 0)
        {
            dt -= Time.deltaTime / (float)zoomToQuadPlayableDirector.duration;

            zoomToQuadPlayableDirector.time = Mathf.Max(dt, 0);
            zoomToQuadPlayableDirector.Evaluate();
            yield return null;
        }
        videoPlayer.loopPointReached += CloseQuad;
        StartCoroutine(IntroToPractical());
        CloseQuad(videoPlayer);
    }
    
    IEnumerator IntroToPractical()
    {
        Debug.Log("Playing IntroToPractical Coroutine...");
        audioSource.PlayOneShot(introToPracticalClip);
        yield return new WaitForSeconds(21.0f);
        ActivateCamera("CMFocusOnBall");
        yield return new WaitForSeconds(introToPracticalClip.length);
    }
    private void CloseQuad(VideoPlayer vp)
    {
        Debug.Log("Disabling the Quad...");
        videoPlayerQuad.SetActive(false);
    }
    private void ActivateCamera(String cameraTagName)
    {
        if (cameraTagName.Equals("ZoomToQuad"))
        {
            virtualCamFocusOnBall.SetActive(false);
            virtualCamZoomToQuad.SetActive(true);
        }
        else if (cameraTagName.Equals("CMFocusOnBall"))
        {
            virtualCamZoomToQuad.SetActive(false);
            virtualCamFocusOnBall.SetActive(true);

        }
    }
}

/*
 * 
Well, now it's time for some real-life magic.

Welcome to our adventure at the Joy Land Park! Before we dive into the fun, let's learn something exciting! Do you see the glowing light? That's the signal! When it shines, it's time to press the magic force sensor connected to our redboard.
When you gently press the sensor, it makes the ball in our scene float up high or down low.  The harder you press, the higher our magical ball floats!

But remember, you can only press the sensor when the light is on. So, let's wait for the signal and then work our magic with the force sensor to make the ball dance in the air! Are you ready? Let's have some fun!
*/