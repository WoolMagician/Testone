using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyBehaviourType
{
    Kamikaze
}

[CreateAssetMenu(fileName = "EnemyBehaviour", menuName = "Scriptable Objects/Enemy Behaviour")]
public class EnemyBehaviourSO : ScriptableObject
{
    public EnemyBehaviourType behaviourType;

    public EnemyBehaviour GetBehaviourScript()
    {
        switch (behaviourType)
        {
            case EnemyBehaviourType.Kamikaze:
                return new KamikazeEnemyBehaviour();
            default:
                return null;
        }
    }
}