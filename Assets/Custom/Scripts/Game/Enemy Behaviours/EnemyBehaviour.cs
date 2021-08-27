using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class EnemyBehaviour
{
    public abstract void Behave(Enemy referenceEnemy);
}