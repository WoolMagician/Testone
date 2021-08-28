using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using UnityEngine;

public class StateMachine
{
    private bool stateFirstUpdate = true;

    /// <summary>
    /// List of all available states for this state machine.
    /// </summary>
    /// <returns></returns>
    public ConcurrentDictionary<string, IState> States { get; set; } = new ConcurrentDictionary<string, IState>();

    /// <summary>
    /// Current state processed by the state machine.
    /// </summary>
    /// <returns></returns>
    public IState CurrentState { get; private set; }

    /// <summary>
    /// Last state that the machine processed.
    /// </summary>
    /// <returns></returns>
    public IState PreviousState { get; private set; }

    public StateMachine()
    {
    }

    /// <summary>
    /// Used to switch to a specific available state.
    /// </summary>
    /// <param name="stateName"></param>
    public void SwitchToState(string stateName)
    {
        IState nextState;

        if (this.States.ContainsKey(stateName))
        {
            nextState = this.States[stateName];

            if (nextState != null)
            {
                this.PreviousState = this.CurrentState;
                this.CurrentState?.ExitState();
                this.CurrentState = nextState;
                this.CurrentState.EnterState();
                this.stateFirstUpdate = true;
            }
        }
    }

    /// <summary>
    /// Used to switch back to the previous state.
    /// </summary>
    public void SwitchToPrevious()
    {
        this.SwitchToState(this.PreviousState.Name);
    }

    public void Update()
    {
        this.OnPreUpdate();

        if (this.CurrentState != null)
        {
            this.CurrentState.UpdateState();

            // If it was first update
            if (this.stateFirstUpdate)
            {
                // If we must execute only once
                if (this.CurrentState.Execution == ExecutionType.ExecuteOnce)
                {
                    if (this.PreviousState != null)
                    {
                        // Go back to the previous state
                        this.SwitchToPrevious();
                        return;
                    }
                }
                this.stateFirstUpdate = false;
            }

            // Check if we have any available transition
            if (this.CurrentState.Transitions?.Count > 0)
            {
                // Scroll through priorities and filter transitions based on priority index
                for (int index = (int)StateTransition.HighestPriority; index >= 0; index += -1)
                {
                    int priorityIndex = index;
                    StateTransition prioritizedTransition = (from stateTrans in this.CurrentState.Transitions
                                                             where stateTrans.Priority == (TransitionPriority)priorityIndex
                                                             select stateTrans).FirstOrDefault();

                    if (prioritizedTransition != null)
                    {
                        Func<bool> transitionPredicate = prioritizedTransition.Predicate;

                        // Check if we met the conditions to allow state switching
                        if (transitionPredicate())
                        {
                            if (prioritizedTransition.WaitTimeBeforeTransition != 0)
                                Thread.Sleep((int)prioritizedTransition.WaitTimeBeforeTransition);

                            if (prioritizedTransition.ReferenceStateMachine != null && !Equals(prioritizedTransition.ReferenceStateMachine, this))
                            {
                                // Switch to new state 
                                prioritizedTransition.ReferenceStateMachine.SwitchToState(prioritizedTransition.NextState);
                            }
                            else
                                // Switch to state
                                this.SwitchToState(prioritizedTransition.NextState);
                            break;
                        }
                    }
                }
            }
        }
        this.OnPostUpdate();
    }

    protected virtual void OnPreUpdate() { }

    protected virtual void OnPostUpdate() { }
}
