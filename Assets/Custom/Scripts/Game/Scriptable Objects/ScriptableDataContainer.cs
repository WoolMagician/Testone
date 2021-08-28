using UnityEngine;

public class ScriptableDataContainer<T> : ScriptableObject, IDataContainer<T> where T : IData
{
    [SerializeField]
    private T _data;
    public T Data { get => _data; }
}