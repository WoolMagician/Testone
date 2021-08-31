using UnityEngine;

[CreateAssetMenu(fileName = "EnemySpeedModifier", menuName = "Scriptable Objects/Power UPs/EnemySpeedModifier")]
public class EnemySpeedModifierSO : PowerUPSO
{
    [SerializeField]
    private EnemySpeedModifier _powerup = new EnemySpeedModifier();
    public override BasePowerUP PowerUP { get => new EnemySpeedModifier() { speedMultiplier = _powerup.speedMultiplier }; }
}

[System.Serializable]
public class EnemySpeedModifier : BasePowerUP
{
    [SerializeField]
    public float speedMultiplier = 1f;

    public override void ApplyPowerUP(IData data, IHasPowerUPs poweredUpObject)
    {
        //Manage base powerup logic
        base.ApplyPowerUP(data, poweredUpObject);

        EnemyData enemyData = (EnemyData)data;
        enemyData.speed = enemyData.speed * speedMultiplier;
    }
}