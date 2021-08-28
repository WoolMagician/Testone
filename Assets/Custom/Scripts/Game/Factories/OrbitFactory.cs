using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitFactory : Factory<OrbitFactory, OrbitSO, Orbit>
{
    public GameObject orbitPrefab;

    [SerializeField]
    private List<Orbit> _createdObjects = new List<Orbit>();

    public override string ObjectName => "Enemy";

    public override List<Orbit> CreatedObjects { get => _createdObjects; set => _createdObjects = value; }

    public override Orbit CreateAtWithRotation(IData data, Vector3 position, Vector3 rotation)
    {
        //If orbit prefab is not set, skip orbit creation and throw error.
        if (orbitPrefab == null)
        {
            Debug.LogError("Orbit prefab property of OrbitFactory is not set. Skipping orbit creation.");
            return null;
        }
        Orbit orbitComp;
        OrbitData orbitData = (OrbitData)data;
        GameObject newOrbit = Instantiate(orbitPrefab, this.transform.position, Quaternion.Euler(Random.Range(-30, 30), 0, Random.Range(-30, 30)), factoryGroupingObject.transform);
        newOrbit.name = string.Format("Orbit{0}", _createdObjects.Count);
        newOrbit.transform.localScale = new Vector3(orbitData.radius, orbitData.radius, orbitData.radius);
        orbitComp = newOrbit.GetComponent<Orbit>();
        _createdObjects.Add(orbitComp);
        return orbitComp;
    }
}