using UnityEngine;

[CreateAssetMenu(fileName = "Missile", menuName = "Scriptable Objects/Missile")]
public class MissileSO : ScriptableObject, IDataContainer<MissileData>
{
    [SerializeField]
    private MissileData _data;
    public MissileData Data { get => _data; set => _data = value; }
}