using UnityEngine;

[CreateAssetMenu(fileName = "Turret", menuName = "Scriptable Objects/Turret")]
public class DroneSO : ScriptableObject, IDataContainer<TurretData>
{
    [SerializeField]
    private TurretData _data;
    public TurretData Data { get => _data; set => _data = value; }
}