using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Subject : MonoBehaviour
{
    //List to store all the observers of this subject.
    private List<IObserver> _observers = new List<IObserver>();

    // Add an observer to Subject's List
    public void AddObserver(IObserver observer)
    {
        _observers.Add(observer);
    }

    // Remove an observer from Subject's List
    public void RemoveObserver(IObserver observer)
    {
        _observers.Remove(observer);
    }

    // Notify each observer when an event has happened.
    protected void NotifyObservers()
    {
        _observers.ForEach((_observer) => { _observer.OnNotify(); });
    }

}
