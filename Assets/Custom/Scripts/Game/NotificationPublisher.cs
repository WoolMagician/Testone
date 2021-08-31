using System.Collections.Generic;
using UnityEngine;

public class NotificationPublisher : MonoBehaviour
{
    private List<INotificationObserver> observers = new List<INotificationObserver>();

    //Send notifications if something has happened
    public void Notify(NotificationEventArgs args)
    {
        for (int i = 0; i < observers.Count; i++)
        {
            //Notify all observers
            //Each observer should check if it is interested in this event
            observers[i].OnNotification(args);
        }
    }

    public void AddObserver(INotificationObserver observer)
    {
        observers.Add(observer);
    }

    public void RemoveObserver(INotificationObserver observer)
    {
        observers.Remove(observer); 
    }

    public void RemoveAllObservers()
    {
        observers.Clear();
    }
}
