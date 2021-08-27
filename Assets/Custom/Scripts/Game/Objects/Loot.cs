using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loot : MonoBehaviour
{
    public LootData lootData;
    private GameObject lootObject;
    private SphereCollider lootCollider;

    public void Start()
    {
        this.gameObject.layer = LayerMask.NameToLayer("Item");
        this.tag = "Item";
        this.lootCollider = this.gameObject.AddComponent<SphereCollider>();
        this.lootCollider.isTrigger = true;
        this.lootCollider.radius = 1f;
    }

    private void DestroySelf()
    {
        Destroy(this.gameObject);
    }

    public void SetLootData(LootData ldata)
    {
        GameObject rarityObject = null;
        lootData = ldata;

        switch (lootData.rarity)
        {
            case LootRarity.Common:
                rarityObject = FindObjectOfType<GameManager>().commonItemPrefab;
                break;
            case LootRarity.Uncommon:
                rarityObject = FindObjectOfType<GameManager>().uncommonItemPrefab;
                break;
            case LootRarity.Rare:
                rarityObject = FindObjectOfType<GameManager>().rareItemPrefab;
                break;
        }
        Instantiate(rarityObject, this.transform);
        this.Invoke("DestroySelf", ldata.destroyAfterSeconds);
    }
}
