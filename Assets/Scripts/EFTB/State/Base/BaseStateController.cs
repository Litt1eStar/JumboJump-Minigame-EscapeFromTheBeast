using System;
using System.Collections.Generic;

namespace JumboJumps.EFTB.State
{
    public abstract class BaseStateController
    {
        /// <summary>
        /// EventStateChanged will be invoked when state changed, and pass previous state and current state as parameters.
        /// </summary>
        public event Action<BaseState, BaseState> EventStateChanged;

        /// <summary>
        /// EventTimerChanged will be invoked when State Transition Timer is changed, and pass the timer value as parameter.
        /// </summary>
        public event Action<float> EventTimerChanged;
        public BaseState CurrentState { get; protected set; }
        protected Dictionary<Type, BaseState> States { get; set; }
        protected abstract Type DefaultTypeState { get; }

        protected BaseStateController()
        {
            States = new Dictionary<Type, BaseState>();
        }

        public virtual void Initialize()
        {
            foreach (var state in States.Values)
            {
                state.Initialize();
            }

            EventStateChanged?.Invoke(null, CurrentState);
        }

        public virtual void Dispose()
        {
            if (CurrentState?.IsStateActive == true)
            {
                CurrentState.OnExitState();
            }
            CurrentState = null;

            foreach (var state in States.Values)
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

        public virtual BaseState ChangeState(Type newState)
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

        public virtual void UpdateLogic(float deltaTime)
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

            if (!CurrentState.StateTransitionMap.TryGetValue(newState, out var changeCondition))
            {
                return false;
            }

            if (changeCondition != null && !changeCondition())
            {
                return false;
            }

            return true;
        }

        public void InvokeEventTimerChanged(float value)
        {
            EventTimerChanged?.Invoke(value);
        }
    }
}