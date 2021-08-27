[System.Serializable]
public class SimulationData : BaseData
{
    //Simulation data
    [ShowOnly] public int mineralAcquired;
    [ShowOnly] public int defeatedEnemies;
    [ShowOnly] public int waveNumber;
    [ShowOnly] public float simulationTimeSeconds;
}
