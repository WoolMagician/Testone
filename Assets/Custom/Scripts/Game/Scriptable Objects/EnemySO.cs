using UnityEngine;

[CreateAssetMenu(fileName = "Enemy", menuName = "Scriptable Objects/Enemy")]
public class EnemySO : ScriptableObject, IDataContainer<EnemyData>
{
    [SerializeField]
    private EnemyData _data;
    public EnemyData Data { get => _data; set => _data = value; }
}