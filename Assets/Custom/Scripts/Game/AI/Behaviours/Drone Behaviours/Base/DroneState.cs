public abstract class DroneState : BaseState
{
    protected Drone drone;

    public override abstract void EnterState();

    public override abstract void ExitState();

    public override abstract void UpdateState();

    public void SetReferenceDrone(Drone drone)
    {
        this.drone = drone;
    }
}