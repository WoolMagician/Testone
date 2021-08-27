using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LootTable", menuName = "Scriptable Objects/LootTable")]
public class LootTableSO : ScriptableObject, IDataContainer<LootTableData>
{
    [SerializeField]
    private LootTableData _data;
    public LootTableData Data { get => _data; set => _data = value; }
}