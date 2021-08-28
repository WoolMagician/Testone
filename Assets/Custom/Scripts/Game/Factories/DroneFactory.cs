using AdvancedUtilities;
using BezierSolution;
using System.Collections.Generic;
using UnityEngine;

public class DroneFactory : Factory<DroneFactory, DroneSO, Drone>
{
    [SerializeField]
    private List<Drone> _createdObjects = new List<Drone>();

    public override string ObjectName => "Drone";

    public override List<Drone> CreatedObjects { get => _createdObjects; set => _createdObjects = value; }

    public override Drone CreateAtWithRotation(IData data, Vector3 position, Vector3 rotation)
    {
        DroneData turretData = (DroneData)data;

        // Do not create and return in case of null data
        if (turretData == null) return null;

        GameObject newTurret = new GameObject
        {
            name = string.Format(ObjectName + "_{0}", Instance._createdObjects.Count)
        };
        newTurret.transform.SetParent(factoryGroupingObject.transform);
        newTurret.layer = LayerMask.NameToLayer(ObjectName);
        Drone turrComponent = newTurret.AddComponent<Drone>();
        //SphereCollider sphereColl = newTurret.AddComponent<SphereCollider>();
        newTurret.AddComponent<VisualizeTransform>();

        turrComponent.droneReferenceData = turretData;
        if (turretData.GetLevelData(turrComponent.CurrentLevel).ammo != null)
        {
            turrComponent.ammoData = turretData.GetLevelData(turrComponent.CurrentLevel).ammo.Data;
        }
        turrComponent.walker = newTurret.AddComponent<BezierWalkerWithSpeed>();
        turrComponent.walker.speed = turretData.GetLevelData(turrComponent.CurrentLevel).maxSpeed;
        turrComponent.walker.travelMode = TravelMode.Loop;
        turrComponent.walker.rotationLerpModifier = 10f;

        //sphereColl.center = Vector3.zero;
        //sphereColl.isTrigger = true;
        //sphereColl.radius = turretData.GetLevelData(turrComponent.CurrentLevel).patrolSphereRadius;

        Instance._createdObjects.Add(turrComponent);

        return turrComponent;
    }
}
