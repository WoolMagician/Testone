using System.Collections.Generic;
using UnityEngine;

public class EnemyFactory : Factory<EnemyFactory, EnemySO, Enemy>
{
    [SerializeField]
    private List<Enemy> _createdObjects = new List<Enemy>();

    public override string ObjectName => "Enemy";

    public override List<Enemy> CreatedObjects { get => _createdObjects; set => _createdObjects = value; }

    public override Enemy CreateAtWithRotation(IData data, Vector3 position, Vector3 rotation)
    {
        EnemyData enemyData = (EnemyData)data;

        // Do not create and return in case of null data
        if (enemyData == null)
            return null;

        Enemy enemyComp;
        GameObject newEnemy = Instantiate(enemyData.GetRandomGameObjectVariant(), position, Quaternion.Euler(rotation), factoryGroupingObject.transform);
        newEnemy.name = string.Format(ObjectName + "_{0}", Instance._createdObjects.Count);
        newEnemy.layer = LayerMask.NameToLayer(ObjectName);

        //Instantiate trail if set
        if (enemyData.enemyTrail != null)
            Instantiate(enemyData.enemyTrail, newEnemy.transform).transform.localScale = enemyData.enemyTrailScaleOverride;

        enemyComp = newEnemy.AddComponent<Enemy>();
        enemyComp.SetEnemyData(enemyData);

        GameManager.Instance.SubscribeTo(enemyComp);
        WaveManager.Instance.SubscribeTo(enemyComp);
        LootFactory.Instance.SubscribeTo(enemyComp);

        newEnemy.transform.localScale = enemyData.enemyObjectScaleOverride;

        MeshRenderer meshRenderer = newEnemy.GetComponent<MeshRenderer>();
        meshRenderer.material = enemyData.enemyMaterial;

        MeshFilter meshFilter = newEnemy.GetComponent<MeshFilter>();
        MeshCollider meshCollider = newEnemy.AddComponent<MeshCollider>();
        meshCollider.sharedMesh = meshFilter.sharedMesh;
        meshCollider.convex = true; //Non convex objects will not work with OverlapSphere!!!

        newEnemy.AddComponent<Rigidbody>().isKinematic = true;

        Instance._createdObjects.Add(enemyComp);

        return enemyComp;
    }

    public static void DestroyAllEnemies()
    {
        foreach (Enemy item in Instance._createdObjects)
        {
            if(item != null)
            {
                Destroy(item.gameObject);
            }
        }
    }
}
