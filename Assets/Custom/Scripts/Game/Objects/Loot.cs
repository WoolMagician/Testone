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
        this.tag = "Item";
        this.lootCollider = this.gameObject.AddComponent<SphereCollider>();
        this.lootCollider.isTrigger = true;
        this.lootCollider.radius = 0.1f;
    }

    private void DestroySelf()
    {
        Destroy(this.gameObject);
    }

    public void SetLootData(LootData ldata)
    {
        lootData = ldata;
        this.Invoke("DestroySelf", ldata.destroyAfterSeconds);
    }
}
