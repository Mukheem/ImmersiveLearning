using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerController : Subject
{
    GameController gameControllerRef = new GameController();
    private Rigidbody playerRigidBody;


    public void Awake()
    {
        gameControllerRef.ConnectionWithArduino(true);
    }

    // Start is called before the first frame update
    void Start()
    {
        playerRigidBody = GetComponent<Rigidbody>();
        //NotifyObservers(PlayerActionsEnum.Intro);
        


    }

    // Update is called once per frame
    void Update()
    {

        int upwardForce = 700;//gameControllerRef.ReadFromArduino();
        playerRigidBody.AddForce(Vector3.up * upwardForce, ForceMode.Force);
        transform.Translate(Vector3.forward * Time.deltaTime * Input.GetAxis("Horizontal"));
    }
    void OnApplicationQuit()
    {
        gameControllerRef.ConnectionWithArduino(false);
    }

}


