using System.Collections.Generic;
using UnityEngine;

public class LootFactory : Factory<LootFactory, LootSO, List<Loot>> , INotificationObserver
{
    [SerializeField]
    private List<List<Loot>> _createdObjects = new List<List<Loot>>();

    public GameObject commonItemPrefab;
    public GameObject uncommonItemPrefab;
    public GameObject rareItemPrefab;

    public override string ObjectName => "Loot";

    public override List<List<Loot>> CreatedObjects { get => _createdObjects; set => _createdObjects = value; }

    public void OnNotification(NotificationEventArgs args)
    {
        System.Type type = args.GetType();

        switch (type)
        {
            case System.Type t when type == typeof(LootRequestNotificationEventArgs):
                LootTableData lootTableData = ((LootRequestNotificationEventArgs)args).lootTableData;
                GameObject publisher = ((LootRequestNotificationEventArgs)args).publisher;
                this.HandleLootRequest(publisher, lootTableData);
                break;
            default:
                break;
        }
    }

    public void SubscribeTo(NotificationPublisher publisher)
    {
        publisher.AddObserver(this);
    }

    public void UnsubscribeFrom(NotificationPublisher publisher)
    {
        publisher.RemoveObserver(this);
    }

    private void HandleLootRequest(GameObject sender, LootTableData lootTableData)
    {
        //Instantiate loot
        Instance.CreateAtWithRotation(lootTableData, sender.transform.position, new Vector3(-90, 0,0));
    }

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
            GameObject lootObject = Instantiate(GetObjectPrefabFromRarity(data.rarity), position, Quaternion.Euler(rotation), factoryGroupingObject.transform);
            lootObject.layer = LayerMask.NameToLayer(ObjectName);
            lootObject.name = string.Format(ObjectName + "_{0}", Instance._createdObjects.Count);
            Loot loot = lootObject.AddComponent<Loot>();
            loot.SetLootData(data);
            return loot;
        }
        return null;
    }

    private GameObject GetObjectPrefabFromRarity(LootRarity rarity)
    {
        switch (rarity)
        {
            case LootRarity.Common:
                return commonItemPrefab;
            case LootRarity.Uncommon:
                return uncommonItemPrefab;
            case LootRarity.Rare:
                return rareItemPrefab;
            default:
                return new GameObject();
        }
    }
}
