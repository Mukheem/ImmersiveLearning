using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO.Ports;
using WebSocketSharp;
using UnityEngine.Video;
using UnityEngine.Playables;

// Subject in the Observer Pattern
public class GameController : Subject
{
    // Serial Port to which Arduino is connected
    SerialPort arduinoPort = new SerialPort("/dev/cu.usbmodem11201", 9600);
    // Websocket Service
    WebSocket ws;
    String esp32IPAddress = "172.20.10.3";
    String esp32WebsocketPort = "81";

    private float gravityModifier = 1.0f;
    private float moonGravityModifier = 0.1667f;
    private float jupiterGravityModifier = 2.4f;

    public AudioClip introClip;
    public AudioClip introToPracticalClip;
    public AudioClip earthGravityNarrationClip;
    public AudioClip pauseNPlayClip;
    public AudioClip moonGravityNarrationClip;
    public AudioClip jupiterGravityNarrationClip;
    public AudioClip outroClip;
    AudioSource audioSource;
    GameObject gravityText;
    GameObject videoPlayerQuad;
    GameObject virtualCamZoomToQuad;
    GameObject virtualCamFocusOnBall;
    GameObject gameMenu;
    PlayableDirector zoomToQuadPlayableDirector;
    public Material moonSkyboxMaterial;
    public Material jupiterSkyBoxMaterial;

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




    // Method to connect ESP32
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
            NotifyObservers(int.Parse(e.Data));
        };
        ws.Connect();
        Debug.Log("Websocket state - " + ws.ReadyState);
    }

    // All the required initialisations and component extractions from the hierarchy
    private void OnEnable()
    {
        ConnectWithESP32();
        gravityText = GameObject.FindGameObjectWithTag("IntroText");
        videoPlayerQuad = GameObject.FindGameObjectWithTag("VideoPlayer");
        virtualCamZoomToQuad = GameObject.FindGameObjectWithTag("ZoomToQuad");
        virtualCamFocusOnBall = GameObject.FindGameObjectWithTag("CMFocusOnBall");
        zoomToQuadPlayableDirector = GameObject.FindGameObjectWithTag("ZoomToQuad").GetComponent<PlayableDirector>();
        gameMenu = GameObject.Find("GameMenu");

        ActivateCamera("ZoomToQuad");
        videoPlayerQuad.SetActive(false);
        gravityText.SetActive(false);
        audioSource = GetComponent<AudioSource>();
    }

    public void Start()
    {
        
        // Starting the first co-routine
        StartCoroutine(IntroNarration());
        
        //Updating gravity across the scene
        changeGravityModifier(gravityModifier);


    }
    // If the user needs the practicality again and again
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {

            ws.Send("Need Force");

        }
    }

    IEnumerator IntroNarration()
    {
        yield return new WaitForSeconds(gameMenu.GetComponent<GameMenuManager>().waitSeconds);
        Debug.Log("Playing IntroNarration Coroutine...");
        if (!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(introClip);
            gravityText.SetActive(true);
        }
        yield return new WaitForSeconds(introClip.length);

        videoPlayerQuad.SetActive(true); // Turning on the quad player
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

        yield return new WaitForSeconds((float)videoPlayer.length - 33.0f);
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
        // Total length of introToPracticalClip is 50 seconds. waiting here for 21 seconds and rest 29 seconds in next wait statement.
        yield return new WaitForSeconds(21.0f);
        ActivateCamera("CMFocusOnBall");
        yield return new WaitForSeconds(29.0f);

        //Sending a request to ESP32-S2-Wroom-Thing Plus to read sensor value
        ws.Send("Need Force");

        yield return new WaitForSeconds(5.0f); //waiting for the user to find the sensor and use it. then the narration can start.
        //Narrates the practicality of earth's Gravity and waits till the completion
        audioSource.PlayOneShot(earthGravityNarrationClip);
        yield return new WaitForSeconds(earthGravityNarrationClip.length);

        //Narrates the pauseNPlay rule and waits till the completion
        audioSource.PlayOneShot(pauseNPlayClip);
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.C));
        //Change camera view to Narrator
        ActivateCamera("ZoomToQuad");

        StartCoroutine(changeToMoon());

    }

    IEnumerator changeToMoon()
    {
        Debug.Log("Change to Moon Coroutine...");
        //Updating gravity across the scene
        changeGravityModifier(moonGravityModifier);

        
        audioSource.PlayOneShot(moonGravityNarrationClip);
        // Waiting 8 seconds so that we change the skybox inline to the narration
        yield return new WaitForSeconds(8.0f);
        RenderSettings.skybox = moonSkyboxMaterial;

        yield return new WaitForSeconds(moonGravityNarrationClip.length-7.0f);
        ActivateCamera("CMFocusOnBall");

        //Sending a request to ESP32-S2-Wroom-Thing Plus to read sensor value
        ws.Send("Need Force");
               
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.C));
        //Change camera view to Narrator
        ActivateCamera("ZoomToQuad");
        StartCoroutine(changeToJupiter());
    }

    IEnumerator changeToJupiter()
    {
        Debug.Log("Change to Jupiter Coroutine...");
        //Updating gravity across the scene
        changeGravityModifier(jupiterGravityModifier);


        audioSource.PlayOneShot(jupiterGravityNarrationClip);
        // Waiting 8 seconds so that we change the skybox inline to the narration
        yield return new WaitForSeconds(10.0f);
        RenderSettings.skybox = jupiterSkyBoxMaterial;

        yield return new WaitForSeconds(jupiterGravityNarrationClip.length - 10.0f);
        ActivateCamera("CMFocusOnBall");

        //Sending a request to ESP32-S2-Wroom-Thing Plus to read sensor value
        ws.Send("Need Force");

        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.C));
        //Change camera view to Narrator
        ActivateCamera("ZoomToQuad");

        StartCoroutine(OutroNarration());
    }

    IEnumerator OutroNarration()
    {
        Debug.Log("Playing OutroNarration Coroutine...");

        audioSource.PlayOneShot(outroClip);
        
        yield return new WaitForSeconds(outroClip.length);

        Application.Quit();


    }

    //Method to close quad player
    private void CloseQuad(VideoPlayer vp)
    {
        Debug.Log("Disabling the Quad...");
        videoPlayerQuad.SetActive(false);
    }

    //Method to switch cameras in the scene.
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

    //Closing websocket upon application quit
    void OnApplicationQuit()
    {
        ws.Close();
        Debug.Log("WebSocket closed on application exit");
    }
    //Method to modify Gravity across the scene
    protected void changeGravityModifier(float gravityModifierValue)
    {
        Debug.Log("Changing Gravity to: " + gravityModifierValue);
        Physics.gravity *= gravityModifierValue;
    }
}

/*
 * 
Well, now it's time for some real-life magic.

Welcome to our adventure at the Joy Land Park! Before we dive into the fun, let's learn something exciting! Do you see the glowing light? That's the signal! When it shines, it's time to press the magic force sensor connected to our redboard.
When you gently press the sensor, it makes the ball in our scene float up high or down low.  The harder you press, the higher our magical ball floats!

But remember, you can only press the sensor when the light is on. So, let's wait for the signal and then work our magic with the force sensor to make the ball dance in the air! Are you ready? Let's have some fun!

 ***
 *
 So when you throw the ball, the kinetic energy it gets from you counteracts the earth's gravity, causing it to move upwards. At the peak, vertical kinetic energy that the ball possesses becomes zero and then gravity becomes dominant pulling it back down



Great job! I hope you've grasped the basics of how gravity operates on Earth. Now, it's time to have fun with the sensor and the ball. The program pauses until you press the 'C' key on the keyboard. Whenever you're ready to continue, press 'C'. Until then, you can keep pressing the 'Space bar' on the keyboard to enjoy the hands-on activity as many times as you like.

I hope you enjoyed our gravity experiment on the Moon! Now, let's explore how it changes on the giant planet in our solar system, Jupiter. Unlike the Moon, Jupiter's gravity is incredibly strong--about 2.4 times stronger than Earth's! If you were there, you'd feel much heavier. You can redo the experiment to feel how it would be on Jupiter!

Thank you all for being part of our gravity exploration today! I trust you've discovered how gravity varies across different planets, offering us a glimpse into the diverse nature of our universe. Keep exploring and diving into the wonders of physics?it's an endless journey filled with remarkable discoveries!



Yours John, Signing off.
        
 */