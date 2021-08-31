using UnityEngine;

[CreateAssetMenu(fileName = "DroneSpeed", menuName = "Scriptable Objects/Power UPs/DroneSpeed")]
public class DroneSpeedSO : PowerUPSO
{
    [SerializeField]
    private DroneSpeed _powerup = new DroneSpeed();
    public override BasePowerUP PowerUP { get => new DroneSpeed() { speedMultiplier = _powerup.speedMultiplier }; }
}

[System.Serializable]
public class DroneSpeed : BasePowerUP
{
    [SerializeField]
    public float speedMultiplier = 1f;

    public override void ApplyPowerUP(IData data, IHasPowerUPs poweredUpObject)
    {
        //Manage base powerup logic
        base.ApplyPowerUP(data, poweredUpObject);

        DroneLevelData droneLevelData = (DroneLevelData)data;
        droneLevelData.maxSpeed = droneLevelData.maxSpeed * speedMultiplier;
    }
}