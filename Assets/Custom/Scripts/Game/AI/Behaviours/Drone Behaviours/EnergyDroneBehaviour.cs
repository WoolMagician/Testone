using BezierSolution;
using UnityEngine;
using System.Collections;

public class EnergyDroneBehaviour : DroneBehaviour
{
    private bool energyGenCooldown = false;

    public override void Behave(Drone referenceObject)
    {
        foreach (Orbit item in OrbitFactory.Instance.CreatedObjects)
        {
            if (item.transform.localScale == referenceObject.orbit.transform.localScale)
            {
                item.orbitData.damageMultiplier = 1.5f;
            }
        }

        BezierWalkerWithSpeed walker = referenceObject.walker;

        // Null checks
        if (walker == null || walker.spline == null) return;

        // Execute walker
        walker.Execute(Time.deltaTime);

        if (!energyGenCooldown) referenceObject.StartCoroutine(GenerateEnergy());
    }

    private IEnumerator GenerateEnergy()
    {
        energyGenCooldown = true;
        GameManager.Instance.simulationData.mineralAcquired += 1;
        yield return new WaitForSeconds(5f);
        energyGenCooldown = false;
    }
}