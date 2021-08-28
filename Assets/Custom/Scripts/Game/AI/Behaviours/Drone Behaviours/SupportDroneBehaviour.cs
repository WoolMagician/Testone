using BezierSolution;
using UnityEngine;

public class SupportDroneBehaviour : DroneBehaviour
{
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
    }
}