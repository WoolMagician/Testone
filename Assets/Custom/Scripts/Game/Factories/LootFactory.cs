using System.Collections.Generic;
using UnityEngine;

public class LootFactory : Factory<LootFactory, LootSO, List<Loot>>   
{
    [SerializeField]
    private List<List<Loot>> _createdObjects = new List<List<Loot>>();

    public override string ObjectName => "Loot";

    public override List<List<Loot>> CreatedObjects { get => _createdObjects; set => _createdObjects = value; }

    public override List<Loot> CreateAtWithRotation(IData data, Vector3 position, Vector3 rotation)
    {
        List<Loot> loots = new List<Loot>();
        List<LootData> lootDatas = null;
        System.Type type = data.GetType();


        switch (type)
        {
            case System.Type t when type == typeof(LootTableData):
                lootDatas = ((LootTableData)data).GetLoot();
                float lootsCountScaled = (lootDatas.Count - 1) * 0.5f;

                for (int i = 0; i < lootDatas.Count; i++)
                {
                    //Scale position based on loots that we have to spawn from this loot table.
                    Vector3 scaledPosition = new Vector3(position.x + Random.Range(-lootsCountScaled, lootsCountScaled),
                                                         position.y,
                                                         position.z + Random.Range(-lootsCountScaled, lootsCountScaled));

                    loots.Add(CreateLootObject(lootDatas[i], scaledPosition, rotation));
                }
                break;
            case System.Type t when type == typeof(LootData):
                loots.Add(CreateLootObject((LootData)data, position, rotation));
                break;
            default:
                loots = null;
                break;
        }

        //Add loots to created objects.
        if (loots != null) CreatedObjects.Add(loots);

            //Return loots
            return loots;

    }

    private Loot CreateLootObject(LootData data, Vector3 position, Vector3 rotation)
    {
        if (data != null)
        {
            GameObject lootObject = new GameObject(string.Format(ObjectName + "_{0}", Instance._createdObjects.Count));
            lootObject.transform.position = position;
            lootObject.transform.rotation = Quaternion.Euler(rotation);

            Loot loot = lootObject.AddComponent<Loot>();
            loot.SetLootData(data);
            return loot;
        }
        return null;
    }
}
