using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class ObjectBehaviour<T>
{
    public abstract void Behave(T referenceObject);
}
    