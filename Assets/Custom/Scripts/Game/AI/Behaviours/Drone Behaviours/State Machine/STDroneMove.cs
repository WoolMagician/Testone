using BezierSolution;
using UnityEngine;

public class STDroneMove : DroneState
{
    BezierWalkerWithSpeed walker;

    public override void EnterState() { }

    public override void ExitState() { }

    public override void UpdateState()
    {
        float speedMultiplier = 1f;

        if (drone.orbit != null)
        {
            speedMultiplier = drone.orbit.orbitData.speedMultiplier;
        }

        // Retrieve drone walker
        walker = drone.walker;

        // Null checks
        if (walker == null || walker.spline == null) return;

        // Set speed
        walker.speed = drone.droneActualData.GetCurrentLevelData().maxSpeed * speedMultiplier;

        // Execute walker
        walker.Execute(Time.deltaTime);
    }
}