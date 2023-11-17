using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;

public class PlayerController : MonoBehaviour
{
    SerialPort arduinoPort = new SerialPort("/dev/cu.usbmodem11201", 9600);
    // Start is called before the first frame update
    void Start()
    {
        //arduinoPort.DtrEnable = true;
        //arduinoPort.RtsEnable = true;
        arduinoPort.Open();
       arduinoPort.ReadTimeout = 50000;
        
    }

    // Update is called once per frame
    void Update()
    {

        if (arduinoPort.IsOpen)
        {
            try
            {
               
                
                Debug.Log("Data From Arduino:"+arduinoPort.ReadLine());
                //arduinoPort.Close();
            }
            catch (System.Exception)
            {
                throw;
            }
        }
        else
        {
            Debug.Log("Hey");
        }
        transform.Translate(Vector3.forward * Time.deltaTime * Input.GetAxis("Horizontal"));
    }
    void OnApplicationQuit()
    {
        if (arduinoPort.IsOpen)
        {
            arduinoPort.Close();
        }
    }

}
