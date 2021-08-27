using UnityEngine;

[CreateAssetMenu(fileName = "Ammo", menuName = "Scriptable Objects/Ammo")]
public class AmmoSO : ScriptableObject, IDataContainer<AmmoData>
{
    [SerializeField]
    private AmmoData _data;
    public AmmoData Data { get => _data; set => _data = value; }
}