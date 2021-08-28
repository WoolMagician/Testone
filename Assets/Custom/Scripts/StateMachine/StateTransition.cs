using System;

public enum TransitionPriority
{
    VeryLow,
    Low,
    Medium,
    High,
    VeryHigh
}

public class StateTransition
{
    /// <summary>
    /// Stores the highest priority
    /// </summary>
    public static TransitionPriority HighestPriority = TransitionPriority.VeryHigh;

    /// <summary>
    /// Time to wait before executing the transition once the predicate has been resolved.
    /// </summary>
    /// <returns></returns>
    public uint WaitTimeBeforeTransition = 0;

    /// <summary>
    /// State name to go to
    /// </summary>
    /// <returns></returns>
    public string NextState;

    ///<summary>
    ///Reference state machine that contains next state
    ///If set to nothing will take the current state machine.
    ///</summary>
    ///<returns></returns>
    public StateMachine ReferenceStateMachine;

    /// <summary>
    /// Condition to check to allow transition to happen
    /// </summary>
    ///<returns></returns>
    public Func<Boolean> Predicate = new Func<Boolean>(DoNothing);

    /// <summary>
    /// Priority of this transition
    /// </summary>
    /// <returns></returns>
    public TransitionPriority Priority;

    public StateTransition(string nextState) : this(nextState, null, TransitionPriority.Low) {}

    public StateTransition(string nextState, TransitionPriority priority) : this(nextState, null, priority) {}

    public StateTransition(string nextState, StateMachine referenceStateMachine, TransitionPriority priority)
    {
        this.NextState = nextState;
        this.ReferenceStateMachine = referenceStateMachine;
        this.Priority = priority;
    }

    private static bool DoNothing()
    {
        return true;

    }
        
}