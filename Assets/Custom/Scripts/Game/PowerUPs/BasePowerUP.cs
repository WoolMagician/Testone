using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public abstract class BasePowerUP : IPowerUP
{
    public abstract void ApplyPowerUP(IData data);
}
