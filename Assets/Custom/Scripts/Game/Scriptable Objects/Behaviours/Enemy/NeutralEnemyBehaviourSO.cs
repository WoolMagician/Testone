using UnityEngine;

[CreateAssetMenu(fileName = "NeutralEnemyBehaviour", menuName = "Scriptable Objects/Behaviours/NeutralEnemyBehaviour")]
public class NeutralEnemyBehaviourSO : EnemyBehaviourSO
{
    public override EnemyBehaviour Behaviour { get => new NeutralEnemyBehaviour(); }
}
public class NeutralEnemyBehaviour : EnemyBehaviour
{
    Vector3 refPos = Vector3.zero;

    public override void Behave(Enemy referenceEnemy)
    {
        if (refPos == Vector3.zero)
        {
            refPos = referenceEnemy.transform.position;
        }

        //Retag enemy
        referenceEnemy.gameObject.layer = LayerMask.NameToLayer("NeutralEnemy");
        referenceEnemy.transform.Rotate(Vector3.one * 5f * Time.deltaTime);
        referenceEnemy.transform.position += Vector3.Cross(refPos, OrbitFactory.Instance.CreatedObjects[Random.Range(0, OrbitFactory.Instance.CreatedObjects.Count)].transform.up).normalized * Time.deltaTime * referenceEnemy.enemyActualData.speed;
    }
}