using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Base abstract class for Factories
/// </summary>
public abstract class Factory<T, T1, T2> : Singleton<T> where T : MonoBehaviour where T1 : IDataContainer<IData>
{
    protected GameObject factoryGroupingObject;

    public T1[] availableObjects;
    public abstract string ObjectName { get; }
    public abstract List<T2> CreatedObjects { get; set; }
    public abstract T2 CreateAtWithRotation(IData data, Vector3 position, Vector3 rotation);

    protected override void OnAwake()
    {
        if(factoryGroupingObject == null)
        {
            factoryGroupingObject = new GameObject(this.GetType().Name + "_Group");
            factoryGroupingObject.transform.parent = this.transform;
        }
    }

    public virtual T2 Create(IData data)
    {
        return this.CreateAt(data, Vector3.zero);
    }

    public virtual T2 CreateAt(IData data, Vector3 position)
    {
        return this.CreateAtWithRotation(data, position, Vector3.zero);
    }

    public virtual T2 Create(int objectID)
    {
        if (!IsObjectIDAvailable(objectID)) return default;
        return this.Create(availableObjects[objectID]);
    }

    public virtual T2 CreateAt(int objectID, Vector3 position)
    {
        if (!IsObjectIDAvailable(objectID)) return default;
        return this.CreateAt(availableObjects[objectID], position);
    }

    public virtual T2 CreateAtWithRotation(int objectID, Vector3 position, Vector3 rotation)
    {
        if (!IsObjectIDAvailable(objectID)) return default;
        return this.CreateAtWithRotation(availableObjects[objectID], position, rotation);
    }

    public virtual T2 Create(T1 data)
    {
        return this.CreateAtWithRotation(data.Data, Vector3.zero, Vector3.zero);
    }

    public virtual T2 CreateAt(T1 data, Vector3 position)
    {
        return this.CreateAt(data.Data, position);
    }

    public virtual T2 CreateAtWithRotation(T1 data, Vector3 position, Vector3 rotation)
    {
        return this.CreateAtWithRotation(data.Data, position, rotation);
    }

    public bool IsObjectIDAvailable(int id)
    {
        return (availableObjects.Length > id);
    }
}