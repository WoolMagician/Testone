using UnityEngine;

[System.Serializable]
public class AmmoData : BaseData
{
    public string ammoName;
    public float damageMultiplier = 1f;
    public float maxSpeed = 0f;

    public GameObject ammoObject;
    public GameObject ammoTrail;

    public Vector3 ammoObjectScaleOverride = Vector3.one;
}
