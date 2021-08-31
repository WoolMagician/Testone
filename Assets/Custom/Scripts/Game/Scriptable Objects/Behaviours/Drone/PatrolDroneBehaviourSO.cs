using UnityEngine;

[CreateAssetMenu(fileName = "PatrolDroneBehaviour", menuName = "Scriptable Objects/Behaviours/PatrolDroneBehaviour")]
public class PatrolDroneBehaviourSO : DroneBehaviourSO
{
    public override DroneBehaviour Behaviour {get => new PatrolDroneBehaviour(); }
}

public class PatrolDroneBehaviour : DroneBehaviour
{
    private Drone drone;
    private StateMachine sm = new StateMachine();

    private STDroneFireSingleTarget fireState;
    private STDroneMove moveState;

    public PatrolDroneBehaviour()
    {
        fireState = new STDroneFireSingleTarget();
        moveState = new STDroneMove();

        fireState.Name = nameof(STDroneFireSingleTarget);
        moveState.Name = nameof(STDroneMove);

        fireState.Execution = ExecutionType.ExecuteOnce;

        sm.States.TryAdd(moveState.Name, moveState);
        sm.States.TryAdd(fireState.Name, fireState);

        sm.SwitchToState(moveState.Name);
    }

    public override void Behave(Drone referenceObject)
    {
        drone = referenceObject;
        fireState.SetReferenceDrone(drone);
        moveState.SetReferenceDrone(drone);

        if (drone.EnemiesWithinRange.Count > 0)
        {
            if(sm.CurrentState.Name != fireState.Name)
                sm.SwitchToState(fireState.Name);
        }
        else
        {
            //Reset firing particles
            if (drone.muzzleFlashObject != null) { drone.muzzleFlashObject.SetActive(false); }

            if (sm.CurrentState.Name != moveState.Name)
                sm.SwitchToState(moveState.Name);
        }
        sm.Update();
    }   
}