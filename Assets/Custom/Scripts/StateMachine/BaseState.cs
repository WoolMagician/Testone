using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseState : IState
{
    public string Name { get; set; }

    public ExecutionType Execution { get; set; }

    public int UpdateTimeStep { get; set; }

    public List<StateTransition> Transitions { get; set; }

    public abstract void EnterState();

    public abstract void ExitState();

    public abstract void UpdateState();
}