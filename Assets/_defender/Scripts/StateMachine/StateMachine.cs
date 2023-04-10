using System;
using System.Collections.Generic;
using System.Linq;

public class StateMachine
{
    public event Action<IState> OnStateChanged;
    
    readonly List<StateTransition> _stateTransitions = new List<StateTransition>();
    readonly List<StateTransition> _anyStateTransitions = new List<StateTransition>();

    public IState CurrentState { get; private set; }


    public void SetState(IState newState)
    {
        if (CurrentState == newState) return;
        CurrentState?.OnExit();
        CurrentState = newState;
        CurrentState?.OnEnter();
        OnStateChanged?.Invoke(CurrentState);
    }

    public void AddTransition(IState from, IState to, Func<bool> condition)
    {
        var transition = new StateTransition(from, to, condition);
        _stateTransitions.Add(transition);
    }

    public void AddAnyTransition(IState to, Func<bool> condition)
    {
        var transition = new StateTransition(null, to, condition);
        _anyStateTransitions.Add(transition);
    }

    public void Tick()
    {
        var transition = CheckForTransition();
        if (transition != null)
        {
            SetState(transition.To);
        }

        CurrentState.Tick();
    }

    StateTransition CheckForTransition()
    {
        var transition = _anyStateTransitions.FirstOrDefault(t => t.Condition());
        return transition ?? _stateTransitions.FirstOrDefault(t => t.From == CurrentState && t.Condition());
    }
}