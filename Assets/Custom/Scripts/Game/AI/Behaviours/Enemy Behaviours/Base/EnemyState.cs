public abstract class EnemyState : BaseState
{
    protected Enemy enemy;

    public override abstract void EnterState();

    public override abstract void ExitState();

    public override abstract void UpdateState();

    public void SetReferenceEnemy(Enemy enemy)
    {
        this.enemy = enemy;
    }
}