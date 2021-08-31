using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyBehaviourSO : ScriptableObject
{
   public abstract EnemyBehaviour Behaviour { get; }
}
