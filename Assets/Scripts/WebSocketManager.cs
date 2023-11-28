using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WebSocketSharp;


public class WebSocketManager : MonoBehaviour
{
    WebSocket ws;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Websocket System-------------------------+");
        ws = new WebSocket("ws://172.20.10.11:81");
        ws.OnOpen += (sender, e) =>
        {
            Debug.Log("WebSocket connected");
            ws.Send("Hello from Unity!");
        };
        ws.OnMessage += (sender, e) =>
        {
            Debug.Log("Received message: "+ e.Data);
           
        };
        ws.Connect();
        Debug.Log("Websocket----"+ws.ReadyState);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ws.Send("Need Force");
        }
    }
}
