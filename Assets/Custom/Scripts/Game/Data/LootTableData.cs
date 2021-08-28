using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LootTableItem
{
    public LootSO loot;
    public float chance;
    public int minQuantity;
    public int maxQuantity;
}

[System.Serializable]
public class LootTableData : BaseData
{
    public bool lootOnlyHighestRarity = true;
    public LootTableItem[] loots;

    public override IData Copy()
    {
        throw new System.NotImplementedException();
    }

    public List<LootData> GetLoot()
    {
        List<LootData> dropList = new List<LootData>();
        float drawn = Random.Range(0f, 100f);

        foreach (LootTableItem lootItem in loots)
        {
            if (drawn <= lootItem.chance)
            {
                lootItem.loot.Data.quantity = RandomQuantity(lootItem);
                dropList.Add(lootItem.loot.Data);
            }
        }

        if (lootOnlyHighestRarity)
        {
            LootData data = null;
            dropList.ForEach(item => {
                int count = 0;

                if ((int)item.rarity >= count)
                {
                    data = item;
                    count = (int)item.rarity;
                }
            });
            dropList.Clear();
            dropList.Add(data);
        }
        return dropList;
    }

    public int RandomQuantity(LootTableItem loot)
    {
        return Random.Range(loot.minQuantity, loot.maxQuantity);
    }
}
