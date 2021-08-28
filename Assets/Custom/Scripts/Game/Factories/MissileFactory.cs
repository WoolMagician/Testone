using UnityEngine;
using System.Collections.Generic;

public class MissileFactory : Factory<MissileFactory, MissileSO, Missile>
{    
    [SerializeField]
    private List<Missile> _createdObjects = new List<Missile>();

    public override string ObjectName => "Missile";

    public override List<Missile> CreatedObjects { get => _createdObjects; set => _createdObjects = value; }

    public override Missile CreateAtWithRotation(IData data, Vector3 position, Vector3 rotation)
    {
        Rigidbody rb;
        Missile missileComp;
        SphereCollider collider;
        MissileData missileData = (MissileData)data;

        // Do not create and return in case of null data
        if (missileData == null) return null;

        GameObject newMissile = Instantiate(missileData.missilePrefab, position, Quaternion.Euler(rotation), factoryGroupingObject.transform);
        newMissile.name = string.Format(ObjectName + "_{0}", Instance._createdObjects.Count);
        newMissile.layer = LayerMask.NameToLayer(ObjectName);
        newMissile.transform.localScale = missileData.missileObjectScaleOverride;

        //Add sphere collider user to trigger hit
        collider = newMissile.AddComponent<SphereCollider>();
        collider.radius = 0.1f;
        collider.isTrigger = true;

        //Add rigidbody to allow torque/force management
        rb = newMissile.AddComponent<Rigidbody>();
        rb.useGravity = false;

        //Add missile component
        missileComp = newMissile.AddComponent<Missile>();
        missileComp.missileData = missileData;

        return missileComp;
    }
}
