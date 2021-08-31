using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

[CreateAssetMenu(fileName = "DroneApplyOnHitEffect", menuName = "Scriptable Objects/Power UPs/DroneApplyOnHitEffect")]
public class DroneApplyOnHitEffectSO : PowerUPSO
{
    [SerializeField]
    private DroneApplyOnHitEffect _powerup = new DroneApplyOnHitEffect();
    public override BasePowerUP PowerUP { get => new DroneApplyOnHitEffect() { onHitEffectSO = _powerup.onHitEffectSO }; }
}

[System.Serializable]
public class DroneApplyOnHitEffect : BasePowerUP, INotificationObserver
{
    [SerializeField]
    public PowerUPSO onHitEffectSO;

    protected Drone referenceDrone;
    protected List<Enemy> oldLockedEnemies = new List<Enemy>();

    public override void ApplyPowerUP(IData data, IHasPowerUPs poweredUpObject)
    {
        //Manage base powerup logic
        base.ApplyPowerUP(data, poweredUpObject);

        //Retrieve drone instance, null checks are missing porcodio
        referenceDrone = (Drone)poweredUpObject;

        //Filter subscribers that we need to remove and new subscribers
        List<Enemy> publishersToRemove = oldLockedEnemies.Except(referenceDrone.EnemiesWithinRange).ToList();
        List<Enemy> publishersToAdd = referenceDrone.EnemiesWithinRange.Except(oldLockedEnemies).ToList();

        if (publishersToRemove != null && publishersToRemove.Count > 0) this.UnsubscribeFrom(publishersToRemove);
        if (publishersToAdd != null && publishersToAdd.Count > 0) this.SubscribeTo(publishersToAdd);

        oldLockedEnemies = referenceDrone.EnemiesWithinRange.ToList();
    }

    public void OnNotification(NotificationEventArgs args)
    {
        Type type = args.GetType();

        switch (type)
        {
            case Type t when type == typeof(EnemyHitNotificationEventArgs):
                this.HandleEnemyGotHit((EnemyHitNotificationEventArgs)args);
                break;
        }
    }

    private void HandleEnemyGotHit(EnemyHitNotificationEventArgs hitArgs)
    {
        if (hitArgs.hitter.GetComponent<Drone>() == referenceDrone)
        {
            //Add on hit effect if is not applied yet
            if (!hitArgs.enemy.PowerUPs.ContainsOfType(onHitEffectSO.PowerUP))
            {
                hitArgs.enemy.PowerUPs.Add(((PowerUPSO)onHitEffectSO).PowerUP);
            }
            else
            {
                ((BasePowerUP)hitArgs.enemy.PowerUPs.GetOfType(onHitEffectSO.PowerUP)).StackDuration();
            }
        }
    }

    public void SubscribeTo(NotificationPublisher publisher)
    {
        publisher.AddObserver(this);
    }

    public void UnsubscribeFrom(NotificationPublisher publisher)
    {
        publisher.RemoveObserver(this);
    }

    private void SubscribeTo(List<Enemy> publishers)
    {
        foreach (Enemy enemy in publishers)
        {
            if (enemy != null)
            {
                SubscribeTo(enemy);
            }
        }
    }

    public void UnsubscribeFrom(List<Enemy> publishers)
    {
        foreach (Enemy enemy in publishers)
        {
            if (enemy != null)
            {
                UnsubscribeFrom(enemy);
            }
        }
    }
}
