using UnityEngine;

[System.Serializable]
public class DroneLevelData : BaseData
{
    public int cost = 0;
    public float health = 100f;
    public float maxSpeed = 3f;
    public float boostSpeedMultiplier = 0f;
    public float fireRate = 1f;
    public float damagePerHit = 10f;
    public float damageOnExplode = 100f;
    public float patrolSphereRadius = 3f;
    public AmmoSO ammo;

    public GameObject droneObject;
    public GameObject droneTrail;
    public GameObject droneMuzzleFlash;
    public Vector3 droneObjectScaleOverride = Vector3.one;

    public override IData Copy()
    {
        return new DroneLevelData
        {
            cost = this.cost,
            health = this.health,
            maxSpeed = this.maxSpeed,
            boostSpeedMultiplier = this.boostSpeedMultiplier,
            fireRate = this.fireRate,
            damagePerHit = this.damagePerHit,
            damageOnExplode = this.damageOnExplode,
            patrolSphereRadius = this.patrolSphereRadius,
            ammo = ammo,
            droneObject = droneObject,
            droneTrail = droneTrail,
            droneMuzzleFlash = droneMuzzleFlash,
            droneObjectScaleOverride = droneObjectScaleOverride
        };
    }
}
