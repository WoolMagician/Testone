using UnityEngine;

[CreateAssetMenu(fileName = "Resource", menuName = "Scriptable Objects/Resource")]
public class ResourceSO : ScriptableObject, IDataContainer<ResourceData>
{
    [SerializeField]
    private ResourceData _data;
    public ResourceData Data { get => _data; set => _data = value; }
}