using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class DroneBehaviourSO : ScriptableObject
{
   public abstract DroneBehaviour Behaviour { get; }
}
