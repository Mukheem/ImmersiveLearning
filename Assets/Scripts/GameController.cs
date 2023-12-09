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
    AudioSource audioSource;
    GameObject gravityText;
    GameObject videoPlayerQuad;
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
        ws = new WebSocket("ws://"+esp32IPAddress+":"+esp32WebsocketPort);
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
        zoomToQuadPlayableDirector = GameObject.FindGameObjectWithTag("ZoomToQuad").GetComponent<PlayableDirector>();
        videoPlayerQuad.SetActive(false);
        gravityText.SetActive(false);
        audioSource = GetComponent<AudioSource>();
    }

    public void Start()
    {
        
        StartCoroutine(IntroNarration());
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
        yield return new WaitForSeconds((float)videoPlayer.length);
        //zoomToQuadPlayableDirector.
    }
}
