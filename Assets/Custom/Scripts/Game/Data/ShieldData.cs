using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ShieldData : BaseData
{
    public float shieldDurability = 1f;
    public float shieldDamageOnHit = 1000f;
    public Mesh shieldMesh;
    public Material shieldMaterial;
    public Material shieldHitMaterial;
    public float shieldActivationSpeed;
    public float shieldHitEffectDuration;
    public Color shieldHitColor;

    public override IData Copy()
    {
        throw new System.NotImplementedException();
    }
}
