using UnityEngine;

public class NotificationEventArgs
{
    public GameObject publisher;
}

public interface INotificationObserver
{
    void OnNotify(NotificationEventArgs args);
}