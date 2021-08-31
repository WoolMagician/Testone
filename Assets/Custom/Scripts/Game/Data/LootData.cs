using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum LootRarity
{
    Common = 0,
    Uncommon = 1,
    Rare = 2,
}

public enum LootType
{
    Resource,
    Planet,
    Drone,
    Shield,
    Missile,
    Ammo,
    Powerup
}

[System.Serializable]
public class LootData : BaseData
{
    public int id;
    public string name;
    public LootRarity rarity = LootRarity.Common;
    public LootType type = LootType.Resource;
    public int destroyAfterSeconds = 10;

    [System.NonSerialized]
    public int quantity;

    [SerializeField]
    public ScriptableObject lootSO;

    public override IData Copy()
    {
        throw new System.NotImplementedException();
    }
}