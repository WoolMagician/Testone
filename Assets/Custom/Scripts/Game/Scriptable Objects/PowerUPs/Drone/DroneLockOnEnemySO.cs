using UnityEngine;

[CreateAssetMenu(fileName = "DroneLockOnEnemy", menuName = "Scriptable Objects/Power UPs/DroneLockOnEnemy")]
public class DroneLockOnEnemySO : PowerUPSO
{
    [SerializeField]
    private DroneLockOnEnemy _powerup = new DroneLockOnEnemy();
    public override BasePowerUP PowerUP { get => new DroneLockOnEnemy() { speedMultiplier = _powerup.speedMultiplier }; }
}

[System.Serializable]
public class DroneLockOnEnemy : DroneSpeed
{
    public override void ApplyPowerUP(IData data, IHasPowerUPs poweredUpObject)
    {        
        //Manage base powerup logic
        base.ApplyPowerUP(data, poweredUpObject);

        Drone referenceDrone = (Drone)poweredUpObject;

        if (referenceDrone.EnemiesWithinRange.Count > 0)
        {
            base.ApplyPowerUP(data, poweredUpObject);
        }
    }
}