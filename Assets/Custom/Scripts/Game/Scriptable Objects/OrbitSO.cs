using UnityEngine;

[CreateAssetMenu(fileName = "Orbit", menuName = "Scriptable Objects/Orbit")]
public class OrbitSO : ScriptableObject, IDataContainer<OrbitData>
{
    [SerializeField]
    private OrbitData _data;
    public OrbitData Data { get => _data; set => _data = value; }
}
