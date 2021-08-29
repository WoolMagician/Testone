using UnityEngine;

[System.Serializable]
public class DroneLevelData : BaseData
{
    public int energyCost = 0;
    public float health = 100f;
    public float maxSpeed = 3f;
    public float fireRate = 1f;
    public float damagePerHit = 10f;
    public float damageOnDeath = 100f;
    public float enemyDetectionRadius = 3f;

    public AmmoSO ammo;

    public GameObject droneObject;
    public GameObject droneTrail;
    public GameObject droneMuzzleFlashPrefab;
    public Vector3 droneObjectScaleOverride = Vector3.one;

    public override IData Copy()
    {
        return new DroneLevelData
        {
            energyCost = this.energyCost,
            health = this.health,
            maxSpeed = this.maxSpeed,
            fireRate = this.fireRate,
            damagePerHit = this.damagePerHit,
            damageOnDeath = this.damageOnDeath,
            enemyDetectionRadius = this.enemyDetectionRadius,
            ammo = ammo,
            droneObject = droneObject,
            droneTrail = droneTrail,
            droneMuzzleFlashPrefab = droneMuzzleFlashPrefab,
            droneObjectScaleOverride = droneObjectScaleOverride
        };
    }
}
