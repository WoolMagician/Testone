using BezierSolution;
using UnityEngine;

[CreateAssetMenu(fileName = "SupportDroneBehaviour", menuName = "Scriptable Objects/Behaviours/SupportDroneBehaviour")]
public class SupportDroneBehaviourSO : DroneBehaviourSO
{
    public override DroneBehaviour Behaviour { get => new SupportDroneBehaviour(); }
}

public class SupportDroneBehaviour : DroneBehaviour
{
    public override void Behave(Drone referenceObject)
    {
        BezierWalkerWithSpeed walker = referenceObject.walker;

        // Null checks
        if (walker == null || walker.spline == null) return;

        // Execute walker
        walker.Execute(Time.deltaTime);
    }
}