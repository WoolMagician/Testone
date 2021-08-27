using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitFactory : Singleton<OrbitFactory>
{
    public GameObject orbitPrefab;
    public List<GameObject> orbits = new List<GameObject>();

    public GameObject CreateNewOrbit(float radius)
    {
        //If orbit prefab is not set, skip orbit creation and throw error.
        if (orbitPrefab == null)
        {
            Debug.LogError("Orbit prefab property of OrbitFactory is not set. Skipping orbit creation.");
            return null;
        }

        GameObject newOrbit = Instantiate(orbitPrefab, this.transform.position, Quaternion.Euler(Random.Range(-30, 30), 0, Random.Range(-30, 30)));
        newOrbit.name = string.Format("Orbit{0}", orbits.Count);
        newOrbit.transform.localScale = new Vector3(radius, radius, radius);
        orbits.Add(newOrbit);
        return newOrbit;
    }
}
