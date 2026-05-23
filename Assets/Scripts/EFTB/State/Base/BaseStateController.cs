using System;
using System.Collections.Generic;

public abstract class BaseStateController
{
    public event Action<BaseState, BaseState> EventStateChanged;
    public BaseState CurrentState { get; protected set; }
    protected Dictionary<Type, BaseState> States { get; set; }
    protected abstract Type DefaultTypeState { get; }

    protected BaseStateController()
    {
        States = new Dictionary<Type, BaseState>();
    }

    public virtual void Initialize()
    {
        foreach(var state in States.Values)
        {
            state.Initialize();
        }
    }

    public virtual void Dispose() 
    {
        if (CurrentState?.IsStateActive == true)
        {
            CurrentState.OnExitState();
        }
        CurrentState = null;

        foreach(var state in States.Values)
        {
            state.Dispose();
        }
    }

    public virtual void StartStateController(Type initialStateType = null)
    {
        var stateType = initialStateType ?? DefaultTypeState;
        if (!States.TryGetValue(stateType, out var initialState))
        {
            return;
        }

        CurrentState = initialState;
        CurrentState.OnEnterState();
    }

    public BaseState ChangeState(Type newState)
    {
        if (!IsValidToChangeState(newState))
        {
            return CurrentState;
        }

        var previousState = CurrentState;

        CurrentState.OnExitState();
        CurrentState = States[newState];
        CurrentState.OnEnterState();

        EventStateChanged?.Invoke(previousState, CurrentState);

        return CurrentState;
    }

    public void UpdateLogic(float deltaTime)
    {
        if (CurrentState == null) return;

        CurrentState.UpdateLogic(deltaTime);
    }

    public bool IsValidToChangeState(Type newState)
    {
        if (CurrentState.GetType() == newState)
        {
            return false;
        }

        if(!CurrentState.StateTransitionMap.TryGetValue(newState, out var changeCondition))
        {
            return false;
        }

        if(changeCondition != null && !changeCondition())
        {
            return false;
        }

        return true;
    }
}