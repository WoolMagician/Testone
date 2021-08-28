using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemyData : BaseData
{
    public float speed = 1f;
    public float maxHealth = 30f;
    public EnemyBehaviourSO enemyBehaviourSO;

    public LootTableSO lootTableSO;
    public GameObject[] enemyObjectVariants;
    public Material enemyMaterial;
    public GameObject enemyTrail;
    public GameObject enemyDieParticles;
    public Vector3 enemyObjectScaleOverride = Vector3.one;
    public Vector3 enemyTrailScaleOverride = Vector3.one;
    public Vector3 enemyDieParticlesScaleOverride = Vector3.one;

    public override IData Copy()
    {
        return new EnemyData()
        {
            speed = this.speed,
            maxHealth = this.maxHealth,
            enemyBehaviourSO = this.enemyBehaviourSO,
            lootTableSO = this.lootTableSO,
            enemyObjectVariants = this.enemyObjectVariants,
            enemyMaterial = this.enemyMaterial,
            enemyTrail = this.enemyTrail,
            enemyDieParticles = this.enemyDieParticles,
            enemyDieParticlesScaleOverride = this.enemyDieParticlesScaleOverride,
            enemyObjectScaleOverride = this.enemyObjectScaleOverride,
            enemyTrailScaleOverride = this.enemyTrailScaleOverride
        };
    }

    public GameObject GetRandomGameObjectVariant()
    {
        if (enemyObjectVariants != null)
        {
            return enemyObjectVariants[Random.Range(0, enemyObjectVariants.Length)];
        }
        return null;
    }
}
