using UnityEngine;

[CreateAssetMenu(fileName = "KamikazeEnemyBehaviour", menuName = "Scriptable Objects/Behaviours/KamikazeEnemyBehaviour")]
public class KamikazeEnemyBehaviourSO : EnemyBehaviourSO
{
    public override EnemyBehaviour Behaviour { get => new KamikazeEnemyBehaviour(); }
}

public class KamikazeEnemyBehaviour : EnemyBehaviour
{
    public override void Behave(Enemy referenceEnemy)
    {
        referenceEnemy.transform.Rotate(Vector3.one * 10f * Time.deltaTime);
        referenceEnemy.transform.position = Vector3.MoveTowards(referenceEnemy.transform.position, 
                                                                Planet.Instance.transform.position, 
                                                                referenceEnemy.enemyActualData.speed * Time.deltaTime);
    }
}
