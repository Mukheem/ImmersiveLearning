using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IObserver
{
    // Subject should use this method to communicate with the observer
    //public void OnNotify(PlayerActionsEnum action);
    // Notify when there is force value (force value as integer)
    public void OnNotify(int forceValue);
}
