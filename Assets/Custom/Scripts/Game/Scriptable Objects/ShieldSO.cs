using UnityEngine;

[CreateAssetMenu(fileName = "Shield", menuName = "Scriptable Objects/Shield")]
public class ShieldSO : ScriptableObject, IDataContainer<ShieldData>
{
    [SerializeField]
    private ShieldData _data;
    public ShieldData Data { get => _data; set => _data = value; }
}