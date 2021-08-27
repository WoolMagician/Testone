using UnityEngine;

[System.Serializable]
public class TurretLevelData : BaseData
{
    public int cost = 0;
    public float health = 100f;
    public float maxSpeed = 3f;
    public float boostSpeedMultiplier = 0f;
    public float fireRate = 1f;
    public float damagePerHit = 10f;
    public float patrolSphereRadius = 3f;
    public AmmoSO ammo;

    public GameObject turretObject;
    public GameObject turretTrail;
    public GameObject turretMuzzleFlash;
    public Vector3 turretObjectScaleOverride = Vector3.one;
}
