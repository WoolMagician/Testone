using UnityEngine;

[System.Serializable]
public class MissileData : BaseData
{
    public float speed = 5f;
    public float damagePerHit = 100f;
    public GameObject missilePrefab;
    public Vector3 missileObjectScaleOverride = Vector3.one;

    public override IData Copy()
    {
        throw new System.NotImplementedException();
    }
}
