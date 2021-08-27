using UnityEngine;

[System.Serializable]
public class TurretData : BaseData
{
    public string turretName;

    [SerializeField]
    public TurretLevelData[] levels = new TurretLevelData[3];

    public TurretLevelData GetLevelData(int level)
    {
        if (levels != null)
        {
            level = Mathf.Clamp(level, 0, levels.Length - 1);
            return levels[level];
        }
        else
        {
            return new TurretLevelData();
        }
    }
}
