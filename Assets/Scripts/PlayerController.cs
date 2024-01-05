using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Observer in the Observer Pattern
public class PlayerController : MonoBehaviour, IObserver
{

    // --Observer Pattern Starts
    [SerializeField]
    private Subject _subjectGameController;
    private Rigidbody playerRigidBody;
    private bool _levitateBall = false;
    int modifiedForceValue = 0;
    
    private void OnEnable()
    {
        _subjectGameController.AddObserver(this);
        playerRigidBody = this.GetComponent<Rigidbody>(); //Getting player's rigid body
        

    }

    public void Start()
    {

    }

    //When force is applied, Game controller notifies and passes on the force value to PlayerController which activates the levitation.
    public void OnNotify(int forceValue)
    {
         modifiedForceValue = forceValue;
        if (forceValue <= 50)
        {
            modifiedForceValue = 100;
        }
        else if (forceValue > 50 && forceValue <= 100)
        {
            modifiedForceValue = 100;
        }
        else if(forceValue>100 && forceValue <= 300)
        {
            modifiedForceValue = 300;
        }
        
        
        Debug.Log("Actual ForceValue:"+forceValue);
        Debug.Log("Modified ForceValue:"+modifiedForceValue);
        _levitateBall = true;
        
    }

    private void OnDisable()
    {
        _subjectGameController.RemoveObserver(this);
    }
    // --Observer Pattern Ends

  
    public void FixedUpdate()
    {
        while (_levitateBall)
        {
            playerRigidBody.AddForce(0, modifiedForceValue, 0, ForceMode.Force);
            _levitateBall = false;
        }
        
    }

}


