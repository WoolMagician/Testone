using System.Collections.Generic;

public enum ExecutionType
{
    ExitOnTransitionRequest = 0,
    ExecuteOnce
}

public interface IState
{
    /// <summary>
    /// Base function called once when the state is entered.
    /// </summary>
    void EnterState();

    /// <summary>
    /// Base function called once when the state is exited.
    /// </summary>
    void ExitState();

    /// <summary>
    /// Base function called every UpdateTimeStep while in this state.
    /// </summary>
    void UpdateState();

    /// <summary>
    /// Name of the state, this should be unique.
    /// </summary>
    /// <returns></returns>
    string Name { get; set; }

    /// <summary>
    /// Execution type defined by enumerator.
    /// </summary>
    /// <returns></returns>
    ExecutionType Execution { get; set; }

    /// <summary>
    /// Time step for UpdateState function.
    /// </summary>
    /// <returns></returns>
    int UpdateTimeStep { get; set; }

    /// <summary>
    /// List of all available transitions.
    /// </summary>
    /// <returns></returns>
    List<StateTransition> Transitions { get; set; }
}