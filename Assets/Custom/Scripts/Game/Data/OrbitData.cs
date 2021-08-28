using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class OrbitData : BaseData
{
    public float radius = 1f;
    public float speedMultiplier = 1;
    public float damageMultiplier = 1;

    public override IData Copy()
    {
        throw new System.NotImplementedException();
    }
}