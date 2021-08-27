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
        TurretData turretData = (TurretData)data;

        // Do not create and return in case of null data
        if (turretData == null) return null;

        GameObject newTurret = new GameObject
        {
            name = string.Format(ObjectName + "_{0}", Instance._createdObjects.Count)
        };
        newTurret.layer = LayerMask.NameToLayer(ObjectName);
        Drone turrComponent = newTurret.AddComponent<Drone>();
        SphereCollider sphereColl = newTurret.AddComponent<SphereCollider>();
        newTurret.AddComponent<VisualizeTransform>();

        turrComponent.turretData = turretData;
        turrComponent.ammoData = turretData.GetLevelData(turrComponent.currentLevel).ammo.Data;
        turrComponent.speed = turretData.GetLevelData(turrComponent.currentLevel).maxSpeed;
        turrComponent.travelMode = TravelMode.Loop;
        turrComponent.rotationLerpModifier = 10f;

        sphereColl.center = Vector3.zero;
        sphereColl.isTrigger = true;
        sphereColl.radius = turretData.GetLevelData(turrComponent.currentLevel).patrolSphereRadius;

        Instance._createdObjects.Add(turrComponent);

        return turrComponent;
    }

    public static void BrakeAllTurrets()
    {
        foreach (Drone item in Instance._createdObjects)
        {
            Instance.StartCoroutine(item.StopBoosterForTime(2));
        }
    }
}
