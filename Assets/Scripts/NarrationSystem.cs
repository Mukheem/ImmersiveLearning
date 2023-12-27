using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NarrationSystem : MonoBehaviour, IObserver
{
    [SerializeField]
    private Subject _playerSubject;
   

    public void OnNotify(PlayerActionsEnum action)
    {
        if(action == PlayerActionsEnum.Intro)
        {
            Debug.Log("INTRO action played");
        }
        Debug.Log("LOG action played");
    }
    public void OnNotify(int forceValue)
    {
        Debug.Log(forceValue);
    }
    private void OnEnable()
    {
        _playerSubject.AddObserver(this);
           
    }

    private void OnDisable()
    {
        _playerSubject.RemoveObserver(this);
    }

    
}

