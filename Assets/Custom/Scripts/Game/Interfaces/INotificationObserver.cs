using UnityEngine;

public interface INotificationObserver
{
    void OnNotification(NotificationEventArgs args);
    void SubscribeTo(NotificationPublisher publisher);
    void UnsubscribeFrom(NotificationPublisher publisher);
}