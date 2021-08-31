[System.Serializable]
public class SimulationData : BaseData
{
    //Simulation data
    [ShowOnly] public int mineralAcquired = 10;
    [ShowOnly] public int defeatedEnemies;
    [ShowOnly] public int waveNumber;
    [ShowOnly] public float simulationTimeSeconds;
    [ShowOnly] public int missilesLeft = 1;
    [ShowOnly] public int shieldHitsLeft = 1;

    public override IData Copy()
    {
        throw new System.NotImplementedException();
    }
}
