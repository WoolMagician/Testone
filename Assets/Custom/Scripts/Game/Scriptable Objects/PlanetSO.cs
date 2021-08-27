using UnityEngine;

[CreateAssetMenu(fileName = "Planet", menuName = "Scriptable Objects/Planet")]
public class PlanetSO : ScriptableObject, IDataContainer<PlanetData>
{
    [SerializeField]
    private PlanetData _data;
    public PlanetData Data { get => _data; set => _data = value; }
}