using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ObjectBehaviour<T>
{
    public abstract void Behave(T referenceObject);
}
