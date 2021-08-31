using UnityEngine;

[CreateAssetMenu(fileName = "KamikazeDroneBehaviour", menuName = "Scriptable Objects/Behaviours/KamikazeDroneBehaviour")]
public class KamikazeDroneBehaviourSO : DroneBehaviourSO
{
    public override DroneBehaviour Behaviour { get => new KamikazeDroneBehaviour(); }
}

public class KamikazeDroneBehaviour : DroneBehaviour
{
    private Drone drone;
    private StateMachine sm = new StateMachine();

    private STDroneExplode explodeState;
    private STDroneMove moveState;

    public KamikazeDroneBehaviour()
    {
        explodeState = new STDroneExplode();
        moveState = new STDroneMove();

        explodeState.Name = nameof(STDroneExplode);
        moveState.Name = nameof(STDroneMove);

        explodeState.Execution = ExecutionType.ExecuteOnce;

        sm.States.TryAdd(moveState.Name, moveState);
        sm.States.TryAdd(explodeState.Name, explodeState);

        sm.SwitchToState(moveState.Name);
    }

    public override void Behave(Drone referenceObject)
    {
        drone = referenceObject;
        explodeState.SetReferenceDrone(drone);
        moveState.SetReferenceDrone(drone);

        if (drone.EnemiesWithinRange.Count > 0)
        {
            if (sm.CurrentState.Name != explodeState.Name)
                sm.SwitchToState(explodeState.Name);
        }
        else
        {
            if (sm.CurrentState.Name != moveState.Name)
                sm.SwitchToState(moveState.Name);
        }
        sm.Update();
    }
}