using UnityEngine;

[CreateAssetMenu(fileName = "Loot", menuName = "Scriptable Objects/Loot")]
public class LootSO : ScriptableObject, IDataContainer<LootData>
{
    [SerializeField]
    private LootData _data;
    public LootData Data { get => _data; set => _data = value; }
}