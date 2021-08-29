using UnityEngine;

public enum AmmoType
{
    Laser,
    Bullet
}

[System.Serializable]
public class AmmoData : BaseData
{
    public string ammoName;
    public float damageMultiplier = 1f;
    public float maxSpeed = 0f;

    public GameObject ammoObject;
    public GameObject ammoTrail;

    public Vector3 ammoObjectScaleOverride = Vector3.one;

    public AmmoType ammoType;

    public override IData Copy()
    {
        return new AmmoData { ammoName = this.ammoName,
                              damageMultiplier = this.damageMultiplier,
                              maxSpeed = this.maxSpeed,
                              ammoObject = this.ammoObject,
                              ammoTrail = this.ammoTrail,
                              ammoObjectScaleOverride = this.ammoObjectScaleOverride };
    }
}
