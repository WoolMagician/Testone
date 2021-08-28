using UnityEngine;

/// <summary>
/// Data class that stores drone properties, stats and behaviour
/// </summary>
[System.Serializable]
public class DroneData : BaseData
{
    /// <summary>
    /// The name assigned to this drone.
    /// </summary>
    public string droneName;

    /// <summary>
    /// Stores ScriptableObject that contains behaviour informations.
    /// </summary>
    public DroneBehaviourSO droneBehaviourSO;

    [SerializeField]
    public DroneLevelData[] levels = new DroneLevelData[3];

    public override IData Copy()
    {
        DroneData copiedDroneData = new DroneData
        {
            droneName = this.droneName,
            droneBehaviourSO = this.droneBehaviourSO,
        };

        copiedDroneData.levels = new DroneLevelData[levels.Length];

        for (int i = 0; i < levels.Length; i++)
        {
            copiedDroneData.levels[i] = (DroneLevelData)levels[i].Copy();
        }

        return copiedDroneData;
    }

    public DroneLevelData GetLevelData(int level)
    {
        if (levels != null)
        {
            level = Mathf.Clamp(level, 0, levels.Length - 1);
            return levels[level];
        }
        else
        {
            return new DroneLevelData();
        }
    }
}
