using System.Collections.Generic;
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
    public string droneName = "New Drone";
    public int currentLevel = 0;

    /// <summary>
    /// Stores ScriptableObject that contains behaviour informations.
    /// </summary>
    public DroneBehaviourSO droneBehaviourSO;

    [SerializeReference]
    public List<PowerUPSO> PowerUPs;

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
        copiedDroneData.PowerUPs = this.PowerUPs;

        for (int i = 0; i < levels.Length; i++)
        {
            copiedDroneData.levels[i] = (DroneLevelData)levels[i].Copy();
        }


        return copiedDroneData;
    }


    public DroneLevelData GetCurrentLevelData()
    {
        if (levels != null)
        {
            return levels[Mathf.Clamp(currentLevel, 0, levels.Length - 1)];
        }
        else
        {
            return new DroneLevelData();
        }
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
